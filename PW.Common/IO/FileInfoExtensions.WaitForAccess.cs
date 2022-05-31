#nullable enable 

using Microsoft.Win32.SafeHandles;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using PW.Functional;

namespace PW.IO
{
  public static partial class FileInfoExtensions
  {
    //private const int ERROR_SHARING_VIOLATION = 32;

    /// <summary>
    /// Attempts to open the file for shared read access. If the file is locked it will retry for the specified time-out period.
    /// </summary>
    /// <param name="file">File to open.</param>
    /// <param name="timeout">Time-out period to wait, when file is locked.</param>
    /// <returns>Open <see cref="FileStream"/> on success or null if the time-out expires.</returns>
    public static FileStream? WaitForAccess(this FileInfo file, TimeSpan timeout) =>
      WaitForAccess(file, timeout, FileOpenArguments.OpenForSharedRead);


    /// <summary>
    /// Determines whether a file can be opened for shared-read access. 
    /// Returns false if it can be opened, or if file or does not exist. Otherwise returns true.
    /// </summary>
    public static bool IsReadLocked(this FileInfo file)
    {
      return file is null
          ? throw new ArgumentNullException(nameof(file))
          : file.Exists
          && Disposable.DisposeAfter(file.OpenFileSharedRead(), x => x.IsInvalid)
          && Marshal.GetLastWin32Error() == Win32Error.ERROR_SHARING_VIOLATION;
    }

    /// <summary>
    /// Determines whether an existing file can be opened for shared-read access. 
    /// Returns true if it exists and is not locked to prevent shared-read. Otherwise returns false.
    /// </summary>
    public static bool IsReadable(this FileInfo file)
    {
      return
        file is null ? throw new ArgumentNullException(nameof(file))
        : file.Exists && file.OpenFileSharedRead().DisposeAfter(x => x.IsInvalid == false);
    }

    private static SafeFileHandle OpenFileSharedRead(this FileInfo file) => CreateFile(
      file.FullName,
      FileAccess.Read.ToWin32FileAccess(),
      FileShare.Read.ToWin32FileShare(),
      IntPtr.Zero,
      FileMode.Open.ToWin32CreationDisposition(),
      Win32FileAttributes.Normal, IntPtr.Zero);

    /// <summary>
    /// Attempts to open the file with a retrying timeout. Useful for files which may initially be locked. NB: Uses <see cref="Thread.Sleep(int)"/> during wait loop.
    /// </summary>
    /// <param name="file">this</param>
    /// <param name="timeout"></param>
    /// <param name="open"></param>
    /// <returns></returns>
    public static FileStream? WaitForAccess(this FileInfo file!!, TimeSpan timeout, FileOpenArguments open!!)
    {
      var start = DateTime.Now;

      while (true)
      {
        // REASON : SafeHandle is disposed by FileStream, when disposed. If 'fileHandle.IsInvalid' we dispose it here.
        // SEE    : https://referencesource.microsoft.com/#q=filestream -- Dispose() method, finally block.
        var fileHandle = CreateFile(
          file.FullName,
          open.Access.ToWin32FileAccess(),
          open.Share.ToWin32FileShare(),
          IntPtr.Zero,
          open.Mode.ToWin32CreationDisposition(),
          Win32FileAttributes.Normal, IntPtr.Zero);

        // Return the file-stream is it opened OK.
        if (!fileHandle.IsInvalid) return new FileStream(fileHandle, open.Access);
        else fileHandle.Dispose();

        // If the failure to open the file was not due to a sharing violation, then throw an exception
        var errorCode = Marshal.GetLastWin32Error();
        if (errorCode != Win32Error.ERROR_SHARING_VIOLATION) throw new IOException(new Win32Exception(errorCode).Message, errorCode);

        // Return null if we have timed out on retries.
        if ((DateTime.Now - start) > timeout) return null;

        Thread.Sleep(100);
      }

    }




    private static Win32FileAccess ToWin32FileAccess(this FileAccess access) =>
      access == FileAccess.ReadWrite
        ? Win32FileAccess.GenericRead | Win32FileAccess.GenericWrite
        : access == FileAccess.Read
          ? Win32FileAccess.GenericRead
          : Win32FileAccess.GenericWrite;


    private static Win32FileShare ToWin32FileShare(this FileShare share) => (Win32FileShare)((uint)share);

    private static Win32CreationDisposition ToWin32CreationDisposition(this FileMode mode) =>
      mode == FileMode.Open
        ? Win32CreationDisposition.OpenExisting
        : mode == FileMode.OpenOrCreate
          ? Win32CreationDisposition.OpenAlways
          : (Win32CreationDisposition)(uint)mode;

    // See: https://docs.microsoft.com/en-us/windows/win32/api/fileapi/nf-fileapi-createfilew

    [DllImport("kernel32.dll", EntryPoint = "CreateFileW", SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern SafeFileHandle CreateFile(
       string lpFileName,
       Win32FileAccess dwDesiredAccess,
       Win32FileShare dwShareMode,
       IntPtr lpSecurityAttributes,
       Win32CreationDisposition dwCreationDisposition,
       Win32FileAttributes dwFlagsAndAttributes,
       IntPtr hTemplateFile);

    [Flags]    
    private enum Win32FileAccess : uint
    {
      //
      // Standard Section
      //

      AccessSystemSecurity = 0x1000000,   // AccessSystemAcl access type
      MaximumAllowed = 0x2000000,     // MaximumAllowed access type

      Delete = 0x10000,
      ReadControl = 0x20000,
      WriteDAC = 0x40000,
      WriteOwner = 0x80000,
      Synchronize = 0x100000,

      StandardRightsRequired = 0xF0000,
      StandardRightsRead = ReadControl,
      StandardRightsWrite = ReadControl,
      StandardRightsExecute = ReadControl,
      StandardRightsAll = 0x1F0000,
      SpecificRightsAll = 0xFFFF,

      FILE_READ_DATA = 0x0001,        // file & pipe
      FILE_LIST_DIRECTORY = 0x0001,       // directory
      FILE_WRITE_DATA = 0x0002,       // file & pipe
      FILE_ADD_FILE = 0x0002,         // directory
      FILE_APPEND_DATA = 0x0004,      // file
      FILE_ADD_SUBDIRECTORY = 0x0004,     // directory
      FILE_CREATE_PIPE_INSTANCE = 0x0004, // named pipe
      FILE_READ_EA = 0x0008,          // file & directory
      FILE_WRITE_EA = 0x0010,         // file & directory
      FILE_EXECUTE = 0x0020,          // file
      FILE_TRAVERSE = 0x0020,         // directory
      FILE_DELETE_CHILD = 0x0040,     // directory
      FILE_READ_ATTRIBUTES = 0x0080,      // all
      FILE_WRITE_ATTRIBUTES = 0x0100,     // all

      //
      // Generic Section
      //

      GenericRead = 0x80000000,
      GenericWrite = 0x40000000,
      GenericExecute = 0x20000000,
      GenericAll = 0x10000000,

      SPECIFIC_RIGHTS_ALL = 0x00FFFF,
      FILE_ALL_ACCESS =
      StandardRightsRequired |
      Synchronize |
      0x1FF,

      FILE_GENERIC_READ =
      StandardRightsRead |
      FILE_READ_DATA |
      FILE_READ_ATTRIBUTES |
      FILE_READ_EA |
      Synchronize,

      FILE_GENERIC_WRITE =
      StandardRightsWrite |
      FILE_WRITE_DATA |
      FILE_WRITE_ATTRIBUTES |
      FILE_WRITE_EA |
      FILE_APPEND_DATA |
      Synchronize,

      FILE_GENERIC_EXECUTE =
      StandardRightsExecute |
        FILE_READ_ATTRIBUTES |
        FILE_EXECUTE |
        Synchronize
    }

    [Flags]
    private enum Win32FileShare : uint
    {
      /// <summary>
      ///
      /// </summary>
      None = 0x00000000,
      /// <summary>
      /// Enables subsequent open operations on an object to request read access.
      /// Otherwise, other processes cannot open the object if they request read access.
      /// If this flag is not specified, but the object has been opened for read access, the function fails.
      /// </summary>
      Read = 0x00000001,
      /// <summary>
      /// Enables subsequent open operations on an object to request write access.
      /// Otherwise, other processes cannot open the object if they request write access.
      /// If this flag is not specified, but the object has been opened for write access, the function fails.
      /// </summary>
      Write = 0x00000002,
      /// <summary>
      /// Enables subsequent open operations on an object to request delete access.
      /// Otherwise, other processes cannot open the object if they request delete access.
      /// If this flag is not specified, but the object has been opened for delete access, the function fails.
      /// </summary>
      Delete = 0x00000004
    }

    private enum Win32CreationDisposition : uint
    {
      /// <summary>
      /// Creates a new file. The function fails if a specified file exists.
      /// </summary>
      New = 1,
      /// <summary>
      /// Creates a new file, always.
      /// If a file exists, the function overwrites the file, clears the existing attributes, combines the specified file attributes,
      /// and flags with FILE_ATTRIBUTE_ARCHIVE, but does not set the security descriptor that the SECURITY_ATTRIBUTES structure specifies.
      /// </summary>
      CreateAlways = 2,
      /// <summary>
      /// Opens a file. The function fails if the file does not exist.
      /// </summary>
      OpenExisting = 3,
      /// <summary>
      /// Opens a file, always.
      /// If a file does not exist, the function creates a file as if dwCreationDisposition is CREATE_NEW.
      /// </summary>
      OpenAlways = 4,
      /// <summary>
      /// Opens a file and truncates it so that its size is 0 (zero) bytes. The function fails if the file does not exist.
      /// The calling process must open the file with the GENERIC_WRITE access right.
      /// </summary>
      TruncateExisting = 5
    }

    // See: https://docs.microsoft.com/en-us/windows/win32/fileio/file-attribute-constants
    [Flags]
    private enum Win32FileAttributes : uint
    {
      Readonly = 0x00000001,
      Hidden = 0x00000002,
      System = 0x00000004,
      Directory = 0x00000010,
      Archive = 0x00000020,
      Device = 0x00000040,
      Normal = 0x00000080,
      Temporary = 0x00000100,
      SparseFile = 0x00000200,
      ReparsePoint = 0x00000400,
      Compressed = 0x00000800,
      Offline = 0x00001000,
      NotContentIndexed = 0x00002000,
      Encrypted = 0x00004000,
      Write_Through = 0x80000000,
      Overlapped = 0x40000000,
      NoBuffering = 0x20000000,
      RandomAccess = 0x10000000,
      SequentialScan = 0x08000000,
      DeleteOnClose = 0x04000000,
      BackupSemantics = 0x02000000,
      PosixSemantics = 0x01000000,
      OpenReparsePoint = 0x00200000,
      OpenNoRecall = 0x00100000,
      FirstPipeInstance = 0x00080000
    }

  }
}
