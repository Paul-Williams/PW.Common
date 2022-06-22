using PW.Exceptions;

namespace PW.IO.FileSystemObjects;

internal static class Validate
{
  public static void FileName(string value)
  {
    if (value is null) throw new InvalidFileNameException("File name cannot be null.");
    if (string.IsNullOrWhiteSpace(value)) throw new InvalidFileNameException("File name cannot be empty or white-space.");
    if (value.IsAll('.')) throw new InvalidFileNameException("File name cannot be all periods.");
    if (value.ContainsAny(Path.GetInvalidFileNameChars())) throw new InvalidFileNameException("File name contains invalid characters.");
  }

  public static void FileExtension(string value)
  {
    if (value is null) throw new InvalidFileExtensionException("File extension cannot be null.");
    if (value.Length == 1) throw new InvalidFileExtensionException("File extension cannot be a single character. When not empty, it must start with a period and at least one other character.");
    if (value[0] != '.') throw new InvalidFileExtensionException("File extension must begin with a period.");
    if (value.IsAll('.')) throw new InvalidFileExtensionException("File extension cannot be all periods.");
    if (value.IsWhiteSpace(1)) throw new InvalidFileExtensionException("File extension cannot be just white-space after the period.");
    if (value.ContainsAny(Path.GetInvalidFileNameChars())) throw new InvalidFileExtensionException("File extension contains invalid characters.");
  }

}
