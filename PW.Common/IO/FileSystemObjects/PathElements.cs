using System.IO;

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
  public static FileName GetFileName(string filePath) => (FileName)Path.GetFileName(filePath);

  #endregion
}
