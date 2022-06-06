using CSharpFunctionalExtensions;

namespace PW.Interfaces;

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


