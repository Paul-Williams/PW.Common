namespace PW.IO.FileSystemObjects;

/// <summary>
/// Represents a path to a file on disk.
/// </summary>
[System.Diagnostics.DebuggerDisplay("{" + nameof(ToString) + "}")]

public partial class FilePath : FileSystemPath, IFilePath
{

  #region Constructors


  /// <summary>
  /// Creates a new instance using the specified file name or full path. 
  /// </summary>
  public FilePath(string value) : base(new FileInfo(value).FullName)
  {
    // This cheats by using the FileInfo constructor for some validation. 
    // Not too good for efficiency.
    if (value.EndsWith(System.IO.Path.PathSeparator)) throw new Exception("A file path cannot end with a directory seperator.");
  }

  /// <summary>
  /// Creates a new instance using an existing <see cref="FileInfo"/> object.
  /// </summary>    
  public FilePath(FileInfo file) : base(file.FullName) 
  {
    // In order to enable implicit convertion of FileSystemObject to string we explcitly disalow a FilePath to end with a slash.
    // Then even if a FilePath is compared to a DirectoryPath they should never match when implicitly converted to string.
    // This assumes that, when created, a DirectoryPath always has a trailing slash appended.
    if (file.FullName.EndsWith(System.IO.Path.PathSeparator)) throw new Exception("A file path cannot end with a directory seperator.");
  }

  #endregion

  #region Lazy Property cache fields
  private FileName? _fileName;
  private DirectoryPath? _directoryPath;
  private FileExtension? _fileExtension;
  private DirectoryName? _directoryName;
  private FileNameWithoutExtension? _fileNameWithoutExtension;
  #endregion

  #region Explicit Casts

  /// <summary>
  /// Casts a <see cref="string"/> to a <see cref="FilePath"/>.
  /// </summary>    
  public static explicit operator FilePath(string filePath) =>
    filePath is not null ? new FilePath(filePath) : throw new(nameof(filePath));

  /// <summary>
  /// Casts a <see cref="FilePath"/> to a <see cref="string"/>.
  /// </summary>    
  public static explicit operator string(FilePath filePath) =>
    filePath is not null ? filePath.Path : throw new(nameof(filePath));

  /// <summary>
  /// Casts a <see cref="FileInfo"/> to a <see cref="FilePath"/>.
  /// </summary>    
  public static explicit operator FilePath(FileInfo fileInfo) =>
    fileInfo is not null ? new FilePath(fileInfo) : throw new(nameof(fileInfo));

  /// <summary>
  /// Casts a <see cref="FilePath"/> to a <see cref="FileInfo"/>.
  /// </summary>    
  public static explicit operator FileInfo(FilePath filePath) =>
    filePath is not null ? new FileInfo(filePath.Path) : throw new(nameof(filePath));


  #endregion

  /// <summary>
  /// Creates a new instance of <see cref="FileInfo"/> for this <see cref="FilePath"/>.
  /// </summary>
  public FileInfo ToFileInfo() => new(Path);

  #region Instance methods to return parts of the FilePath
  /// <summary>
  /// Returns the file extension. Cached after first call.
  /// </summary>
  /// <returns></returns>
  public FileExtension Extension => _fileExtension is not null ? _fileExtension : _fileExtension = (FileExtension)this;

  /// <summary>
  /// Returns the directory path which contains the file. Cached after first call.
  /// </summary>
  public DirectoryPath DirectoryPath => _directoryPath is not null ? _directoryPath : _directoryPath = (DirectoryPath)this;

  /// <summary>
  /// Returns the directory name for the current instance. Cached after first call.
  /// </summary>
  public DirectoryName DirectoryName => _directoryName is not null ? _directoryName : _directoryName = new DirectoryName(this);

  /// <summary>
  /// Returns the file's name including the extension. Cached after first call.
  /// </summary>
  public FileName Name => _fileName is not null ? _fileName : _fileName = (FileName)this;

  /// <summary>
  /// Returns the file's name excluding the extension. Cached after first call.
  /// </summary>
  public FileNameWithoutExtension NameWithoutExtension => _fileNameWithoutExtension is not null
    ? _fileNameWithoutExtension : _fileNameWithoutExtension = (FileNameWithoutExtension)this;

  #endregion


  /// <summary>
  /// Determines whether the path refers to an existing file on disk. Not cached.
  /// </summary>
  public override bool Exists => File.Exists(Path);

}
