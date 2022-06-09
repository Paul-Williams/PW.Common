namespace PW.IO;

public static class Paths
{

  /// <summary>
  /// Used for equality comparison.
  /// </summary>

  public const StringComparison EqualityComparison = StringComparison.OrdinalIgnoreCase;


  /// <summary>
  /// Used for equality comparison.
  /// </summary>
  public static StringComparer EqualityComparer { get; } = StringComparer.OrdinalIgnoreCase;

  public static IComparer<string> NaturalSortComparer { get; } = StringNaturalComparer.AscendingComparer;



  /// <summary>
  /// Unless empty, ensures it starts with a period.
  /// </summary>
  public static string NormalizeFileExtension(string extension!!)
    => extension.Length > 0 && !extension.StartsWith('.')
    ? '.' + extension
    : extension;


  /// <summary>
  /// Unless empty, ensures the directory path string is terminated with a <see cref="Path.DirectorySeparatorChar"/>
  /// </summary>
  public static string NormalizeDirectoryPath(string directoryPath!!)
    => directoryPath.Length > 0 && !directoryPath.EndsWith(Path.DirectorySeparatorChar)
    ? directoryPath + Path.DirectorySeparatorChar
    : directoryPath;

  /// <summary>
  /// Compares the two file paths using <see cref="EqualityComparison"/>.
  /// </summary>
  public static bool FilePathsAreEqual(string path1!!, string path2!!) => string.Equals(path1, path2, EqualityComparison);

  /// <summary>
  /// Compares the two file paths using <see cref="EqualityComparison"/>.
  /// </summary>
  public static bool FilePathsAreEqual(this FileInfo file!!, FileInfo other!!) => FilePathsAreEqual(file.FullName, other.FullName);

  /// <summary>
  /// Compares the two file paths using <see cref="EqualityComparison"/>.
  /// </summary>
  public static bool FilePathsAreEqual(this FileInfo file!!, string otherPath!!) => FilePathsAreEqual(file.FullName, otherPath);


  /// <summary>
  /// Tests if the directory paths are the same, other than in their casing. Before comparing, trailing back-slashes are normalized as required.
  /// Comparison is made using <see cref="EqualityComparison"/>
  /// </summary>
  public static bool DirectoryPathsMatch(string path1!!, string path2!!)
  {
    return path1.Length == 0 && path2.Length == 0
      || string.Equals(NormalizeDirectoryPath(path1), NormalizeDirectoryPath(path2), EqualityComparison);
  }
}
