namespace PW.Threading;

/// <summary>
/// A thread-safe wrapper for the initialisation of <see cref="Random"/>.
/// See: https://stackoverflow.com/questions/273313/randomize-a-listt
/// </summary>
public static class ThreadSafeRandom
{
  /// <summary>
  /// Instance is created for each thread.
  /// </summary>
  [ThreadStatic] private static Random? Local;

  /// <summary>
  /// Returns a <see cref="Random"/> for use on the calling thread.
  /// </summary>
  public static Random ThisThreadsRandom => Local ??= new Random(GenerateSeed());

  private static int GenerateSeed() => unchecked(Environment.TickCount * 31 + Environment.CurrentManagedThreadId);

}
