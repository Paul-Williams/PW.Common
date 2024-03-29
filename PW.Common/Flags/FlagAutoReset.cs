﻿namespace PW.Flags;


/// <summary>
/// A class which acts as a wrapper for a bool-flag and additionally provides a mechanism to automatically set/unset the flag in a using block.
/// </summary>
[System.Diagnostics.DebuggerDisplay("{Value}")]
public class AutoResetFlag
{
  /// <summary>
  /// The value of the flag
  /// </summary>
  public bool Value { get; set; }

  /// <summary>
  /// Converts flag -> bool
  /// </summary>
  /// <param name="flag"></param>
  public static implicit operator bool(AutoResetFlag flag) => flag.Value;

  /// <summary>
  /// Creates a new <see cref="AutoResetFlag"/> instance with the specified value.
  /// </summary>
  /// <param name="value"></param>
  public static implicit operator AutoResetFlag(bool value) => new() { Value = value };

  /// <summary>
  /// Returns <see cref="Value"/> as string.
  /// </summary>
  /// <returns></returns>
  public override string ToString() => Value.ToString();

  /// <summary>
  /// Whether the <see cref="AutoResetFlag"/> is set to true.
  /// </summary>
  public bool IsSet => Value == true;


  /// <summary>
  /// Creates a disposable 'token' which sets and resets the flag within a using block or Try-Finally block.
  /// </summary>
  /// <param name="value">The value for the flag while the token is in use. 
  /// This is then toggled when the token is disposed.</param>
  public IDisposable CreateAutoResetToken(bool value) => new FlagAutoReset(this, value);


  /// <summary>
  /// Use to set a bool flag in a using-block, for example 'don't fire event for the moment...' Flag is flipped back when disposed.
  /// Ensures flag value is restored in the event of an exception.
  /// </summary>
  private class FlagAutoReset : IDisposable
  {

    private AutoResetFlag Flag { get; }

    public FlagAutoReset(AutoResetFlag flag, bool valueWhileInUse)
    {
      Flag = flag;
      Flag.Value = valueWhileInUse;
    }

    /// <summary>
    /// Flip the flag state when token is released.
    /// </summary>
    void IDisposable.Dispose() => Flag.Value = !Flag.Value;
  }


}

