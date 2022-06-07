namespace PW.Dates;

/// <summary>
/// Used when querying data from a DB WHERE rows fall between 
/// two dates or between the start and end of a single day.
/// </summary>
public class DateTimeRange : ValueObject
{
  /// <summary>
  /// The start date of the range
  /// </summary>
  public DateTime Start { get; }

  /// <summary>
  /// The end date of the range
  /// </summary>
  public DateTime End { get; }

  /// <summary>
  /// Creates and instance with dates representing the start (00:00:00) and end (23:59:59) of a single day.
  /// </summary>
  /// <param name="date"></param>
  public DateTimeRange(DateTime date)
  {
    Start = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
    End = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
  }

  /// <summary>
  /// Constructs an instance from the <paramref name="startDate"/> with time 00:00:00 to the <paramref name="endDate"/> with time 23:59:59
  /// </summary>
  /// <param name="startDate"></param>
  /// <param name="endDate"></param>
  public DateTimeRange(DateTime startDate, DateTime endDate)
  {
    Start = new DateTime(startDate.Year, startDate.Month, startDate.Day, 0, 0, 0);
    End = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);
  }

  /// <summary>
  /// Returns to <see cref="DateTime"/> values representing the range of time for the current day. The date component for both are the same, however the time
  /// component is 00:00:00 for Start and 23:59:59 for End. Useful for date-between type queries.
  /// </summary>    
  public static (DateTime Start, DateTime End) Today()
  {
    var today = DateTime.Now;
    return (new DateTime(today.Year, today.Month, today.Day, 0, 0, 0), new DateTime(today.Year, today.Month, today.Day, 23, 59, 59));
  }

  /// <summary>
  /// Returns the elements by which instances of this class are compared.
  /// </summary>
  protected override IEnumerable<object> GetEqualityComponents()
  {
    yield return Start;
    yield return End;
  }
}
