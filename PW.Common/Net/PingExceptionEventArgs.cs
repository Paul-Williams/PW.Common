namespace PW.Net;

/// <summary>
/// <see cref="PeriodicPing.OnPingException"/> event arguments.
/// </summary>
public class PingExceptionEventArgs : EventArgs
{
  /// <summary>
  /// The exception that occurred.
  /// </summary>
  public Exception Exception { get; }
  internal PingExceptionEventArgs(Exception exception)
  {
    Exception = exception;
  }
}
