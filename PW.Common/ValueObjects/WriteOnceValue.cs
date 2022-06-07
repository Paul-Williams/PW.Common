namespace PW.ValueObjects;

/// <summary>
/// Represents a value type, which can only be set once.
/// </summary>
public struct WriteOnceValue<T> : PW.Interfaces.IValue<T> where T : struct
{
  private T _value;

  /// <summary>
  /// Whether the value has been set.
  /// </summary>
  public bool HasValue { get; private set; }

  /// <summary>
  /// Returns <see cref="WriteOnceValue{T}.Value"/> as a string, or an empty string if not set.
  /// </summary>    
  public override string? ToString() => HasValue ? Convert.ToString(_value)! : null;

  /// <summary>
  /// Get/Set the value. Throws exceptions if attempting to set more than once, or get when not set.
  /// </summary>
  public T Value
  {
    get => HasValue ? _value : throw new InvalidOperationException("Value not set");

    set
    {
      if (HasValue) throw new InvalidOperationException("Value already set");
      _value = value;
      HasValue = true;
    }
  }

  /// <summary>
  /// Returns the value if set, otherwise returns default.
  /// </summary>
  public T ValueOrDefault => HasValue ? Value : default;

  /// <summary>
  /// Implicitly converts <see cref="WriteOnceValue{T}"/> to <typeparamref name="T"/>.
  /// </summary>
  /// <param name="value"></param>
  public static implicit operator T(WriteOnceValue<T> value) => value.ValueOrDefault;

}
