#nullable enable 

using PW.FailFast;
using PW.IO;
using System;
using System.IO;

namespace PW.IO
{

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
    {return file is null ? throw new ArgumentNullException(nameof(file)) : newDirectory is null ? throw new ArgumentNullException(nameof(newDirectory)) : new FileInfo(Path.Combine(newDirectory.FullName, Path.GetFileName(file.Name)));
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
    public static void SendToRecycleBin(this FileInfo file)
    {
      Guard.NotNull(file, nameof(file));

      Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(file.FullName,
        Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs,
        Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin);
    }

    /// <summary>
    /// Opens explorer and selects the specified file.
    /// </summary>
    /// <param name="file"></param>
    public static void SelectInExplorer(this FileInfo file)
    {
      Guard.NotNull(file, nameof(file));
      Assert.Exists(file);
      System.Diagnostics.Process.Start("explorer.exe", "/select, \"" + file.FullName + "\"");
    }

    /// <summary>
    /// Returns the file name without extension.
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public static string NameWithoutExtension(this FileInfo file)
    {
      Guard.NotNull(file, nameof(file));
      return Path.GetFileNameWithoutExtension(file.Name);
    }

    /// <summary>
    /// Changes the name of the file to that specified by <paramref name="newName"/>
    /// </summary>
    public static FileInfo Rename(this FileInfo file, string newName)
    {
      Guard.NotNull(file, nameof(file));
      Guard.NotNull(newName, nameof(newName));
      file.MoveTo(Path.Combine(file.DirectoryName!, newName)!);
      return file;
    }

    /// <summary>
    /// Checks whether the <see cref="FileInfo"/> has the specified extension. Include the dot!
    /// </summary>
    /// <param name="file">this</param>
    /// <param name="extension">The file extension, including the dot!</param>
    /// <returns></returns>
    public static bool HasExtension(this FileInfo file, string extension)
    {
      Guard.NotNull(file, nameof(file));
      Guard.NotNullOrWhitespace(extension, nameof(extension));

      return string.Equals(file.Extension, extension, StringComparison.OrdinalIgnoreCase);
    }


    /// <summary>
    /// Creates a file rename operation that can be performed later.
    /// </summary>
    /// <param name="file">The file to be renamed.</param>
    /// <param name="newName">The new name for the file.</param>
    /// <returns>An operation that can later be performed.</returns>
    public static FileRenameOperation CreateRenameOperation(this FileInfo file, string newName) => new(file, newName);

    /// <summary>
    /// Moves the file to a new directory. The file name will remain the same. The directory will be created if it does not exist.
    /// To both move and rename a file use  <see cref="MoveTo(FileInfo, FileInfo)"/> or <see cref="FileInfo.MoveTo(string)"/>
    /// </summary>
    public static void MoveTo(this FileInfo file, DirectoryInfo dir)
    {
      Guard.NotNull(file, nameof(file));
      Guard.NotNull(dir, nameof(dir));
      if (!dir.Exists) dir.Create();
      file.MoveTo(Path.Combine(dir.FullName, file.Name));
    }

    /// <summary>
    /// Moves the file new a new location.
    /// </summary>
    public static void MoveTo(this FileInfo file!!, FileInfo newLocation!!)
    {
      file.MoveTo(newLocation.FullName);
    }



    ///// <summary>
    ///// Returns a new <see cref="DirectoryInfo"/> instance for the directory containing the file.
    ///// </summary>
    //public static DirectoryInfo Directory(this FileInfo file) =>
    //  file != null ? new DirectoryInfo(file.DirectoryName) : throw new ArgumentNullException(nameof(file));

  }
}
