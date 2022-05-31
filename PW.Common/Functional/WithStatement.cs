// Judgment: Using these would reduce clarity and performance. 
// It is also possible that 'Func<T> objectExpression' would be re-evaluated for each action
// Would need to test that.

using System;

namespace PW.Functional
{
  /// <summary>
  /// Extension methods equivalent to the Visual Basic 'With...End With' Statement block.
  /// </summary>
  [Obsolete("Will revert if used anywhere much.",false)] 
  public static class WithStatement
  {
    /// <summary>
    /// Equivalent to the Visual Basic 'With...End With' Statement block.
    /// </summary>
    /// <typeparam name="T">The type returned by <paramref name="objectExpression"/>.</typeparam>
    /// <param name="objectExpression">An expression that evaluates to an object of type <typeparamref name="T"/>. The expression may be arbitrarily complex and is evaluated only once. The expression can evaluate to any data type, including elementary types.</param>
    /// <param name="statements">One or more statements that refer to members of an object that's produced by the evaluation of objectExpression.</param>
    public static void With<T>(Func<T> objectExpression!!, Action<T> statements!!) => statements.Invoke(objectExpression());

    /// <summary>
    /// Equivalent to the Visual Basic 'With...End With' Statement block.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <param name="obj">An object of type <typeparamref name="T"/> against which statements are performed.</param>
    /// <param name="statements">One or more statements that refer to members of the object <paramref name="obj"/>.</param>
    [Obsolete("Will revert if used anywhere much.", false)]
    public static void With<T>(T obj!!, Action<T> statements!!) => statements.Invoke(obj);
  }
}
