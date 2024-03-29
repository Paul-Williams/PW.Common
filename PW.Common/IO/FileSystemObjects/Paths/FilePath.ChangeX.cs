﻿namespace PW.IO.FileSystemObjects;

/// <summary>
/// Instance methods for FilePath.ChangeX
/// </summary>
public partial class FilePath
{

  /// <summary>
  /// Creates a new <see cref="FilePath"/> using the original file name with a different path.
  /// </summary>
  public FilePath ChangeDirectory(DirectoryPath directory)
  {
    return directory is null
        ? throw new ArgumentNullException(nameof(directory))
        : new FilePath(System.IO.Path.Combine(directory, Name.Value));
  }

  /// <summary>
  /// Returns a new instance with the file extension changed. Does not change the file on disk.
  /// </summary>
  public FilePath ChangeExtension(FileExtension newExtension) =>
    newExtension is null ? throw new ArgumentNullException(nameof(newExtension))
    : new FilePathParts(this) { Extension = (string)newExtension }.ToFilePath();

  /// <summary>
  /// Returns a new instance with the file name changed. Does not change the file on disk.
  /// </summary>
  public FilePath ChangeName(FileName newName)
  {
    return newName is null
        ? throw new ArgumentNullException(nameof(newName))
        : new FilePathParts(this) { Name = (string)newName }.ToFilePath();
  }

  /// <summary>
  /// Creates a new <see cref="FileInfo"/> with the name changed, using a delegate function.
  /// </summary>
  public FilePath ChangeName(Func<string, string> f)
  {
    return f is null
      ? throw new ArgumentNullException(nameof(f))
      : ChangeName((FileName)f.Invoke(System.IO.Path.GetFileName(Path)));
  }


  /// <summary>
  /// Returns a new instance with the file name changed but with the same extension. Does not change the file on disk.
  /// </summary>
  public FilePath ChangeName(FileNameWithoutExtension newName)
  {
    return newName is null
        ? throw new ArgumentNullException(nameof(newName))
        : new FilePathParts(this) { FileNameWithoutExtension = (string)newName }.ToFilePath();
  }

}

