using System.Diagnostics;

// See: https://stackoverflow.com/questions/470256/process-waitforexit-asynchronously/470288#470288

namespace PW.Extensions;

/// <summary>
/// Extension methods for <see cref="Process"/>
/// </summary>
public static class ProcessExtensions
{
  /// <summary>
  /// Waits asynchronously for the process to exit. Polling interval is 1 second.
  /// </summary>
  public static async Task WaitForExitAsync(this Process process!!, CancellationToken cancellationToken)
  {
    while (!process.HasExited) await Task.Delay(1000, cancellationToken).ConfigureAwait(false);
  }

  /// <summary>
  /// Waits asynchronously for the process to exit, with the specified polling interval.
  /// </summary>
  public static async Task WaitForExitAsync(this Process process!!, TimeSpan pollingInterval, CancellationToken cancellationToken)
  {
    while (!process.HasExited) await Task.Delay(pollingInterval, cancellationToken).ConfigureAwait(false);
  }



}
