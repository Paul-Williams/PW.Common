namespace PW.IO;

using PW.FailFast;
using System;
using System.IO;
using static PW.IO.Paths;

#if NET48
using static PW.Extensions.StringExtensions;
#endif

/// <summary>
/// Extension methods for the <see cref="FileInfo"/> class.
/// </summary>
public static partial class FileInfoExtensions

{

  /// <summary>
  /// Returns a new <see cref="FileInfo"/> instance with the file extension changed.
  /// </summary>
  public static FileInfo ChangeExtension(this FileInfo file, string newExtension)
  {
    return file is null
        ? throw new ArgumentNullException(nameof(file))
        : newExtension is null
        ? throw new ArgumentNullException(nameof(newExtension))
        : new FileInfo(Path.Combine(file.DirectoryName!, Path.GetFileNameWithoutExtension(file.FullName) + newExtension));
  }

  /// <summary>
  /// Returns a new <see cref="FileInfo"/> instance with the directory path changed.
  /// </summary>
  public static FileInfo ChangeDirectory(this FileInfo file, string newDirectory)
  {
    return file is null
      ? throw new ArgumentNullException(nameof(file))
      : newDirectory is null
        ? throw new ArgumentNullException(nameof(newDirectory))
        : new FileInfo(Path.Combine(newDirectory, Path.GetFileName(file.Name)));
  }

  /// <summary>
  /// Returns a new <see cref="FileInfo"/> instance with the directory path changed.
  /// </summary>
  public static FileInfo ChangeDirectory(this FileInfo file, DirectoryInfo newDirectory)
  {
    return file is null ? throw new ArgumentNullException(nameof(file)) : newDirectory is null ? throw new ArgumentNullException(nameof(newDirectory)) : new FileInfo(Path.Combine(newDirectory.FullName, Path.GetFileName(file.Name)));
  }


  // This is not required, as it is already implemented on FileSystemObject
  ///// <summary>
  ///// Returns the file's extension.
  ///// </summary>
  //public static string Extension(this FileInfo file) => 
  //  file != null ?  Path.GetExtension(file.FullName) : throw new ArgumentNullException(nameof(file));

  /// <summary>
  /// Deletes the file to the recycle bin.
  /// </summary>    
  public static void SendToRecycleBin(this FileInfo file!!)
  {

    Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(file.FullName,
      Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs,
      Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin);
  }

  /// <summary>
  /// Opens explorer and selects the specified file.
  /// </summary>
  /// <param name="file"></param>
  public static void SelectInExplorer(this FileInfo file!!)
  {
    Guard.MustExist(file, nameof(file));
    System.Diagnostics.Process.Start("explorer.exe", "/select, \"" + file.FullName + "\"");
  }

  /// <summary>
  /// Returns the file name without extension.
  /// </summary>
  /// <param name="file"></param>
  /// <returns></returns>
  public static string NameWithoutExtension(this FileInfo file!!) => Path.GetFileNameWithoutExtension(file.Name);


  /// <summary>
  /// Changes the name of the file to that specified by <paramref name="newName"/>
  /// </summary>
  public static void Rename(this FileInfo file!!, string newName!!)
    => file.MoveTo(Path.Combine(file.DirectoryName ?? throw new Exception($"{nameof(file)}.DirectoryName returned null."), newName));

  /// <summary>
  /// Checks whether the <see cref="FileInfo"/> has the specified extension.
  /// </summary>
  public static bool HasExtensionOf(this FileInfo file!!, string extension!!)
    => string.Equals(file.Extension, NormalizeFileExtension(extension), StringComparison.OrdinalIgnoreCase);

  /// <summary>
  /// Returns true if the file has an extension.
  /// </summary>
  public static bool HasExtension(this FileInfo file!!) => file.Extension.Length != 0;


  ///// <summary>
  ///// Creates a file rename operation that can be performed later.
  ///// </summary>
  ///// <param name="file">The file to be renamed.</param>
  ///// <param name="newName">The new name for the file.</param>
  ///// <returns>An operation that can later be performed.</returns>
  //public static FileRenameOperation CreateRenameOperation(this FileInfo file, string newName) => new(file, newName);

  /// <summary>
  /// Moves the file to a new directory. The file name will remain the same. The directory will be created if it does not exist.
  /// To both move and rename a file use  <see cref="MoveTo(FileInfo, FileInfo)"/> or <see cref="FileInfo.MoveTo(string)"/>
  /// </summary>
  public static void MoveTo(this FileInfo file!!, DirectoryInfo newLocation!!)
  {
    newLocation.Create();
    var newFile = new FileInfo(Path.Combine(newLocation.FullName, file.Name));
    file.MoveTo(newFile.FullName);
  }

  /// <summary>
  /// Moves the file new a new location.
  /// </summary>
  public static void MoveTo(this FileInfo file!!, FileInfo newLocation!!)
    => file.MoveTo(newLocation?.Directory ?? throw new Exception($"{nameof(newLocation)}.Directory returned null."));
}
