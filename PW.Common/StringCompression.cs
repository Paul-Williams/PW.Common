using System.IO;
using System.IO.Compression;
using System.Text;
using static PW.Functional.Disposable;

// See: http://gigi.nullneuron.net/gigilabs/compressing-strings-using-gzip-in-c/
// This article formed the basis for this class. 
// However it is flawed and does not close the compression stream before using the output stream.
// As a result, the output stream is always zero-length.

// See: https://gist.github.com/define-private-public/98d8e985fe74a23ab3797c01c3a689a5
// This class provided the information for fixing the code of the bug, mentioned above.

namespace PW
{
  public static class StringCompression
  {
    public static byte[] Compress(this string inputStr!!, CompressionLevel compressionLevel = CompressionLevel.Optimal) 
      => CompressToStream(inputStr, compressionLevel).DisposeAfter(stream => stream.ToArray());

    public static MemoryStream CompressToStream(this string inputStr!!, CompressionLevel compressionLevel = CompressionLevel.Optimal)
    {
      byte[] inputBytes = Encoding.UTF8.GetBytes(inputStr);

      var outputStream = new MemoryStream();
      using var compressor = new DeflateStream(outputStream, compressionLevel, true);
      compressor.Write(inputBytes, 0, inputBytes.Length);
      compressor.Close(); // VERY important, otherwise output stream will always be zero-length.
      return outputStream;  
    }


    public static string Decompress(this byte[] data!!) => Decompress(new MemoryStream(data));


    public static string Decompress(Stream data!!)
    {
      using var deflateStream = new DeflateStream(data, CompressionMode.Decompress);
      using var streamReader = new StreamReader(deflateStream);
      return streamReader.ReadToEnd();
    }

  }

}
