 

using PW.FailFast;
using PW.ValueObjects;
using System.Collections.Generic;

namespace PW.Net;

/// <summary>
/// Primitive to represent an IP4 Address
/// </summary>
public class IP4Address : ValueOf<string, IP4Address>
{

  /// <summary>
  /// Byte for each octet of the array.
  /// </summary>
  public IReadOnlyList<byte> Octets { get; private set; } = null!;

  /// <summary>
  /// String for each octet of the array.
  /// </summary>
  public IReadOnlyList<string> OctetStrings { get; private set; } = null!;


  /// <summary>
  /// Validates that the value represents an IP4 address.
  /// </summary>
  protected override void Validate()
  {

    var octetStrings = Value.Split('.');

    var octets = new byte[4];

    if (octetStrings.Length != 4) throw new System.Exception("IP does not contain 4 octets.");

    for (int i = 0; i < 4; i++)
    {
      // Using int.TryParse here, instead of byte, because byte wraps if > 255
      if (int.TryParse(octetStrings[i], out var v) == false || v < 0 || v > 255)
        throw new System.Exception($"Invalid IP address '{Value}'. Octet {i} is invalid");

      octets[i] = System.Convert.ToByte(v);
    }

    OctetStrings = octetStrings;
    Octets = octets;

  }
}


