 

using System;
using System.Collections.Generic;

namespace PW.Net
{
  /// <summary>
  /// Represents the path-name-extension components of which a file url is comprised.
  /// </summary>
  public class FileUrlComponents : CSharpFunctionalExtensions.ValueObject
  {
    /// <summary>
    /// Url path to the file
    /// </summary>
    public string Path { get; }

    /// <summary>
    /// Name of the file, without the file extension
    /// </summary>
    public string NameWithoutExtension { get; }

    /// <summary>
    /// File extension
    /// </summary>
    public string Extension { get; }

    /// <summary>
    /// The file name and extension
    /// </summary>
    public string Name => NameWithoutExtension + Extension;

    /// <summary>
    /// Reconstructs path elements back to a single string.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => Path + "/" + Name;


    /// <summary>
    /// Creates a new instance from a complete file url.
    /// </summary>
    public FileUrlComponents(string fileUrl!!)
    {
      var lastSlash = fileUrl.LastIndexOf('/');

      if (lastSlash != -1)
      {
        Path = fileUrl[..lastSlash];
        fileUrl = fileUrl[(lastSlash + 1)..];
      }
      else
      {
        Path = string.Empty;
      }

      var lastDot = fileUrl.LastIndexOf('.');

      if (lastDot != -1)
      {
        NameWithoutExtension = fileUrl[..lastDot];
        Extension = fileUrl[lastDot..];
      }
      else
      {
        NameWithoutExtension = fileUrl;
        Extension = string.Empty;
      }

    }

    /// <summary>
    /// Creates a new instance with the specified values.
    /// </summary>
    public FileUrlComponents(string path, string name, string extension)
    {
      Path = path;
      NameWithoutExtension = name;
      Extension = extension;
    }

    /// <summary>
    /// Returns a new instance with the name set to the value returned by <paramref name="func"/>.
    /// </summary>
    public FileUrlComponents Rename(Func<string, string> func) =>
      func is null ? throw new ArgumentNullException(nameof(func))
      : new FileUrlComponents(Path, func(NameWithoutExtension), Extension);

    /// <summary>
    /// Returns a new instance with the specified new name.
    /// </summary>
    public FileUrlComponents Rename(string newName) => new(Path, newName, Extension);

    /// <summary>
    /// Returns a new instance with the extension set to the supplied value.
    /// </summary>
    public FileUrlComponents ChangeExtension(string newExtension) => new(Path, NameWithoutExtension, newExtension);


    /// <summary>
    /// Returns a new instance with the path set to the supplied value.
    /// </summary>
    public FileUrlComponents ChangePath(string newPath) => new(newPath, NameWithoutExtension, Extension);

    /// <summary>
    /// Returns a new instance with the path set to the value returned by <paramref name="func"/>.
    /// </summary>
    public FileUrlComponents ChangePath(Func<string, string> func) =>
      func is null ? throw new ArgumentNullException(nameof(func))
      : new FileUrlComponents(func(Path), NameWithoutExtension, Extension);

    /// <summary>
    /// Returns the components (values) which are required for equality comparison.
    /// </summary>
    /// <returns></returns>
    protected override IEnumerable<object> GetEqualityComponents()
    {
      yield return Name;
      yield return Extension;
      yield return Path;
    }
  }

}
