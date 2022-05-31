namespace PW.Collections
{
  /// <summary>
  /// Implemented by objects that support loping, such as certain enumerators.
  /// </summary>
  public interface ILoopable
  {
    /// <summary>
    /// Determines whether the object will loop which it reaches the end of a sequence.
    /// </summary>
    bool Looping { get; set; }
  }
}
