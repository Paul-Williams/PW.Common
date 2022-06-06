using System;
using System.Collections;
using System.Collections.Generic;

// See: https://msdn.microsoft.com/en-us/magazine/mt809121.aspx
// For possible simplification.

namespace PW.Collections;

/// <summary>
/// An enumerator that traverses a range, in both directions, as a loop. I.e. Loop(first -> last -> first)
/// </summary>
public sealed class OscillatingRangeEnumerator : IEnumerable<int>, IEnumerator<int>
{

  private enum MoveDirection
  {
    Forward,
    Back
  }

  /// <summary>
  /// First value in the range
  /// </summary>
  public int First { get; }

  /// <summary>
  /// Last value in the range.
  /// </summary>
  public int Last { get; }

  private MoveDirection Direction { get; set; }

  /// <summary>
  /// The current value
  /// </summary>
  public int Current { get; private set; }

  object IEnumerator.Current => Current;

  /// <summary>
  /// Creates a new instance
  /// </summary>
  /// <param name="first"></param>
  /// <param name="last"></param>
  public OscillatingRangeEnumerator(int first, int last)
  {
    if (first == last) throw new ArgumentException("'first' and 'last' have the same value.");
    if (last < first) throw new ArgumentException("'last' cannot be less than 'first'.");

    First = first;
    Last = last;
    Current = first;
    Direction = MoveDirection.Forward;
  }

  /// <summary>
  /// Returns the range enumerator
  /// </summary>
  /// <returns></returns>
  public IEnumerator<int> GetEnumerator() => this;

  IEnumerator IEnumerable.GetEnumerator() => this;

  /// <summary>
  /// Does nothing. Required for <see cref="IEnumerator{T}"/> interface.
  /// </summary>
  public void Dispose() { }

  /// <summary>
  /// Move to the next value in the range.
  /// </summary>
  /// <returns></returns>
  public bool MoveNext()
  {
    if (Direction == MoveDirection.Forward)
    {
      if (Current < Last)
      {
        Current++;
      }
      else
      {
        Direction = MoveDirection.Back;
        Current--;
      }

    }
    else //Moving back
    {
      if (Current > First)
      {
        Current--;
      }
      else
      {
        Direction = MoveDirection.Forward;
        Current++;
      }

    }

    return true;

  }

  /// <summary>
  /// Resets the enumerator to the initial state: first value, moving forwards.
  /// </summary>
  public void Reset()
  {
    Current = First;
    Direction = MoveDirection.Forward;
  }
}
