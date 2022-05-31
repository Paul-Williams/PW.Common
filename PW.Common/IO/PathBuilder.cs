#nullable enable 

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PW.Extensions;
using PW.FailFast;

namespace PW.IO
{
  /// <summary>
  /// Class for building file paths -- seems a bit rubbish
  /// </summary>
  public class PathBuilder
  {
    private StringBuilder StringBuilder { get; } = new(256);

    /// <summary>
    /// ctor
    /// </summary>
    public PathBuilder()
    {
    }

    /// <summary>
    /// ctor
    /// </summary>
    public PathBuilder(string path)
    {
      Guard.NotNull(path, nameof(path));
      StringBuilder.Append(path);
    }
    /// <summary>
    /// ctor
    /// </summary>
    public PathBuilder(IEnumerable<string> directoryNames)
    {
      Guard.NotNull(directoryNames, nameof(directoryNames));
      directoryNames.ForEach(name => StringBuilder.Append(name + Path.DirectorySeparatorChar));
    }

    /// <summary>
    /// tostring
    /// </summary>
    public override string ToString() => StringBuilder.ToString();

    /// <summary>
    /// Appends the directory name to the path
    /// </summary>
    /// <param name="name"></param>
    public void AppendDirectory(string name)
    {
      Guard.NotNull(name, nameof(name));
      StringBuilder.Append(name + Path.DirectorySeparatorChar);
    }

    /// <summary>
    /// Appends the file name to the path
    /// </summary>
    /// <param name="name"></param>
    public void AppendFilename(string name)
    {
      Guard.NotNull(name, nameof(name));
      StringBuilder.Append(name);
    }
      
      
  }
}
