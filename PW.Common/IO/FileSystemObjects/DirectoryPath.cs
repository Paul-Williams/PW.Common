# nullable enable

using PW.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using static PW.BackingField;
using System.Linq;

namespace PW.IO.FileSystemObjects
{
  /// <summary>
  /// Represents a file system directory path.
  /// </summary>
  public class DirectoryPath : FileSystemPath<DirectoryPath>
  {
    #region Constructors

    private DirectoryPath() { }

    /// <summary>
    /// Creates a new instance from a string. Basic validation performed. <see cref="Path.DirectorySeparatorChar"/> appended if missing.
    /// Supports relative paths. (e.g. . or ..) Path of just [Drive]: (e.g. C:) will return current directory for that drive.
    /// </summary>
    public DirectoryPath(string directoryPath)
    {
      if (directoryPath is null) throw new ArgumentNullException(nameof(directoryPath), "Value cannot be null.");
      if (string.IsNullOrWhiteSpace(directoryPath)) throw new ArgumentException("Value cannot be empty or white-space.", nameof(directoryPath));

      try
      {
        // Cheat and use DirectoryInfo to further validate and construct.
        // To avoid issues during comparison, normalize all directories to end with a path separator.
        Value = Helpers.PathHelper.NormalizeDirectoryPath(new DirectoryInfo(directoryPath).FullName);
      }
      catch (Exception ex)
      {
        // Any exception from attempting to create a DirectoryInfo will be classed 
        // as a argument exception, as the issue will have been caused by the 
        // argument being invalid in some way.
        throw new ArgumentException(ex.Message, nameof(directoryPath));
      }
    }

    /// <summary>
    /// Creates an instance from an existing <see cref="DirectoryInfo"/> object. Path validation skipped. <see cref="Path.DirectorySeparatorChar"/> appended if missing.
    /// </summary>    
    public DirectoryPath(DirectoryInfo directory!!)
    {
      Value = Helpers.PathHelper.NormalizeDirectoryPath(directory.FullName);
    }

    /// <summary>
    /// Creates a new instance
    /// </summary>
    /// <param name="filePath"></param>
    public DirectoryPath(FilePath filePath)
    {
      Value = filePath is not null
        ? Helpers.PathHelper.NormalizeDirectoryPath(Path.GetDirectoryName((string)filePath)!)
        : throw new ArgumentNullException(nameof(filePath));
    }

    #endregion

    #region Private Methods


    /// <summary>
    /// Creates a new instance from a string, without performing validation on the string.
    /// </summary>
    private static DirectoryPath FromStringInternal(string value)
    {
      return new() { Value = Helpers.PathHelper.NormalizeDirectoryPath(Path.GetDirectoryName(value)!) };
    }

    #endregion


    #region Explicit Casts

    /// <summary>
    /// Casts a <see cref="String"/> to a <see cref="DirectoryPath"/>.
    /// </summary>    
    public static explicit operator DirectoryPath(string value) => new(value);

    /// <summary>
    /// Casts a <see cref="String"/> to a <see cref="DirectoryPath"/>.
    /// </summary>    
    public static explicit operator DirectoryPath(DirectoryInfo value) => new(value);

    /// <summary>
    /// Casts a <see cref="DirectoryPath"/> to a <see cref="string"/>.
    /// </summary>    
    public static explicit operator string(DirectoryPath value) =>
      value is not null ? value.Value : throw new ArgumentNullException(nameof(value));

    /// <summary>
    /// Casts a <see cref="DirectoryPath"/> to a <see cref="DirectoryInfo"/>.
    /// </summary>    
    public static explicit operator DirectoryInfo(DirectoryPath value) =>
      value is not null ? new DirectoryInfo(value.Value) : throw new ArgumentNullException(nameof(value));

    /// <summary>
    /// Casts a <see cref="FilePath"/> to a <see cref="DirectoryPath"/>.
    /// </summary>
    public static explicit operator DirectoryPath(FilePath filePath) =>
      filePath is not null ? new DirectoryPath(filePath) : throw new ArgumentNullException(nameof(filePath));

    #endregion

    #region Operator overloads

    /// <summary>
    /// Creates a FilePath from a DirectoryPath and FileName.
    /// </summary>
    public static FilePath operator +(DirectoryPath directoryPath, FileName fileName)
    {
      return directoryPath is null ? throw new ArgumentNullException(nameof(directoryPath)) : fileName is null ? throw new ArgumentNullException(nameof(fileName)) : (FilePath)(directoryPath.Value + fileName.Value);
    }

    /// <summary>
    /// Creates a DirectoryPath from a DirectoryPath and Sub-DirectoryName.
    /// </summary>
    public static DirectoryPath operator +(DirectoryPath directoryPath, DirectoryName directoryName)
    {
      return directoryPath is null
          ? throw new ArgumentNullException(nameof(directoryPath))
          : directoryName is null
            ? throw new ArgumentNullException(nameof(directoryName))
            : (DirectoryPath)(directoryPath.Value + directoryName.Value);
    }


    #endregion


    /// <summary>
    /// Creates a new <see cref="DirectoryInfo"/> object using this instance.
    /// </summary>
    /// <returns></returns>
    public DirectoryInfo ToDirectoryInfo() => (DirectoryInfo)this;


    #region Lazy Property Cache variables


    /// <summary>
    /// Cache variable for <see cref="Parent"/> property
    /// </summary>
    private DirectoryPath? _Parent;

    /// <summary>
    /// Cache variable for <see cref="DirectoryName"/> property
    /// </summary>
    private DirectoryName? _DirectoryName;


    #endregion


    #region Public Methods

    /// <summary>
    /// Returns a new <see cref="DirectoryPath"/> instance with the specified sub-directory name appended.
    /// </summary>
    public DirectoryPath Append(DirectoryName subDirectory)
    {
      return subDirectory is null
          ? throw new ArgumentNullException(nameof(subDirectory))
          : (DirectoryPath)(System.IO.Path.Combine(Value, subDirectory.ToString()));
    }

    /// <summary>
    /// Returns a new <see cref="DirectoryPath"/> instance with the specified file name appended.
    /// </summary>
    public FilePath Append(FileName file) =>
      file is null ? throw new ArgumentNullException(nameof(file)) : (FilePath)(Value + file.ToString());


    /// <summary>
    /// Returns a new <see cref="DirectoryPath"/> instance with the specified file name appended.
    /// </summary>
    public FilePath File(string file) => Append((FileName)file);


    /// <summary>
    /// Returns true if this directory is below the specified directory. It may be a direct sub-directory or further down the same path.
    /// </summary>
    public bool IsBelow(DirectoryPath directory)
    {
      return directory is null
          ? throw new ArgumentNullException(nameof(directory))
          : directory.Value.StartsWith(Value, StringComparison.InvariantCultureIgnoreCase);
    }

    /// <summary>
    /// Returns the parent directory or null if the directory does not have a parent. Value is cached after initial call.
    /// </summary>
    public DirectoryPath? Parent => GetLazy(ref _Parent, () => Path.GetDirectoryName(Value) is string parent ? FromStringInternal(parent) : null);

    /// <summary>
    /// Returns the name of the last directory in the path. Value is cached after initial call.
    /// </summary>
    public DirectoryName Name => GetLazy(ref _DirectoryName, () => new DirectoryName(this))!;

    /// <summary>
    /// Returns the path as a string, specifying whether to include the terminating slash.
    /// </summary>
    /// <returns></returns>
    internal string ToString(bool includeTerminatingSlash) =>
      includeTerminatingSlash ? Value : Value[0..^1];





    #endregion


    #region Public Properties

    /// <summary>
    /// Determines whether the path refers to an existing directory on disk.
    /// </summary>
    public override bool Exists => Directory.Exists(Value);

    #endregion




  }
}
