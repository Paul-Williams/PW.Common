# nullable enable 

using System;

namespace PW.FailFast
{
  /// <summary>
  /// Add to methods that check input for null and throw if the input is null.
  /// Prevents 'CA1062: Validate arguments of public methods' warning for arguments.
  /// </summary>
  [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
  public sealed class ValidatedNotNullAttribute : Attribute { }
}
