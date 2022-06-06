using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PW.Helpers;

/// <summary>
/// Enum helpers
/// </summary>
public static class EnumHelper
{
  /// <summary>
  /// Returns an array of values for the enumeration <typeparamref name="T"/>.
  /// </summary>
  /// <typeparam name="T">MUST BE AN ENUM !!</typeparam>
  public static T[] GetValues<T>() where T : Enum => (T[])Enum.GetValues(typeof(T)) ;

  /// <summary>
  /// The count of values in the Enum <typeparamref name="T"/>.
  /// </summary>
  public static int Count<T>() where T : Enum => GetValues<T>().Length;

  /// <summary>
  /// Creates a dictionary containing an Enum's descriptions and values. Any elements without a description are ignored and omitted from the dictionary.
  /// </summary>
  /// <typeparam name="T">An enum type.</typeparam>
  public static Dictionary<string, T> GetDescriptionValueDictionary<T>() where T : Enum
  {
    var list = new Dictionary<string, T>();

    // Changed for .NET 5 upgrade -- many methods now return nullable.
    // This seems really hacky now !!

    //var type = typeof(T);

    if (typeof(T) is Type typeofT)
    {
      foreach (T value in Enum.GetValues(typeofT))
      {
        var valueString = value.ToString();
        if (valueString is null) continue;

        var fieldInfo = typeofT.GetField(valueString);
        if (fieldInfo is null) continue;

        var typeofDescriptionAttribute = typeof(DescriptionAttribute);
        if (typeofDescriptionAttribute is null) continue;

        if (Attribute.GetCustomAttribute(fieldInfo, typeofDescriptionAttribute) is DescriptionAttribute attr)
        {
          list.Add(attr.Description, value);
        }
      }
    }
    return list;
  }
}
