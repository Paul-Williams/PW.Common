#nullable enable

using Microsoft.VisualBasic.FileIO;
using PW.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

// NB: SendFileToRecycleBin() will not work from non UI interactive apps like windows services.
// See: https://stackoverflow.com/questions/2342628/deleting-file-to-recycle-bin-on-windows-x64-in-c-sharp

namespace PW.IO.FileSystemObjects
{
  /// <summary>
  /// Static method for operating with file system directories and files.
  /// </summary>
  public static class FileSystem
  {
    #region Recycle bin methods

    /// <summary>
    /// Sends a file to the recycle bin.
    /// </summary>
    public static void SendFileToRecycleBin(string file) => Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(file, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);

    /// <summary>
    /// Sends a directory to the recycle bin.
    /// </summary>
    public static void SendDirectoryToRecycleBin(string directory) => Microsoft.VisualBasic.FileIO.FileSystem.DeleteDirectory(directory, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);

    /// <summary>
    /// Sends a file to the recycle bin.
    /// </summary>
    public static void SendToRecycleBin(this FilePath file!!)
    {
      Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(file.Value, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
    }

    /// <summary>
    /// Sends a directory to the recycle bin.
    /// </summary>
    public static void SendToRecycleBin(this DirectoryPath directory!!)
    {
      Microsoft.VisualBasic.FileIO.FileSystem.DeleteDirectory(directory.Value, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
    }

    #endregion


    #region FilePath methods

    /// <summary>
    /// Opens explorer and selects the specified file.
    /// </summary>
    /// <param name="filePath"></param>
    public static void SelectInExplorer(this FilePath filePath!!)
    {
      if (!filePath.Exists) throw new FileNotFoundException("File not found:" + (string)filePath);

      System.Diagnostics.Process.Start("explorer.exe", "/select, \"" + (string)filePath + "\"");
    }

    /// <summary>
    /// Moves a file on disk.
    /// </summary>
    public static FilePath Move(this FilePath file!!, DirectoryPath directory!!)
    {
      var r = file.ChangeDirectory(directory);
      File.Move(file.Value, r.Value);
      return r;
    }

    /// <summary>
    /// 'Unblocks' a file by deleting its associated 'Zone.Identifier' file. 
    /// Returns true on success or false if there was no associated zone identifier for the file. 
    /// </summary>
    /// <exception cref="FileNotFoundException">File specified file does not exist.</exception>
    /// <exception cref="ArgumentNullException">The file path was null.</exception>
    /// <exception cref="UnauthorizedAccessException">The caller does not have the required permission.</exception>
    ///<exception cref="IOException">The specified file is in use.</exception>
    ///<exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    public static bool Unblock(this FilePath file!!)
    {
      if (!file.Exists) throw ExceptionFactory.FileNotFoundException(file);

      var ziFile = file.Value + ":Zone.Identifier";
      if (!File.Exists(ziFile)) return false;
      File.Delete(ziFile);
      return true;
    }

    // // Found that DeleteFileW() is not required, as standard .Net objects can see :Zone.Identifier just fine. Doh!
    ///// <summary>
    ///// 'Unblocks' a file by deleting its associated 'Zone.Identifier' file. 
    ///// Returns true on success or false if there was no associated zone identifier for the file. 
    ///// </summary>
    ///// <exception cref="FileNotFoundException">File specified file does not exist.</exception>
    ///// <exception cref="ArgumentNullException">The file path was null.</exception>
    ///// <exception cref="System.ComponentModel.Win32Exception">Win32 returned an unexpected error.</exception>
    ///// <exception cref="UnauthorizedAccessException">Access was denied when attempting to delete the zone identifier.</exception>
    //public static bool Unblock(FilePath file)
    //{
    //  if (file is null) throw new ArgumentNullException(nameof(file));
    //  if (!Exists(file)) throw ExceptionFactory.FileNotFoundException(file);


    //  if (!Win32.DeleteFileW(file.Value + ":Zone.Identifier"))
    //  {
    //    var error = Win32Helper.GetLastWin32Error();

    //    // At this point, file not found refers to the zone identifier not being found rather than
    //    // the actual file. Therefore we will just ignore this and return false to denote that the file was not blocked.
    //    if (error == Win32Error.ERROR_FILE_NOT_FOUND) return false;

    //    // Potentially the zone identifier could be read-only ?
    //    else if (error == Win32Error.ERROR_ACCESS_DENIED) throw new UnauthorizedAccessException(Win32Helper.GetWin32ErrorMessage(error));

    //    // Other errors are unexpected, just pass along as Win32 error.
    //    else throw new System.ComponentModel.Win32Exception(error);

    //  }

    //  return true;
    //}


    /// <summary>
    /// Renames the file on disk and returns a new <see cref="FilePath"/> instance for the renamed file.
    /// </summary>
    public static FilePath Rename(this FilePath file!!, FileName newName!!)
    {
      if (!file.Exists) throw new FileNotFoundException("File not found: " + file.Value, file.Value);

      var newFilePath = file.ChangeName(newName);

      // This prevents changing the casing of a file name, as Exists() will return true.
      // if (Exists(newFilePath)) throw new Exception("File already exists: " + newFilePath.Value);

      File.Move(file.Value, newFilePath.Value);
      return newFilePath;
    }

    /// <summary>
    /// Moves the file on disk and returns the renamed file path.
    /// </summary>
    public static FilePath Move(this FilePath file!!, FilePath newFile!!)
    {

      // This prevents changing the casing of a file name, as Exists() will return true.
      // if (Exists(newFile)) throw new Exception("File already exists: " + newFile.Value);

      File.Move(file.Value, newFile.Value);
      return newFile;
    }




    #endregion


    #region DirectoryPath methods

    /// <summary>
    /// Returns Environment.SpecialFolder.ApplicationData\appName\
    /// </summary>
    public static DirectoryPath AppDataDirectory(string appName) =>
      (DirectoryPath)Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + (DirectoryName)appName;



    /// <summary>
    /// Wrapper: See <see cref="Environment.GetFolderPath(Environment.SpecialFolder)"/>
    /// </summary>
    public static DirectoryPath GetFolderPath(Environment.SpecialFolder folder) =>
      (DirectoryPath)Environment.GetFolderPath(folder);


    /// <summary>
    /// Wrapper: See <see cref="Environment.GetFolderPath(Environment.SpecialFolder, Environment.SpecialFolderOption)"/>
    /// </summary>
    public static DirectoryPath GetFolderPath(Environment.SpecialFolder folder, Environment.SpecialFolderOption option) =>
      (DirectoryPath)Environment.GetFolderPath(folder, option);


    /// <summary>
    /// Moves the directory on disk.
    /// </summary>
    public static DirectoryPath Move(this DirectoryPath path!!, DirectoryPath newPath!!)
    {
      var r = NativeMethods.SafeNativeMethods.MoveFile(path.Value, newPath.Value);

      return r == false ? throw new Win32Exception() : newPath;
    }

    /// <summary>
    /// Creates all directories and subdirectories in the specified path unless they already exist.
    /// </summary>
    [Obsolete("Use " + nameof(Create) + "' instead.")]
    public static DirectoryPath CreateIfNotExists(this DirectoryPath directory) => directory.Create();

    /// <summary>
    /// Creates all directories and subdirectories in the specified path, unless they already exist.
    /// </summary>
    public static DirectoryPath Create(this DirectoryPath directory!!)
    {
      _ = Directory.CreateDirectory(directory.Value);
      return directory;
    }



    /// <summary>
    /// Creates sub-directories within the existing directory. Skips any sub-directory that already exist.
    /// </summary>
    public static List<DirectoryPath> CreateSubdirectories(this DirectoryPath directory!!, IEnumerable<DirectoryName> subDirectories!!)
    {
      var newDirectoryPaths = subDirectories
        .Where(x => x is not null)  // Ensure no nulls
        .Distinct()               // Ensure no duplicates
        .Select(x => directory.Append(x))   // Create a new full path from this path + sub-directory name
        .ToList();                // Going to be used more than once.

      // Create any directories which do not already exist.
      newDirectoryPaths.Where(x => !x.Exists).ForEach(x => Directory.CreateDirectory(x.ToString()));

      return newDirectoryPaths;
    }

    /// <summary>
    /// Enumerates a directory on disk and returns a <see cref="FilePath"/> object for each file which matches <paramref name="searchPattern"/>.
    /// </summary>
    public static IEnumerable<FilePath> EnumerateFiles(this DirectoryPath directory!!, string searchPattern!!, System.IO.SearchOption searchOption)
    {
      if (!directory.Exists) throw new DirectoryNotFoundException("Directory not found: " + directory.Value);

      foreach (var file in directory.ToDirectoryInfo().EnumerateFiles(searchPattern, searchOption))
      {
        yield return (FilePath)file;
      }
    }

    /// <summary>
    /// Enumerates a directory on disk and returns a <see cref="DirectoryPath"/> object for each sub-directory which matches <paramref name="searchPattern"/>.
    /// </summary>
    public static IEnumerable<DirectoryPath> EnumerateDirectories(this DirectoryPath directory!!, string searchPattern, System.IO.SearchOption searchOption)
    {
      if (string.IsNullOrEmpty(searchPattern)) throw new ArgumentException("message", nameof(searchPattern));
      if (!directory.Exists) throw new DirectoryNotFoundException("Directory not found: " + directory.Value);

      foreach (var dir in directory.ToDirectoryInfo().EnumerateDirectories(searchPattern, searchOption))
      {
        yield return (DirectoryPath)dir;
      }

    }

    /// <summary>
    /// Enumerates a directory on disk and returns a <see cref="DirectoryPath"/> object for each sub-directory./>.
    /// </summary>
    public static IEnumerable<DirectoryPath> EnumerateDirectories(this DirectoryPath directory!!, System.IO.SearchOption searchOption)
    {
      if (!directory.Exists) throw new DirectoryNotFoundException("Directory not found: " + directory.Value);

      foreach (var dir in directory.ToDirectoryInfo().EnumerateDirectories("*", searchOption))
      {
        yield return (DirectoryPath)dir;
      }

    }


    /// <summary>
    /// Enumerates a directory on disk and returns a <see cref="FilePath"/> object for each file.
    /// </summary>
    public static IEnumerable<FilePath> EnumerateFiles(this DirectoryPath directory!!, System.IO.SearchOption searchOption)
    {
      if (!directory.Exists) throw new DirectoryNotFoundException("Directory not found: " + directory.Value);

      foreach (var file in directory.ToDirectoryInfo().EnumerateFiles("*.*", searchOption))
      {
        yield return (FilePath)file;
      }
    }

    /// <summary>
    /// Enumerates all files which match <paramref name="searchPattern"/>.
    /// </summary>
    public static IEnumerable<FilePath> EnumerateFiles(this DirectoryPath directory, string searchPattern) =>
      EnumerateFiles(directory, searchPattern, System.IO.SearchOption.TopDirectoryOnly);



    /// <summary>
    /// Enumerates all files with the specified extension.
    /// </summary>
    public static IEnumerable<FilePath> EnumerateFiles(this DirectoryPath directory, FileExtension ofType, System.IO.SearchOption searchOption) =>
      directory.EnumerateFiles(ofType.CreateMask(), searchOption);


    /// <summary>
    /// Enumerates a directory on disk and returns a <see cref="FilePath"/> object for each file.
    /// </summary>
    public static IEnumerable<FilePath> EnumerateFiles(this DirectoryPath directory!!)
    {
      if (!directory.Exists) throw new DirectoryNotFoundException("Directory not found: " + directory.Value);

      foreach (var file in directory.ToDirectoryInfo().EnumerateFiles())
      {
        yield return (FilePath)file;
      }
    }



    ///// <summary>
    ///// Enumerates files from the directory with image file extensions.
    ///// </summary>
    //public static IEnumerable<FilePath> EnumerateGdiSupportedImages(this DirectoryPath directory)
    //  => EnumerateGdiSupportedImages(directory, System.IO.SearchOption.TopDirectoryOnly);


    #endregion








  }
}
