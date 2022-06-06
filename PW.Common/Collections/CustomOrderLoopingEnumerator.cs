 

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// See: https://msdn.microsoft.com/en-us/magazine/mt809121.aspx
// For possible simplification.

namespace PW.Collections
{
  /// <summary>
  /// An enumerator that traverses a range, as a loop. That is, when it reaches <see cref="Last"/> it moves to <see cref="First"/>.
  /// </summary>
  public sealed class CustomOrderLoopingEnumerator : IEnumerable<int>, IEnumerator<int>, ILoopable
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
    public int Current => _order[OrderPos];

    object IEnumerator.Current => Current;

    private readonly int[] _order;

    private int OrderPos { get; set; }

    /// <summary>
    /// Determines whether the enumerator will loop which it reaches the end.
    /// </summary>
    public bool Looping { get; set; }

    /// <summary>
    /// Creates a new instance
    /// </summary>
    public CustomOrderLoopingEnumerator(IEnumerable<int> order)
    {
      _order = order.ToArray();
      First = _order[0];
      Last = _order[^1];
      OrderPos = 0;
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
    /// Move to the next value in the order. If at the end, then loop to the first.
    /// </summary>
    /// <returns></returns>
    public bool MoveNext()
    {
      OrderPos = OrderPos < _order.Length - 1 ? OrderPos + 1 : 0;

      //if (OrderPos < _order.Length - 1) OrderPos++;
      //else OrderPos = 0;

      return true;
    }

    /// <summary>
    /// Resets the enumerator to the initial state: first value, moving forwards.
    /// </summary>
    public void Reset() => OrderPos = 0;
  }
}
