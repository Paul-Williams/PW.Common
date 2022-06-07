using System.Drawing;
using System.Drawing.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using static PW.Win32.SafeNativeMethods;

namespace MouseProfiles;

/// <summary>
/// Enables access to fonts embedded within an assembly's resource.
/// Uses the singleton pattern for app-wide sharing of fonts.
/// </summary>
internal class FontResources
{
  /// <summary>
  /// Private constructor -- Singleton pattern. Access class via Instance property.
  /// </summary>
  private FontResources() { }

  /// <summary>
  /// Returns a singleton instance.
  /// </summary>
  public static FontResources Instance { get; } = new();

  private PrivateFontCollection Fonts { get; } = new();

  private Dictionary<string, int> Lookup { get; } = new(StringComparer.OrdinalIgnoreCase);

  /// <summary>
  /// Creates a regular-style font of the specified family and size.
  /// </summary>
  public Font CreateFont(string family, int size) => CreateFont(family, size, FontStyle.Regular);

  /// <summary>
  /// Creates a font of the specified family, size and style.
  /// </summary>
  /// <exception cref="Exception">No embedded fonts or requested font family not found.</exception>
  public Font CreateFont(string family, int size, FontStyle style)
  {
    if (Fonts.Families.Length == 0)
    {
      AddAllEmbeddedFonts();
      if (Fonts.Families.Length == 0) throw new Exception("No embedded font resources found.");
      PopulateLookup();
    }

    return !Lookup.TryGetValue(family, out var index)
      ? throw new Exception("Font not found: " + family)
      : (new(Fonts.Families[index], size, style));
  }




  /// <summary>
  /// Returns just the embedded font resource names from the assembly manifest.
  /// </summary>  
  private static IEnumerable<string> EnumerateEmbeddedFontResources()
  {
    var resourceNames = Assembly.GetExecutingAssembly()
      .GetManifestResourceNames()
      .Where(x => x.EndsWith(".ttf", StringComparison.OrdinalIgnoreCase));

    foreach (var resourceName in resourceNames) yield return resourceName;

  }

  /// <summary>
  /// Creates a byte array from the specified assembly resource.
  /// </summary>
  private static byte[] GetResourceBytes(Assembly assembly, string resourceName)
  {
    using var stream = assembly.GetManifestResourceStream(resourceName);
    if (stream == null) throw new Exception(string.Format($"The embedded resource {resourceName} does not exist."));
    var bytes = new byte[stream.Length];
    stream.Read(bytes, 0, (int)stream.Length);
    return bytes;
  }

  /// <summary>
  /// Adds all embedded fonts to the PrivateFontCollection.
  /// </summary>
  private void AddAllEmbeddedFonts() => EnumerateEmbeddedFontResources().ForEach(AddFontFromResource);
  //{
  //  foreach (var resourceName in EnumerateEmbeddedFontResources())
  //    AddFontFromResource(resourceName);
  //}


  private void AddFontFromResource(string fontResourceName)
  {
    uint count = 0;

    var bytes = GetResourceBytes(Assembly.GetExecutingAssembly(), fontResourceName);
    var pointer = Marshal.AllocCoTaskMem(bytes.Length);
    Marshal.Copy(bytes, 0, pointer, bytes.Length);
    AddFontMemResourceEx(pointer, (uint)bytes.Length, IntPtr.Zero, ref count);
    Fonts.AddMemoryFont(pointer, bytes.Length);
    Marshal.FreeCoTaskMem(pointer);
  }

  /// <summary>
  /// Populates the lookup from font family name -> PrivateFontCollection.Families[index].
  /// </summary>
  private void PopulateLookup()
  {
    for (int i = 0; i < Fonts.Families.Length; i++)
      Lookup.Add(Fonts.Families[i].Name, i);
  }


}
