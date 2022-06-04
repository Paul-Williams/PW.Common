#nullable enable

using CSharpFunctionalExtensions;

namespace PW
{
  /// <summary>
  /// Interface to perform void returning operation.
  /// </summary>
  public interface IOperation
  {
    /// <summary>
    /// Performs the operation
    /// </summary> 
    void Perform();
  }

  /// <summary>
  /// Interface to perform an operation which returns a value of type <typeparamref name="T"/>
  /// </summary>
  public interface IOperation<T>
  {
    /// <summary>
    /// Performs the operation
    /// </summary>    
    T Perform();
  }

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

  /// <summary>
  /// An interface to an operation, which when performed and returns a <see cref="Result"/>
  /// </summary>
  public interface IResultOperation
  {
    /// <summary>
    /// Performs the operation and returns a <see cref="Result"/> of success or failure.
    /// </summary>    
    Result Perform();
  }

  /// <summary>
  /// An interface to an operation, which when performed and returns a <see cref="Result{T}"/>
  /// </summary>
  public interface IResultOperation<T>
  {
    /// <summary>
    /// Performs the operation.
    /// </summary>
    Result<T> Perform();
  }


}
