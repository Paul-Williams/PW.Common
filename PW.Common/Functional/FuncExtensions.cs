using OneOf;
using PW.OptionTypes;
using System;

namespace PW.Functional;

/// <summary>
/// Extension methods for use with Func.
/// </summary>
public static class FuncExtensions
{

  #region Pipe


  // Lifted from: https://weblogs.asp.net/dixin/functional-csharp-function-composition-and-method-chaining

  //public static Func<T, T> Then<T>(this Func<T, T> func1, Func<T, T> func2) => value => func2(func1(value));

  //// function2 then function1
  //public static Func<T, TResult2> After<T, TResult1, TResult2>(this Func<TResult1, TResult2> function2, Func<T, TResult1> function1)
  //  => value => function2(function1(value));

  //// function1 then function2
  /// <summary>
  /// Composes (chains) two functions. E.g. val = f1.Then(f2);
  /// </summary>
  /// <typeparam name="T">Type of input to function <paramref name="f1"/></typeparam>
  /// <typeparam name="TR1">Return type of function <paramref name="f1"/> and input type of function <paramref name="f2"/></typeparam>
  /// <typeparam name="TR2">Return type of function <paramref name="f2"/></typeparam>
  /// <param name="f1">Function 1</param>
  /// <param name="f2">Function 2</param>
  /// <returns></returns>
  [Obsolete("Use Pipe<T1, T2, T3> instead.", false)]
  public static Func<T, TR2> Then<T, TR1, TR2>(this Func<T, TR1> f1, Func<TR1, TR2> f2)
    => value => f2(f1(value));


  // Don't think this is being used and it's meaning is quite confusing. Is it a reverse-pipe?

  ///// <summary>
  ///// Pass !
  ///// </summary>
  ///// <typeparam name="T"></typeparam>
  ///// <typeparam name="TR1"></typeparam>
  ///// <typeparam name="TR2"></typeparam>
  ///// <param name="f2"></param>
  ///// <param name="f1"></param>
  ///// <returns></returns>
  //public static Func<T, TR2> After<T, TR1, TR2>(this Func<TR1, TR2> f2, Func<T, TR1> f1)
  //  => value => f2(f1(value));

  /// <summary>
  /// Pipes the output of a function into the input of the next function.
  /// </summary>
  public static Func<T2> Pipe<T1, T2>(this Func<T1> f1, Func<T1, T2> f2)
    => () => f2(f1());

  /// <summary>
  /// Pipes the output of a function into the input of the next function.
  /// </summary>
  public static Func<T1, T3> Pipe<T1, T2, T3>(this Func<T1, T2> f1, Func<T2, T3> f2)
    => x => f2(f1(x));

  /// <summary>
  /// Pipes the output of a function into the input of the next function.
  /// </summary>
  public static Func<T1, T2, T4> Pipe<T1, T2, T3, T4>(this Func<T1, T2, T3> f1, Func<T3, T4> f2)
    => (x1, x2) => f2(f1(x1, x2));

  #endregion


  #region ValueOrDefault and OneOf


  /// <summary>
  /// Returns the result of <paramref name="func"/> or default, if it fails.
  /// </summary>
  public static T? ValueOrDefault<T>(this Func<T> func!!) where T : class
  {
    try { return func(); }
    catch { return default; }
  }

  /// <summary>
  /// Returns the result of <paramref name="func"/> or default, if it fails.
  /// </summary>
  public static TR? ValueOrDefault<T, TR>(this Func<T, TR> func!!, T arg) where TR : class
  {
    try { return func(arg); }
    catch { return default; }
  }


  /// <summary>
  /// Returns the result of <paramref name="func"/> or default, if it fails.
  /// </summary>
  public static TR? ValueOrDefault<T, TR>(T arg, Func<T, TR> func!!) where TR : class
  {
    try { return func(arg); }
    catch { return default; }
  }




  /// <summary>
  /// Returns <typeparamref name="T"/> or <see cref="OneOf.Types.None"/> or <see cref="Exception"/>
  /// If <paramref name="func"/> returns a value, this returns the value. 
  /// If <paramref name="func"/> returns null, this returns <see cref="OneOf.Types.None"/>. 
  /// If <paramref name="func"/> throws an exception, this returns <see cref="Exception"/>.
  /// </summary>
  public static ValueOrNoneOrException<T> ValueOrNoneOrException<T>(this Func<T> func!!)
  {
    try
    {

      return typeof(T).IsValueType
        // Checking for null-result does not make sense for value-type T
        ? func.Invoke()
        // For reference types, check for null and in that case return 'None'
        : func.Invoke() is T r ? r : new OneOf.Types.None();
    }
    catch (Exception ex)
    {
      return ex;
    }
  }

  /// <summary>
  /// Returns the value from <paramref name="func"/>, or <see cref="Exception"/>.
  /// </summary>
  public static ValueOrException<T> ValueOrException<T>(this Func<T> func!!)
  {
    try
    {
      return func.Invoke();
    }
    catch (Exception ex)
    {
      return ex;
    }
  }

  #endregion

}
