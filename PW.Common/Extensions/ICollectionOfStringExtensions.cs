using System;
using System.Collections.Generic;

namespace PW.Extensions;
internal static class ICollectionOfStringExtensions
{
  /// <summary>
  /// Removes all strings from <paramref name="first"/> that are in <paramref name="second"/> and returns the number of strings removed.
  /// </summary>
  public static int RemoveAllIgnoreCase(this ICollection<string> first!!, IEnumerable<string> second!!)
    => ICollectionExtensions.RemoveAll(first, second, StringComparer.OrdinalIgnoreCase);
}
