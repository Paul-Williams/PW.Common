﻿namespace PW.Win32;

public enum FileMode : uint
{
  /// <summary>
  /// Creates a new file. The function fails if a specified file exists.
  /// </summary>
  New = 1,

  /// <summary>
  /// Creates a new file, always.
  /// If a file exists, the function overwrites the file, clears the existing attributes, combines the specified file attributes,
  /// and flags with <see cref="FileAttributes.Archive"/>, but does not set the security descriptor that the SECURITY_ATTRIBUTES structure specifies.
  /// </summary>
  CreateAlways = 2,
  
  /// <summary>
  /// Opens a file. The function fails if the file does not exist.
  /// </summary>
  OpenExisting = 3,
  
  /// <summary>
  /// Opens a file, always.
  /// If a file does not exist, the function creates a file as if <see cref="FileMode.New"/> was used.
  /// </summary>
  OpenAlways = 4,
  
  /// <summary>
  /// Opens a file and truncates it so that its size is 0 (zero) bytes. The function fails if the file does not exist.
  /// The calling process must open the file with the <see cref="FileAccess.GenericWrite"/> access right.
  /// </summary>
  TruncateExisting = 5
}
