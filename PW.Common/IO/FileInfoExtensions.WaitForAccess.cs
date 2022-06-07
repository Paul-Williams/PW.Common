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
    WaitForAccess(file, timeout, FileOpenArguments.OpenForSharedRead);


  /// <summary>
  /// Determines whether a file can be opened for shared-read access. 
  /// Returns false if it can be opened, or if file or does not exist. Otherwise returns true.
  /// </summary>
  public static bool IsReadLocked(this FileInfo file!!)
    => file.Exists
        && file.OpenFileSharedRead().DisposeAfter(handle => handle.IsInvalid)
        && Marshal.GetLastWin32Error() == Error.ERROR_SHARING_VIOLATION;

  /// <summary>
  /// Determines whether an existing file can be opened for shared-read access. 
  /// Returns true if it exists and is not locked to prevent shared-read. Otherwise returns false.
  /// </summary>
  public static bool IsReadable(this FileInfo file!!)
    => file.Exists && file.OpenFileSharedRead().DisposeAfter(handle => handle.IsInvalid == false);

  private static SafeFileHandle OpenFileSharedRead(this FileInfo file)
    => SafeNativeMethods.CreateFile(
        file.FullName,
        System.IO.FileAccess.Read.ToWin32FileAccess(),
        System.IO.FileShare.Read.ToWin32FileShare(),
        IntPtr.Zero,
        FileMode.Open.ToWin32CreationDisposition(),
        Win32.FileAttributes.Normal, IntPtr.Zero);

  /// <summary>
  /// Attempts to open the file with a retrying timeout. 
  /// Useful for files which may initially be locked. 
  /// NB: Blocks thread for <paramref name="pollInterval"/> milliseconds during wait loop.
  /// </summary>
  public static FileStream? WaitForAccess(this FileInfo file!!, TimeSpan timeout, FileOpenArguments arguments!!, int pollInterval = 200)
  {
    var start = DateTime.Now;

    while (true)
    {
      if (OpenFileInternal(file, arguments) is FileStream retval) return retval;

      // Return null if we have timed out on retries.
      if ((DateTime.Now - start) > timeout) return null;

      using var t = new ManualResetEvent(false);
      t.WaitOne(pollInterval);
    }

  }

  public static async Task<FileStream?> WaitForAccessAsync(this FileInfo file!!, TimeSpan timeout, FileOpenArguments arguments!!)
  {
    var start = DateTime.Now;

    while (true)
    {
      if (OpenFileInternal(file, arguments) is FileStream retval) return retval;

      // Return null if we have timed out on retries.
      if ((DateTime.Now - start) > timeout) return null;

      await Task.Delay(500);
    }
  }

  private static FileStream? OpenFileInternal(this FileInfo file!!, FileOpenArguments open!!)
  {
    // WHAT : SafeHandle is not always disposed.
    // WHY  : FileStream disposes the SafeHandle when disposed.
    //        So we only dispose SafeHandle here if we are not returning a FileStream.
    // SEE  : https://referencesource.microsoft.com/#q=filestream -- Dispose() method, finally block.

    var fileHandle = SafeNativeMethods.CreateFile(
      file.FullName,
      open.Access.ToWin32FileAccess(),
      open.Share.ToWin32FileShare(),
      IntPtr.Zero,
      open.Mode.ToWin32CreationDisposition(),
      Win32.FileAttributes.Normal,
      IntPtr.Zero);

    // Return the file-stream if it opened OK.
    if (!fileHandle.IsInvalid) return new FileStream(fileHandle, open.Access);
    else
    {
      fileHandle.Dispose();

      // If the failure to open the file was not due to a sharing violation, then throw an exception
      var errorCode = Marshal.GetLastWin32Error();
      return errorCode != Error.ERROR_SHARING_VIOLATION
        ? throw new IOException(new Win32Exception(errorCode).Message, errorCode)
        : null;
    }
  }



  private static Win32.FileAccess ToWin32FileAccess(this System.IO.FileAccess access) =>
    access == System.IO.FileAccess.ReadWrite
      ? Win32.FileAccess.GenericRead | Win32.FileAccess.GenericWrite
      : access == System.IO.FileAccess.Read
        ? Win32.FileAccess.GenericRead
        : Win32.FileAccess.GenericWrite;


  private static Win32.FileShare ToWin32FileShare(this System.IO.FileShare share) => (Win32.FileShare)((uint)share);

  private static CreationDisposition ToWin32CreationDisposition(this FileMode mode) =>
    mode == FileMode.Open
      ? CreationDisposition.OpenExisting
      : mode == FileMode.OpenOrCreate
        ? CreationDisposition.OpenAlways
        : (CreationDisposition)(uint)mode;





}
