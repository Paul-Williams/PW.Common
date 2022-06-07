namespace PW.ValueObjects;

/// <summary>
/// Represents a ok/error validation result.
/// </summary>
public class ValidationResult : ValueOf<(bool Ok, ErrorMessage Error), ValidationResult>
{
  /// <summary>
  /// Returns an 'OK' validation result.
  /// </summary>
  public static ValidationResult Ok { get; } = (ValidationResult)(true, ErrorMessage.Empty);

  /// <summary>
  /// Creates a failure validation result.
  /// </summary>
  public static ValidationResult Fail(ErrorMessage error) => (ValidationResult)(false, error);

  /// <summary>
  /// Returns true if the <see cref="ValidationResult"/> represents success.
  /// </summary>
  public bool IsSuccess => Value.Ok == true;

  /// <summary>
  /// Returns true if the <see cref="ValidationResult"/> represents failure.
  /// </summary>
  public bool IsFailure => Value.Ok == false;

  /// <summary>
  /// If the <see cref="ValidationResult"/> represents failure, returns reason, otherwise returns <see cref="ErrorMessage.Empty"/>.
  /// </summary>
  public ErrorMessage Error => Value.Error ?? ErrorMessage.Empty;

  /// <summary>
  /// Implicitly convert <see cref="ErrorMessage"/> into a new instance of <see cref="Fail(ErrorMessage)"/>
  /// </summary>
  public static implicit operator ValidationResult(ErrorMessage error) => Fail(error);

  /// <summary>
  /// 
  /// </summary>
  public static ValidationResult FromErrorMessage(ErrorMessage error) => Fail(error);

}

