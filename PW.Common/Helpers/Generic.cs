namespace PW.Helpers;

/// <summary>
/// Helper methods for working with generic types.
/// </summary>
public static class Generic
{
  /// <summary>
  /// Alternative to checking for null on generic types, where the type may or may not be a reference type.
  /// </summary>
  public static bool IsNull<T>(T value) =>
    !typeof(T).IsValueType && value is null;//EqualityComparer<T>.Default.Equals(value, default!);

  /// <summary>
  /// Determines whether two generic types are equal.
  /// </summary>
  public static bool Equals<T>(this T value1, T value2) => EqualityComparer<T>.Default.Equals(value1, value2);

  /// <summary>
  /// If <typeparamref name="T"/> is either a value type or non-null reference type it is simply returned.
  /// If <typeparamref name="T"/> is a null reference then <paramref name="factory"/> is called to create the exception to be thrown.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="value">Value to be null-checked</param>
  /// <param name="factory">Function to create exception message. Only called when <paramref name="value"/> is null.</param>
  /// <returns></returns>
  /// <exception cref="ArgumentNullException">Thrown when <paramref name="factory"/> is null.</exception>
  public static T OrIfNullThrow<T>([ValidatedNotNull] this T value, Func<Exception> factory) =>
    factory is null
    ? throw new ArgumentNullException(nameof(factory))
    : IsNull(value)
    ? throw factory()
    : value;

}
