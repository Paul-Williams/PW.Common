namespace PW.Extensions;

/// <summary>
/// Extension methods for numbers.
/// </summary>
public static class NumericExtensions
{
  /// <summary>
  /// Converts a value into a string that represents the number expressed as a size value in bytes, kilobytes, megabytes, or gigabytes, depending on the size.
  /// </summary>
  public static string ToStringByteSize(this int value) => Win32.SafeNativeMethods.StrFormatByteSize((ulong)value);

  /// <summary>
  /// Converts a value into a string that represents the number expressed as a size value in bytes, kilobytes, megabytes, or gigabytes, depending on the size.
  /// </summary>
  public static string ToStringByteSize(this uint value) => Win32.SafeNativeMethods.StrFormatByteSize((ulong)value);

  /// <summary>
  /// Converts a value into a string that represents the number expressed as a size value in bytes, kilobytes, megabytes, or gigabytes, depending on the size.
  /// </summary>
  public static string ToStringByteSize(this long value) => Win32.SafeNativeMethods.StrFormatByteSize((ulong)value);

  /// <summary>
  /// Converts a value into a string that represents the number expressed as a size value in bytes, kilobytes, megabytes, or gigabytes, depending on the size.
  /// </summary>
  public static string ToStringByteSize(this ulong value) => Win32.SafeNativeMethods.StrFormatByteSize(value);

}
