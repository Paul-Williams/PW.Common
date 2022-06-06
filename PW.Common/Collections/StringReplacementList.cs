using PW.FailFast;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PW.Collections;

/// <summary>
/// A list of string-pairs for 'find and replace' purposes.
/// </summary>
public class StringReplacementList : List<(string Find, string ReplaceWith)>
{
  /// <summary>
  /// Creates a new instance from the enumeration.
  /// </summary>
  /// <param name="collection"></param>
  /// <exception cref="ArgumentException">If any string is null, or any 'Find' string is empty.</exception>
  public StringReplacementList(IEnumerable<(string Find, string ReplaceWith)> collection!!) : base(collection)
  {
    Guard.False(this.Any(x => x.Find is null || x.ReplaceWith is null), nameof(collection), "Collection contains at least one null 'Find' or 'ReplaceWith' entry.");
    Guard.False(this.Any(x => x.Find == string.Empty), nameof(collection), "Collection contains at least one empty 'Find' entry.");
  }
}
