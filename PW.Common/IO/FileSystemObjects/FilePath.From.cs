using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
  private static FilePath From(string filePath) => new() { Value = filePath };



  /// <summary>
  /// Creates a new <see cref="FilePath"/> instance.
  /// </summary>
  public static FilePath From(DirectoryPath directoryPath, FileName fileName) => 
    directoryPath is null ? throw new(nameof(directoryPath))
    : fileName is null ? throw new(nameof(fileName))
    : From(directoryPath.Value + fileName.Value);

  /// <summary>
  /// Creates a new <see cref="FilePath"/> instance.
  /// </summary>
  public static FilePath From(DirectoryPath directoryPath, FileNameWithoutExtension fileNameWithoutExtension, FileExtension fileExtension)    => 
    directoryPath is null ? throw new(nameof(directoryPath)) 
    : From(directoryPath.Value + fileNameWithoutExtension.Value + fileExtension.Value);

}
