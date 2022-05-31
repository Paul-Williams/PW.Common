#nullable enable 

using System;

namespace PW
{
  /// <summary>
  /// Adds a payload to the standard <see cref="EventArgs"/> class.
  /// </summary>  
  public class EventArgs<T> : EventArgs
  {
    /// <summary>
    /// Creates a new instance.
    /// </summary>
    /// <param name="payload"></param>
    public EventArgs(T payload) => Payload = payload;

    /// <summary>
    /// Payload carried by the event.
    /// </summary>
    public T Payload { get; }
  }
}
