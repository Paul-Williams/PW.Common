namespace PW.Lazy;

/// <summary>
/// A version of <see cref="Lazy{T}"/> which supports resetting to its initial state, before the value was created.
/// </summary>
/// <typeparam name="T"></typeparam>
public class ResettableLazy<T>
{

  private Lazy<T> Lazy { get; set; }
  private Func<T> Factory { get; }

  /// <summary>
  /// Gets the lazily initialized value of the current <see cref="ResettableLazy{T}"/> instance.
  /// </summary>
  public T Value => Lazy.Value;

  /// <summary>
  /// Gets a value that indicates whether a value has been created for this <see cref="ResettableLazy{T}"/> instance.
  /// </summary>
  public bool IsValueCreated => Lazy.IsValueCreated;


  /// <summary>
  /// Initializes a new instance of the <see cref="ResettableLazy{T}"/> class. When lazy initialization
  /// occurs, the specified initialization function is used.
  /// </summary>
  public ResettableLazy(Func<T> factory)
  {
    Factory = factory;
    Lazy = InitNewLazyUsingFactory();
  }




  /// <summary>
  /// Centralized initialization of the object.
  /// </summary>
  private Lazy<T> InitNewLazyUsingFactory() => new(Factory, LazyThreadSafetyMode.ExecutionAndPublication);

  /// <summary>
  /// Resets the instance so that there is no longer a value. Does nothing if there is no current value.
  /// </summary>
  /// <param name="shutdownAction">Optional shutdown operation to perform on the value <typeparamref name="T"/></param>
  public void Reset(Action<T>? shutdownAction = null)
  {
    // Nothing to do here...
    if (!Lazy.IsValueCreated) return;

    // We will work with a local copy to stop interference from the outside.
    var localCopy = Lazy;

    // Get a fresh instance ready ASAP
    Lazy = InitNewLazyUsingFactory();

    // If an action is required on shutdown, run it now.
    shutdownAction?.Invoke(localCopy.Value);

    // When the Lazy is wrapping a disposable object properly dispose of it.
    if (localCopy.Value is IDisposable disposable) disposable.Dispose();

  }



}