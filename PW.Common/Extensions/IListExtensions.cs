using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PW.Extensions
{
  /// <summary>
  /// Extensions methods for <see cref="IList{T}"/>.
  /// </summary>
  public static class IListExtensions
  {

    /// <summary>
    /// Attempts to replace <paramref name="item"/> with <paramref name="replacement"/>
    /// </summary>
    /// <param name="list">Target</param>
    /// <param name="item">Item to be replaced.</param>
    /// <param name="replacement">Replacement item.</param>
    /// <returns>Returns the index of the replaced item, if found, otherwise -1.</returns>
    public static int Replace<T>(this IList<T> list!!, T item!!, T replacement)
    {
      if (list.IndexOf(item) is int i & i != -1) list[i] = replacement;
      return i;
    }

    /// <summary>
    /// Attempts to replace the first item returned by <paramref name="itemSelector"/> with <paramref name="replacement"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="itemSelector"></param>
    /// <param name="replacement"></param>
    /// <returns>Returns replaced item, if found by <paramref name="itemSelector"/>, otherwise default(<typeparamref name="T"/>).</returns>
    public static int ReplaceFirst<T>(this IList<T> list, Func<T, bool> itemSelector, T replacement) =>
      list is null ? throw new ArgumentNullException(nameof(list))
      : itemSelector is null ? throw new ArgumentNullException(nameof(itemSelector))
      : list.FirstOrDefault(itemSelector) is T item ? list.Replace(item, replacement) : -1;

    /// <summary>
    /// Attempts to replace the first item returned by <paramref name="itemSelector"/> with object created by <paramref name="replacement"/>. 
    /// <paramref name="replacement"/> is only invoked if <paramref name="itemSelector"/> returns a match.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="itemSelector"></param>
    /// <param name="replacement">Function to create a replacement, only if item is found.</param>
    /// <returns>Returns replaced item, if found by <paramref name="itemSelector"/>, otherwise default(<typeparamref name="T"/>).</returns>
    public static int ReplaceFirst<T>(this IList<T> list, Func<T, bool> itemSelector, Func<T> replacement) =>
      list is null ? throw new ArgumentNullException(nameof(list))
      : itemSelector is null ? throw new ArgumentNullException(nameof(itemSelector))
      : list.FirstOrDefault(itemSelector) is T item ? list.Replace(item, replacement()) : -1;
  }
}
