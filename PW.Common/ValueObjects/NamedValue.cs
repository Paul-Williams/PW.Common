using System;
using System.Collections.Generic;
using static PW.Helpers.Generic;

namespace PW.ValueObjects
{
  /// <summary>
  /// Represents a value which has a name.
  /// </summary>
  /// <typeparam name="T">The type of the value</typeparam>
  public class NamedValue<T> : CSharpFunctionalExtensions.ValueObject
  {
    /// <summary>
    /// Creates a new instance.
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    public NamedValue(string name!!, T value!!)
    {
      Name = name;
      Value = value;  //value.OrIfNullThrow(() => new ArgumentNullException(nameof(value)));
    }

    /// <summary>
    /// Name of the value.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// The value.
    /// </summary>
    public T Value { get; }

    /// <summary>
    /// 
    /// </summary>
    protected override IEnumerable<object> GetEqualityComponents()
    {
      yield return Name;
      yield return Value!;
    }

    /// <summary>
    /// Custom ToString
    /// </summary>
    public override string ToString() => $"Name: {Name} - Value: {Value}";
  }
}
