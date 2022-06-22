namespace PW.IO.FileSystemObjects;

/// <summary>
/// Enables polymorphism for classes that inherit from <see cref="FileSystemPath"/>
/// </summary>
public interface IFileSystemPath : IReadOnlyValue<string>, IComparable<IFileSystemPath>, IEquatable<IFileSystemPath>
{
  /// <summary>
  /// Determines whether the path exists.
  /// </summary>
  public bool Exists { get; }

  /// <summary>
  /// A path to a file system object.
  /// </summary>
  string Path { get; }

}



