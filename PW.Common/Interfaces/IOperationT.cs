namespace PW.Interfaces
{
  /// <summary>
  /// Interface to perform an operation which returns a value of type <typeparamref name="T"/>
  /// </summary>
  public interface IOperation<T>
  {
    /// <summary>
    /// Performs the operation
    /// </summary>    
    T Perform();
  }


}
