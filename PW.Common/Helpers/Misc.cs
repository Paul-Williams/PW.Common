using PW.Extensions;

namespace PW.Helpers;

/// <summary>
/// Methods with no place to live :(
/// </summary>
public static class Misc
{

  /// <summary>
  /// Swaps the values held by the two references.
  /// </summary>
  public static void Swap<T>(ref T? x, ref T? y) where T : class => (y, x) = (x, y);



  /// <summary>
  /// Creates a composite hash code from for multiple objects.
  /// </summary>
  public static int GetCompositeHashCode(params object[] objs)
  {
    unchecked // Overflow is fine, just wrap
    {
      int hash = (int)2166136261;

      foreach (var item in objs.SkipNulls())
        hash = (hash * 16777619) ^ item.GetHashCode();

      return hash;
    }
  }

  /// <summary>
  /// Creates a composite hashcode for the set of objects.
  /// </summary>
  /// <param name="objs"></param>
  /// <returns></returns>
  public static int CreateHashcode(params object[] objs)
  {

    // See: https://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-overriding-gethashcode

    int hash = 17;
    unchecked
    {
      foreach (var obj in objs.SkipNulls())
        if (obj != null) hash = hash * 23 + obj.GetHashCode();
    }
    return hash;
  }


}
