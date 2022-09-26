namespace PW.IO.FileSystemObjects;

// Static methods for FilePath.From
public partial class FilePath
{

  /// <summary>
  /// Creates a new instance from the given <see cref="FileInfo"/> object.
  /// </summary>
  public static FilePath From(FileInfo file) =>
    file is null ? throw new System.ArgumentNullException(nameof(file))
    : new FilePath(file);


  /// <summary>
  /// Internal helper method to construct new instance from known good path-string. 
  /// I.e. one which has been created from combining existing FileSystemPath and FileSystemPathSection values.
  /// </summary>
  private static FilePath From(string filePath) => new(filePath);



  /// <summary>
  /// Creates a new <see cref="FilePath"/> instance.
  /// </summary>
  public static FilePath From(DirectoryPath directoryPath, FileName fileName) =>
    From(directoryPath.Path + fileName.Value);

  /// <summary>
  /// Creates a new <see cref="FilePath"/> instance.
  /// </summary>
  public static FilePath From(DirectoryPath directoryPath, FileNameWithoutExtension fileNameWithoutExtension, FileExtension fileExtension) =>
    From(directoryPath.Path + fileNameWithoutExtension.Value + fileExtension.Value);

}
