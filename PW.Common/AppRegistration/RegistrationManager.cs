namespace PW.AppRegistration;

using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System;


/// <summary>
/// Management of application registrations for LaunchPad.
/// </summary>
public static class RegistrationManager
{
  private const string RegPath = @"Software\PW\AppRegistration";

  /// <summary>
  /// Registers an application with LaunchPad using the application's product name.
  /// </summary>
  public static void Register(string title, string path)
  {
    if (string.IsNullOrWhiteSpace(title)) throw new System.ArgumentException($"'{nameof(title)}' cannot be null or whitespace", nameof(title));
    if (string.IsNullOrWhiteSpace(path)) throw new System.ArgumentException($"'{nameof(path)}' cannot be null or whitespace", nameof(path));

    using var key = GetAppRegKey();
    key.SetValue(title, path);
  }

  /// <summary>
  /// Registers the current application with LaunchPad. 
  /// </summary>
  public static void Register() => Register(GetProductName(), Assembly.GetExecutingAssembly().Location);

  /// <summary>
  /// Registers the current application with LaunchPad using a custom title. 
  /// </summary>
  public static void Register(string title) => Register(title, Assembly.GetExecutingAssembly().Location);




  /// <summary>
  /// Unregisters an application with LaunchPad.
  /// </summary>
  public static void UnRegister(string title)
  {
    if (string.IsNullOrWhiteSpace(title)) throw new System.ArgumentException($"'{nameof(title)}' cannot be null or whitespace", nameof(title));
    using var key = GetAppRegKey();
    key.DeleteValue(title, false);
  }

  /// <summary>
  /// Unregisters the current application with LaunchPad.
  /// </summary>
  public static void UnRegister() => UnRegister(GetProductName());

  /// <summary>
  /// Returns a list of all existing application registrations.
  /// </summary>
  public static List<(string Title, string Path)> GetRegistrations()
  {
    using var key = GetAppRegKey();
    return key.GetValueNames()
            .OrderBy(appTitle => appTitle)
            .Select(appTitle => (appTitle, key.GetValue(appTitle)!.ToString()!))
            .ToList();
  }

  /// <summary>
  /// Opens the registry key used by LaunchPad.
  /// </summary>   
  private static RegistryKey GetAppRegKey() => Registry.CurrentUser.CreateSubKey(RegPath);

  private static string GetProductName()
  {
    return Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyProductAttribute)) is AssemblyProductAttribute t
      ? t.Product
      : throw new Exception("AssemblyProductAttribute is null.");
  }
}
