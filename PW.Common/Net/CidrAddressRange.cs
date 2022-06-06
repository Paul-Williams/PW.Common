 

using PW.Extensions;
using PW.ValueObjects;
using System;
using System.Linq;

namespace PW.Net
{
  /// <summary>
  /// Represents a CIDR IP4 address range. E.g. 192.168.0.0/24 is IP4 address range: 192.168.0.1 to 192.168.0.254 .
  /// Currently only supports /8, /16, /24 and /32 masks, when creating from existing <see cref="IP4Address"/>.
  /// </summary>
  public class CidrAddressRange : ValueOf<string, CidrAddressRange>
  {

    /// <summary>
    /// Sizes of address range by class
    /// </summary>
    public enum Range { 
      /// <summary>
      /// Class A
      /// </summary>
      A = 8,
      /// <summary>
      /// Class B
      /// </summary>
      B = 16,
      /// <summary>
      /// Class C
      /// </summary>
      C = 24,
      /// <summary>
      /// Single IP
      /// </summary>
      SingleIP = 32 
    }

    /// <summary>
    /// 
    /// </summary>
    public static CidrAddressRange From(IP4Address address!!, Range range)
    {
      var octets = address.Octets;

      return range switch
      {
        Range.A => From(octets[0] + ".0.0.0/8"),
        Range.B => From(string.Join(".", new[] { octets[0], octets[1] }) + ".0.0/16"),
        Range.C => From(string.Join(".", new[] { octets[0], octets[1], octets[2] }) + ".0/24"),
        Range.SingleIP => From(address.Value + "/32"),
        _ => throw new ArgumentException("Unsupported mask."),
      };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public IP4AddressMask Mask()
    {
      var range = Value.SubstringAfterLast('/');
      return range.IsNullOrWhiteSpace()
          ? throw new Exception("/ not found or nothing after /")
          : range switch
            {
              "8" => (IP4AddressMask)"255.0.0.0",
              "16" => (IP4AddressMask)"255.255.0.0",
              "24" => (IP4AddressMask)"255.255.255.0",
              "32" => (IP4AddressMask)"255.255.255.255",
              _ => throw new Exception("Cannot create mask. Unsupported CIDR range"),
            };
    }

    /// <summary>
    /// Converts a single IP4 address into CIDR notation. E.g. 192.168.0.1 --> 192.168.0.1/32
    /// </summary>
    public static CidrAddressRange From(IP4Address address) => 
      address is null ? throw new ArgumentNullException(nameof(address)) 
      : (CidrAddressRange)(address.Value + "/32");
  }


}
