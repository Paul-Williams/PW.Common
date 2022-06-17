using static PW.Extensions.IListExtensions;

namespace PW.Collections;

/// <summary>
/// A generic list which can be accessed by position of a 'cursor'. 
/// An event is raised whenever the cursor position changes.
/// </summary>
/// <typeparam name="T"></typeparam>
public partial class CursoredList<T> : IReadOnlyList<T>
{
  /// <summary>
  /// <see cref="PositionChanged"/> event delegate.
  /// </summary>
  public delegate void PositionChangedHandler(object sender, PositionChangedEventData<T> e);

  /// <summary>
  /// Raised whenever the cursor position changes. 
  /// </summary>
  public event PositionChangedHandler? PositionChanged;

  /// <summary>
  /// Default constructor
  /// </summary>
  public CursoredList()
  {
    List = new List<T>();
    _position = -1;
  }



  /// <summary>
  /// Creates a new instance with the specified initial capacity.
  /// </summary>
  /// <param name="capacity"></param>
  public CursoredList(int capacity)
  {
    List = new List<T>(capacity);
    _position = -1;
  }

  /// <summary>
  /// Creates a new instance containing items from <paramref name="seq"/>.
  /// </summary>
  /// <param name="seq"></param>
  public CursoredList(IEnumerable<T> seq)
  {
    List = new List<T>(seq);
    _position = List.Count == 0 ? -1 : 0;
  }

  /// <summary>
  /// Determines whether the list has any items.
  /// </summary>
  public bool IsEmpty => List.Count == 0;


  /// <summary>
  /// Whether or not to loop the list when moving forward past the end, or backwards past the start.
  /// </summary>
  public bool AutoLoop { get; set; } = true;

  private List<T> List { get; }

  // Only change _position from within Position() & constructors!
  private int _position;

  /// <summary>
  /// The cursor position.
  /// </summary>
  public int Position
  {
    get => _position;
    private set
    {
      if (IsEmpty) throw new InvalidOperationException("List is empty.");

      Assert.IsTrue(!IsEmpty && value >= 0 && value < List.Count, "Value not within list bounds or list is empty.");
      if (value == _position) return;
      var previousPosition = _position;
      _position = value;
      PositionChanged?.Invoke(this, new PositionChangedEventData<T>(_position, previousPosition, List[_position], List[previousPosition]));
    }
  }

  /// <summary>
  /// Adds the files to the playlist. NB: This is *not* thread-safe. The caller is responsible for using a lock, or other mechanism, to prevent multi-thread access issues.
  /// </summary>
  /// <param name="seq"></param>
  public void AddRange(IEnumerable<T> seq)
  {
    // SyncLock moved to the caller method - added note to method comment summary
    List.AddRange(seq);
    if (_position == -1 && !IsEmpty) _position = 0;
  }

  /// <summary>
  /// Adds the item to the end of the list.
  /// </summary>
  /// <param name="item"></param>
  public void Add(T item)
  {
    List.Add(item);
    if (_position == -1 && !IsEmpty) _position = 0;
  }

  /// <summary>
  /// Randomizes the order of the list items
  /// </summary>
  public void Shuffle() => List.Shuffle();

  /// <summary>
  /// Moves the cursor to the first item in the list.
  /// </summary>
  public void MoveFirst()
  {
    if (IsEmpty) throw new InvalidOperationException("List is empty.");
    Position = 0;
  }

  /// <summary>
  /// Moves the cursor to the previous item in the list. Returns true if move was successful, otherwise false.
  /// </summary>
  public bool MoveBack()
  {
    if (IsEmpty) throw new InvalidOperationException("List is empty.");

    // If we are not at the beginning, just move back.
    if (Position > 0) Position--;

    // If we are at the beginning and auto-looping is enabled, then move to the end.
    else if (AutoLoop) Position = List.Count - 1;

    // Otherwise return that we failed to move
    else return false;

    // If we got here, then to moved, so return success.
    return true;
  }

  /// <summary>
  /// Moves the cursor to the next item in the list. Returns true if move was successful, otherwise false.
  /// </summary>
  public bool MoveNext()
  {
    if (IsEmpty) throw new InvalidOperationException("List is empty.");

    if (Position < List.Count - 1) Position++;
    else if (AutoLoop) Position = 0;
    else return false;

    return true;
  }

  /// <summary>
  /// Moves <see cref="Position"/> to value for which <paramref name="selector"/> returns true. 
  /// </summary>
  /// <param name="selector">Predicate to determine <see cref="Position"/>.</param>
  /// <returns><typeparamref name="T"/> at new <see cref="Position"/>, or default(<typeparamref name="T"/>) if <paramref name="selector"/> never returns true.</returns>
  public T MoveTo(Func<T, bool> selector)
  {
    if (IsEmpty) throw new InvalidOperationException("List is empty.");
    for (int i = 0; i < List.Count; i++)
    {
      if (selector(List[i]))
      {
        Position = i;
        return List[i];
      }
    }
    return default!;
  }

  /// <summary>
  /// Move the cursor in the specified direction.
  /// </summary>
  public T Move(IterateDirection direction)
  {
    if (IsEmpty) throw new InvalidOperationException("List is empty.");
    (direction == IterateDirection.Forward ? (Func<bool>)MoveNext : MoveBack).Invoke();
    return Current;
  }


  /// <summary>
  /// Returns the number of items in the list.
  /// </summary>
  public int Count => List.Count;

  /// <summary>
  /// Returns the item at the current cursor position.
  /// </summary>
  public T Current => !IsEmpty ? List[Position] : throw new InvalidOperationException("List is empty.");



  /// <summary>
  /// Removes the item at the current cursor position from the list.
  /// </summary>
  public void RemoveCurrent()
  {
    if (IsEmpty) throw new InvalidOperationException("List is empty.");
    List.RemoveAt(Position);

    // If the list is now empty, then move the cursor to -1.
    if (IsEmpty) _position = -1;
    // If we just removed the last image, then move to the first image.
    else if (Position >= List.Count) Position = 0;
  }

  #region IReadOnlyList Support

  /// <summary>
  /// Gets the element at the specified index.
  /// </summary>
  /// <param name="index"></param>
  /// <returns></returns>
  public T this[int index] => IsEmpty ? throw new InvalidOperationException("List is empty.") : List[index];

  IEnumerator<T> IEnumerable<T>.GetEnumerator() => List.GetEnumerator();

  IEnumerator IEnumerable.GetEnumerator() => List.GetEnumerator();

  #endregion

}

