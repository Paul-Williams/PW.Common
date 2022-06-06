 

using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace PW.Extensions
{
  /// <summary>
  /// Extension methods for Enums
  /// </summary>
  public static class EnumExtensions
  {
    /// <summary>
    /// Returns the <see cref="DescriptionAttribute"/> value for the supplied <see cref="Enum"/>, or <see cref="string.Empty"/> if none is found.
    /// </summary>
    public static string Description(this Enum value)
    {
      return value is null
          ? throw new ArgumentNullException(nameof(value))
          : value
            .GetType()
            .GetMember(value.ToString())
            .FirstOrDefault()
            ?.GetCustomAttribute<DescriptionAttribute>()
            ?.Description
            ?? string.Empty;
    }

    /// <summary>
    /// Formats enum string for display by inserting a space before each upper-case character.
    /// </summary>
    public static string DisplayName(this Enum e) => 
      e is null ? throw new ArgumentNullException(nameof(e)) : e.ToString().SpaceDelimitCapitals();
  }
}
