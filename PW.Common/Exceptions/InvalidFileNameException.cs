

using System;

namespace PW.Exceptions
{
  /// <summary>
  /// File name is invalid.
  /// </summary>
  [Serializable]
  public class InvalidFileNameException : Exception
  {
    /// <summary>
    /// Creates a new instance.
    /// </summary>
    /// <param name="message"></param>
    public InvalidFileNameException(string message) : base(message)
    {
    }

    /// <summary>
    /// Creates a new instance.
    /// </summary>
    public InvalidFileNameException(string message, Exception innerException) : base(message, innerException)
    {
    }

    /// <summary>
    /// Creates a new instance.
    /// </summary>
    public InvalidFileNameException()
    {
    }

    /// <summary>
    /// Not implemented
    /// </summary>
    protected InvalidFileNameException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
    {
      throw new NotImplementedException();
    }
  }
}
