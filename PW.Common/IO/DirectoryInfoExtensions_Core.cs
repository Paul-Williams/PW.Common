 

using CSharpFunctionalExtensions;
using PW.Extensions;
using PW.FailFast;
using PW.IO.FileSystemObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using static CSharpFunctionalExtensions.Result;

namespace PW.IO
{
  public static partial class DirectoryInfoExtensions
  {
    /// <summary>
    /// Returns the named sub-directory, creates it if it does not exist.
    /// </summary>
    public static DirectoryInfo GetOrCreateSubdirectory(this DirectoryInfo directory!!, string subdirectoryName)
    {
      Guard.NotNullOrWhitespace(subdirectoryName, nameof(subdirectoryName));

      if (subdirectoryName.ContainsAny(Path.GetInvalidFileNameChars()))
        throw new ArgumentException("Sub-directory name contains invalid characters.", nameof(subdirectoryName));

      Guard.MustExist(directory, nameof(directory));

      return directory.GetDirectories(subdirectoryName).FirstOrDefault() ?? directory.CreateSubdirectory(subdirectoryName);
    }


    /// <summary>
    /// Returns a new <see cref="DirectoryInfo"/> instance with the sub-directory appended.
    /// </summary>
    public static DirectoryInfo Append(this DirectoryInfo directory, DirectoryName subDirectory)
    {
      return directory is null 
        ? throw new ArgumentNullException(nameof(directory)) 
        : subDirectory is null 
          ? throw new ArgumentNullException(nameof(subDirectory)) 
          : new DirectoryInfo(Path.Combine(directory.FullName, subDirectory.Value));
    }

    /// <summary>
    /// Returns a new <see cref="DirectoryInfo"/> instance with the sub-directory appended.
    /// </summary>
    public static DirectoryInfo Append(this DirectoryInfo directory, string subDirectory)
    {
      return directory is null 
        ? throw new ArgumentNullException(nameof(directory)) 
        : subDirectory is null 
          ? throw new ArgumentNullException(nameof(subDirectory)) 
          : new DirectoryInfo(Path.Combine(directory.FullName, subDirectory));
    }


    /// <summary>
    /// Returns a new <see cref="DirectoryInfo"/> instance with the sub-directories appended.
    /// If no arguments or a zero-length array are passed to <paramref name="subDirectories"/>, the original directory is returned.
    /// </summary>
    public static DirectoryInfo Append(this DirectoryInfo directory!!, params DirectoryName[] subDirectories!!)
    {


      // If no arguments are passed via 'params' the length of the params list is zero.
      // In which case, just return the original directory.
      if (subDirectories.Length == 0) return directory;

      // Ensure none of the sub-directory objects are null.
      Guard.NoNulls(subDirectories, nameof(subDirectories));

      var strings = subDirectories.Select(x => x.Value).Prepend(directory.FullName);
      return new DirectoryInfo(Path.Combine(strings.ToArray()));
    }

    /// <summary>
    /// Returns a new <see cref="DirectoryInfo"/> instance with the sub-directories appended.
    /// If no arguments or a zero-length array are passed to <paramref name="subDirectories"/>, the original directory is returned.
    /// </summary>
    public static DirectoryInfo Append(this DirectoryInfo directory!!, params string[] subDirectories!!)
    {

      // If no arguments are passed via 'params' the length of the params list is zero.
      // In which case, just return the original directory.
      if (subDirectories.Length == 0) return directory;

      // Ensure none of the sub-directory objects are null.
      Guard.NoNulls(subDirectories, nameof(subDirectories));

      var strings = subDirectories.Prepend(directory.FullName);
      return new DirectoryInfo(Path.Combine(strings.ToArray()));
    }


    /// <summary>
    /// Creates sub-directories within the existing directory. Skips any sub-directory that already exist.
    /// </summary>
    public static void CreateSubdirectories(this DirectoryInfo directory!!, IEnumerable<DirectoryName> subDirectories!!)
    {
      subDirectories
        .Distinct()
        .Select(subdirectory => Path.Combine(directory.FullName, subdirectory.Value))
        .Where(fullPath => !Directory.Exists(fullPath))
        .ForEach(fullPath => Directory.CreateDirectory(fullPath));
    }


    /// <summary>
    /// Walks up the directory path from this directory to the root. Returns each directory on that path.
    /// </summary>
    public static IEnumerable<DirectoryInfo> WalkPathToRoot(this DirectoryInfo directory!!, bool includeSelf = true)
    {
      var current = includeSelf ? directory : directory.Parent;

      while (current != null)
      {
        yield return current;
        current = current.Parent;
      }

    }

    /// <summary>
    /// Walks the path to this directory from its root. Returns each directory on that path.
    /// </summary>
    public static IEnumerable<DirectoryInfo> WalkPathFromRoot(this DirectoryInfo directory, bool includeSelf = true)
      => directory.WalkPathToRoot(includeSelf).Reverse();


    /// <summary>
    /// Creates the directory if it does not already exist. 
    /// </summary>
    /// <param name="directory"></param>
    /// <returns>The original directory, for fluent-chaining etc.
    /// An exception will be thrown if the directory cannot be created or is null.</returns>
    public static DirectoryInfo EnsureExists(this DirectoryInfo directory!!)
    {
      if (!directory.Exists) directory.Create();
      return directory;
    }


    /// <summary>
    /// Creates the directory if it does not already exist. 
    /// </summary>
    /// <param name="directory"></param>
    /// <returns>Returns (true, null), if the directory already exists or it is created successfully. 
    /// If an exception occurs creating the directory, returns (false,error) where 'error' is the exception message.</returns>
    /// <exception cref="ArgumentNullException"> If <paramref name="directory"/> is null.</exception>
    public static Result<DirectoryInfo> TryEnsureExists(this DirectoryInfo directory!!)
    {
      try
      {
        return Success(directory.EnsureExists());
      }
      catch (Exception ex)
      {
        return Failure<DirectoryInfo>($"Unable to create directory:{directory.FullName}\n {ex.Message}");
      }
    }

    /// <summary>
    /// Returns a list of files from the directory which have file extensions as specified by <paramref name="extensions"/>
    /// </summary>
    public static IEnumerable<FileInfo> GetFiles(this DirectoryInfo directory!!, IEnumerable<string> extensions!!, SearchOption searchOption)
    {

      var set = new HashSet<string>(extensions, StringComparer.OrdinalIgnoreCase);
      // TODO: Use - Enumerable.SelectMany(directory.EnumerateAuthorizedDirectories(true), x =>
      return directory.GetFiles("*.*", searchOption).Where(fileInfo => set.Contains(fileInfo.Extension));

    }

    //Enumerable.SelectMany(directory.EnumerateAuthorizedDirectories(true), x => x.EnumerateGdiSupportedImages(SearchOption.TopDirectoryOnly))

    /// <summary>
    /// Enumerates files from the directory which have file extensions as specified by <paramref name="extensions"/>
    /// </summary>
    public static IEnumerable<FileInfo> EnumerateFiles(this DirectoryInfo directory!!, IEnumerable<string> extensions!!, SearchOption searchOption)
    {
      var set = new HashSet<string>(extensions, StringComparer.OrdinalIgnoreCase);
      // TODO: Use - Enumerable.SelectMany(directory.EnumerateAuthorizedDirectories(true), x =>
      return directory.EnumerateFiles("*.*", searchOption).Where(fileInfo => set.Contains(fileInfo.Extension));
    }




    //private static IEnumerable<FileInfo> GetImageEnumerator() =>
    //  new DirectoryInfo(@"C:\Windows\Web\Screen\").EnumerateFiles(new GdiSupportedImageFileExtensions(), SearchOption.AllDirectories);

    /// <summary>
    /// Returns a new <seealso cref="DirectoryInfo"/> instance representing the named sub-directory. 
    /// Does not check if the directory or sub-directory exist or create them.
    /// </summary>
    /// <param name="directory">The parent directory</param>
    /// <param name="subDirectoryName">The short-name of the sub-directory. Not the full path to it!"</param>
    /// <returns></returns>
    public static DirectoryInfo SubDirectory(this DirectoryInfo directory, string subDirectoryName)
    {
      return directory is null
          ? throw new ArgumentNullException(nameof(directory))
          : subDirectoryName is null
            ? throw new ArgumentNullException(nameof(subDirectoryName))
            : new DirectoryInfo(Path.Combine(directory.FullName, subDirectoryName));
    }

    /// <summary>
    /// Recursively deletes all sub-directories that are either empty or only contain empty sub-directories.
    /// Returns an list of <see cref="DirectoryInfo"/> containing an entry for each directory deleted.
    /// If the initial directory does not exist, an empty list is returned.
    /// </summary>
    public static List<DirectoryInfo> RecursiveDeleteEmptySubDirectories(this DirectoryInfo initialDirectory!!)
    {
      var deletedDirectories = new List<DirectoryInfo>(); 
      if (!initialDirectory.Exists) return deletedDirectories;

      var subDirectories = initialDirectory.GetDirectories("*.*", SearchOption.AllDirectories);

      // Note that we delete as we go, rather than after examining all directories. 
      // This is so that 'lower' empty sub-directories are deleted first, resulting in 'higher' sub-directories 
      // becoming empty, when they previously only contained one or more empty sub-directories.
      // Reverse-looping the list SHOULD result in a bottom-up iteration of directories

      for (int i = subDirectories.Length - 1; i > -1; i--)
      {
        var subDirectory = subDirectories[i];
        if (subDirectory.GetFileSystemInfos("*.*").Length == 0)
        {
          subDirectory.Delete();
          deletedDirectories.Add(subDirectory);
        }
      }

      return deletedDirectories;
    }
  }

}
