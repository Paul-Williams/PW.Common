


using PW.Helpers;
using System.CodeDom;

namespace PW
{

  /// <summary>
  /// Generic mapping class. Associates <typeparamref name="TFrom"/> with <typeparamref name="TTo"/>.
  /// </summary>
  /// <typeparam name="TFrom">Left, map from.</typeparam>
  /// <typeparam name="TTo">Right, map to.</typeparam>
  public struct Mapping<TFrom, TTo> : System.IEquatable<Mapping<TFrom, TTo>>
  {
    /// <summary>
    /// Map from.
    /// </summary>
    public TFrom From { get; }

    /// <summary>
    /// Map to.
    /// </summary>
    public TTo To { get; }

    /// <summary>
    /// Hashcode for this struct instance. NB: Can only precalculate when From and To are immutable.
    /// </summary>
    private int HashCode { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="from">Map from.</param>
    /// <param name="to">Map to.</param>
    public Mapping(TFrom from, TTo to)
    {
      From = from;
      To = to;
      HashCode = Misc.CreateHashcode(from!, to!);
    }

    /// <summary>
    /// Tests for equality.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj)
    {
      if (obj is null) return false;
      if (obj.GetType() != typeof(Mapping<TFrom, TTo>)) return false;
      var other = (Mapping<TFrom, TTo>)obj;
      return Equals(other);
    }

    /// <summary>
    /// Returns the has code.
    /// </summary>
    public override int GetHashCode() => HashCode;

    /// <summary>
    /// Test for equality.
    /// </summary>
    public static bool operator ==(Mapping<TFrom, TTo> left, Mapping<TFrom, TTo> right) => left.Equals(right);

    /// <summary>
    /// Test for inequality.
    /// </summary>
    public static bool operator !=(Mapping<TFrom, TTo> left, Mapping<TFrom, TTo> right) => !(left == right);

    /// <summary>
    /// Test for equality.
    /// </summary>
    public bool Equals(Mapping<TFrom, TTo> other) => Equals(other.From, From) && Equals(other.To, To);
  }
}
