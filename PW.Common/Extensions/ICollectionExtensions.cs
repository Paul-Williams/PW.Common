namespace PW.Extensions;

/// <summary>
/// ICollection extension methods.
/// </summary>
public static class ICollectionExtensions
{
  /// <summary>
  /// Returns true if any elements in the sequence are null.
  /// </summary>
  public static bool ContainsNulls<T>(this ICollection<T> seq) => seq.Any(x => x is null);



  /// <summary>
  /// Removes all elements from <paramref name="first"/> that are in <paramref name="second"/> and returns the number of elements removed.
  /// </summary>
  public static int RemoveAll<T>(this ICollection<T> first!!, IEnumerable<T> second!!) => RemoveAll(first, second, null);

  /// <summary>
  /// Removes all elements from <paramref name="first"/> that are in <paramref name="second"/> and returns the number of elements removed.
  /// </summary>
  public static int RemoveAll<T>(this ICollection<T> first!!, IEnumerable<T> second!!, IEqualityComparer<T>? comparer)
  {
    var intersect = first.Intersect(second, comparer).ToArray();
    if (intersect.Length != 0)
    {
      Array.ForEach(intersect, x => _ = first.Remove(x));
      return intersect.Length;
    }
    else return 0;
  }
}
