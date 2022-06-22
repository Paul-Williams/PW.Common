namespace PW.IO.FileSystemObjects;

/// <summary>
/// Replacement for <see cref="System.IO.Path"/> which returns 'FileSystemObjects' instead of strings.
/// </summary>
public static class PathElements
{
  #region System.IO.Path replacement

  /// <summary>
  /// Gets the FileName from a file path string
  /// </summary>
  public static FileName GetFileName(string filePath) => 
    string.IsNullOrWhiteSpace(filePath)
      ? throw new ArgumentException($"'{nameof(filePath)}' cannot be null or whitespace.", nameof(filePath))
      : (FileName)Path.GetFileName(filePath);

  public static FileNameWithoutExtension GetFileNameWithoutExtension(string filePath) => 
    string.IsNullOrWhiteSpace(filePath)
      ? throw new ArgumentException($"'{nameof(filePath)}' cannot be null or whitespace.", nameof(filePath))
      : (FileNameWithoutExtension)Path.GetFileNameWithoutExtension(filePath);

  public static FileExtension GetFileExtension(string filePath) => 
    string.IsNullOrWhiteSpace(filePath)
      ? throw new ArgumentException($"'{nameof(filePath)}' cannot be null or whitespace.", nameof(filePath))
      : (FileExtension)Path.GetExtension(filePath);

  public static DirectoryPath? GetDirectoryPath(string filePath) => 
    string.IsNullOrWhiteSpace(filePath)
      ? throw new ArgumentException($"'{nameof(filePath)}' cannot be null or whitespace.", nameof(filePath))
      : Path.GetDirectoryName(filePath) is string directoryPath
      ? (DirectoryPath)directoryPath
      : null;

  public static DirectoryName? GetDirectoryName(string filePath) => GetDirectoryPath(filePath)?.Name;


  public static DirectoryPath? GetPathRoot(string path)
  {
    return string.IsNullOrWhiteSpace(path)
      ? throw new ArgumentException($"'{nameof(path)}' cannot be null or whitespace.", nameof(path))
      : Path.GetPathRoot(path) is string root && !string.IsNullOrEmpty(root)
      ? (DirectoryPath)root
      : null;
  }

  #endregion
}
