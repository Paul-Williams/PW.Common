using OneOf;
using OneOf.Types;
using System;

namespace PW.OptionTypes;

/// <summary>
/// Represents either a value or an exception.
/// </summary>
/// <typeparam name="TValue"></typeparam>
public class ValueOrNoneOrException<TValue> : OneOfBase<TValue, None, Exception>
{
  /// <summary>
  /// Creates a new instance.
  /// </summary>
  /// <param name="input"></param>
  public ValueOrNoneOrException(OneOf<TValue, None, Exception> input) : base(input)
  {
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="_"></param>
  public static implicit operator ValueOrNoneOrException<TValue>(TValue _) => new(_);

  /// <summary>
  /// 
  /// </summary>
  /// <param name="_"></param>
  public static explicit operator TValue(ValueOrNoneOrException<TValue> _) => _.AsT0;

  /// <summary>
  /// 
  /// </summary>
  /// <param name="_"></param>
  public static implicit operator ValueOrNoneOrException<TValue>(None _) => new(_);

  /// <summary>
  /// 
  /// </summary>
  /// <param name="_"></param>
  public static explicit operator None(ValueOrNoneOrException<TValue> _) => _.AsT1;


  /// <summary>
  /// 
  /// </summary>
  /// <param name="_"></param>
  public static implicit operator ValueOrNoneOrException<TValue>(Exception _) => new(_);

  /// <summary>
  /// 
  /// </summary>
  /// <param name="_"></param>
  public static explicit operator Exception(ValueOrNoneOrException<TValue> _) => _.AsT2;

}

