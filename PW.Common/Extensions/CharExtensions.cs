using System.Collections.Generic;

namespace PW.Extensions;

/// <summary>
/// Extension methods for use with chars.
/// </summary>
public static class CharExtensions
{
  /// <summary>
  /// Concatenates an enumeration of chars in to a string.
  /// </summary>
  public static string AsString(this IEnumerable<char> chars) => string.Concat(chars);

}
