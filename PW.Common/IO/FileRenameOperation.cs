

using PW.FailFast;
using System.IO;
using static CSharpFunctionalExtensions.Result;
using CSharpFunctionalExtensions;
using PW.Interfaces;

namespace PW.IO
{
  /// <summary>
  /// Encapsulates a file rename operation
  /// </summary>
  public class FileRenameOperation : IResultOperation<FileInfo>
  {
    private FileInfo File { get; }
    private string NewName { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="file">File to be renamed</param>
    /// <param name="newName">New name for the file</param>
    public FileRenameOperation(FileInfo file!!, string newName)
    {
      Guard.NotNullOrWhitespace(newName, nameof(newName));

      File = file;
      NewName = newName;
    }

    // Implements: IOperation<FileInfo>.Perform() 
    /// <summary>
    /// Performs the rename operation
    /// </summary>
    /// <returns></returns>
    public Result<FileInfo> Perform()
    {
      try
      {
        File.Rename(NewName);
        return Success(File);
      }
      catch (System.Exception ex)
      {
        return Failure<FileInfo>(ToString() + " failed. " + ex.Message);
      }
    }

    /// <summary>
    /// ToString()
    /// </summary>
    public override string ToString() => $"Rename: {File.FullName} -> {NewName}";


  }
}
