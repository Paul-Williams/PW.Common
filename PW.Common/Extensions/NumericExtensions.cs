using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PW.Extensions;

/// <summary>
/// Extension methods for numbers.
/// </summary>
public static class NumericExtensions
{
  /// <summary>
  /// Converts a value into a string that represents the number expressed as a size value in bytes, kilobytes, megabytes, or gigabytes, depending on the size.
  /// </summary>
  public static string ToStringByteSize(this int value) => NativeMethods.SafeNativeMethods.StrFormatByteSize((ulong)value);

  /// <summary>
  /// Converts a value into a string that represents the number expressed as a size value in bytes, kilobytes, megabytes, or gigabytes, depending on the size.
  /// </summary>
  public static string ToStringByteSize(this uint value) => NativeMethods.SafeNativeMethods.StrFormatByteSize((ulong)value);

  /// <summary>
  /// Converts a value into a string that represents the number expressed as a size value in bytes, kilobytes, megabytes, or gigabytes, depending on the size.
  /// </summary>
  public static string ToStringByteSize(this long value) => NativeMethods.SafeNativeMethods.StrFormatByteSize((ulong)value);

  /// <summary>
  /// Converts a value into a string that represents the number expressed as a size value in bytes, kilobytes, megabytes, or gigabytes, depending on the size.
  /// </summary>
  public static string ToStringByteSize(this ulong value) => NativeMethods.SafeNativeMethods.StrFormatByteSize(value);

}
