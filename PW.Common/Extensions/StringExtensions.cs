using System.Text.RegularExpressions;
using static CSharpFunctionalExtensions.Result;

namespace PW.Extensions;

/// <summary>
/// Extensions for the <see cref="String"/> class
/// </summary>
public static class StringExtensions
{
  /// <summary>
  /// 
  /// </summary>
  public const string EmptyString = "";
  /// <summary>
  /// 
  /// </summary>
  public const char Space = ' ';

  /// <summary>
  /// Backing field for <see cref="UpperCamelCaseRegex"/>
  /// </summary>
  private static Regex? _upperCamelCaseRegex;

  private static Regex UpperCamelCaseRegex =>
    _upperCamelCaseRegex ??= new Regex(@"(?<!^)((?<!\d)\d|(?(?<=[A-Z])[A-Z](?=[a-z])|[A-Z]))", RegexOptions.Compiled);

  /// <summary>
  /// Compresses the string.
  /// </summary>
  public static CompressedString Compress(this string str) => str is not null ? new CompressedString(str) : default!;


  /// <summary>
  /// Removes the last character from the end of the string and returns the new string. 
  /// </summary>
  public static string RemoveLastCharacter(this string str) => str is not null
      ? str.Length < 1
      ? string.Empty
      : str[0..^1]
      : throw new ArgumentNullException(nameof(str));
  //net48: str.Substring(0, str.Length - 1)

  /// <summary>
  /// Removes any numbers from just the end of the string. If the string is all numbers an empty string will be returned.
  /// </summary>
  public static string RemoveNumbersFromEnd(this string str) =>
    str.EndsWithNumber() ? str.Reverse().SkipWhile(c => char.IsNumber(c)).Reverse().AsString() : str;


  /// <summary>
  /// Returns just the numbers from the start of the string, up to the first non-numeric character.
  /// </summary>
  public static string NumbersFromStart(this string str) =>
    str.StartsWithNumber() ? str.TakeWhile(c => char.IsNumber(c)).AsString() : string.Empty;


  /// <summary>
  /// If any character within <paramref name="str"/> is a number returns true, otherwise returns false.
  /// </summary>
  public static bool ContainsAnyNumber(this string str!!) => str.Length != 0 && str.Any(char.IsNumber);

  /// <summary>
  /// Returns true if the last character in <paramref name="str"/> is a number, otherwise returns false.
  /// </summary>
  public static bool EndsWithNumber(this string str!!) => str.Length != 0 && char.IsNumber(str[^1]);

  /// <summary>
  /// Returns true if the first character of <paramref name="str"/> is a number, otherwise false.
  /// </summary>
  public static bool StartsWithNumber(this string str!!) => str.Length != 0 && char.IsNumber(str[0]);

  /// <summary>
  /// Determines whether <paramref name="str"/> starts with any of the strings in <paramref name="values"/>.
  /// </summary>
  public static bool StartsWithAny(this string str!!, IEnumerable<string> values!!, StringComparison comparisonType) =>
    str.Length != 0 && values.Any(x => str.StartsWith(x, comparisonType));

  /// <summary>
  /// Tests whether the <paramref name="str"/> ends with <paramref name="value"/>, as an ordinal comparison, ignoring case.
  /// </summary>
  public static bool EndWithIgnoreCase(this string str!!, string value!!) =>
    str.EndsWith(value, StringComparison.OrdinalIgnoreCase);

  /// <summary>
  /// Inserts a space before each capital letter, other than the first. Useful for converting enums, class names etc. to a 'display' string.
  /// </summary>
  /// <example>"NameOfSomething".SpaceDelimitCapitals() => "Name Of Something"/></example>
  public static string SpaceDelimitCapitals(this string s) => UpperCamelCaseRegex.Replace(s, " $1");

  /// <summary>
  /// Converts the specified string to title case (except for words that are entirely
  /// in uppercase, which are considered to be acronyms).
  /// </summary>
  public static string ToTitleCase(this string text) =>
    text is not null ? System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text)
    : throw new ArgumentNullException(nameof(text));

  /// <summary>
  /// Converts the specified string to title case. If <paramref name="text"/> is all capitals, set <paramref name="isAllCaps"/> to true.
  /// </summary>
  public static string ToTitleCase(this string text, bool isAllCaps = false) =>
    text is not null ? isAllCaps ? text.ToLower().ToTitleCase() : text.ToTitleCase()
    : throw new ArgumentNullException(nameof(text));

  /// <summary>
  /// Wraps the string in quotations.
  /// </summary>
  public static string Enquote(this string str) =>
    str is not null ? "\"" + str + "\""
    : throw new ArgumentNullException(nameof(str));

  /// <summary>
  /// Determines if the string starts and ends with a quotation mark.
  /// </summary>
  public static bool IsEnquoted(this string str) =>
    str is not null ? str.Length > 1 && str[0] == '"' && str[^1] == '"'
    : throw new ArgumentNullException(nameof(str));

  /// <summary>
  /// If the string is enclosed in quotes, then returns the string from within the quotes
  /// </summary>
  public static string Unenquote(this string str) =>
    str is not null
    ? !str.IsEnquoted() ? str : str.Length > 2 ? str[1..^1] : string.Empty
    : throw new ArgumentNullException(nameof(str));

  /// <summary>
  /// Appends char <paramref name="c"/> to the end of the string if it does not already exist.
  /// </summary>
  public static string EnsureEndsWith(this string str, char c) =>
    str is null ? throw new ArgumentNullException(nameof(str))
    : str.Length == 0 ? c.ToString()
    : str.EndsWith(c) ? str : str + c;

  /// <summary>
  /// Appends <paramref name="suffix"/> to the end of the string if it does not already exist.
  /// </summary>
  public static string EnsureEndsWith(this string str, string suffix) =>
    str is null ? throw new ArgumentNullException(nameof(str))
    : suffix is null ? throw new ArgumentNullException(nameof(suffix))
    : str.Length == 0 ? suffix : str.EndsWith(suffix, StringComparison.Ordinal) ? str : str + suffix;

  /// <summary>
  /// Appends a space to the string if it does not already end with a space.
  /// </summary>
  public static string EnsureEndsWithSpace(this string str) =>
    str[^1] == Space ? str : str + Space;  //str.EndsWith(Space) ? str : str + Space;

  /// <summary>
  /// Returns true if the string is zero length, false if it has a length. Throws an exception if it is null.
  /// </summary>
  /// <param name="str"></param>
  /// <returns></returns>
  /// <exception cref="ArgumentNullException"/>
  public static bool IsEmpty(this string str) =>
    str != null ? str.Length == 0 : throw new ArgumentNullException(nameof(str));

  /// <summary>
  /// Checks if the string is null or empty.
  /// </summary>
  public static bool IsNullOrEmpty(this string str) => string.IsNullOrEmpty(str);

  /// <summary>
  /// Checks is the string is null, empty or white-space.
  /// </summary>
  public static bool IsNullOrWhiteSpace(this string str) => string.IsNullOrWhiteSpace(str);

  /// <summary>
  /// Returns true when the string consists of only white-space characters. Returns false for null or empty string.
  /// </summary>
  public static bool IsWhiteSpace(this string value) => value.IsWhiteSpace(0);

  /// <summary>
  /// Returns true when the string consists of only white-space characters. Returns false if string is null or empty.
  /// Skips the first <paramref name="skip"/> chars.
  /// If <paramref name="skip"/> is greater or equal to the length of the string, then returns false.
  /// </summary>
  /// <param name="value">this</param>
  /// <param name="skip">Optionally skip the first N characters.</param>
  /// <returns></returns>
  public static bool IsWhiteSpace(this string value, int skip)
  {
    // Code is modified version of: https://referencesource.microsoft.com/#mscorlib/system/string.cs,55e241b6143365ef
    if (string.IsNullOrEmpty(value) || skip >= value.Length) return false;

    for (int i = skip; i < value.Length; i++)
    {
      if (!char.IsWhiteSpace(value[i])) return false;
    }

    return true;
  }

  /// <summary>
  /// Returns true when all character in the sting are <paramref name="c"/>. Returns false if string is null or empty.
  /// </summary>
  public static bool IsAll(this string value, char c)
  {
    if (string.IsNullOrEmpty(value)) return false;

    for (int i = 0; i < value.Length; i++)
    {
      if (value[i] != c) return false;
    }

    return true;
  }

  /// <summary>
  /// Returns the count of character <paramref name="c"/> within the string <paramref name="str"/>
  /// </summary>
  public static int CountOf(this string str, char c) => str.Aggregate(0, (a, x) => x == c ? a + 1 : a);

  /// <summary>
  /// Determines whether the string is composed only of numeric digits.
  /// </summary>
  public static bool IsAllNumbers(this string str) => str.All(char.IsDigit);

  /// <summary>
  /// Returns bool whether str2 is contained within str1. Ordinal case-insensitive comparison is used.
  /// </summary>
  public static bool ContainsIgnoreCase(this string str1, string str2) => str1.Contains(str2, StringComparison.OrdinalIgnoreCase);

  /// <summary>
  /// Returns bool whether str2 is contained within str1. Allows choice of string comparison method.
  /// </summary>
  public static bool Contains(this string str1, string str2, StringComparison stringComparison)
  {
    return str1 is null ? throw new ArgumentNullException(nameof(str1))
        : str2 is null ? throw new ArgumentNullException(nameof(str2))
        : str2.IsEmpty() || str1.IsEmpty() && str2.IsEmpty() || str1.IndexOf(str2, stringComparison) != -1;
  }

  /// <summary>
  /// Returns true if <paramref name="str1"/> contains any of the characters in <paramref name="chars"/>.
  /// </summary>
  public static bool ContainsAny(this string str1!!, params char[] chars!!) =>
    str1.Length != 0 && str1.Any(c => chars.Contains(c));


#if NET48

  /// <summary>
  /// Determines whether the character is contained within the string.
  /// </summary>
  public static bool ContainsChar(this string str!!, char c) => str.IndexOf(c) != -1;

  /// <summary>
  /// Determines whether a specified char is a prefix of the current instance.
  /// </summary>
  public static bool StartsWith(this string str!!, char c) =>  str.Length > 0 && str[0] == c;

  /// <summary>
  /// Determines whether a specified char is a suffix of the current instance.
  /// </summary>
  public static bool EndsWith(this string str!!, char c) =>  str.Length > 0 && str[^1] == c;
#endif


  /// <summary>
  /// Returns bool whether 'http://' or 'https://' is contained within the string
  /// </summary>
  /// <param name="str"></param>
  /// <returns></returns>
  public static bool ContainsHyperlink(this string str) =>
    (str.Contains("http://", StringComparison.OrdinalIgnoreCase) || str.Contains("https://", StringComparison.OrdinalIgnoreCase));

  /// <summary>
  /// Returns a limited length version of the string.
  /// </summary>
  public static string Truncate(this string str!!, int maxLength)
  {
    Guard.GreaterThanZero(maxLength, nameof(maxLength));
    return maxLength == 0 ? string.Empty : str.Length <= maxLength ? str : str[..maxLength];
  }

  /// <summary>
  /// Splits a string into an array, based on new lines. Empty lines are removed.
  /// </summary>
  /// <exception cref="ArgumentNullException"/>
  public static string[] SplitOnNewLine(this string lines!!)
  {
#if NET48
    return lines.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
#else
    return lines.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
#endif
  }

  /// <summary>
  /// Splits a string into an array, based on new lines.
  /// </summary>
  /// <exception cref="ArgumentNullException"/>
  public static string[] SplitOnNewLine(this string lines!!, StringSplitOptions splitOptions)
    => lines.Split(new string[] { Environment.NewLine }, splitOptions);

  /// <summary>
  /// Get the string slice between the two indexes.
  /// Inclusive for start index, exclusive for end index.
  /// A negative value n for <paramref name="end"/> will omit the last n characters.
  /// </summary>
  /// <exception cref="ArgumentNullException"/>
  /// <exception cref="ArgumentOutOfRangeException">End is greater than string length.</exception>
  public static string Slice(this string source!!, int start, int end)
  {
    if (end > source.Length) throw new ArgumentOutOfRangeException(nameof(end), $"Argument '{nameof(end)}' cannot be greater than the length of the string.");

    // Enable negative-end support
    if (end < 0) end = source.Length + end;

    return source[start..end];
  }


  /// <summary>
  /// Returns a copy of the original string with all occurrences of <paramref name="subString"/> removed.
  /// </summary>
  public static string RemoveAll(this string source!!, string subString!!) => source.Replace(subString, string.Empty);

  /// <summary>
  /// Removes all instances of <see cref="char"/> <paramref name="c"/> from the string.
  /// </summary>
  public static string RemoveAll(this string source!!, char c) => source.Where(x => x != c).AsString();


  /// <summary>
  /// Returns the substring between the locations of str1 and str2, within the source string.
  /// </summary>
  public static Result<string> ResultOfSubstringBetween(
    this string source!!,
    string str1!!,
    string str2!!,
    StringComparison comparisonType = StringComparison.OrdinalIgnoreCase,
    int startIndex = 0,
    string str1NotFoundError!! = "'str1' was not found.",
    string str2NotFoundError!! = "'str2' was not found.")
  {
    var str1Start = source.IndexOf(str1, startIndex, comparisonType);
    if (str1Start == Constants.NotFound) return Failure<string>(str1NotFoundError);
    var str1End = str1Start + str1.Length;

    var str2Start = source.IndexOf(str2, startIndex: str1End, comparisonType);
    return str2Start == Constants.NotFound ? Failure<string>(str2NotFoundError) : Success(source.Slice(str1End, str2Start));
  }

  /// <summary>
  /// Returns the substring between the locations of str1 and str2, within the source string.
  /// </summary>
  /// <param name="source">The string to parse</param>
  /// <param name="str1">Return string between this...</param>
  /// <param name="str2">...and this</param>
  /// <param name="comparisonType">String comparison type.</param>
  /// <param name="startIndex">Offset from the beginning at which to start parsing.</param>
  /// <param name="defaultValue">Returned when <paramref name="str1"/> or <paramref name="str2"/> is not found. Can be null.</param>
  /// <returns></returns>
  public static string SubstringBetween(
    this string source!!,
    string str1!!,
    string str2!!,
    StringComparison comparisonType = StringComparison.OrdinalIgnoreCase,
    int startIndex = 0,
    string defaultValue = EmptyString)
  {
    var str1Start = source.IndexOf(str1, startIndex, comparisonType);
    if (str1Start == -1) return defaultValue;
    var str1End = str1Start + str1.Length;

    var str2Start = source.IndexOf(str2, startIndex: str1End, comparisonType);
    return str2Start == Constants.NotFound ? defaultValue : source.Slice(str1End, str2Start);
  }


  /// <summary>
  /// Checks whether the string starts with either 'https://' or 'http://'. Throws <see cref="ArgumentNullException"/>.
  /// </summary>
  /// <param name="str"></param>
  /// <returns></returns>
  public static bool IsUrl(this string str!!)
    => str.StartsWith("https://", StringComparison.OrdinalIgnoreCase) || str.StartsWith("http://", StringComparison.OrdinalIgnoreCase);

  /// <summary>
  /// Enumerates a string as a series of lines.
  /// </summary>
  public static IEnumerable<string> ReadLines(this string str!!)
  {
    if (str is null) yield break;
    using var sr = new StringReader(str);
    while (sr.ReadLine() is string s) yield return s;
  }


  /// <summary>
  /// Return a substring after the last instance of the specified character. If the character is not found the original string is returned.
  /// </summary>
  /// <param name="str"></param>
  /// <param name="c"></param>
  /// <returns></returns>
  /// <remarks>If original string is nothing, returns nothing. If original string's length is zero, returns String.Empty. If c is not found, returns the original string.</remarks>
  public static string SubstringAfterLast(this string str!!, char c)
  {
    if (str.Length == 0) return string.Empty;

    var startIndex = str.LastIndexOf(c) + 1;

    // LastIndexOf returns -1 for not-found, but we added 1, so not-found will be 0.
    const int CharNotFound = 0;

    // The char was found and it was not the last char in the string.
    return (startIndex != CharNotFound) && (startIndex < str.Length) ? str[startIndex..] : string.Empty;
  }

  /// <summary>
  /// Attempts to convert <paramref name="str"/> to an <see cref="int"/>. If convertion is not possible <paramref name="defaultValue"/> is returned.
  /// </summary>
  public static int ToIntOrDefault(this string str, int defaultValue = 0) => int.TryParse(str, out var i) ? i : defaultValue;

  /// <summary>
  /// If the string contains leading-zeros, returns the length of the string, otherwise zero.
  /// This seems quite naff. [Sometime much later...] Indeed it does!
  /// </summary>
  public static int ZeroPaddedLength(this string str!!) => str[0] == '0' ? str.Length : 0;


  /// <summary>
  /// If <paramref name="matchTo"/> has leading-zero padding, then <paramref name="str"/> is padded to the same length, otherwise <paramref name="str"/>
  /// is returned as-is.
  /// </summary>
  /// <param name="str">The string to possibly be leading-zero padded.</param>
  /// <param name="matchTo">The string to match against to determine whether to pad <paramref name="str"/>.</param>
  /// <returns></returns>
  public static string MatchZeroPadding(this string str!!, string matchTo!!)
    => matchTo[0] == '0' ? str.PadLeft(matchTo.Length, '0') : str;
}




