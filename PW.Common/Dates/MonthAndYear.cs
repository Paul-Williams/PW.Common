using System;
using System.Collections.Generic;

namespace PW.Dates;

/// <summary>
/// Simple class to store and operate on a month and year as a single unit.
/// </summary>
public class MonthAndYear : CSharpFunctionalExtensions.ValueObject
{

  /// <summary>
  /// Year component of the Month and Year pair
  /// </summary>
  public int Year { get; }

  /// <summary>
  /// Month component of the Month and Year pair
  /// </summary>
  public Months Month { get; }

  /// <summary>
  /// Constructs an instance using the current month and year.
  /// </summary>
  public MonthAndYear()
  {
    Year = DateTime.Now.Year;
    Month = (Months)DateTime.Now.Month;
  }

  /// <summary>
  /// Constructs an instance for the specified month and year.
  /// </summary>
  /// <param name="month"></param>
  /// <param name="year"></param>
  public MonthAndYear(Months month, int year)
  {
    if ((int)month is < 1 or > 12) throw new ArgumentOutOfRangeException(nameof(month));
    Month = month;
    Year = year;
  }


  /// <summary>
  /// Creates an instance with the current month and year values.
  /// </summary>
  /// <returns></returns>
  public static MonthAndYear Current() => new();

  /// <summary>
  /// Returns the previous month for the current year. If the current month is January then returns December and the previous year.
  /// </summary>
  public MonthAndYear PreviousMonth()
  {
    return (Month != Months.January)
      ? new MonthAndYear(Month - 1, Year)
      : new MonthAndYear(Months.December, Year - 1);
  }

  /// <summary>
  /// Returns the elements by which instances of this class are compared.
  /// </summary>
  protected override IEnumerable<object> GetEqualityComponents()
  {
    yield return Month;
    yield return Year;
  }
}
