 

namespace PW
{

  /// <summary>
  /// Simple integer accumulator class
  /// </summary>
  public class Accumulator
  {

    /// <summary>
    /// Creates a new instance with an initial value of 1.
    /// </summary>
    public Accumulator()
    {
      Current = 1;
    }

    /// <summary>
    /// Creates a new instance with the specified initial value.
    /// </summary>
    /// <param name="initialValue"></param>
    public Accumulator(int initialValue)
    {
      Current = initialValue;
    }

    /// <summary>
    /// Increments the accumulator and returns the new value.
    /// </summary>
    public int Next => Current++;

    /// <summary>
    /// The current value of the accumulator.
    /// </summary>
    public int Current { get; private set; }

  }
}
