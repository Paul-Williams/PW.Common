namespace PW.Extensions;

/// <summary>
/// Extension methods for use with IEnumerable.
/// </summary>
public static class IEnumerableExtensions
{
  #region Public Methods

  /// <summary>
  /// Encloses each string in the sequence within quotation marks.
  /// </summary>
  public static IEnumerable<string> Enquote(this IEnumerable<string> seq) =>
    seq.Select(StringExtensions.Enquote);

  /// <summary>
  /// Returns a dictionary of duplicates which match <paramref name="keySelector"/>.
  /// </summary>
  public static IDictionary<TKey, int> FindDuplicateCounts<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) where TKey : notnull =>
    source.FindDuplicates(keySelector).ToDictionary(x => x.Key, y => y.Count());

  /// <summary>
  /// Returns groups of duplicates which match <paramref name="keySelector"/>
  /// </summary>
  public static IEnumerable<IGrouping<TKey, TSource>> FindDuplicates<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) =>
        source.GroupBy(keySelector).LargerThan(1);


  /// <summary>
  /// Returns either the first item from the list or, if that is null, <paramref name="defaultValue"/>.
  /// </summary>
  public static T FirstOrDefault<T>(this IEnumerable<T> list, T defaultValue) where T : class
    => list.FirstOrDefault() ?? defaultValue;

  /// <summary>
  /// Returns either the first item from the list or, if that is null, <paramref name="defaultValue"/>.   
  /// </summary>
  public static T FirstOrDefault<T>(this IEnumerable<T> list, Func<T, bool> predicate, T defaultValue) where T : class
    => list.FirstOrDefault(predicate) ?? defaultValue;

  /// <summary>
  /// Performs <paramref name="action"/> or each item in <paramref name="seq"/>.
  /// </summary>
  public static void ForEach<T>(this IEnumerable<T> seq!!, Action<T> action!!)
  {
    foreach (var x in seq) action(x);
  }

  /// <summary>
  /// Performs <paramref name="f"/> for each item in <paramref name="seq"/>
  /// </summary>
  public static IEnumerable<T> ForEach<T>(this IEnumerable<T> seq!!, Func<T, T> f!!)
  {
    foreach (var x in seq) yield return f.Invoke(x);
  }

  /// <summary>
  /// Performs <paramref name="f"/> for each item in <paramref name="seq"/>
  /// </summary>
  public static IEnumerable<TR> ForEach<T, TR>(this IEnumerable<T> seq!!, Func<T, TR> f!!)
  {
    foreach (var x in seq) yield return f.Invoke(x);
  }


  /// <summary>
  /// Returns a single string containing each string in the sequence separated by the specified separator.
  /// </summary>
  public static string Join(this IEnumerable<string> seq, string separator, bool enquote = false) =>
    string.Join(separator, !enquote ? seq : seq.Select(x => $"\"{x}\""));

#if NET5_0_OR_GREATER
  private static string StringJoinInternal(char separator, IEnumerable<string> seq) => string.Join(separator, seq);
#else
  private static string StringJoinInternal(char separator, IEnumerable<string> seq ) => string.Join(separator.ToString(), seq);
#endif


  /// <summary>
  /// Returns a single string containing each string in the sequence separated by the specified separator.
  /// </summary>
  public static string Join(this IEnumerable<string> seq, char separator, bool enquote = false) =>
    seq is not null
      ? StringJoinInternal(separator, !enquote ? seq : seq.Select(x => $"\"{x}\""))
      : throw new ArgumentNullException(nameof(seq));

  /// <summary>
  /// Returns a single string containing each string in the sequence separated by a comma.
  /// </summary>
  public static string JoinWithCommas(this IEnumerable<string> seq, bool enquote = false) => Join(seq, ',', enquote);

  /// <summary>
  /// Returns a single string containing each string in the sequence separated by a space.
  /// </summary>
  public static string JoinWithSpaces(this IEnumerable<string> seq, bool enquote = false) => Join(seq, ' ', enquote);

  /// <summary>
  /// Performs a natural ascending ordering.
  /// </summary>
  public static IEnumerable<string> NaturalOrder(this IEnumerable<string> seq) =>
    seq.OrderBy(x => x, StringNaturalComparer.AscendingComparer);

  /// <summary>
  /// Performs the specified natural ordering.
  /// </summary>
  public static IEnumerable<string> NaturalOrder(this IEnumerable<string> seq, SortOrder sortOrder)
    => seq.OrderBy(x => x, StringNaturalComparer.GetInstance(sortOrder));

  ///// <summary>
  ///// Returns a single string containing each string in the sequence separated by the specified separator.
  ///// </summary>
  //[Obsolete("Use Join()")]
  //public static string Concat(this IEnumerable<string> seq, string separator) => string.Join(separator, seq);
  /// <summary>
  /// Performs a natural descending ordering.
  /// </summary>
  public static IEnumerable<string> NaturalOrderDescending(this IEnumerable<string> seq) =>
    seq.OrderBy(x => x, StringNaturalComparer.DescendingComparer);

  /// <summary>
  /// Opposite of <see cref="Enumerable.Any{TSource}(IEnumerable{TSource})"/>
  /// </summary>
  public static bool None<T>(this IEnumerable<T> seq) => !seq.Any();

  /// <summary>
  /// Opposite of <see cref="Enumerable.Any{TSource}(IEnumerable{TSource}, Func{TSource, bool})"/>
  /// </summary>
  public static bool None<T>(this IEnumerable<T> seq, Func<T, bool> predicate) => !seq.Any(predicate);

  /// <summary>
  /// Returns a new sequence where each element in <paramref name="seq"/> is repeated <paramref name="count"/> times.
  /// </summary>
  public static IEnumerable<T> RepeatEach<T>(this IEnumerable<T> seq!!, int count)
  {
    if (count < 1) throw new ArgumentException($"Argument '{nameof(count)}' must be greater than zero.");

    foreach (var t in seq) for (int i = 0; i < count; i++) yield return t;
  }

  ///// <summary>
  ///// Performs <paramref name="funcs"/> for each item in <paramref name="seq"/>
  ///// </summary>
  //[Obsolete("Use " + nameof(ForEachExecute))]
  //public static IEnumerable<T> Select<T>(this IEnumerable<T> seq, FunctionPipeline<T> funcs) => ForEachExecute(seq, funcs);


  /// <summary>
  /// Skips over any nulls found within the sequence.
  /// </summary>
  public static IEnumerable<T> SkipNulls<T>(this IEnumerable<T> seq) where T : class => seq.OfType<T>();

  /// <summary>
  /// Trims white-space from each string in the enumeration. Employs Deferred Execution with Lazy Evaluation
  /// </summary>
  public static IEnumerable<string> Trim(this IEnumerable<string> strings!!)
  {
    foreach (var str in strings) yield return str.Trim();
  }

  /// <summary>
  /// Trims trailing white-space from each string in the enumeration. Employs Deferred Execution with Lazy Evaluation
  /// </summary>
  public static IEnumerable<string> TrimEnd(this IEnumerable<string> strings!!)
  {
    foreach (var str in strings) yield return str.TrimEnd();
  }
  /// <summary>
  /// Trims leading white-space from each string in the enumeration. Employs Deferred Execution with Lazy Evaluation
  /// </summary>
  public static IEnumerable<string> TrimStart(this IEnumerable<string> strings!!)
  {
    foreach (var str in strings) yield return str.TrimStart();
  }

  #endregion Public Methods
}
