#nullable enable 

using PW.FailFast;
using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;

namespace PW.Net
{

  /// <summary>
  /// Pings an IP address at a specified interval. Instances should be disposed after use.
  /// </summary>
  public sealed class PeriodicPing : IDisposable
  {
    const int PingTimeout = 5000;

    /// <summary>
    /// Notifies clients of a reply from a ping operation.
    /// </summary>
    public event PingEventHandler? OnPing;

    /// <summary>
    /// Represents the method which will handle the <see cref="OnPing"/> event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void PingEventHandler(object sender, PingCompletedEventArgs e);


    /// <summary>
    /// Invoked when Ping throws an exception
    /// </summary>
    public event PingExceptionEventHandler? OnPingException;

    /// <summary>
    /// Delegate for <see cref="OnPingException"/> event
    /// </summary>
    public delegate void PingExceptionEventHandler(object sender, PingExceptionEventArgs e);

    private Timer Timer { get; }

    private IPAddress IPAddress { get; }

    /// <summary>
    /// The interval between pings
    /// </summary>
    public long Period { get; set; } = 1000;

    /// <summary>
    /// Stops the ping timer. Any ping(s) in flight will still trigger the <see cref="OnPing"/> event.
    /// </summary>
    public void Stop() => Timer.Change(Timeout.Infinite, Timeout.Infinite);

    /// <summary>
    /// Starts the ping timer, using the current <see cref="Period"/>
    /// </summary>
    public void Start() => Timer.Change(WaitForPeriodAfterStart ? Period : 0, Period);

    /// <summary>
    /// After starting, whether the first ping should be immediate or after a <see cref="Period"/> has elapsed. Default value is false.
    /// </summary>
    public bool WaitForPeriodAfterStart { get; set; } //= false;

    private SynchronizationContext? SynchronizationContext { get; }

    /// <summary>
    /// Creates a new <see cref="PeriodicPing"/> instance to ping the specified IP address.
    /// </summary>
    public PeriodicPing(IPAddress ipAddress!!, SynchronizationContext? synchronizationContext)
    {
      IPAddress = ipAddress;
      Timer = new Timer(Timer_Callback);
      SynchronizationContext = synchronizationContext;
    }

    
    private void Timer_Callback(object? state)
    {
      // NB: This code is running on the timer thread, not the client/UI thread.
      // Unhandled exceptions here DO NOT propagate back to the client and will cause app to crash.
      try
      {
        var ping = new Ping();
        ping.PingCompleted += PingCompleted_EventHandler;
        ping.SendAsync(IPAddress, PingTimeout, null);
      }
      catch (Exception ex)
      {
        // TODO: Determine a way to inform the client than an exception has occurred. 
        // Raise Event? SynchronizationContext...
        // For now, just stop the timer.
        Stop();

        var evt = OnPingException;

        if (SynchronizationContext != null && evt != null)
        {
          SynchronizationContext.Post(new SendOrPostCallback((o) => evt(this, new PingExceptionEventArgs(ex))), null);
        }

      }

    }

    private void PingCompleted_EventHandler(object sender, PingCompletedEventArgs e)
    {
      // TODO: Use SynchronizationContext instead of Invoke()
      using ((Ping)sender) OnPing?.Invoke(this, e);
    }

    /// <summary>
    /// Disposes resources used by the <see cref="PeriodicPing"/> instance.
    /// </summary>
    public void Dispose() => Timer.Dispose();

  }
}
