using static PW.BackingField;

namespace PW.IO.FileSystemObjects;

/// <summary>
/// Represents a file system directory path.
/// </summary>
public class DirectoryPath : FileSystemPath
{
  #region Constructors

  //private DirectoryPath() { }

  /// <summary>
  /// Creates a new instance from a string. Basic validation performed. <see cref="Path.DirectorySeparatorChar"/> appended if missing.
  /// Supports relative paths. (e.g. . or ..) Path of just [Drive]: (e.g. C:) will return current directory for that drive.
  /// </summary>
  public DirectoryPath(string directoryPath) : base(Paths.NormalizeDirectoryPath(new DirectoryInfo(directoryPath).FullName))
  {
  }

  /// <summary>
  /// Creates an instance from an existing <see cref="DirectoryInfo"/> object. 
  /// Path validation skipped. <see cref="Path.DirectorySeparatorChar"/> appended if missing.
  /// </summary>    
  public DirectoryPath(DirectoryInfo directory) : base(Paths.NormalizeDirectoryPath(directory.FullName))
  {
  }

  /// <summary>
  /// Creates a new instance
  /// </summary>
  /// <param name="filePath"></param>
  public DirectoryPath(FilePath filePath): base (Paths.NormalizeDirectoryPath(System.IO.Path.GetDirectoryName(filePath.Path)!))
  {
  }

  #endregion

  #region Private Methods


  /// <summary>
  /// Creates a new instance from a string, without performing validation on the string.
  /// </summary>
  private static DirectoryPath FromStringInternal(string value)
  {
    return new(Paths.NormalizeDirectoryPath(System.IO.Path.GetDirectoryName(value)!));
  }

  #endregion


  #region Explicit Casts

  /// <summary>
  /// Casts a <see cref="String"/> to a <see cref="DirectoryPath"/>.
  /// </summary>    
  public static explicit operator DirectoryPath(string value) => new(value);

  /// <summary>
  /// Casts a <see cref="String"/> to a <see cref="DirectoryPath"/>.
  /// </summary>    
  public static explicit operator DirectoryPath(DirectoryInfo value) => new(value);

  /// <summary>
  /// Casts a <see cref="DirectoryPath"/> to a <see cref="string"/>.
  /// </summary>    
  public static explicit operator string(DirectoryPath value) =>
    value is not null ? value.Path : throw new ArgumentNullException(nameof(value));

  /// <summary>
  /// Casts a <see cref="DirectoryPath"/> to a <see cref="DirectoryInfo"/>.
  /// </summary>    
  public static explicit operator DirectoryInfo(DirectoryPath value) =>
    value is not null ? new DirectoryInfo(value.Path) : throw new ArgumentNullException(nameof(value));

  /// <summary>
  /// Casts a <see cref="FilePath"/> to a <see cref="DirectoryPath"/>.
  /// </summary>
  public static explicit operator DirectoryPath(FilePath filePath) =>
    filePath is not null ? new DirectoryPath(filePath) : throw new ArgumentNullException(nameof(filePath));

  #endregion

  #region Operator overloads

  /// <summary>
  /// Creates a FilePath from a DirectoryPath and FileName.
  /// </summary>
  public static FilePath operator +(DirectoryPath directoryPath, FileName fileName)
  {
    return directoryPath is null ? throw new ArgumentNullException(nameof(directoryPath)) : fileName is null ? throw new ArgumentNullException(nameof(fileName)) : (FilePath)(directoryPath.Path + fileName.Path);
  }

  /// <summary>
  /// Creates a DirectoryPath from a DirectoryPath and Sub-DirectoryName.
  /// </summary>
  public static DirectoryPath operator +(DirectoryPath directoryPath, DirectoryName directoryName)
  {
    return directoryPath is null
        ? throw new ArgumentNullException(nameof(directoryPath))
        : directoryName is null
          ? throw new ArgumentNullException(nameof(directoryName))
          : (DirectoryPath)(directoryPath.Path + directoryName.Path);
  }


  #endregion


  /// <summary>
  /// Creates a new <see cref="DirectoryInfo"/> object using this instance.
  /// </summary>
  /// <returns></returns>
  public DirectoryInfo ToDirectoryInfo() => (DirectoryInfo)this;


  #region Lazy Property Cache variables


  /// <summary>
  /// Cache variable for <see cref="Parent"/> property
  /// </summary>
  private DirectoryPath? _Parent;

  /// <summary>
  /// Cache variable for <see cref="DirectoryName"/> property
  /// </summary>
  private DirectoryName? _DirectoryName;


  #endregion


  #region Public Methods

  /// <summary>
  /// Returns a new <see cref="DirectoryPath"/> instance with the specified sub-directory name appended.
  /// </summary>
  public DirectoryPath Append(DirectoryName subDirectory)
  {
    return subDirectory is null
        ? throw new ArgumentNullException(nameof(subDirectory))
        : (DirectoryPath)(System.IO.Path.Combine(Path, subDirectory.ToString()));
  }

  /// <summary>
  /// Returns a new <see cref="DirectoryPath"/> instance with the specified file name appended.
  /// </summary>
  public FilePath Append(FileName file) =>
    file is null ? throw new ArgumentNullException(nameof(file)) : (FilePath)(Path + file.ToString());


  /// <summary>
  /// Returns a new <see cref="DirectoryPath"/> instance with the specified file name appended.
  /// </summary>
  public FilePath File(string file) => Append((FileName)file);


  /// <summary>
  /// Returns true if this directory is below the specified directory. It may be a direct sub-directory or further down the same path.
  /// </summary>
  public bool IsBelow(DirectoryPath directory)
  {
    return directory is null
        ? throw new ArgumentNullException(nameof(directory))
        : directory.Path.StartsWith(Path, StringComparison.OrdinalIgnoreCase);
  }

  /// <summary>
  /// Returns the parent directory or null if the directory does not have a parent. Value is cached after initial call.
  /// </summary>
  public DirectoryPath? Parent => GetLazy(ref _Parent, () => System.IO.Path.GetDirectoryName(Path) is string parent ? FromStringInternal(parent) : null);

  /// <summary>
  /// Returns the name of the last directory in the path. Value is cached after initial call.
  /// </summary>
  public DirectoryName Name => GetLazy(ref _DirectoryName, () => new DirectoryName(this))!;

  /// <summary>
  /// Returns the path as a string, specifying whether to include the terminating slash.
  /// </summary>
  /// <returns></returns>
  internal string ToString(bool includeTerminatingSlash) =>
    includeTerminatingSlash ? Path : Path[0..^1];





  #endregion


  #region Public Properties

  /// <summary>
  /// Determines whether the path refers to an existing directory on disk.
  /// </summary>
  public override bool Exists => Directory.Exists(Path);

  #endregion




}
