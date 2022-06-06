

using PW.Exceptions;
using PW.Extensions;
using System;
using System.IO;
using System.Linq;

namespace PW.IO.FileSystemObjects
{
  /// <summary>
  /// Represents a file name, without it's extension element.
  /// </summary>
  public class FileNameWithoutExtension : FileSystemPathSection<FileNameWithoutExtension>
  {

    #region Constructors

    /// <summary>
    /// Creates a new instance.
    /// </summary>
    public FileNameWithoutExtension(string value)
    {
      Validation.ValidateFileName(value);
      Value = value;
    }

    /// <summary>
    /// Creates a new instance.
    /// </summary>
    public FileNameWithoutExtension(FilePath filePath!!)
    {
      Value = Path.GetFileNameWithoutExtension((string)filePath);
    }

    /// <summary>
    /// Creates a new instance.
    /// </summary>
    public FileNameWithoutExtension(FileName fileName!!)
    {
      Value = Path.GetFileNameWithoutExtension((string)fileName);
    }

    /// <summary>
    /// Creates a new instance.
    /// </summary>
    public FileNameWithoutExtension(FileInfo fileInfo!!)
    {
      Value = Path.GetFileNameWithoutExtension(fileInfo.FullName);
    }

    #endregion


    #region Explicit casts

    /// <summary>
    /// Casts a <see cref="String"/> to a <see cref="FileNameWithoutExtension"/>.
    /// </summary>    
    public static explicit operator FileNameWithoutExtension(string value) =>
      value is not null ? new FileNameWithoutExtension(value) : throw new ArgumentNullException(nameof(value));

    /// <summary>
    /// Casts a <see cref="FileNameWithoutExtension"/> to a <see cref="String"/>.
    /// </summary>    
    public static explicit operator string(FileNameWithoutExtension value) =>
      value is not null ? value.Value : throw new ArgumentNullException(nameof(value));

    /// <summary>
    /// 
    /// </summary>    
    public static explicit operator FileNameWithoutExtension(FileName value) =>
      value is not null ? new FileNameWithoutExtension(value) : throw new ArgumentNullException(nameof(value));

    /// <summary>
    /// 
    /// </summary>    
    public static explicit operator FileNameWithoutExtension(FilePath value) =>
      value is not null ? new FileNameWithoutExtension(value) : throw new ArgumentNullException(nameof(value));



    #endregion




    ///// <summary>
    ///// Creates a new <see cref="FileName"/> instance by appending a <see cref="FileExtension"/> to this <see cref="FileNameWithoutExtension"/>
    ///// </summary>
    //public FileName Append(FileExtension extension)
    //{
    //  if (extension is null) throw new ArgumentNullException(nameof(extension));
    //  return (FileName)(Value + extension.ToString());
    //}


    ///// <summary>
    ///// Creates a new <see cref="FileNameWithoutExtension"/> instance by appending a <see cref="string"/> to this <see cref="FileNameWithoutExtension"/>
    ///// </summary>
    //public FileNameWithoutExtension Append(string suffix)
    //{
    //  if (suffix is null) throw new ArgumentNullException(nameof(suffix));

    //  return (FileNameWithoutExtension)(Value + suffix);

    //}

    #region Operator overloads

    /// <summary>
    /// Creates a FileName from a FileNameWithoutExtension and FileExtension.
    /// </summary>
    public static FileName operator +(FileNameWithoutExtension fileNameWithoutExtension, FileExtension fileExtension) => 
      fileNameWithoutExtension is null
      ? throw new ArgumentNullException(nameof(fileNameWithoutExtension))
      : fileExtension is null
      ? throw new ArgumentNullException(nameof(fileExtension))
      : (FileName)(fileNameWithoutExtension.Value + fileExtension.Value);

    #endregion

    /// <summary>
    /// Creates a mask for all files of the same name but any extension.
    /// </summary>    
    public string CreateMask() => Value + ".*";

  }
}
