namespace PW.Functional;

/// <summary>
/// Extension methods for working with disposable objects in a functional manor.
/// </summary>
public static class Disposable
{
  /// <summary>
  /// Disposes object <paramref name="disposable"/> after performing operation <paramref name="func"/>.
  /// </summary>
  public static TR DisposeAfter<TDisposable, TR>(this TDisposable disposable, Func<TDisposable, TR> func!!) where TDisposable : IDisposable
  {
    using (disposable) return func(disposable);
  }

  /// <summary>
  /// Disposes object <paramref name="disposable"/> after performing operation <paramref name="action"/>.
  /// </summary>
  public static void DisposeAfter<T>(this T disposable, Action<T> action!!) where T : IDisposable
  {
    using (disposable) action(disposable);
  }


  /// <summary>
  /// Executes <paramref name="func"/> and disposes <paramref name="disposable"/>
  /// </summary>
  public static TR Using<T, TR>(T disposable, Func<T, TR> func!!) where T : IDisposable
  {
    using (disposable) return func(disposable);
  }

  /// <summary>
  /// Executes <paramref name="action"/> and disposes <paramref name="disposable"/>
  /// </summary>
  public static void Using<T>(T disposable, Action<T> action!!) where T : IDisposable
  {
    using (disposable) action(disposable);
  }

  /// <summary>
  /// Creates a disposable object using <paramref name="factory"/> and performs <paramref name="func"/> on it, before disposing.
  /// </summary>
  public static TR Using<T, TR>(Func<T> factory!!, Func<T, TR> func!!) where T : IDisposable
  {
    using var disposable = factory();
    return func(disposable);
  }

  /// <summary>
  /// Creates a disposable object using <paramref name="factory"/> and performs <paramref name="action"/> on it, before disposing.
  /// </summary>
  public static void Using<T>(Func<T> factory!!, Action<T> action!!) where T : IDisposable
  {
    using var disposable = factory();
    action(disposable);
  }

  /// <summary>
  /// Creates a disposable object using <paramref name="factory"/> and performs the async <paramref name="func"/> on it, before disposing.
  /// </summary>
  public static async Task<TR> UsingAsync<T, TR>(Func<T> factory!!, Func<T, Task<TR>> func!!) where T : IDisposable
  {
    using var disposable = factory();
    return await func(disposable).ConfigureAwait(false);
  }

  /// <summary>
  /// Creates a disposable object using <paramref name="factory"/> and performs the async <paramref name="action"/> on it, before disposing.
  /// </summary>
  public static async Task UsingAsync<T>(Func<T> factory!!, Func<T, Task> action!!) where T : IDisposable
  {
    using var disposable = factory();
    await action(disposable).ConfigureAwait(false);
  }

}

