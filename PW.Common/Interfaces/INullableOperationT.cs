namespace PW.Interfaces;

/// <summary>
/// Interface to perform an operation which returns a value of type <typeparamref name="T"/>, which may be null.
/// </summary>
public interface INullableOperation<T> where T : class
{
  /// <summary>
  /// Performs the operation
  /// </summary>    
  T? Perform();
}


