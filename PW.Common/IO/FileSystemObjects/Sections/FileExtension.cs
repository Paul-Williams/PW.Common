namespace PW.IO.FileSystemObjects;

/// <summary>
/// Represents a file's extension. Instances of this class are cached. The same instance will always be returned for a particular extension.
/// </summary>
public class FileExtension : FileSystemPathSection<FileExtension>
{
  #region Cache

  /// <summary>
  /// A cache mapping string -> FileExtension. 
  /// String comparison is deliberately NOT case-insensitive. [ED: WHY?!]
  /// Extensions should be converted to lower-case before caching.
  /// </summary>
  private static Dictionary<string, FileExtension> Cache { get; } = new Dictionary<string, FileExtension>(StringComparer.Ordinal);

  private static readonly object CacheLock = new();

  private static FileExtension GetInstance(string extension)
  {
    // Save case-insensitive string comparison by always dealing with lower case.
    extension = extension.ToLower();

    // Lock the cache to prevent possibility of race condition between TryGetValue() and Add().
    lock (CacheLock)
    {
      // Just return the existing instance, if there is one.
      if (Cache.TryGetValue(extension, out var fileExtension)) return fileExtension;

      // Otherwise create, cache and return a new instance.
      fileExtension = new FileExtension(extension);
      Cache.Add(extension, fileExtension);
      return fileExtension;
    };
  }

  #endregion

  /// <summary>
  /// Creates a new instance. NB: value is not validated. This MUST be done with the factory methods.
  /// </summary>
  /// <param name="value"></param>
  private FileExtension(string value)
  {
    Path = value;
  }


  #region Factory Methods

  /// <summary>
  /// Cannot be null. Cannot be single character. Can be empty. 
  /// When not empty, the first character must be a period and the remaining characters must include at least one other non-white-space, non-period character.
  /// </summary>
  /// <param name="value"></param>
  public static FileExtension From(string value)
  {
    Validate.FileExtension(value);
    return GetInstance(value);
  }

  /// <summary>
  /// Creates an instance from an existing <see cref="FilePath"/> object.
  /// </summary>
  public static FileExtension From(FilePath filePath) =>
    filePath is null ? throw new ArgumentNullException(nameof(filePath)) 
    : GetInstance(System.IO.Path.GetExtension((string)filePath));

  /// <summary>
  /// Creates an instance from an existing <see cref="FilePath"/> object.
  /// </summary>
  public static FileExtension From(FileName fileName) =>
    fileName is null ? throw new ArgumentNullException(nameof(fileName)) 
    : GetInstance(System.IO.Path.GetExtension((string)fileName));

  /// <summary>
  /// Creates an instance from an existing <see cref="FileInfo"/> object.
  /// </summary>
  public static FileExtension From(FileInfo fileInfo) =>
    fileInfo is null ? throw new ArgumentNullException(nameof(fileInfo)) 
    : GetInstance(fileInfo.Extension);

  #endregion


  #region Explicit casts

  /// <summary>
  /// Casts a <see cref="String"/> to a <see cref="FileExtension"/>.
  /// </summary>    
  public static explicit operator FileExtension(string fileNameOrPath) => From(fileNameOrPath);

  /// <summary>
  /// Casts a <see cref="FilePath"/> to a <see cref="FileExtension"/>.
  /// </summary>  
  public static explicit operator FileExtension(FilePath filePath) => From(filePath);

  /// <summary>
  /// Casts a <see cref="FileInfo"/> to a <see cref="FileExtension"/>.
  /// </summary>  
  public static explicit operator FileExtension(FileInfo fileInfo) => From(fileInfo);

  /// <summary>
  /// Casts a file name to a file extension.
  /// </summary>    
  public static explicit operator FileExtension(FileName fileName) =>
    fileName is not null ? From(fileName) : throw new ArgumentNullException(nameof(fileName));

  /// <summary>
  /// Casts a file extension to a string.
  /// </summary>    
  public static explicit operator string(FileExtension fileExtension) =>
    fileExtension is not null ? fileExtension.Path : throw new ArgumentNullException(nameof(fileExtension));


  #endregion


  /// <summary>
  /// Creates a file mask for this file type. E.g. passing '*' as <paramref name="filenameMask"/> will return '*.extension'
  /// </summary>
  public string CreateMask(string filenameMask) => filenameMask + Path;

  /// <summary>
  /// Creates a file mask for all files with this extension.
  /// </summary>
  public string CreateMask() => "*" + Path;


}
