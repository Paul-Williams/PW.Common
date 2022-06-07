using Microsoft.Win32.SafeHandles;
using PW.Win32;
using System.ComponentModel;
using System.Runtime.InteropServices;
using static PW.Functional.Disposable;

namespace PW.IO;

public static partial class FileInfoExtensions
{


  //private const int ERROR_SHARING_VIOLATION = 32;

  /// <summary>
  /// Attempts to open the file for shared read access. If the file is locked it will retry for the specified time-out period.
  /// </summary>
  /// <param name="file">File to open.</param>
  /// <param name="timeout">Time-out period to wait, when file is locked.</param>
  /// <returns>Open <see cref="FileStream"/> on success or null if the time-out expires.</returns>
  public static FileStream? WaitForAccess(this FileInfo file!!, TimeSpan timeout) =>
    WaitForAccess(file, timeout, FileOpenArguments.OpenExistingForSharedRead);


  /// <summary>
  /// Determines whether a file can be opened for shared-read access. 
  /// Returns false if it can be opened, or if file or does not exist. Otherwise returns true.
  /// </summary>
  public static bool IsReadLocked(this FileInfo file!!)
    => file.Exists
        && file.OpenFileSharedRead().DisposeAfter(handle => handle.IsInvalid)
        && Marshal.GetLastWin32Error() == Error.SharingViolation;

  /// <summary>
  /// Determines whether an existing file can be opened for shared-read access. 
  /// Returns true if it exists and is not locked to prevent shared-read. Otherwise returns false.
  /// </summary>
  public static bool IsReadable(this FileInfo file!!)
    => file.Exists && file.OpenFileSharedRead().DisposeAfter(handle => !handle.IsInvalid);

  private static SafeFileHandle OpenFileSharedRead(this FileInfo file)
    => SafeNativeMethods.CreateFile(
        file.FullName,
        Win32.FileAccess.GenericRead,
        Win32.FileShare.Read,
        IntPtr.Zero,
        Win32.FileMode.OpenExisting,
        Win32.FileAttributes.Normal, IntPtr.Zero);

  /// <summary>
  /// Attempts to open the file with a retrying timeout. Useful for accessing files which may initially be locked. 
  /// NB: Blocks thread for <paramref name="pollInterval"/> milliseconds during wait loop.
  /// </summary>
  /// <param name="file">The file to open.</param>
  /// <param name="timeout">Number of milliseconds to wait.</param>
  /// <param name="arguments">Defaults to <see cref="FileOpenArguments.OpenExistingForSharedRead"/></param>
  /// <param name="pollInterval">Interval, in milliseconds, at which to try to be opening the file.</param>
  /// <returns>Either a stream or null if the file cannot be opened within the time-out period.</returns>
  public static FileStream? WaitForAccess(this FileInfo file!!, TimeSpan timeout, FileOpenArguments? arguments = null, int pollInterval = 200)
  {
    var start = DateTime.Now;
    if (arguments == null) arguments = FileOpenArguments.OpenExistingForSharedRead;

    while (true)
    {
      if (TryOpenPossiblyLockedFile(file, arguments) is FileStream retval) return retval;

      // Return null if we have timed out on retries.
      if ((DateTime.Now - start) > timeout) return null;

      using var t = new ManualResetEvent(false);
      t.WaitOne(pollInterval);
    }

  }

  /// <summary>
  /// Attempts to open the file with a retrying timeout.  Useful for accessing files which may initially be locked. 
  /// </summary>
  /// <param name="file">The file to open.</param>
  /// <param name="timeout">Number of milliseconds to wait.</param>
  /// <param name="arguments">Defaults to <see cref="FileOpenArguments.OpenExistingForSharedRead"/></param>
  /// <returns>Either a stream or null if the file cannot be opened within the time-out period.</returns>
  public static async Task<FileStream?> WaitForAccessAsync(this FileInfo file!!, TimeSpan timeout, FileOpenArguments? arguments = null)
  {
    var start = DateTime.Now;
    if (arguments == null) arguments = FileOpenArguments.OpenExistingForSharedRead;
    while (true)
    {
      if (TryOpenPossiblyLockedFile(file, arguments) is FileStream retval) return retval;

      // Return null if we have timed out on retries.
      if ((DateTime.Now - start) > timeout) return null;

      await Task.Delay(500);
    }
  }

  private static FileStream? TryOpenPossiblyLockedFile(this FileInfo file!!, FileOpenArguments arguments!!)
  {
    // WHAT : SafeHandle is not always disposed.
    // WHY  : FileStream disposes the SafeHandle when disposed.
    //        So we only dispose SafeHandle here if we are not returning a FileStream.
    // SEE  : https://referencesource.microsoft.com/#q=filestream -- Dispose() method, finally block.

    var fileHandle = SafeNativeMethods.CreateFile(
      file.FullName,
      arguments.Access,
      arguments.Share,
      IntPtr.Zero,
      arguments.Mode,
      Win32.FileAttributes.Normal,
      IntPtr.Zero);

    // Return the file-stream if it opened OK.
    if (!fileHandle.IsInvalid) return new FileStream(fileHandle, (System.IO.FileAccess)arguments.Access);
    else
    {
      fileHandle.Dispose();

      // If the failure to open the file was not due to a sharing violation, then throw an exception
      var errorCode = Marshal.GetLastWin32Error();
      return errorCode != Error.SharingViolation
        ? throw new IOException(new Win32Exception(errorCode).Message, errorCode)
        : null;
    }
  }
}
