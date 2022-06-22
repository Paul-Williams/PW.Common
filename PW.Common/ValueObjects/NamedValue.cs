namespace PW.ValueObjects;

/// <summary>
/// Represents a value which has a name.
/// </summary>
public class NamedValue<T> : ValueObject
{

  /// <summary>
  /// Creates a new instance.
  /// </summary>
  /// <param name="name">Value's name. Cannot be null.</param>
  /// <param name="value">Value - May be null.</param>
  /// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is null.</exception>
  public NamedValue(string name, T value)
  {
    Name = name ?? throw new ArgumentNullException(nameof(name));    
    Value = value;
  }

  /// <summary>
  /// Name of the value.
  /// </summary>
  public string Name { get; }

  /// <summary>
  /// The value.
  /// </summary>
  public T? Value { get; }

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
