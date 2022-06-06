using System.IO.Compression;
using System.Runtime.Serialization;
using static PW.StringCompression;

// See: http://gigi.nullneuron.net/gigilabs/compressing-strings-using-gzip-in-c/
// This article formed the basis for this class. 
// However it is flawed and does not close the compression stream before using the output stream.
// As a result, the output stream is always zero-length.

// See: https://gist.github.com/define-private-public/98d8e985fe74a23ab3797c01c3a689a5
// This class provided the information for fixing the code of the bug, mentioned above.

namespace PW;

/// <summary>
/// Wrapper around <see cref="DeflateStream"/> for easy in-memory string compression/decompression.
/// </summary>
public class CompressedString : ISerializable
{

  /// <summary>
  /// Array of bytes representing the compressed string.
  /// </summary>    
  public byte[]? Bytes { get; }

  /// <summary>
  /// Creates a new instance from a previously compressed byte-data. For use by deserializers.
  /// </summary>
  /// <param name="bytes">Previously compressed string's byte-data.</param>
  public CompressedString(byte[] bytes!!) => Bytes = bytes;

  /// <summary>
  /// Creates a new instance by compressing the supplied string.
  /// </summary>
  /// <param name="str">String to compress.</param>
  public CompressedString(string str!!) => Bytes = str.Compress();


  /// <summary>
  /// Decompresses the byte-data to return the original string.
  /// </summary>
  public override string ToString() => Bytes != null ? Bytes.Decompress() : string.Empty;

  #region ISerializable support
  public void GetObjectData(SerializationInfo info, StreamingContext context) => info.AddValue(nameof(Bytes), Bytes);

  public CompressedString(SerializationInfo info, StreamingContext context) =>
    Bytes = (byte[]?)info.GetValue(nameof(Bytes), typeof(byte[]));

  #endregion

}

