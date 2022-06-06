

using PW.Interfaces;

namespace PW
{

  /// <summary>
  /// Simple integer accumulator class
  /// </summary>
  public class Accumulator : IReadOnlyValue<int>
  {

    /// <summary>
    /// Creates a new instance with an initial value of 1.
    /// </summary>
    public Accumulator() => Value = 1;

    /// <summary>
    /// Creates a new instance with the specified initial value.
    /// </summary>
    public Accumulator(int initialValue) => Value = initialValue;


    /// <summary>
    /// Amount by which to increment the accumulator.
    /// </summary>
    public int IncrementBy { get; set; } = 1;

    /// <summary>
    /// Increments the accumulator and returns the new value.
    /// </summary>
    public int Increment() => Value += IncrementBy;

    /// <summary>
    /// The current value of the accumulator.
    /// </summary>
    public int Value { get; private set; }

  }
}
