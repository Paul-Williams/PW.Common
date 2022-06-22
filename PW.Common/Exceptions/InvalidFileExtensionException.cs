namespace PW.Exceptions;

/// <summary>
/// File extension is invalid.
/// </summary>
[Serializable]
public class InvalidFileExtensionException : Exception
{
  /// <summary>
  /// Creates a new instance.
  /// </summary>
  /// <param name="message"></param>
  public InvalidFileExtensionException(string message) : base(message) { }

  /// <summary>
  /// Creates a new instance.
  /// </summary>
  public InvalidFileExtensionException(string message, Exception innerException) : base(message, innerException) { }

  /// <summary>
  /// Creates a new instance.
  /// </summary>
  public InvalidFileExtensionException() { }

  /// <summary>
  /// Not implemented
  /// </summary>
  protected InvalidFileExtensionException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
  {
    throw new NotImplementedException();
  }
}
