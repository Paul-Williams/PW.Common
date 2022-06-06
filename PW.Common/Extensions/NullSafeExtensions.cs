using System;

namespace PW.Extensions;

/// <summary>
/// Extension methods for use with objects.
/// </summary>
public static class NullSafeExtensions
{

  /// <summary>
  /// If <paramref name="o"/> is not null, performs <paramref name="action"/> using <paramref name="o"/> and returns true. Otherwise returns false.
  /// </summary>
  public static bool IfNotNull<T>(this T? o, Action<T> action) where T : class
  {
    if (o is null) return false;
    if (action is null)
      throw new ArgumentNullException(nameof(action));
    action(o);
    return true;
  }

  /// <summary>
  /// If <paramref name="o"/> is not null, performs <paramref name="action"/> using <paramref name="o"/> and returns true. Otherwise returns false.
  /// </summary>
  public static bool IfNotNull<T>(this T o, Action action) where T : class
  {
    if (o is null) return false;
    if (action is null)
      throw new ArgumentNullException(nameof(action));
    action();
    return true;
  }


  /// <summary>
  /// If <paramref name="o"/> is not null, returns the result of <paramref name="func"/>, otherwise returns default.
  /// </summary>
  /// <typeparam name="T">Input type of <paramref name="func"/></typeparam>
  /// <typeparam name="TR">Return type of <paramref name="func"/></typeparam>
  /// <param name="o">Object passed to <paramref name="func"/></param>
  /// <param name="func">Function to perform on <paramref name="o"/></param>
  /// <returns>Result of <paramref name="func"/>, or default</returns>
  public static TR? IfNotNull<T, TR>(this T? o, Func<T, TR?> func) where T : class where TR : class => 
    func is null ? throw new ArgumentNullException(nameof(func)) : o != null ? func(o) : default;
}
