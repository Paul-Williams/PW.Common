using System.ComponentModel;
using System.Runtime.InteropServices;

namespace PW.Win32;

internal static class Win32Helper
{
  public static string GetErrorMessage(int errorCode) => new Win32Exception(errorCode).Message;

  public static string GetErrorMessage() => new Win32Exception().Message;

  public static int GetLastError() => Marshal.GetLastWin32Error();

}

