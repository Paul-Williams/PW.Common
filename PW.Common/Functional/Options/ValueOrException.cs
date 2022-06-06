using OneOf;
using System;

namespace PW.Functional.Options;

/// <summary>
/// Represents either a value or an exception.
/// </summary>
/// <typeparam name="TValue"></typeparam>
public class ValueOrException<TValue> : OneOfBase<TValue, Exception>
{
  /// <summary>
  /// Creates a new instance.
  /// </summary>
  /// <param name="input"></param>
  public ValueOrException(OneOf<TValue, Exception> input) : base(input)
  {
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="_"></param>
  public static implicit operator ValueOrException<TValue>(TValue _) => new(_);

  /// <summary>
  /// 
  /// </summary>
  /// <param name="_"></param>
  public static explicit operator TValue(ValueOrException<TValue> _) => _.AsT0;

  /// <summary>
  /// 
  /// </summary>
  /// <param name="_"></param>
  public static implicit operator ValueOrException<TValue>(Exception _) => new(_);

  /// <summary>
  /// 
  /// </summary>
  /// <param name="_"></param>
  public static explicit operator Exception(ValueOrException<TValue> _) => _.AsT1;

}

