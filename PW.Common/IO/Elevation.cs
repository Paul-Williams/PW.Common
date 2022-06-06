using CSharpFunctionalExtensions;
using System;
using System.Diagnostics;
using static CSharpFunctionalExtensions.Result;

namespace PW.IO;

// On user cancellation exception thrown:
// System.ComponentModel.Win32Exception (0x80004005)
//		ErrorCode	-2147467259	int


/// <summary>
/// For creation of process with user rights elevation.
/// </summary>
public static class Elevation
{
  private const int HRESULT_E_FAIL = -2147467259;//0x80004005;


  /// <summary>
  /// Runs/opens the specified executable/file with administrative rights.
  /// Only catches 'user-cancelled' exception when <paramref name="ignoreCancellationException"/> is true.
  /// </summary>
  /// <returns>On success: Maybe{Process}. On cancellation: Maybe{Process}.None. On other exception: throws.</returns>
  public static Maybe<Process> RunAsAdministrator(string path, bool ignoreCancellationException)
  {
    try { return Process.Start(new ProcessStartInfo(path) { Verb = "runas" })!; }
    // Catch this specifically as it is expected. It happens if the user cancels the UAC prompt.
    catch (System.ComponentModel.Win32Exception ex) when (ignoreCancellationException == true && ex.ErrorCode == HRESULT_E_FAIL)
    { return Maybe<Process>.None; }
  }

  /// <summary>
  /// Executes / opens the specified file with administrator rights.
  /// Handles no exceptions.
  /// </summary>
  public static Process? RunAsAdministrator(string path) => Process.Start(new ProcessStartInfo(path) { Verb = "runas" });


  /// <summary>
  /// Executes / opens the specified file with administrator rights.
  /// Wraps exceptions from <see cref="Process.Start()"/>
  /// </summary>
  public static Result<Process> TryRunAsAdministrator(string path)
  {
    try { return Success(RunAsAdministrator(path)!); } // BUG??
    catch (Exception ex) { return Failure<Process>(ex.Message); }
  }


}
