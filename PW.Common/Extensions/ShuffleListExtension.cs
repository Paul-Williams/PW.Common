using PW.FailFast;
using System.Collections.Generic;

// See: https://stackoverflow.com/questions/273313/randomize-a-listt

namespace PW.Extensions;

/// <summary>
/// Extension methods for shuffling lists.
/// </summary>
public static class ShuffleListExtension
{

  /// <summary>
  /// Performs an in-place shuffle of the items in the list.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="list"></param>
  public static void Shuffle<T>(this IList<T> list!!)
  {
    // If there are more then 2 elements in the list then perform the shuffle.

    // Method:
    // Cursor backwards through the list
    // Swap the element at the cursor position with a random element
    // A temporary variable holds the random element's value during the swap

    if (list.Count > 2)
    {
      int cursor = list.Count;
      while (cursor > 1)
      {
        cursor--;

        // CHANGE: THe original implementation used .Next(cursor+1)
        // However that kept caused elements to be swapped with themself!
        int random = Threading.ThreadSafeRandom.ThisThreadsRandom.Next(cursor);

        (list[cursor], list[random]) = (list[random], list[cursor]);
      }
    }
    // If there are only two element in the list then simply swap them.
    // This saves all the looping and calling Random.Next etc.
    // If there is less than two, then there is nothing to do !
    else if (list.Count == 2)
    {
      (list[1], list[0]) = (list[0], list[1]);
    }
  }

}
