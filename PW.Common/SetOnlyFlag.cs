namespace PW
{
  /// <summary>
  /// A flag value, which can only be set.
  /// </summary>
  public struct SetOnlyFlag
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
}
