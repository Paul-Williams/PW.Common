namespace PW.Interfaces;

/// <summary>
/// Interface to a class which has a value that it represents. 
/// Use for equality comparison, sorting etc.
/// </summary>
public interface IReadOnlyValue<T>
{
  /// <summary>
  /// The value represented by this interface.
  /// </summary>
  T Path { get; }
}

