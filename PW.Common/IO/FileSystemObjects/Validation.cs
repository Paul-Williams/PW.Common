#nullable enable

using PW.Exceptions;
using PW.Extensions;
using System;
using System.IO;

namespace PW.IO.FileSystemObjects
{
  internal static class Validation
  {
    public static void ValidateFileName(this string value)
    {
      if (value is null) throw new ArgumentNullException(nameof(value), "File name cannot be null.");
      if (string.IsNullOrWhiteSpace(value)) throw new InvalidFileNameException("File name cannot be empty or white-space.");
      if (value.IsAll('.')) throw new InvalidFileNameException("File name cannot be all periods.");
      if (value.ContainsAny(Path.GetInvalidFileNameChars())) throw new InvalidFileNameException("File name contains invalid characters.");
    }

  }
}
