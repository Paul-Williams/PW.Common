namespace PW.Extensions;

/// <summary>
/// Extensions methods for IGrouping interface.
/// </summary>
public static class IGroupingExtensions
{
  /// <summary>
  /// Where clause: returns those groups with more than <paramref name="count"/> elements.
  /// </summary>
  public static IEnumerable<IGrouping<TKey, TElement>> LargerThan<TKey, TElement>(this IEnumerable<IGrouping<TKey, TElement>> group, int count)
    => group.Where(g => g.Count() > count);

  /// <summary>
  /// Where clause: returns those groups having more than one element.
  /// </summary>
  public static IEnumerable<IGrouping<TKey, TElement>> WithMultipleElements<TKey, TElement>(this IEnumerable<IGrouping<TKey, TElement>> group)
    => group.LargerThan(1);
}
