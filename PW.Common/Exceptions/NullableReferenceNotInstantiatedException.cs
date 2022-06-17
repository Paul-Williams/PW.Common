using System.Runtime.Serialization;

namespace PW.Exceptions;

/// <summary>
/// Thrown when attempting to access a nullable reference and finding it to be null.
/// </summary>
[Serializable]
public class NullableReferenceNotInstantiatedException : Exception
{
  private const string NullReferenceNameString = "<MISSING>";

  /// <summary>
  /// 
  /// </summary>
  /// <param name="referenceName"></param>
  public NullableReferenceNotInstantiatedException(string referenceName)
  {
    ReferenceName = referenceName ?? NullReferenceNameString;
  }

  /// <summary>
  /// 
  /// </summary>
  public string ReferenceName { get; } = NullReferenceNameString;

  /// <summary>
  /// 
  /// </summary>
  public override string Message => $"Nullable reference '{ReferenceName}' has not been instantiated.";

  /// <summary>
  /// 
  /// </summary>
  public NullableReferenceNotInstantiatedException()
  {
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="message"></param>
  /// <param name="innerException"></param>
  public NullableReferenceNotInstantiatedException(string message, Exception innerException) : base(message, innerException)
  {
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="info"></param>
  /// <param name="streamingContext"></param>
  protected NullableReferenceNotInstantiatedException(SerializationInfo info, StreamingContext streamingContext)
  {
    ReferenceName = info.GetString(nameof(ReferenceName)) ?? NullReferenceNameString;
    base.GetObjectData(info, streamingContext);
  }

  /// <summary>
  /// GetObjectData
  /// </summary>
  public override void GetObjectData(SerializationInfo info, StreamingContext context)
  {
    info.AddValue(nameof(ReferenceName), ReferenceName);
    base.GetObjectData(info, context);
  }
}
