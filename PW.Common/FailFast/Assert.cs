using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace PW.FailFast;

/// <summary>
/// Throws <see cref="AssertionException"/> when assertions fail. For many, the inner-exception will be a more specific exception.
/// Will also throw <see cref="ArgumentNullException"/> if the supplied argument(s) are null.
/// Note that, unlike with <see cref="Debug.Assert(bool)"/>, methods of this class will still be executed in a release build.
/// </summary>
public static class Assert
{

  // Simple helper to return a 'Class.MethodName' string. Leave 'caller' as null.
  private static string ClassMethodName([CallerMemberName] string caller = "") => nameof(Assert) + "." + caller;


  /// <summary>
  /// Throws exception if the file does not exist
  /// </summary>    
  public static void Exists(FileInfo file!!)
  {
    if (!file.Exists) throw new AssertionException(ClassMethodName(), "File not found: " + file.FullName, new FileNotFoundException());
  }

  /// <summary>
  /// Throws exception if the directory does not exist
  /// </summary>    
  public static void Exists(DirectoryInfo directory!!)
  {
    if (!directory.Exists) throw new AssertionException(ClassMethodName(), "Directory not found: " + directory.FullName, new DirectoryNotFoundException());
  }


  /// <summary>
  /// Throws exception if <paramref name="condition"/> is false.
  /// </summary>
  public static void IsTrue(bool condition, string message)
  {
    if (!condition) throw new AssertionException(ClassMethodName(), message);
  }

  /// <summary>
  /// Throws <see cref="AssertionException"/> if <paramref name="condition"/> is true.
  /// </summary>
  public static void IsFalse(bool condition, string message)
  {
    if (condition) throw new AssertionException(ClassMethodName(), message);
  }


  /// <summary>
  /// Throws exception is <paramref name="o"/> is null.
  /// </summary>    
  public static void IsNotNull<T>([ValidatedNotNull] T? o, [ValidatedNotNull] string message = "Must not be null.") where T : class
  {
    if (o is null) throw new AssertionException(ClassMethodName(), message ?? "Must not be null.");
  }

  /// <summary>
  /// Throws <see cref="ArgumentException"/> if <paramref name="o"/> is not null.
  /// </summary>
  public static void IsNull<T>(T? o, string message) where T : class
  {
    if (o != null) throw new AssertionException(ClassMethodName(), message);
  }

}
