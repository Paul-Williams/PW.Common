#nullable enable 

using System.ComponentModel;
using System.Runtime.InteropServices;

namespace PW
{
  internal static class Win32Helper
  {
    public static string GetWin32ErrorMessage(int errorCode) => new Win32Exception(errorCode).Message;

    public static string GetWin32ErrorMessage() => new Win32Exception().Message;

    public static int GetLastWin32Error() => Marshal.GetLastWin32Error();

  }
}

