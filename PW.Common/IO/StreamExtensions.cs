using System;
using System.IO;
using PW.FailFast;

namespace PW.IO;

/// <summary>
/// Extension methods for the <see cref="Stream"/> class.
/// </summary>
public static class StreamExtensions
{

  /// <summary>
  /// Copies the source stream to a new <seealso cref="MemoryStream"/>.
  /// </summary>
  /// <param name="source"></param>
  /// <returns>New instance of <seealso cref="MemoryStream"/></returns>
  /// <exception cref="ArgumentNullException"></exception>
  /// <exception cref="ArgumentException"></exception>
  public static MemoryStream CopyToMemory(this Stream source!!)
  {
    Guard.True(source.CanRead, $"Stream {nameof(source)} does not support reading.");

    MemoryStream? returnStream = null;
    try
    {
      returnStream = new MemoryStream();
      source.CopyTo(returnStream);
      returnStream.Position = 0;
      return returnStream;
    }
    catch
    {
      returnStream?.Dispose();
      throw;
    }

  }

  /// <summary>
  /// Copies <paramref name="length"/> bytes to the specified <paramref name="consumer"/> action.
  /// </summary>
  /// <param name="source">The source stream</param>
  /// <param name="consumer">The action which will consume the bytes</param>
  /// <param name="start">Start position in <paramref name="source"/> to start copying</param>
  /// <param name="length">The number of bytes passed to the <paramref name="consumer"/> action.</param>
  /// <exception cref="ArgumentNullException"></exception>
  /// <exception cref="ArgumentException"></exception>

  public static void CopyTo(this Stream source!!, Action<byte[]> consumer!!, int start, int length)
  {
    Guard.GreaterThanZero(length, nameof(length));
    Guard.ZeroOrGreater(start, nameof(start));

    source.Position = start;
    var buffer = new byte[length];
    source.Read(buffer, 0, length);
    consumer(buffer);
  }

}

