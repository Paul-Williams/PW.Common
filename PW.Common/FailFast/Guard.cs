using PW.Extensions;

namespace PW.FailFast;

/// <summary>
/// Guard class for argument validation, such as null-checking etc.
/// </summary>
public static partial class Guard
{
  //  /// <summary>
  //  /// Guards against invalid object type. 
  //  /// Throws an exception if <paramref name="argument"/> is not of type <typeparamref name="T"/>
  //  /// </summary>
  //  public static void IsOfType<T>(object argument, string argumentName) where T : class
  //  {
  //    NotNull(argument, argumentName);
  //    throw new ArgumentException($"Argument '{argumentName}' is not of type {typeof(T).FullName}");
  //  }

  /// <summary>
  /// Guard against null reference arguments etc. Example usage: <code>obj.GuardNull(nameof(obj))</code>
  /// Throws <see cref="ArgumentNullException"/> if <paramref name="argument"/> is null.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="argument">The instance variable to be null-guarded.</param>
  /// <param name="argumentName">Name of the argument in code. Use nameof() to pass this string.</param>
  /// <exception cref="ArgumentNullException"></exception>
  public static void NotNull<T>([ValidatedNotNull] T? argument, [ValidatedNotNull] string argumentName) where T : class
  {
    if (argument is null) throw new ArgumentNullException(argumentName ?? "<-- ARGUMENT NAME IS NULL !! -->");
  }

  //internal static void NullGuard<T>([ValidatedNotNull] this T? argument, string argumentName) where T : class
  //{
  //  if (argument is null) throw new ArgumentNullException(argumentName ?? "<-- ARGUMENT NAME IS NULL !! -->");
  //}

  // NB: Changed from IEnumerable<T> for safety. Otherwise could cause multiple-execution side-effects or performance issues.
  /// <summary>
  /// Throws <see cref="Exception"/> is the collection contains any null items.
  /// </summary>
  public static void NoNulls<T>([ValidatedNotNull] ICollection<T>? argument, [ValidatedNotNull] string argumentName) where T : class
  {
    if (argumentName is null) argumentName = "<-- ARGUMENT NAME IS NULL !! -->";
    if (argument is null) throw new ArgumentNullException(argumentName);
    if (argument.ContainsNulls()) throw new Exception($"Collection '{argumentName}' contains one or more null elements.");
  }


  /// <summary>
  /// Throws <see cref="ArgumentException"/> if <paramref name="argumentTest"/> is true.
  /// </summary>
  public static void False(bool argumentTest, string argumentName!!, string failureMessage!!)
  {
    if (argumentTest) throw new ArgumentException(failureMessage, argumentName);
  }



  // This does not follow the design principle that an argument is being tested
  //public static void True(Func<bool> argumentTest, string argumentName, string failureMessage)
  //{
  //  NotNull(argumentTest, nameof(argumentTest));
  //  NotNull(argumentName, nameof(argumentName));
  //  NotNull(failureMessage, nameof(failureMessage));
  //  if (!argumentTest()) throw new ArgumentException(failureMessage, argumentName);

  //}

  /// <summary>
  /// Throws <see cref="ArgumentException"/> if <paramref name="argumentTest"/> is false.
  /// </summary>
  public static void True(bool argumentTest, string argumentName!!, string failureMessage!!)
  {
    if (!argumentTest) throw new ArgumentException(failureMessage, argumentName);
  }

  /// <summary>
  /// Throws <see cref="ArgumentException"/> if <paramref name="argumentTest"/> is false.
  /// </summary>
  public static void True(bool argumentTest, string failureMessage!!)
  {
    if (!argumentTest) throw new ArgumentException(failureMessage);
  }

  /// <summary>
  /// Throws <see cref="ArgumentException"/> if <paramref name="argument"/> is null or white-space.
  /// </summary>
  public static void NotNullOrWhitespace(string argument, string argumentName)
  {
    if (string.IsNullOrWhiteSpace(argument))
      throw new ArgumentException($"Argument '{argumentName}' is null or white-space.");
  }


  /// <summary>
  /// Throws <see cref="ArgumentException"/> if <paramref name="argument"/> is not greater than zero.
  /// </summary>
  public static void GreaterThanZero(int argument, string argumentName)
  {
    if (argument < 1) throw new ArgumentException(argumentName + " must be greater than zero.");
  }

  /// <summary>
  /// Throws <see cref="ArgumentException"/> if <paramref name="argument"/> is not greater than zero.
  /// </summary>
  public static void GreaterThan(int argument, int value, string argumentName)
  {
    if (argument <= value) throw new ArgumentException(argumentName + $" must be greater than {value}.");
  }

  /// <summary>
  /// Throws <see cref="ArgumentException"/> if <paramref name="argument"/> is not greater than zero.
  /// </summary>
  public static void GreaterThanOrEqualTo(int argument, int value, string argumentName)
  {
    if (argument < value) throw new ArgumentException(argumentName + $" must be greater than or equal to {value}.");
  }


  /// <summary>
  /// Throws <see cref="ArgumentException"/> if <paramref name="argument"/> is not zero or greater.
  /// </summary>
  public static void ZeroOrGreater(int argument, string argumentName)
  {
    if (argument < 0) throw new ArgumentException(argumentName + " must be zero or greater.");
  }

  /// <summary>
  /// Ensures that <paramref name="directory"/> is not null and that it exists.
  /// </summary>
  public static void MustExist(DirectoryInfo directory!!, string argumentName!!)
  {
    if (!directory.Exists) throw new DirectoryNotFoundException($"The directory '{directory.FullName}' supplied as argument '{argumentName}' does not exist.");
  }

  /// <summary>
  /// Ensures that <paramref name="directory"/> is not null and that it exists.
  /// </summary>
  public static void MustExist(DirectoryPath directory!!, string argumentName!!)
  {
    if (!directory.Exists) throw new DirectoryNotFoundException($"The directory '{directory.Value}' supplied as argument '{argumentName}' does not exist.");
  }

  /// <summary>
  /// Ensures that <paramref name="file"/> is not null and that it exists.
  /// </summary>
  public static void MustExist(FileInfo file!!, string argumentName!!)
  {
    if (!file.Exists) throw new FileNotFoundException($"The directory '{file.FullName}' supplied as argument '{argumentName}' does not exist.");
  }

  /// <summary>
  /// Ensures that <see cref="FilePath"/> is not null and that the file it exists on disk.
  /// </summary>
  public static void MustExist(FilePath file!!, string argumentName!!)
  {
    if (!file.Exists) throw new FileNotFoundException($"The directory '{file.Value}' supplied as argument '{argumentName}' does not exist.");
  }


  /// <summary>
  /// Ensures that <paramref name="o"/> is not null and that it exists.
  /// </summary>
  public static void MustExist(FileSystemInfo o!!, string argumentName!!)
  {
    if (!o.Exists) throw new FileNotFoundException($"The file system object '{o.FullName}' supplied as argument '{argumentName}' does not exist.");
  }

  /// <summary>
  /// Ensures that <paramref name="o"/> is not null and that it exists.
  /// </summary>
  public static void MustExist(IFileSystemPath o!!, string argumentName!!)
  {
    if (!o.Exists) throw new FileNotFoundException($"The file system object '{o.Value}' supplied as argument '{argumentName}' does not exist.");
  }

}

