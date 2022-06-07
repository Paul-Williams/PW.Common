using System.Diagnostics;

namespace PW.Diagnostics;

/// <summary>
/// Very basic code timer. Wrap code in a 'using(new CodeTimer("timer-name"))' block
/// Elapsed time is written to Trace (output window) when the instance is disposed
/// </summary>
public sealed class CodeTimer : IDisposable
{
  private readonly long startTicks;

  /// <summary>
  /// A <see cref="TimeSpan"/> representing the number of ticks that elapsed between instance creation and the subsequent call to <see cref="Stop"/>, or object disposal.
  /// </summary>
  public TimeSpan Ticks { get; private set; }

  /// <summary>
  /// An optional name to be assigned to the timer instance.
  /// </summary>
  public string Name { get; }

  /// <summary>
  /// Constructor
  /// </summary>
  /// <param name="name"></param>
  public CodeTimer(string? name = null)
  {
    Name = name ?? nameof(CodeTimer);
    startTicks = DateTime.Now.Ticks;
  }

  /// <summary>
  /// Stops the timer. Does not actually dispose anything. Useful to using the timer in a 'using' block.
  /// </summary>
  public void Dispose()
  {
    Stop();
  }

  /// <summary>
  /// Stops the timer.
  /// </summary>
  public void Stop()
  {
    Ticks = new TimeSpan(DateTime.Now.Ticks - startTicks);
    Trace.WriteLine($"{nameof(CodeTimer)} '{Name}' took {Ticks.TotalMilliseconds}ms.");
  }

}

