namespace PW.Collections;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TElement"></typeparam>
public class Grouping<TKey, TElement> : IGrouping<TKey, TElement>, IList<TElement>
{
  /// <summary>
  /// 
  /// </summary>
  /// <param name="key"></param>
  /// <param name="elements"></param>
  public Grouping(TKey key, IEnumerable<TElement> elements)
  {
    Key = key;
    elements.ForEach(Add);
  }

  /// <summary>
  /// 
  /// </summary>
  public TKey Key { get; }

  private TElement[] _elements = Array.Empty<TElement>();
  private int count;
  //internal Grouping hashNext;
  //internal Grouping next;

  private void Add(TElement element)
  {
    if (_elements.Length == count) Array.Resize(ref _elements, checked(count * 2));
    _elements[count] = element;
    count++;
  }

  public IEnumerator<TElement> GetEnumerator()
  {
    for (int i = 0; i < count; i++) yield return _elements[i];
  }

  IEnumerator IEnumerable.GetEnumerator()
  {
    return GetEnumerator();
  }


  int ICollection<TElement>.Count
  {
    get { return count; }
  }

  bool ICollection<TElement>.IsReadOnly
  {
    get { return true; }
  }

  void ICollection<TElement>.Add(TElement item)
  {
    throw new NotSupportedException();
  }

  void ICollection<TElement>.Clear()
  {
    throw new NotSupportedException();
  }

  bool ICollection<TElement>.Contains(TElement item)
  {
    return Array.IndexOf(_elements, item, 0, count) >= 0;
  }

  void ICollection<TElement>.CopyTo(TElement[] array, int arrayIndex)
  {
    Array.Copy(_elements, 0, array, arrayIndex, count);
  }

  bool ICollection<TElement>.Remove(TElement item)
  {
    throw new NotSupportedException();
  }

  int IList<TElement>.IndexOf(TElement item)
  {
    return Array.IndexOf(_elements, item, 0, count);
  }

  void IList<TElement>.Insert(int index, TElement item)
  {
    throw new NotSupportedException();
  }

  void IList<TElement>.RemoveAt(int index)
  {
    throw new NotSupportedException();
  }


  TElement IList<TElement>.this[int index]
  {
    get
    {
      return index < 0 || index >= count
        ? throw new ArgumentOutOfRangeException(nameof(index))
        : _elements[index];
    }
    set
    {
      throw new NotSupportedException();
    }
  }
}
