#nullable enable

using PW.IO.FileSystemObjects;
using System.ComponentModel;
using System.IO;

namespace PW
{
  /// <summary>
  /// Exception factory
  /// </summary>
  public static class ExceptionFactory
  {
    /// <summary>
    /// Creates a new <see cref="System.IO.FileNotFoundException"/>
    /// </summary>
    public static FileNotFoundException FileNotFoundException(string? file) => 
      file is not null ? new FileNotFoundException("File not found: " + file, file) : new FileNotFoundException();

    /// <summary>
    /// Creates a new <see cref="System.IO.FileNotFoundException"/>
    /// </summary>
    public static FileNotFoundException FileNotFoundException(FilePath file) => FileNotFoundException(file?.Value);

    /// <summary>
    /// Creates a new <see cref="System.IO.FileNotFoundException"/>
    /// </summary>
    public static FileNotFoundException FileNotFoundException(FileInfo file) => FileNotFoundException(file?.FullName);

    /// <summary>
    /// Creates a new <see cref="System.ComponentModel.Win32Exception"/> using the specified error code.
    /// </summary>
    public static Win32Exception Win32Exception(int error) => new(error);

    /// <summary>
    /// Creates a new <see cref="System.ComponentModel.Win32Exception"/> using the error code from the last Win32 error.
    /// </summary>
    public static Win32Exception Win32Exception() => new();

  }


}
