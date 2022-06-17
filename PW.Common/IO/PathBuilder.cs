using System.Text;
using static PW.IO.Paths;

namespace PW.IO;

/// <summary>
/// Class for building file paths -- seems a bit rubbish
/// </summary>
public class PathBuilder
{
  private StringBuilder StringBuilder { get; } = new(256);

  /// <summary>
  /// ctor
  /// </summary>
  public PathBuilder() { }

  /// <summary>
  /// ctor
  /// </summary>
  public PathBuilder(string path) => StringBuilder.Append(path);
  /// <summary>
  /// ctor
  /// </summary>
  public PathBuilder(IEnumerable<string> directoryNames) => AppendDirectorNames(directoryNames);

  /// <summary>
  /// Returns the full path.
  /// </summary>
  public override string ToString() => StringBuilder.ToString();



  /// <summary>
  /// Appends the directory name to the path
  /// </summary>
  /// <param name="directoryName"></param>
  public PathBuilder AppendDirectory(string directoryName)
  {
    StringBuilder.Append(NormalizeDirectoryPath(directoryName));
    return this;
  }

  public PathBuilder AppendDirectorNames(IEnumerable<string> directoryNames)
  {
    StringBuilder.Append(NormalizeDirectoryPath(Path.Combine(directoryNames as string[] ?? directoryNames.ToArray())));
    return this;
  }

  /// <summary>
  /// Appends the file name to the path
  /// </summary>
  /// <param name="fileName"></param>
  public PathBuilder AppendFilename(string fileName)
  {
    StringBuilder.Append(fileName);
    return this;
  }

  /// <summary>
  /// Appends the file name to the path
  /// </summary>
  /// <param name="fileExtension"></param>
  public PathBuilder AppendFileExtension(string fileExtension)
  {
    StringBuilder.Append(fileExtension.StartsWith('.') ? fileExtension : '.' + fileExtension);
    return this;
  }

}
