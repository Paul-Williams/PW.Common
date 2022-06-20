namespace PW.IO.FileSystemObjects;

/// <summary>
/// Base class for objects representing a file system path. E.g. DirectoryPath or FilePath.
/// </summary>
public abstract class FileSystemPathSection<T> : IComparable, IComparable<FileSystemPathSection<T>>, IEquatable<FileSystemPathSection<T>>, IReadOnlyValue<string>
{
  private string? _value;


  /// <summary>
  /// Returns the value contained by this instance.
  /// </summary>
  public string Path
  {
    get => _value ?? throw new InvalidOperationException($"Implementation error: {nameof(Path)} was not set by the inherited class.");
    protected set
    {
      if (_value is not null) throw new InvalidOperationException($"{nameof(Path)} cannot be changed once set.");
      if (value is null) throw new ArgumentNullException(nameof(value), $"{nameof(Path)} cannot be null.");
      //if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException($"{nameof(Value)} cannot be empty or white-space.", nameof(value));
      _value = value;
    }
  }

  /// <summary>
  /// Performs equality comparison of the two instances
  /// </summary>
  public override bool Equals(object? obj) =>
    Paths.EqualityComparer.Equals(Path, (obj as FileSystemPathSection<T>)?.Path);

  // See: https://stackoverflow.com/questions/11475737/gethashcode-for-ordinalignorecase-dependent-string-classes

  /// <summary>
  /// Returns hash code.
  /// </summary>
  /// <returns></returns>
  public override int GetHashCode() => Paths.EqualityComparer.GetHashCode(Path);


  /// <summary>
  /// This instance's path as a string.
  /// </summary>
  public override string ToString() => Path;

  #region IEquatable<FileSystemPathSection>

  /// <summary>
  /// Performs equality comparison.
  /// </summary>
  /// <param name="other"></param>
  /// <returns></returns>
  public bool Equals(FileSystemPathSection<T>? other) => Paths.EqualityComparer.Equals(Path, other?.Path);

  /// <summary>
  /// Compares two instances for sorting.
  /// </summary>
  /// <param name="other"></param>
  /// <returns></returns>
  public int CompareTo(FileSystemPathSection<T>? other) => Paths.NaturalSortComparer.Compare(Path, other?.Path);

  /// <summary>
  /// Compares two instances for sorting.
  /// </summary>
  /// <returns></returns>
  public int CompareTo(object? obj) => Paths.NaturalSortComparer.Compare(Path, (obj as FileSystemPathSection<T>)?.Path);



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
