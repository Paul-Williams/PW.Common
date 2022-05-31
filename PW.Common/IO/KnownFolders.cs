#nullable enable 

using System;
using System.Runtime.InteropServices;

namespace PW.IO
{
  // This was obtained from 'http://stackoverflow.com/questions/10667012/getting-downloads-folder-in-c' Ray Koopa's answer.
  // His full article is here: https://www.codeproject.com/Articles/878605/Getting-all-Special-Folders-in-NET
  // NB: The array '_knownFolderGuids' and 'KnownFolder' enum have both been greatly reduced from the original code.

  // NB: The order of the GUID lookup array '_knownFolderGuids' MUST match the order of enum 'KnownFolder'



  /// <summary>
  /// Class containing methods to retrieve specific file system paths.
  /// </summary>
  public static class KnownFolders

  {
    private static readonly string[] _knownFolderGuids = new string[]
    {
        "{56784854-C6CB-462B-8169-88E350ACB882}", // Contacts
        "{B4BFCC3A-DB2C-424C-B029-7FE99A87C641}", // Desktop
        "{FDD39AD0-238F-46AF-ADB4-6C85480369C7}", // Documents
        "{374DE290-123F-4565-9164-39C4925E467B}", // Downloads
        "{1777F761-68AD-4D8A-87BD-30B759FA33DD}", // Favorites
        "{BFB9D5E0-C6A9-404C-B2B2-AE6DB6AF4968}", // Links
        "{4BD8D571-6D19-48D3-BE97-422220080E43}", // Music
        "{33E28130-4E1E-4676-835A-98395C3BC3BB}", // Pictures
        "{4C5C32FF-BB9D-43B0-B5B4-2D72E54EAAA4}", // SavedGames
        "{7D1D3A04-DEBB-4115-95CF-2F29DA2920DA}", // SavedSearches
        "{18989B1D-99B5-455B-841C-AB7C74E4DDFC}", // Videos
        "{A52BBA46-E9E1-435f-B3D9-28DAA648C0F6}"  // OneDrive / SkyDrive
    };

    /// <summary>
    /// Gets the current path to the specified known folder as currently configured. This does
    /// not require the folder to be existent.
    /// </summary>
    /// <param name="knownFolder">The known folder which current path will be returned.</param>
    /// <returns>The default path of the known folder.</returns>
    /// <exception cref="System.Runtime.InteropServices.ExternalException">Thrown if the path
    ///     could not be retrieved.</exception>
    public static string GetPath(KnownFolder knownFolder) => GetPath(knownFolder, false);


    /// <summary>
    /// Gets the current path to the specified known folder as currently configured. This does
    /// not require the folder to be existent.
    /// </summary>
    /// <param name="knownFolder">The known folder which current path will be returned.</param>
    /// <param name="defaultUser">Specifies if the paths of the default user (user profile
    ///     template) will be used. This requires administrative rights.</param>
    /// <returns>The default path of the known folder.</returns>
    /// <exception cref="System.Runtime.InteropServices.ExternalException">Thrown if the path
    ///     could not be retrieved.</exception>
    public static string GetPath(KnownFolder knownFolder, bool defaultUser) => GetPath(knownFolder, KnownFolderFlags.DontVerify, defaultUser);

    private static Guid GuidFor(KnownFolder knownFolder) => new(_knownFolderGuids[(int)knownFolder]);

    private static string GetPath(KnownFolder knownFolder, KnownFolderFlags flags, bool defaultUser)
    {
      IntPtr Token() => new(defaultUser ? -1 : 0);

      int errorCode = SHGetKnownFolderPath(GuidFor(knownFolder), (uint)flags, Token(), out IntPtr outPath);

      if (errorCode >= 0)
      {
        // Code updated after reading comment about freeing up memory
        // See: https://www.codeproject.com/Messages/5375444/memory-leak.aspx
        var path = Marshal.PtrToStringUni(outPath);
        Marshal.FreeCoTaskMem(outPath);
        return path ?? throw new("Marshal.PtrToStringUni returned null.");
      }
      else throw new ExternalException("Unable to retrieve the known folder path. It may not be available on this system.", errorCode);

    }

    [DllImport("Shell32.dll")]
    private static extern int SHGetKnownFolderPath(
        [MarshalAs(UnmanagedType.LPStruct)]Guid rfid, uint dwFlags, IntPtr hToken,
        out IntPtr ppszPath);

    [Flags]
    private enum KnownFolderFlags : uint
    {
      SimpleIDList = 0x00000100,
      NotParentRelative = 0x00000200,
      DefaultPath = 0x00000400,
      Init = 0x00000800,
      NoAlias = 0x00001000,
      DontUnexpand = 0x00002000,
      DontVerify = 0x00004000,
      Create = 0x00008000,
      NoAppcontainerRedirection = 0x00010000,
      AliasOnly = 0x80000000
    }
  }

  /// <summary>
  /// Standard folders registered with the system. These folders are installed with Windows Vista
  /// and later operating systems, and a computer will have only folders appropriate to it
  /// installed.
  /// </summary>
  public enum KnownFolder
  {
    /// <summary>
    /// Contacts folder
    /// </summary>
    Contacts,
    /// <summary>
    /// Desktop folder
    /// </summary>
    Desktop,
    /// <summary>
    /// Documents folder
    /// </summary>
    Documents,
    /// <summary>
    /// Downloads folder
    /// </summary>
    Downloads,
    /// <summary>
    /// Favorites folder
    /// </summary>
    Favorites,
    /// <summary>
    /// Links folder
    /// </summary>
    Links,
    /// <summary>
    /// Music folder
    /// </summary>
    Music,
    /// <summary>
    /// Pictures folder
    /// </summary>
    Pictures,
    /// <summary>
    /// Saved Games folder
    /// </summary>
    SavedGames,
    /// <summary>
    /// Saved Searches folder
    /// </summary>
    SavedSearches,
    /// <summary>
    /// Videos folder
    /// </summary>
    Videos,
    /// <summary>
    /// OneDrive folder
    /// </summary>
    OneDrive
  }
}
