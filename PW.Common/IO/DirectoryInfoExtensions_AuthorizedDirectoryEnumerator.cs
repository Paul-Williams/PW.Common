 

using System;
using System.Collections.Generic;
using System.IO;
using PW.FailFast;

namespace PW.IO
{
  /// <summary>
  /// Extensions for the <see cref="DirectoryInfo"/> class.
  /// </summary>
  public static partial class DirectoryInfoExtensions
  {

    /// <summary>
    /// Enumerates all subdirectories to which access is authorized.
    /// </summary>
    public static IEnumerable<DirectoryInfo> EnumerateAuthorizedDirectories(this DirectoryInfo startDirectory)
    {
      //Guard.NotNull(startDirectory, nameof(startDirectory));

      //// We will not catch UnauthorizedAccessException on the initial directory
      //// The initial directory will not be returned. It is not a subdirectory of itself!

      //var subDirectories = startDirectory.GetDirectories();

      //// If we got here then the start directory is accessible
      //foreach (var subDirectory in subDirectories)
      //  foreach (var nextDirectory in EnumerateInternal(subDirectory))
      //    yield return nextDirectory;

      return EnumerateAuthorizedDirectories(startDirectory, false);

    }

    /// <summary>
    /// Enumerates all subdirectories to which access is authorized.
    /// </summary>
    public static IEnumerable<DirectoryInfo> EnumerateAuthorizedDirectories(this DirectoryInfo startDirectory!!, bool includeStartDirectory)
    {
      // We will not catch UnauthorizedAccessException on the initial directory
      // The initial directory will not be returned. It is not a subdirectory of itself!

      if (includeStartDirectory) yield return startDirectory;

      var subDirectories = startDirectory.GetDirectories();

      // If we got here then the start directory is accessible
      foreach (var subDirectory in subDirectories)
        foreach (var nextDirectory in EnumerateInternal(subDirectory))
          yield return nextDirectory;

    }




    private static IEnumerable<DirectoryInfo> EnumerateInternal(DirectoryInfo currentDirectory)
    {

      DirectoryInfo[]? subDirectories = null;

      try
      {
        subDirectories = currentDirectory.GetDirectories();
      }
      catch (UnauthorizedAccessException) { }

      // If we got a list of subdirectories, then the current directory can be accessed, 
      // so return it and continue down the tree
      if (subDirectories != null)
      {
        yield return currentDirectory;

        foreach (var subDirectory in subDirectories)
          foreach (var nextDirectory in EnumerateInternal(subDirectory))
            yield return nextDirectory;
      }
    }

  }
}
