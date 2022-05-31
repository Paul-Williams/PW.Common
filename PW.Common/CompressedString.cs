#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// See: http://gigi.nullneuron.net/gigilabs/compressing-strings-using-gzip-in-c/
// This article formed the basis for this class. 
// However it is flawed and does not close the compression stream before using the output stream.
// As a result, the output stream is always zero-length.

// See: https://gist.github.com/define-private-public/98d8e985fe74a23ab3797c01c3a689a5
// This class provided the information for fixing the code of the bug, mentioned above.

namespace PW
{
  /// <summary>
  /// Wrapper around <see cref="DeflateStream"/> for easy in-memory string compression/decompression.
  /// </summary>
  public class CompressedString 
  {

    /// <summary>
    /// Array of bytes representing the compressed string.
    /// </summary>    
    public byte[] Bytes { get; }

    /// <summary>
    /// Creates a new instance from a previously compressed byte-data. For use by de-serializers.
    /// </summary>
    /// <param name="bytes">Previously compressed string's byte-data.</param>
    public CompressedString(byte[] bytes!!)
    {
      Bytes = bytes;
    }

    /// <summary>
    /// Creates a new instance by compressing the supplied string.
    /// </summary>
    /// <param name="str">String to compress.</param>
    public CompressedString(string str!!)
    {
      Bytes = Compress(str, CompressionLevel.Optimal);
    }


    /// <summary>
    /// Decompresses the byte-data to return the original string.
    /// </summary>
    public override string ToString() => Decompress(Bytes);

    ///// <summary>
    ///// Returns a copy of the byte array representing the compressed string.
    ///// </summary>    
    //public byte[] GetBytes() => (byte[])Data.Clone();


    private static byte[] Compress(string inputStr!!, CompressionLevel compressionLevel)
    {
      byte[] inputBytes = Encoding.UTF8.GetBytes(inputStr);

      using var outputStream = new MemoryStream();
      using var compressor = new DeflateStream(outputStream, compressionLevel, true);
      compressor.Write(inputBytes, 0, inputBytes.Length);
      compressor.Close(); // VERY important, otherwise output stream will always be zero-length.
      return outputStream.ToArray();
    }

    private static string Decompress(byte[] data!!)
    {
      using var inputStream = new MemoryStream(data);
      using var deflateStream = new DeflateStream(inputStream, CompressionMode.Decompress);
      using var streamReader = new StreamReader(deflateStream);
      return streamReader.ReadToEnd();
    }

  }
}
