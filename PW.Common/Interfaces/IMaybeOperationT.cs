namespace PW.Interfaces;

/// <summary>
/// Interface to perform an operation which returns a value of type <see cref="Maybe{T}"/>.
/// </summary>
public interface IMaybeOperation<T>
{
  /// <summary>
  /// Performs the operation
  /// </summary>    
  Maybe<T> Perform();
}


