#nullable enable 

using System.Collections.Generic;

namespace PW.Extensions
{
  /// <summary>
  /// Extension methods for use with chars.
  /// </summary>
  public static class CharExtensions
  {
    /// <summary>
    /// Converts an enumeration of chars in to a string.
    /// </summary>
    [System.Obsolete("Use AsString()")]
    public static string Concat(this IEnumerable<char> chars) => string.Concat(chars);


    /// <summary>
    /// Converts an enumeration of chars in to a string.
    /// </summary>
    public static string AsString(this IEnumerable<char> chars) => string.Concat(chars);

  }
}
