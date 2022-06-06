using PW.Collections;
using PW.Interfaces;
using System;

namespace PW.IO.FileSystemObjects;

/// <summary>
/// Base class for objects representing a file system path. E.g. DirectoryPath or FilePath.
/// </summary>
public abstract class FileSystemPathSection<T> : IComparable, IComparable<FileSystemPathSection<T>>, IEquatable<FileSystemPathSection<T>>, IReadOnlyValue<string>
{
  //NB: Generic type T is used to ensure that only like sub-types can be compared.
  private static StringNaturalComparer Comparer => StringNaturalComparer.AscendingComparer;

  private string? _value;


  /// <summary>
  /// Returns the value contained by this instance.
  /// </summary>
  public string Value
  {
    get => _value ?? throw new InvalidOperationException($"Implementation error: {nameof(Value)} was not set by the inherited class.");
    protected set
    {
      if (_value is not null) throw new InvalidOperationException($"{nameof(Value)} cannot be changed once set.");
      if (value is null) throw new ArgumentNullException(nameof(value), $"{nameof(Value)} cannot be null.");
      //if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException($"{nameof(Value)} cannot be empty or white-space.", nameof(value));
      _value = value;
    }
  }

  /// <summary>
  /// Performs equality comparison of the two instances
  /// </summary>
  public override bool Equals(object? obj) =>
    Comparer.Equals(this, obj as FileSystemPathSection<T>);

  // See: https://stackoverflow.com/questions/11475737/gethashcode-for-ordinalignorecase-dependent-string-classes

    /// <summary>
    /// Returns hash code.
    /// </summary>
    /// <returns></returns>
  public override int GetHashCode() => Comparer.GetHashCode(this);


  /// <summary>
  /// This instance's path as a string.
  /// </summary>
  public override string ToString() => Value;

  #region IEquatable<FileSystemPathSection>

  /// <summary>
  /// Performs equality comparison.
  /// </summary>
  /// <param name="other"></param>
  /// <returns></returns>
  public bool Equals(FileSystemPathSection<T>? other) => Comparer.Equals(this, other);

  /// <summary>
  /// Compares two instances for sorting.
  /// </summary>
  /// <param name="other"></param>
  /// <returns></returns>
  public int CompareTo(FileSystemPathSection<T>? other) => Comparer.Compare(this, other);

  /// <summary>
  /// Compares two instances for sorting.
  /// </summary>
  /// <returns></returns>
  public int CompareTo(object? obj) => Comparer.Compare(this, obj as FileSystemPathSection<T>);



  #endregion

  #region  Operator overloads

  /// <summary>
  /// Performs equality comparison of the two instances
  /// </summary>
  /// <returns></returns>
  public static bool operator ==(FileSystemPathSection<T> a, FileSystemPathSection<T> b)
  {
    return a is null && b is null || (a is not null && b is not null && a.Equals(b));
  }

  /// <summary>
  /// Performs negative-equality comparison of the two instances
  /// </summary>
  /// <returns></returns>
  public static bool operator !=(FileSystemPathSection<T> a, FileSystemPathSection<T> b) => !(a == b);

  /// <summary>
  /// Less than operator.
  /// </summary>
  /// <param name="left"></param>
  /// <param name="right"></param>
  /// <returns></returns>
  public static bool operator <(FileSystemPathSection<T> left, FileSystemPathSection<T> right) => 
    left is null ? right is not null : left.CompareTo(right) < 0;

  /// <summary>
  /// Less than or equal operator.
  /// </summary>
  /// <param name="left"></param>
  /// <param name="right"></param>
  /// <returns></returns>
  public static bool operator <=(FileSystemPathSection<T> left, FileSystemPathSection<T> right) => 
    left is null || left.CompareTo(right) <= 0;

  /// <summary>
  /// Greater than operator.
  /// </summary>
  /// <param name="left"></param>
  /// <param name="right"></param>
  /// <returns></returns>
  public static bool operator >(FileSystemPathSection<T> left, FileSystemPathSection<T> right) => 
    left is not null && left.CompareTo(right) > 0;

  /// <summary>
  /// Greater than or equal operator.
  /// </summary>
  /// <param name="left"></param>
  /// <param name="right"></param>
  /// <returns></returns>
  public static bool operator >=(FileSystemPathSection<T> left, FileSystemPathSection<T> right) => 
    left is null ? right is null : left.CompareTo(right) >= 0;

  #endregion


}
