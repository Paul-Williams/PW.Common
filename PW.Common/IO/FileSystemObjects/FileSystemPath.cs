

using PW.Collections;
using System;

namespace PW.IO.FileSystemObjects
{
  /// <summary>
  /// Base class for objects representing a file system path. E.g. DirectoryPath or FilePath.
  /// </summary>
  public abstract class FileSystemPath<T> : IComparable<FileSystemPath<T>>, IEquatable<FileSystemPath<T>>, IFileSystemPath
  {
    /// <summary>
    /// Creates a new instance.
    /// </summary>
    protected FileSystemPath() { }

    /// <summary>
    /// Default comparer for sorting.
    /// </summary>
    protected static StringNaturalComparer Comparer => StringNaturalComparer.AscendingComparer;

    private string? _path;

    /// <summary>
    /// The path encapsulated by this <see cref="FileSystemPath{T}"/>
    /// </summary>
    public string Value
    {
      get => _path ?? throw new InvalidOperationException($"Implementation error: {nameof(Value)} was not set by the inherited class.");
      protected set
      {
        if (_path is not null) throw new InvalidOperationException($"{nameof(Value)} cannot be changed once set.");
        if (value is null) throw new ArgumentNullException(nameof(value), $"{nameof(Value)} cannot be null.");
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException($"{nameof(Value)} cannot be empty or white-space.", nameof(value));
        _path = value;
      }
    }

    public abstract bool Exists { get; }

    /// <summary>
    /// Performs equality comparison.
    /// </summary>
    public override bool Equals(object? obj) => Comparer.Equals(this, obj as FileSystemPath<T>);


    private int _hashCode;

    // See: https://stackoverflow.com/questions/11475737/gethashcode-for-ordinalignorecase-dependent-string-classes
    /// <summary>
    /// Returns hash code. Cached after first call.
    /// </summary>

    public override int GetHashCode() => _hashCode != 0 ? _hashCode : _hashCode = Comparer.GetHashCode(this);


    /// <summary>
    /// This instance's path as a string.
    /// </summary>
    public override string ToString() => Value;

    #region IEquatable<FileSystemPath>

    /// <summary>
    /// Performs equality comparison.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(FileSystemPath<T>? other) => Comparer.Equals(this, other);

    /// <summary>
    /// Compares two instances for sorting.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int CompareTo(FileSystemPath<T>? other) => Comparer.Compare(this, other);

    /// <summary>
    /// Compares two instances for sorting.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int CompareTo(object other) => Comparer.Compare(this, other as FileSystemPath<T>);

    #endregion


    #region  Operator overloads

    /// <summary>
    /// Performs equality comparison of the two instances
    /// </summary>
    /// <returns></returns>
    public static bool operator ==(FileSystemPath<T> a, FileSystemPath<T> b) => Comparer.Equals(a, b);

    /// <summary>
    /// Performs negative-equality comparison of the two instances
    /// </summary>
    /// <returns></returns>
    public static bool operator !=(FileSystemPath<T> a, FileSystemPath<T> b) => !Comparer.Equals(a, b);

    /// <summary>
    /// 
    /// </summary>
    public static bool operator <(FileSystemPath<T> left, FileSystemPath<T> right) =>
      left is null ? right is not null : left.CompareTo(right) < 0;

    /// <summary>
    /// 
    /// </summary>
    public static bool operator <=(FileSystemPath<T> left, FileSystemPath<T> right) =>
      left is null || left.CompareTo(right) <= 0;

    /// <summary>
    /// 
    /// </summary>
    public static bool operator >(FileSystemPath<T> left, FileSystemPath<T> right) =>
      left is not null && left.CompareTo(right) > 0;

    /// <summary>
    /// 
    /// </summary>
    public static bool operator >=(FileSystemPath<T> left, FileSystemPath<T> right) =>
      left is null ? right is null : left.CompareTo(right) >= 0;

    #endregion


  }

}
