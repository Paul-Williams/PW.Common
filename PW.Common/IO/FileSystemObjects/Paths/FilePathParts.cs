namespace PW.IO.FileSystemObjects;

/// <summary>
/// Separates a <see cref="FilePath"/> into its constituent parts.
/// </summary>
public class FilePathParts
{
  /// <summary>
  /// Creates a new instance.
  /// </summary>
  /// <param name="filePath"></param>
  public FilePathParts(FilePath filePath)
  {

    /* Unmerged change from project 'PW.Common (net48)'
    Before:
        Directory = Helpers.Paths.NormalizeDirectoryPath(Path.GetDirectoryName(filePath.Value)!);
    After:
        Directory = Paths.NormalizeDirectoryPath(Path.GetDirectoryName(filePath.Value)!);
    */
    Directory = IO.Paths.NormalizeDirectoryPath(Path.GetDirectoryName(filePath)!);
    FileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
    Extension = Path.GetExtension(filePath);

  }

  /// <summary>
  /// The directory path to the file.
  /// </summary>
  public string Directory { get; set; }

  /// <summary>
  /// The name of the file, excluding the extension.
  /// </summary>
  public string FileNameWithoutExtension { get; set; }

  /// <summary>
  /// The file's extension
  /// </summary>
  public string Extension { get; set; }

  public string Name
  {
    get => FileNameWithoutExtension + Extension;
    set
    {
      FileNameWithoutExtension = Path.GetFileNameWithoutExtension(value);
      Extension = Path.GetExtension(value);
    }
  }


  /// <summary>
  /// Reconstructs the parts back into a file path string.
  /// </summary>
  /// <returns></returns>
  public override string ToString() => Directory + FileNameWithoutExtension + Extension;

  /// <summary>
  /// Converts back to a <see cref="FilePath"/> object.
  /// </summary>
  /// <returns></returns>
  public FilePath ToFilePath() => (FilePath)ToString();


  //public static implicit operator FilePath (FilePathParts obj) => (FilePath)obj.ToString();

}

