using System.Diagnostics;

namespace PW.IO;

/// <summary>
/// Extension methods for <see cref="FileSystemInfo"/> instances.
/// </summary>
public static class FileSystemInfoExtensions
{
  /// <summary>
  /// Deletes the <see cref="FileSystemInfo"/> entry from disk, if it exists. Otherwise does nothing.
  /// </summary>
  /// <param name="fso"></param>
  public static void DeleteIfExists(this FileSystemInfo fso)
  {
    if (fso.Exists) fso.Delete();
  }

  /// <summary>
  /// Launches the <see cref="FileSystemInfo"/> in a new process and returns the new process.
  /// </summary>
  public static Process Launch(this FileSystemInfo fso) => Process.Start(fso.FullName);

}
