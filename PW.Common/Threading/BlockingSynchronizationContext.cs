﻿using System.Collections.Concurrent;

namespace PW.Threading;

/// <summary> 
/// A SynchronizationContext which blocks and waits for completion.
/// Use: Thread A starts thread B and is blocked until thread B is complete or disposed.
/// Thread B can post messages back to run in the context of thread A.
/// </summary>
public sealed class BlockingSynchronizationContext : SynchronizationContext, IDisposable
{
  /// <summary>The queue of work items.</summary>
  private BlockingCollection<KeyValuePair<SendOrPostCallback, object>> Queue { get; } =
      new BlockingCollection<KeyValuePair<SendOrPostCallback, object>>();

  /// <summary>Dispatches an asynchronous message to the synchronization context.</summary>
  /// <param name="d">The System.Threading.SendOrPostCallback delegate to call.</param>
  /// <param name="state">The object passed to the delegate.</param>
  public override void Post(SendOrPostCallback d!!, object? state)
  {
    Queue.Add(new KeyValuePair<SendOrPostCallback, object>(d, state!));
  }

  /// <summary>Not supported.</summary>
  public override void Send(SendOrPostCallback d, object? state)
  {
    throw new NotSupportedException("Synchronously sending is not supported.");
  }

  /// <summary>Runs an loop to process all queued work items. Will block until cancelled via the <see cref="CancellationToken"/>.</summary>
  public void WaitForCompletion(CancellationToken cancellationToken)
  {
    try
    {
      foreach (var workItem in Queue.GetConsumingEnumerable(cancellationToken))
        workItem.Key(workItem.Value);
    }
    catch (OperationCanceledException) { }

  }

  /// <summary>Notifies the context that no more work will arrive.</summary>
  public void Complete() { Queue.CompleteAdding(); }

  /// <summary>
  /// Releases resources.
  /// </summary>
  public void Dispose() => Queue.Dispose();
}


