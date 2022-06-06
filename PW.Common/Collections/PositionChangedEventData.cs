namespace PW.Collections;


/// <summary>
/// PositionChangedEventArgs
/// </summary>
public class PositionChangedEventData<T>
{
  /// <summary>
  /// Creates a new instance.
  /// </summary>
  public PositionChangedEventData(int currentPosition, int previousPosition, T currentItem, T previousItem) 
  {
    CurrentPosition = currentPosition;
    PreviousPosition = previousPosition;
    CurrentItem = currentItem;
    PreviousItem = previousItem;
  }

  /// <summary>
  /// The current position of the cursor.
  /// </summary>
  public int CurrentPosition { get; }

  /// <summary>
  /// The previous position of the cursor.
  /// </summary>
  public int PreviousPosition { get; }

  /// <summary>
  /// The current position of the cursor.
  /// </summary>
  public T CurrentItem { get; }

  /// <summary>
  /// The previous position of the cursor.
  /// </summary>
  public T PreviousItem { get; }

}


