using System;
using System.Collections.Generic;

namespace PW;

/// <summary>
/// Compares two objects by any comparable property.
/// </summary>
/// <typeparam name="T"></typeparam>
public class PropertyComparer<T> : IComparer<T>
{

  /// <summary>
  /// Creates a new instance.
  /// </summary>
  /// <param name="selector">Function to determine property to compare.</param>
  /// <param name="sortOrder">Guess</param>
  public PropertyComparer(Func<T?, IComparable> selector, SortOrder sortOrder = SortOrder.Ascending)
  {
    Selector = selector;
    SortOrder = sortOrder;
  }

  private Func<T?, IComparable> Selector { get; }

  /// <summary>
  /// Sort order for compare method
  /// </summary>
  public SortOrder SortOrder { get; set; }

  /// <summary>
  /// Compare two objects for sorting purposes.
  /// </summary>
  public int Compare(T? x, T? y)
  {
    // Swap comparison objects to invert sort order
    var (left, right) = SortOrder == SortOrder.Descending ? (Selector(y), Selector(x)) : (Selector(x), Selector(y));

    return left == null ? right == null ? 0 : -right.CompareTo(null) : left.CompareTo(right);
  }
}
