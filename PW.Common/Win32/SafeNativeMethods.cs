using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace PW.Win32;

/// <summary>
/// Wrapper for Win32 API calls.
/// </summary>
[SuppressUnmanagedCodeSecurity]
internal static class SafeNativeMethods
{

  // See: https://docs.microsoft.com/en-us/windows/win32/api/fileapi/nf-fileapi-createfilew

  [DllImport("kernel32.dll", EntryPoint = "CreateFileW", SetLastError = true, CharSet = CharSet.Unicode)]
  internal static extern SafeFileHandle CreateFile(
     string lpFileName,
     Win32.FileAccess dwDesiredAccess,
     Win32.FileShare dwShareMode,
     IntPtr lpSecurityAttributes,
     CreationDisposition dwCreationDisposition,
     Win32.FileAttributes dwFlagsAndAttributes,
     IntPtr hTemplateFile);

  // See: https://msdn.microsoft.com/en-us/library/windows/desktop/aa365743(v=vs.85).aspx
  [DllImport("kernel32.dll", SetLastError = true)]
  public static extern bool Wow64DisableWow64FsRedirection(ref IntPtr ptr);

  // See: https://msdn.microsoft.com/en-us/library/windows/desktop/aa365745(v=vs.85).aspx
  [DllImport("kernel32.dll", SetLastError = true)]
  public static extern bool Wow64RevertWow64FsRedirection(IntPtr ptr);


  // See: https://docs.microsoft.com/en-us/windows/win32/api/winbase/nf-winbase-movefile
  [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
  public static extern bool MoveFile(string lpExistingFileName, string lpNewFileName);



  // See: https://docs.microsoft.com/en-us/windows/win32/api/shlwapi/nf-shlwapi-strcmplogicalw

  /// <summary>
  /// Compares two Unicode strings. Digits in the strings are considered as numerical content rather than text. This test is not case-sensitive.
  /// </summary>
  /// <param name="psz1">A pointer to the first null-terminated string to be compared.</param>
  /// <param name="psz2">A pointer to the second null-terminated string to be compared.</param>
  /// <returns>    
  /// Returns zero if the strings are identical.    
  /// Returns 1 if the string pointed to by psz1 has a greater value than that pointed to by psz2.     
  /// Returns -1 if the string pointed to by psz1 has a lesser value than that pointed to by psz2.
  /// Returns -2 if either or both strings are null.
  /// </returns>
  [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
  public static extern int StrCmpLogicalW(string psz1, string psz2);





  // Bastardised version of: PWSTR StrFormatByteSizeW(LONGLONG qdw, PWSTR pszBuf, UINT cchBuf)
  // See: https://docs.microsoft.com/en-us/windows/win32/api/shlwapi/nf-shlwapi-strformatbytesizew

  /// <summary>
  /// Converts a numeric value into a string that represents the number expressed as a size value in bytes, kilobytes, megabytes, or gigabytes, depending on the size.
  /// </summary>
  /// <param name="valueToConvert">The numeric value to be converted.</param>
  /// <param name="builder">A <see cref="StringBuilder"/> that, when this function returns successfully, receives the converted number.</param>
  /// <param name="builderCapacity">The size of the buffer pointed to by <paramref name="builder"/>, in characters.</param>
  /// <returns>Returns a pointer to the converted string, or NULL if the conversion fails.</returns>
  [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
  private static extern IntPtr StrFormatByteSizeW(ulong valueToConvert, StringBuilder builder, uint builderCapacity);


  public static string StrFormatByteSize(ulong value)
  {
    var sb = new StringBuilder(128);
    _ = StrFormatByteSizeW(value, sb, (uint)sb.Capacity);
    return sb.ToString();
  }


  //// See: https://docs.microsoft.com/en-gb/windows/win32/api/fileapi/nf-fileapi-deletefilew
  //// See: https://www.pinvoke.net/default.aspx/kernel32/deletefile.html
  //// If the function succeeds, the return value is nonzero.
  //// If the function fails, the return value is zero (0). To get extended error information, call GetLastError.
  //[DllImport("kernel32.dll", SetLastError = true)]
  //[return: MarshalAs(UnmanagedType.Bool)]
  //public static extern bool DeleteFileW([MarshalAs(UnmanagedType.LPWStr)]string lpFileName);


}



