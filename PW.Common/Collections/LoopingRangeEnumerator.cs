// See: https://msdn.microsoft.com/en-us/magazine/mt809121.aspx
// For possible simplification.

namespace PW.Collections;

/// <summary>
/// An enumerator that traverses a range, as a loop. That is, when it reaches <see cref="Last"/> it moves to <see cref="First"/>.
/// </summary>
public sealed class LoopingRangeEnumerator : IEnumerable<int>, IEnumerator<int>

{

  /// <summary>
  /// First value in the range
  /// </summary>
  public int First { get; }

  /// <summary>
  /// Last value in the range.
  /// </summary>
  public int Last { get; }


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
  public LoopingRangeEnumerator(int first, int last)
  {
    if (first == last) throw new ArgumentException("'first' and 'last' have the same value.");
    if (last < first) throw new ArgumentException("'last' cannot be less than 'first'.");

    First = first;
    Last = last;
    Current = first;
    //Direction = MoveDirection.Forward;
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
    Current = (Current < Last) ? ++Current : Current = First;
    return true;
  }

  /// <summary>
  /// Resets the enumerator to the initial state: first value, moving forwards.
  /// </summary>
  public void Reset() => Current = First;
}
