using System.IO;

#if NET48
using static PW.Extensions.StringExtensions;
#endif

namespace PW.Helpers;

public static class PathHelper
{
  /// <summary>
  /// Unless empty, ensures it starts with a period.
  /// </summary>
  public static string NormalizeFileExtension(string extension!!)
    => extension.Length > 0 && !extension.StartsWith('.')
    ? '.' + extension
    : extension;


  /// <summary>
  /// Unless empty, ensures the directory path string is terminated with a <see cref="Path.DirectorySeparatorChar"/>
  /// </summary>
  public static string NormalizeDirectoryPath(string directoryPath!!)
    => directoryPath.Length > 0 && !directoryPath.EndsWith(Path.DirectorySeparatorChar)
    ? directoryPath + Path.DirectorySeparatorChar
    : directoryPath;
}
