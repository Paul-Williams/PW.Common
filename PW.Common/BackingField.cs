namespace PW;

/// <summary>
/// Helper class for use with backing field assignment.
/// </summary>
public static class BackingField
{
  /// <summary>
  /// Useful when setting backing fields. Atomically determine if the field needs updating and update it.
  /// Returns true if storage was updated with value.
  /// Assigns '<paramref name="value"/>' to '<paramref name="storage"/>', only if they are different.
  /// </summary>
  /// <typeparam name="T">The type of the two values.</typeparam>
  /// <param name="storage">Reference to the stored value.</param>
  /// <param name="value">The new value to be stored. </param>
  /// <returns>Returns <see langword="true"/> if the value was updated, otherwise <see langword="false"/>. </returns>
  public static bool AssignIfNotEqual<T>(this T value, ref T storage)
  {
    if (Helpers.Generic.Equals(storage, value)) return false;
    storage = value;
    return true;
  }

  /// <summary>
  /// Returns the value of the lazy-initialized variable '<paramref name="field"/>'. 
  /// If <paramref name="field"/> is currently null <paramref name="factory"/> is first called to obtain and set its value.
  /// For use with nullable reference types.
  /// </summary>
  /// <typeparam name="T">The data type of the reference-type."/> </typeparam>
  /// <param name="field">The value to be returned, or set then returned.</param>
  /// <param name="factory">A <see cref="Func{TResult}"/> used to populate <paramref name="field"/>, as required</param>
  /// <returns>The value <paramref name="field"/></returns>
  public static T? GetLazy<T>(ref T? field, Func<T?> factory) where T : class
    => field ??= factory();

  /// <summary>
  /// Returns the value of the lazy-initialized variable '<paramref name="field"/>'. 
  /// If <paramref name="field"/> has no value <paramref name="factory"/> is first called to obtain and set its value.
  /// For use with nullable value types.
  /// </summary>
  /// <typeparam name="T">The data type of the value-type field."/> </typeparam>
  /// <param name="field">The value to be returned, or set then returned.</param>
  /// <param name="factory">A <see cref="Func{TResult}"/> used to populate <paramref name="field"/>, as required</param>
  /// <returns>The value <paramref name="field"/></returns>
  public static T GetLazyValue<T>(ref T? field, Func<T> factory) where T : struct
  {
    if (!field.HasValue) field = factory();
    return field.Value;
  }
}
