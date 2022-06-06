using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace PW.Extensions;

/// <summary>
/// Extension methods for Enums
/// </summary>
public static class EnumExtensions
{
  /// <summary>
  /// Returns the <see cref="DescriptionAttribute.Description"/> value for the supplied <see cref="Enum"/>, or <see cref="string.Empty"/> if none is found.
  /// </summary>
  public static string Description(this Enum enumMember!!)
  {
    return enumMember
      .GetType()
      .GetMember(enumMember.ToString())
      .FirstOrDefault()?
      .GetCustomAttribute<DescriptionAttribute>()?
      .Description
      ?? string.Empty;
  }

  /// <summary>
  /// Returns the <see cref="DescriptionAttribute.Description"/> value for the supplied <see cref="Enum"/>.
  /// </summary>
  /// <exception cref="Exception">Thrown if <paramref name="enumMember"/> is not defined or does not have a description attribute.</exception>
  public static string Description(this Enum enumMember, bool throwExceptions)
  {
    if (!throwExceptions) return Description(enumMember);
    
    var member = enumMember.GetType().GetMember(enumMember.ToString()).FirstOrDefault() 
      ?? throw new Exception($"Enum member not defined: {enumMember}.");

    return member.GetCustomAttribute<DescriptionAttribute>()?.Description
      ?? throw new Exception($"Member {enumMember} does not have a description attribute.");
  }

  /// <summary>
  /// Formats enum string for display by inserting a space before each upper-case character.
  /// </summary>
  public static string DisplayName(this Enum enumMember!!) => enumMember.ToString().SpaceDelimitCapitals();
}
