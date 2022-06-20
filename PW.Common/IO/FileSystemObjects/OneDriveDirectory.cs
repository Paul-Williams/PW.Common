namespace PW.IO.FileSystemObjects;

/// <summary>
/// A <see cref="DirectoryPath"/> pointing to the OneDrive folder.
/// </summary>
public class OneDriveDirectory : DirectoryPath
{

  /// <summary>
  /// Creates a new instance.
  /// </summary>
  /// <exception cref="DirectoryNotFoundException">OneDrive environmental variable is not set.</exception>
  public OneDriveDirectory() :
    base(Environment.GetEnvironmentVariable("OneDrive")
      ?? Environment.GetEnvironmentVariable("OneDriveConsumer")
      ?? throw new DirectoryNotFoundException("The OneDrive environmental variable is not set."))
  {
  }

  // Property cache variables.
  private DirectoryPath? _databases;
  private DirectoryPath? _desktop;
  private DirectoryPath? _personalDocuments;
  private DirectoryPath? _workDocuments;

  /// <summary>
  /// 
  /// </summary>
  public DirectoryPath Databases => _databases ??= _databases = this + (DirectoryName)"Db";

  /// <summary>
  /// 
  /// </summary>
  public DirectoryPath Desktop => _desktop ??= _desktop = this + (DirectoryName)"Desktop";

  /// <summary>
  /// 
  /// </summary>
  public DirectoryPath PersonalDocuments =>
    _personalDocuments ??= _personalDocuments = this + (DirectoryName)"Documents" + (DirectoryName)"Personal";

  /// <summary>
  /// 
  /// </summary>
  public DirectoryPath WorkDocuments =>
    _workDocuments ??= _workDocuments = this + (DirectoryName)"Documents" + (DirectoryName)"Work";


}
