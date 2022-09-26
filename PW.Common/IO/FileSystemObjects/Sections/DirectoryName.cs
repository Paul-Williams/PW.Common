namespace PW.IO.FileSystemObjects;

/// <summary>
/// Represents a single directory name, without the rest of the directory path.
/// </summary>
public class DirectoryName : FileSystemPathSection<DirectoryName>
{
  /// <summary>
  /// ctor -- Throws exceptions if invalid <paramref name="value"/> is passed.
  /// </summary>    
  public DirectoryName(string value) : base(value)
  {
    if (value is null)
      throw new ArgumentNullException(nameof(value), "Value cannot be null.");

    if (string.IsNullOrWhiteSpace(value))
      throw new ArgumentException("Value cannot be empty or white-space.", nameof(value));

    if (value.ContainsAny(System.IO.Path.GetInvalidFileNameChars()))
      throw new ArgumentException("Value contains invalid characters.", nameof(value));

  }

  /// <summary>
  /// Creates a new instance from an existing <see cref="DirectoryPath"/>. Skips validation.
  /// </summary>
  public DirectoryName(DirectoryPath directoryPath) : base(System.IO.Path.GetFileName(directoryPath.ToString(false)))
  {
    // ASSUMES: DirectoryPath values are always normalized to be terminated with a trailing back-slash.
    // This needs to be removed before calling Path.GetFileName(), otherwise an empty string will be returned.
  }

  /// <summary>
  /// Creates a new instance from an existing <see cref="FilePath"/>. Skips validation.
  /// </summary>
  public DirectoryName(FilePath filePath) : base(filePath.ToFileInfo().Directory!.Name) { }

  /// <summary>
  /// Casts a string to a <see cref="DirectoryName"/>.
  /// </summary> 
  public static explicit operator DirectoryName(string str) =>
    str != null ? new DirectoryName(str) : throw new ArgumentNullException(nameof(str));

  /// <summary>
  /// Casts a <see cref="DirectoryName"/> to a string.
  /// </summary>    
  public static explicit operator string(DirectoryName directoryName) =>
    directoryName?.Value ?? throw new ArgumentNullException(nameof(directoryName));

}
