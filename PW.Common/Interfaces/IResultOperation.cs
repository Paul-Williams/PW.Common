using CSharpFunctionalExtensions;

namespace PW.Interfaces;

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


