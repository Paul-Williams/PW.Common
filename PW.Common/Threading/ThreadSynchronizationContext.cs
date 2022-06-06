 

// A simple SynchronizationContext that encapsulates it's own dedicated task queue and processing
// thread for servicing Send() & Post() calls.  
// Based upon http://blogs.msdn.com/b/pfxteam/archive/2012/01/20/10259049.aspx but uses it's own thread
// rather than running on the thread that it's instantiated on

// See: https://stackoverflow.com/questions/1882417/looking-for-an-example-of-a-custom-synchronizationcontext-required-for-unit-tes

using System;
using System.Collections.Concurrent;
using System.Threading;

namespace PW.Threading
{

  /// <summary>
  /// Supports thread synchronization between non-UI threads.
  /// </summary>
  public sealed class ThreadSynchronizationContext : SynchronizationContext, IDisposable
  {
    // The worker thread to process the queue
    private Thread WorkerThread { get; }

    // The queue of work items.
    private BlockingCollection<QueueItem> Queue { get; } = new BlockingCollection<QueueItem>();

    /// <summary>
    /// Creates a new instance
    /// </summary>
    public ThreadSynchronizationContext()
    {
      // I added the IsBackground
      WorkerThread = new Thread(WorkerThreadProc) { IsBackground = true };
      WorkerThread.Start(this);
    }


    /// <summary>Dispatches an asynchronous message to the synchronization context.</summary>
    /// <param name="d">The System.Threading.SendOrPostCallback delegate to call.</param>
    /// <param name="state">The object passed to the delegate.</param>
    public override void Post(SendOrPostCallback d!!, object? state)
    {
      Queue.Add(new QueueItem(d, state!));
    }

    private class CallbackArgs
    {
      public CallbackArgs(SendOrPostCallback c, object o, ManualResetEvent e)
      {
        Callback = c;
        Object = o;
        Event = e;
      }

      public SendOrPostCallback Callback { get; }
      public object Object { get; }
      public ManualResetEvent Event { get; }
    }

    private class QueueItem
    {
      public QueueItem(SendOrPostCallback c, object o)
      {
        Callback = c;
        Object = o;
      }

      public SendOrPostCallback Callback { get; }
      public object Object { get; }
    }

    /// <summary>
    /// Sends and waits
    /// </summary>
    /// <param name="d"></param>
    /// <param name="state"></param>
    public override void Send(SendOrPostCallback d, object? state)
    {
      using var handledEvent = new ManualResetEvent(false);
      Post(SendOrPostCallback_BlockingWrapper, new CallbackArgs(d, state!, handledEvent));
      handledEvent.WaitOne(); // This seems dodgy -- Possible infinite wait?
    }

    /// <summary>
    /// Managed thread id of the internal worker thread
    /// </summary>
    public int WorkerThreadId => WorkerThread.ManagedThreadId;

    private static void SendOrPostCallback_BlockingWrapper(object? state)
    {
      if (state is CallbackArgs args)
      {
        try { args.Callback(args.Object); }
        finally { args.Event.Set(); }
      }
      else throw new ArgumentException($"Argument '{nameof(state)}' was not of type '{nameof(CallbackArgs)}'.", nameof(state));
    }



    /// <summary>Runs an loop to process all queued work items.</summary>
    private void WorkerThreadProc(object? data)
    {
      if (data is SynchronizationContext context)
      { 
        SetSynchronizationContext(context); 
      }
      else
      {
        throw new ArgumentException($"Argument '{nameof(data)}' was not of type '{nameof(SynchronizationContext)}'.", nameof(data));
      }

      try
      {
        foreach (var item in Queue.GetConsumingEnumerable()) item.Callback(item.Object);
      }
      catch (ObjectDisposedException) { }

      finally { Queue.Dispose(); } // I think this should be added here.
    }


    /// <summary>
    /// Disposes the object
    /// </summary>
    public void Dispose() => Queue.CompleteAdding();


  }
}