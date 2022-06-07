namespace PW.ValueObjects;

/// <summary>
/// Represents a error message string. String is not validated.
/// </summary>
public class ErrorMessage : ValueOf<string, ErrorMessage>
{
  /// <summary>
  /// Returns an empty error message.
  /// </summary>
  public static ErrorMessage Empty { get; } = (ErrorMessage)string.Empty;

}

