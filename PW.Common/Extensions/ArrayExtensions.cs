using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PW.FailFast;

namespace PW.Extensions;

/// <summary>
/// Extension methods for use with arrays.
/// </summary>
public static class ArrayExtensions
{

  /// <summary>
  /// Compares two byte arrays for sequence equality. If either of the arrays is null then the result will be false.
  /// </summary>
  public static bool SequenceEquals(this byte[] array1, byte[] array2)
  {

    if (array1 == null || array2 == null) return false;
    if (ReferenceEquals(array1, array2)) return true;
    if (array1.Length != array2.Length) return false;

    for (int i = 0; i < array1.Length; i++)
      if (array1[i] != array2[i]) return false;

    return true;
  }

  /// <summary>
  /// Transforms the vector (1D array) into a matrix (2D array). 
  /// The returned matrix will have the same length for both dimensions (square).
  /// Therefore the length of the input vector must have a perfect square-root.
  /// </summary>
  /// <exception cref="ArgumentNullException">The input vector is null.</exception>
  /// <exception cref="AssertionException">The input vector length does not have a perfect square-root.</exception>
  public static byte[,] ToMatrix(this byte[] vector!!)
  {
    var (success, root) = Maths.GetPerfectRoot(vector.Length);
    Assert.IsTrue(success, "Vector's length must be the root of a square.");

    var matrix = new byte[root, root];
    Buffer.BlockCopy(vector, 0, matrix, 0, vector.Length);
    return matrix;
  }

  /// <summary>
  /// Transforms (flattens) the input matrix (2D array) into a vector (1D array).
  /// </summary>
  public static byte[] ToVector(this byte[,] matrix!!)
  {
    var len = (matrix.GetUpperBound(0) + 1) * (matrix.GetUpperBound(1) + 1);
    var vector = new byte[len];
    Buffer.BlockCopy(matrix, 0, vector, 0, len);
    return vector;
  }

  /// <summary>
  /// Returns both the dimensions of a matrix (2D array) as a size.
  /// </summary>
  public static Size Dimensions<T>(this T[,] matrix!!) => new(matrix.GetUpperBound(0) + 1, matrix.GetUpperBound(1) + 1);

  /// <summary>
  /// Returns the upper bound of both dimensions of a matrix (2D array) as a size.
  /// </summary>
  public static Size Bounds<T>(this T[,] matrix!!) => new(matrix.GetUpperBound(0), matrix.GetUpperBound(1));


  /// <summary>
  /// Prepends <paramref name="item"/> to <paramref name="source"/> 
  /// and returns a new array with a maximum length of <paramref name="maxItems"/>
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="source">The existing array to which <paramref name="item"/> is prepended.</param>
  /// <param name="item">Item to prepend.</param>
  /// <param name="maxItems">Maximum length of resulting array. -1 (default) means all items.</param>
  /// <returns></returns>
  public static T[] Prepend<T>(this T[] source!!, T item!!, int maxItems = Constants.NotFound)
  {
    Guard.GreaterThanOrEqualTo(maxItems, -1, nameof(maxItems));

    // Allow for stupid ;)
    if (maxItems == 1 || source.Length == 0) return new[] { item };

    // Handle normal
    var newSize = maxItems == -1 ? source.Length + 1 : Math.Min(maxItems, source.Length + 1);
    var r = new T[newSize];
    r[0] = item;
    Array.Copy(source, 0, r, 1, newSize - 1);
    return r;
  }

  /// <summary>
  /// Splits an array into multiple enumerations.
  /// </summary>
  /// <typeparam name="T">The type of the array.</typeparam>
  /// <param name="array">The array to split.</param>
  /// <param name="segmentSize">The size of the smaller arrays.</param>
  /// <returns>An array containing smaller arrays.</returns>
  public static IEnumerable<IEnumerable<T>> Segment<T>(this T[] array!!, int segmentSize)
  {
    if (segmentSize < 1) throw new ArgumentException("Size must be at least one.", nameof(segmentSize));

    if (segmentSize == 1) yield return array;

    else
    {
      for (var i = 0; i < (float)array.Length / segmentSize; i++)
        yield return array.Skip(i * segmentSize).Take(segmentSize);
    }
  }

}



