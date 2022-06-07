namespace PW.IO;

/// <summary>
/// Equality comparer for <see cref="DirectoryInfo"/> objects using FullName equality, rather than reference equality.
/// Ordinal case insensitive comparison is used.
/// </summary>
public class DirectoryInfoPathEqualityComparer : IEqualityComparer<DirectoryInfo>
{
  /// <summary>
  /// Default instance.
  /// </summary>
  public static DirectoryInfoPathEqualityComparer Instance { get; } = new DirectoryInfoPathEqualityComparer();

  /// <summary>
  /// Returns true if the two <see cref="DirectoryInfo"/> instances have the same path. Otherwise returns false.
  /// Casing is ignored.
  /// </summary>
  public bool Equals(DirectoryInfo? x, DirectoryInfo? y) =>
    (x != null && y != null && string.Equals(x.FullName, y.FullName, System.StringComparison.OrdinalIgnoreCase));

  /// <summary>
  /// Returns the hash code for FullName.
  /// </summary>
  public int GetHashCode(DirectoryInfo obj!!)
  {

    // Added the .ToLower() as a bug fix. (Could also have used .ToUpper(), no difference)
    // Found that (in this case) HashSet<DirectoryInfo>.Contains() would otherwise return false
    // for a DirectoryInfo which DID exist in set, but had different casing for the FullPath property.
    // Assume that this would also happen elsewhere, when hash codes are used for comparison.
    return obj.FullName.ToLower().GetHashCode();
  }
}