using PW.FailFast;
using PW.IO.FileSystemObjects;
using System;
using System.IO;

namespace PW.IO;

/// <summary>
/// Class for writing to delimited text files.
/// </summary>
public sealed class DelimitedFieldFileWriter : IDisposable
{
  private string FieldSeparator { get; }
  private StreamWriter Writer { get; set; }
  private string[] FieldNames { get; }
  private FileInfo OutputFile { get; }

  /// <summary>
  /// Log file mode
  /// </summary>
  public enum FileMode
  {
    /// <summary>
    /// Append to existing data when writing.
    /// </summary>
    Append,
    /// <summary>
    /// Clear existing data before writing.
    /// </summary>
    Truncate
  }

  /// <summary>
  /// Flush data to disk after each write.
  /// </summary>
  public bool FlushAfterEachWrite { get; set; } = true;

  /// <summary>
  /// Creates a new instance of the <see cref="DelimitedFieldFileWriter"/> class.
  /// </summary>
  /// <param name="outputFile">Path to/for the delimited file.</param>
  /// <param name="fieldSeparator">The separator used to delimit fields in the file.</param>
  /// <param name="fieldNames">The names of the fields. Used as a file header.</param>
  /// <param name="fileMode">Either truncate (overwrite) or append to the file, if it already exists.</param>
  public DelimitedFieldFileWriter(FileInfo outputFile!!, char fieldSeparator, string[] fieldNames, FileMode fileMode)
  {
    if (outputFile.Directory is not DirectoryInfo di) throw new Exception("Output file does not have a directory.");

    if (!di.Exists) throw new DirectoryNotFoundException(di.FullName);

    FieldSeparator = fieldSeparator.ToString();
    FieldNames = fieldNames;
    OutputFile = outputFile;

    var isNewFile = !outputFile.Exists;

    Writer = fileMode == FileMode.Append
      ? File.AppendText(outputFile.FullName)
      : File.CreateText(outputFile.FullName);

    if (isNewFile) WriteFields(fieldNames);

  }

  /// <summary>
  /// Truncates all data from the file and writes the field names.
  /// </summary>
  public void Truncate()
  {
    Writer?.Dispose();
    Writer = OutputFile.CreateText();// File.CreateText(OutputFile);
    WriteFields(FieldNames);
  }

  /// <summary>
  /// Writes a line to the log file using the supplied field values separated by <see cref="FieldSeparator"/>.
  /// </summary>
  /// <param name="fields"></param>
  public void WriteFields(string[] fields)
  {
    Writer.WriteLine(string.Join(FieldSeparator, fields));
    if (FlushAfterEachWrite) Writer.Flush();
  }

  /// <summary>
  /// Releases all resources used by the <see cref="DelimitedFieldFileWriter"/> object.
  /// </summary>
  public void Dispose() => Writer?.Dispose();

}

