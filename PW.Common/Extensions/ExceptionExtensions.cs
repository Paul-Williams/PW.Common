using System;
using System.Collections.Generic;

namespace PW.Extensions;

/// <summary>
/// Various helper methods for use with <see cref="Exception"/> objects.
/// </summary>
public static class ExceptionExtensions
{

  /// <summary>
  /// Returns the exception and all inner-exceptions. To exclude the top-level exception, set <paramref name="includeTopLevel"/> to false.
  /// </summary>
  public static IEnumerable<Exception> EnumerateExceptions(this Exception ex, bool includeTopLevel = true)
  {
    if (includeTopLevel) yield return ex;

    while ((ex = ex?.InnerException!) is not null) 
      yield return ex;      
  }

  /// <summary>
  /// Returns all exception messages, going down the stack.
  /// </summary>
  public static IEnumerable<string> EnumerateMessages(this Exception ex, bool includeTopLevel = true)
  {
    foreach (var e in ex.EnumerateExceptions(includeTopLevel)) 
      yield return e.Message;
  }

}


