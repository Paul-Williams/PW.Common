namespace PW.IO.FileSystemObjects;

/// <summary>
/// Base class for objects representing a file system path. E.g. DirectoryPath or FilePath.
/// </summary>
public abstract class FileSystemPath<T> : IComparable<FileSystemPath<T>>, IEquatable<FileSystemPath<T>>, IFileSystemPath
{
  /// <summary>
  /// Creates a new instance.
  /// </summary>
  protected FileSystemPath() { }

  private string? value;

  /// <summary>
  /// The path encapsulated by this <see cref="FileSystemPath{T}"/>
  /// </summary>
  public string Value
  {
    get => value ?? throw new InvalidOperationException($"Implementation error: {nameof(Value)} was not set by the inherited class.");
    protected set
    {
      if (this.value is not null) throw new InvalidOperationException($"{nameof(FileSystemPath<T>.Value)} cannot be changed once set.");
      if (value is null) throw new ArgumentNullException(nameof(value), $"{nameof(Value)} cannot be null.");
      if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException($"{nameof(Value)} cannot be empty or white-space.", nameof(value));
      this.value = value;
    }
  }

  public abstract bool Exists { get; }

  /// <summary>
  /// Performs equality comparison.
  /// </summary>
  public override bool Equals(object? obj) => Paths.EqualityComparer.Equals(Value, (obj as FileSystemPath<T>)?.Value);


  private int _hashCode;

  // See: https://stackoverflow.com/questions/11475737/gethashcode-for-ordinalignorecase-dependent-string-classes
  /// <summary>
  /// Returns hash code. Cached after first call.
  /// </summary>

  public override int GetHashCode() => _hashCode != 0 ? _hashCode : _hashCode = Paths.EqualityComparer.GetHashCode(Value);


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
  public bool Equals(FileSystemPath<T>? other) => Paths.EqualityComparer.Equals(Value, other?.Value);

  /// <summary>
  /// Compares two instances for sorting.
  /// </summary>
  /// <param name="other"></param>
  /// <returns></returns>
  public int CompareTo(FileSystemPath<T>? other) => Paths.NaturalSortComparer.Compare(Value, other?.Value);

  /// <summary>
  /// Compares two instances for sorting.
  /// </summary>
  /// <param name="other"></param>
  /// <returns></returns>
  public int CompareTo(object other) => Paths.NaturalSortComparer.Compare(Value, (other as FileSystemPath<T>)?.Value);

  #endregion


  #region  Operator overloads

  /// <summary>
  /// Performs equality comparison of the two instances
  /// </summary>
  /// <returns></returns>
  public static bool operator ==(FileSystemPath<T> a, FileSystemPath<T> b) => Paths.EqualityComparer.Equals(a?.value, b?.value);

  /// <summary>
  /// Performs negative-equality comparison of the two instances
  /// </summary>
  /// <returns></returns>
  public static bool operator !=(FileSystemPath<T> a, FileSystemPath<T> b) => !Paths.EqualityComparer.Equals(a?.value, b?.value);

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

