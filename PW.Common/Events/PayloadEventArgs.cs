using System;

namespace PW.Events
{
  /// <summary>
  /// Adds a payload to the standard <see cref="EventArgs"/> class.
  /// </summary>  
  public class PayloadEventArgs<T> : EventArgs
  {
    /// <summary>
    /// Creates a new instance.
    /// </summary>
    /// <param name="payload"></param>
    public PayloadEventArgs(T payload) => Payload = payload;

    /// <summary>
    /// Payload carried by the event.
    /// </summary>
    public T Payload { get; }
  }
}
