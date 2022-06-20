namespace PW;

/// <summary>
/// Simple integer accumulator class
/// </summary>
public class Accumulator : IReadOnlyValue<int>
{

  /// <summary>
  /// Creates a new instance with an initial value of 1.
  /// </summary>
  public Accumulator() => Path = 1;

  /// <summary>
  /// Creates a new instance with the specified initial value.
  /// </summary>
  public Accumulator(int initialValue) => Path = initialValue;


  /// <summary>
  /// Amount by which to increment the accumulator.
  /// </summary>
  public int IncrementBy { get; set; } = 1;

  /// <summary>
  /// Increments the accumulator and returns the new value.
  /// </summary>
  public int Increment() => Path += IncrementBy;

  /// <summary>
  /// The current value of the accumulator.
  /// </summary>
  public int Path { get; private set; }

}
