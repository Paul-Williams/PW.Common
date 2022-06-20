using PW.Win32;

namespace PW.Collections;

/// <summary>
/// Performs 'natural' compare, sort and equality tests for strings and classes implementing <see cref="IReadOnlyValue{T}"/> of string
/// Digits in the strings are considered as numerical content rather than text. Tests are not case-sensitive.
/// </summary>
public class StringNaturalComparer : IComparer<IReadOnlyValue<string?>?>, IEqualityComparer<IReadOnlyValue<string?>?>, IComparer, IEqualityComparer, IComparer<string?>, IEqualityComparer<string?>
{

  #region Constructors

  /// <summary>
  /// Creates a new instance which sorts in ascending order.
  /// </summary>
  public StringNaturalComparer() => SortOrder = SortOrder.Ascending;

  /// <summary>
  /// Creates a new instance with the specified <see cref="PW.SortOrder"/>
  /// </summary>
  public StringNaturalComparer(SortOrder direction) => SortOrder = direction;


  #endregion

  #region Static Instances

  /// <summary>
  /// Static instance of an ascending natural string comparer.
  /// </summary>
  public static StringNaturalComparer AscendingComparer { get; } = new StringNaturalComparer(SortOrder.Ascending);

  /// <summary>
  /// Static instance of an descending natural string comparer.
  /// </summary>
  public static StringNaturalComparer DescendingComparer { get; } = new StringNaturalComparer(SortOrder.Descending);

  /// <summary>
  /// Returns a static instance with the specified <see cref="SortOrder"/>.
  /// </summary>
  public static StringNaturalComparer GetInstance(SortOrder sortOrder)
    => sortOrder == SortOrder.Ascending ? AscendingComparer : DescendingComparer;

  #endregion

  /// <summary>
  /// The sort order of this <see cref="StringNaturalComparer"/> instance.
  /// </summary>
  public SortOrder SortOrder { get; set; }



  #region Implementation of IComparer<string>, IEqualityComparer<string>

  /// <summary>
  /// Compares two instances.
  /// </summary>
  public int Compare(string? x, string? y)
  {
    // StrCmpLogicalW returns -2 if either/both are null.
    // This does not match the framework return values, so handle nulls here rather than
    // allowing the API to do so.
    if (ReferenceEquals(x, y)) return 0;

    // Invert x and y for descending comparison.
    if (SortOrder == SortOrder.Descending) Helpers.Misc.Swap(ref x, ref y);

    if (x is null) return -1;
    if (y is null) return 1;

    //See: https://docs.microsoft.com/en-us/windows/win32/api/shlwapi/nf-shlwapi-strcmplogicalw

    return SafeNativeMethods.StrCmpLogicalW(x, y);
  }

  /// <summary>
  /// Compares two strings for equality
  /// </summary>
  public bool Equals(string? x, string? y) => string.Equals(x, y, StringComparison.OrdinalIgnoreCase);

  /// <summary>
  /// Gets the hash code for the specified string
  /// </summary>
  public int GetHashCode(string? str) =>
    str is null
    ? throw new ArgumentNullException(nameof(str))
    : StringComparer.OrdinalIgnoreCase.GetHashCode(str);

  #endregion

  #region Implementation of IComparer<IObjectValue<string>>, IEqualityComparer<IObjectValue<string>>

  /// <summary>
  /// Compares two instances.
  /// </summary>
  public int Compare(IReadOnlyValue<string?>? x, IReadOnlyValue<string?>? y) => Compare(x?.Path, y?.Path);

  /// <summary>
  /// Determines whether two instances are equal.
  /// </summary>
  public bool Equals(IReadOnlyValue<string?>? x, IReadOnlyValue<string?>? y) => Equals(x?.Path, y?.Path);

  /// <summary>
  /// Returns a hashcode for the instance.
  /// </summary>
  public int GetHashCode(IReadOnlyValue<string?>? obj) => GetHashCode(obj?.Path);

  #endregion


  #region Implementation of IComparer and IEqualityComparer

  /// <summary>
  /// 
  /// </summary>
  public int Compare(object? x, object? y)
  {
    if (ReferenceEquals(x, y)) return 0;
    if (x is string sx && y is string sy) return Compare(sx, sy);
    if (x is IReadOnlyValue<string> vx && y is IReadOnlyValue<string> vy) return Compare(vx.Path, vy.Path);

    // Invert x and y for descending comparison.
    if (SortOrder == SortOrder.Descending) Helpers.Misc.Swap(ref x, ref y);

    return x is null
      ? -1
      : y is null
        ? 1
        : x is IComparable xc
          ? xc.CompareTo(y)
          : throw new ArgumentException("Implement IComparable.");
  }

  /// <summary>
  /// 
  /// </summary>
  public new bool Equals(object? x, object? y)
  {
    return x is string sx && y is string sy
        ? Equals(sx, sy)
        : x is IReadOnlyValue<string> vx && y is IReadOnlyValue<string> vy
          ? Equals(vx.Path, vy.Path)
          : object.Equals(x, y);
  }

  /// <summary>
  /// 
  /// </summary>
  public int GetHashCode(object obj)
  {
    // Implementation taken and modified from System.StringComparer:
    // See: https://referencesource.microsoft.com/#mscorlib/system/stringcomparer.cs,65a413891296af3a
    return obj is null
        ? throw new ArgumentNullException(nameof(obj))
        : obj is string s ? GetHashCode(s) : obj is IReadOnlyValue<string> vs ? GetHashCode(vs.Path) : obj.GetHashCode();
  }

  #endregion
}


