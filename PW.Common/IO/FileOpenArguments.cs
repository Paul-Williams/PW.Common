namespace PW.IO;

/// <summary>
/// 
/// </summary>
public class FileOpenArguments
{

  /// <summary>
  /// Constructor
  /// </summary>
  public FileOpenArguments(Win32.FileMode mode, Win32.FileAccess access, Win32.FileShare share)
  {
    Mode = mode;
    Access = access;
    Share = share;
  }

  /// <summary>
  /// Specifies how the operating system should open the file.
  /// </summary>
  public Win32.FileMode Mode { get; }

  /// <summary>
  /// Open for read, write read/write access to the file.
  /// </summary>
  public Win32.FileAccess Access { get; }

  /// <summary>
  /// The kind of access other <see cref="FileStream"/> objects can have to the same file.
  /// </summary>
  public Win32.FileShare Share { get; }

  /// <summary>
  /// Returns a new file open for shared read instance.
  /// </summary>
  public static FileOpenArguments OpenExistingForSharedRead 
    => new(Win32.FileMode.OpenExisting, Win32.FileAccess.GenericRead, Win32.FileShare.Read);

  //public static FileOpenArguments OpenExistingForReadWriteWithSharedRead 
  //  => new(Win32.CreationDisposition.OpenExisting, Win32.FileAccess.GenericReadWrite, Win32.FileShare.Read);

  //public static FileOpenArguments OpenExistingForExclusiveReadWrite 
  //  => new(Win32.CreationDisposition.OpenExisting, Win32.FileAccess.GenericReadWrite, Win32.FileShare.None);

  //public static FileOpenArguments OpenExistingForExclusiveWrite 
  //  => new(Win32.CreationDisposition.OpenExisting, Win32.FileAccess.GenericWrite, Win32.FileShare.None);


}
