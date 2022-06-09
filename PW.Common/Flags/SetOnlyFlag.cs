namespace PW.Flags;

// Don't make this a struct as that limits it to use as a field
// because a property getter will otherwise pass a copy.


/// <summary>
/// A flag value, which can only be set.
/// </summary>
public class SetOnlyFlag
{
  /// <summary>
  /// Whether the flag has been set.
  /// </summary>
  public bool IsSet { get; private set; }

  /// <summary>
  /// Sets the flag.
  /// </summary>
  public void Set() => IsSet = true;

  /// <summary>
  /// Returns true if the flag has been set, otherwise false.
  /// </summary>
  /// <param name="flag"></param>
  public static implicit operator bool(SetOnlyFlag flag) => flag.IsSet;

}
