#nullable enable 

using PW.FailFast;
using System.Diagnostics;
using System.IO;

namespace PW.IO
{
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
      Guard.NotNull(fso, nameof(fso));
      if (fso.Exists) fso.Delete();
    }

    /// <summary>
    /// Launches the <see cref="FileSystemInfo"/> in a new process and returns the new process.
    /// </summary>
    public static Process Launch(this FileSystemInfo fso)
    {
      Guard.NotNull(fso, nameof(fso));
      Guard.MustExist(fso, nameof(fso));

      return Process.Start(fso.FullName);

    }

  }
}
