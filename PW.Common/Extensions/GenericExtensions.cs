namespace PW.Extensions
{
  /// <summary>
  /// Generic extension methods.
  /// </summary>
  public static class GenericExtensions
  {
    /// <summary>
    /// Switches (toggles) the value <paramref name="current"/> <typeparamref name="T"/> between two values of <paramref name="one"/> and <paramref name="other"/>
    /// </summary>
    public static T Toggle<T>(this T current, T one, T other) => (Equals(current, one)) ? other : one;


  }
}
