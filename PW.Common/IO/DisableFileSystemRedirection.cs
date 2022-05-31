#nullable enable 

using System;
using System.Runtime.InteropServices;

namespace PW.IO
{
  /// <summary>
  /// Disables file system redirection for the current thread of a 32-bit application.
  /// Does nothing if the current application is 64-bit.
  /// Dispose method reverts redirection. Use in a using block, for example.
  /// Calls native methods: Wow64DisableWow64FsRedirection() and Wow64RevertWow64FsRedirection()
  /// </summary>
  public sealed class DisableFileSystemRedirection : IDisposable
  {
    readonly IntPtr _wow64Value = IntPtr.Zero;
    bool _isDisabled;//= false;

    // See: https://msdn.microsoft.com/en-us/library/windows/desktop/aa365743(v=vs.85).aspx
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool Wow64DisableWow64FsRedirection(ref IntPtr ptr);

    // See: https://msdn.microsoft.com/en-us/library/windows/desktop/aa365745(v=vs.85).aspx
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool Wow64RevertWow64FsRedirection(IntPtr ptr);

    /// <summary>
    /// Creates a new instance which disables redirection until it is disposed.
    /// </summary>
    public DisableFileSystemRedirection()
    {

      // File system redirection only applies to 32-bit programs
      if (Environment.Is64BitProcess) return;

      // Disable redirection.
      _isDisabled = Wow64DisableWow64FsRedirection(ref _wow64Value);

    }

    /// <summary>
    /// Reverts file system redirection.
    /// </summary>
    public void Dispose()
    {
      // Re-enable redirection.
      if (_isDisabled)
      {
        Wow64RevertWow64FsRedirection(_wow64Value);
        _isDisabled = false;
      }
    }
  }

}
