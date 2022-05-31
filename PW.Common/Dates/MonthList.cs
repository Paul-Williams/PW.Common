#nullable enable 

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PW.Dates
{
  /// <summary>
  /// Singleton class providing an list of month names, in English.
  /// </summary>
  public class MonthList : IReadOnlyList<Months>
  {
    private MonthList() { }

    private static IReadOnlyList<Months> Months { get; } = Enumerable.Range(1, 12).Select(m => (Months)m).ToArray();

    /// <summary>
    /// Returns the singleton instance of the class
    /// </summary>
    public static MonthList Instance { get; } = new MonthList();

    /// <summary>
    /// The number of months in a year
    /// </summary>
    public int Count => 12;

    /// <summary>
    /// Returns a month by index
    /// </summary>
    public Months this[int index] => Months[index];

    /// <summary>
    /// Returns an enumerator to iterate through the months
    /// </summary>
    public IEnumerator<Months> GetEnumerator() => Months.GetEnumerator();


    IEnumerator IEnumerable.GetEnumerator() => Months.GetEnumerator();

  }
}
