namespace PW.IO;

/// <summary>
/// 
/// </summary>
public class FileOpenArguments
{

  /// <summary>
  /// Constructor
  /// </summary>
  public FileOpenArguments(FileMode mode, FileAccess access, FileShare share)
  {
    Mode = mode;
    Access = access;
    Share = share;
  }

  /// <summary>
  /// Specifies how the operating system should open the file.
  /// </summary>
  public FileMode Mode { get; }

  /// <summary>
  /// Open for read, write read/write access to the file.
  /// </summary>
  public FileAccess Access { get; }

  /// <summary>
  /// The kind of access other <see cref="FileStream"/> objects can have to the same file.
  /// </summary>
  public FileShare Share { get; }

  /// <summary>
  /// Returns a new file open for shared read instance.
  /// </summary>
  public static FileOpenArguments OpenForSharedRead => new(FileMode.Open, FileAccess.Read, FileShare.Read);

}
