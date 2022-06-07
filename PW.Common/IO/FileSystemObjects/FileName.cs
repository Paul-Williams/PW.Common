namespace PW.IO.FileSystemObjects;

/// <summary>
/// Represents a file's name, without the directory path element.
/// </summary>
public class FileName : FileSystemPathSection<FileName>
{
  #region Constructors

  /// <summary>
  /// Creates a new instance from the specified string. Basic validation performed.
  /// </summary>
  public FileName(string value)
  {
    Validation.ValidateFileName(value);
    Value = value;
  }

  /// <summary>
  /// Creates a new instance from the existing <see cref="FilePath"/>.
  /// </summary>
  public FileName(FilePath file!!)
  {
    Value = Path.GetFileName((string)file);
  }

  /// <summary>
  /// Creates a new instance from the existing <see cref="FilePath"/>.
  /// </summary>
  public FileName(FileInfo file!!)
  {
    Value = file.Name;
  }

  #endregion

  #region Explicit Casts
  /// <summary>
  /// Casts a <see cref="string"/> to a <see cref="FileName"/>.
  /// </summary>    
  public static explicit operator FileName(string value) => value is null ? throw new ArgumentNullException(nameof(value)) : new FileName(value);

  /// <summary>
  /// Casts a <see cref="FilePath"/> to a <see cref="FileName"/>.
  /// </summary>
  public static explicit operator FileName(FilePath value) => value is null ? throw new ArgumentNullException(nameof(value)) : new FileName(value);

  /// <summary>
  /// Casts a <see cref="FilePath"/> to a <see cref="FileName"/>.
  /// </summary>
  public static explicit operator FileName(FileInfo value) => value is null ? throw new ArgumentNullException(nameof(value)) : new FileName(value);

  /// <summary>
  /// Casts a file name to a string.
  /// </summary>    
  public static explicit operator string(FileName fileName) => fileName is not null ? fileName.Value : throw new ArgumentNullException(nameof(fileName));

  #endregion

  #region Property cache fields
  private FileExtension? _extension;
  private FileNameWithoutExtension? _fileNameWithoutExtension;
  #endregion


  /// <summary>
  /// Returns the file name without extension. Cached after first use.
  /// </summary>
  public FileNameWithoutExtension WithoutExtension => _fileNameWithoutExtension is not null
    ? _fileNameWithoutExtension
    : _fileNameWithoutExtension = (FileNameWithoutExtension)this;


  /// <summary>
  /// Returns the file extension.
  /// </summary>
  public FileExtension Extension => _extension is not null
    ? _extension
    : _extension = (FileExtension)this;

  /// <summary>
  /// Creates a mask for all files of the same name but any extension.
  /// </summary>    
  public string CreateMask() => WithoutExtension.CreateMask();

}
