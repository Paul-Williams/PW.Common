namespace PW.IO.FileSystemObjects;

/// <summary>
/// Base class for objects representing a file system path. E.g. DirectoryPath or FilePath.
/// </summary>
public class FileSystemPath : IFileSystemPath
{
  /// <summary>
  /// Creates a new instance.
  /// </summary>
  public FileSystemPath(string path)
  {
    if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException($"'{nameof(path)}' cannot be null or whitespace.", nameof(path));
    Path = path;
  }

  /// <summary>
  /// The path encapsulated by this <see cref="FileSystemPath"/>
  /// </summary>
  public string Path { get; }

  public virtual bool Exists => Directory.Exists(Path) || File.Exists(Path);

  string IReadOnlyValue<string>.Value => Path;


  /// <summary>
  /// Performs equality comparison.
  /// </summary>
  public override bool Equals(object? obj) => Paths.EqualityComparer.Equals(Path, (obj as FileSystemPath)?.Path);


  private int _hashCode;

  // See: https://stackoverflow.com/questions/11475737/gethashcode-for-ordinalignorecase-dependent-string-classes
  /// <summary>
  /// Returns hash code. Cached after first call.
  /// </summary>

  public override int GetHashCode() => _hashCode != 0 ? _hashCode : _hashCode = Paths.EqualityComparer.GetHashCode(Path);


  /// <summary>
  /// This instance's path as a string.
  /// </summary>
  public override string ToString() => Path;

  #region IEquatable<IFileSystemPath>

  /// <summary>
  /// Performs equality comparison.
  /// </summary>
  /// <param name="other"></param>
  /// <returns></returns>
  public bool Equals(IFileSystemPath? other) => Paths.EqualityComparer.Equals(Path, other?.Path);

  /// <summary>
  /// Compares two instances for sorting.
  /// </summary>
  /// <param name="other"></param>
  /// <returns></returns>
  public int CompareTo(IFileSystemPath? other) => Paths.NaturalSortComparer.Compare(Path, other?.Path);

  /// <summary>
  /// Compares two instances for sorting.
  /// </summary>
  /// <param name="other"></param>
  /// <returns></returns>
  public int CompareTo(object other) => Paths.NaturalSortComparer.Compare(Path, (other as FileSystemPath)?.Path);

  #endregion


  #region  Operator overloads

  /// <summary>
  /// Performs equality comparison of the two instances
  /// </summary>
  /// <returns></returns>
  public static bool operator ==(FileSystemPath a, FileSystemPath b) => Paths.EqualityComparer.Equals(a?.Path, b?.Path);

  /// <summary>
  /// Performs negative-equality comparison of the two instances
  /// </summary>
  /// <returns></returns>
  public static bool operator !=(FileSystemPath a, FileSystemPath b) => !Paths.EqualityComparer.Equals(a?.Path, b?.Path);

  /// <summary>
  /// 
  /// </summary>
  public static bool operator <(FileSystemPath left, FileSystemPath right) =>
    left is null ? right is not null : left.CompareTo(right) < 0;

  /// <summary>
  /// 
  /// </summary>
  public static bool operator <=(FileSystemPath left, FileSystemPath right) =>
    left is null || left.CompareTo(right) <= 0;

  /// <summary>
  /// 
  /// </summary>
  public static bool operator >(FileSystemPath left, FileSystemPath right) =>
    left is not null && left.CompareTo(right) > 0;

  /// <summary>
  /// 
  /// </summary>
  public static bool operator >=(FileSystemPath left, FileSystemPath right) =>
    left is null ? right is null : left.CompareTo(right) >= 0;


  public static implicit operator string(FileSystemPath path) => path.Path;

  #endregion


}

