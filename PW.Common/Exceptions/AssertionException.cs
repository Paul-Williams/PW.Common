 

using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace PW.FailFast
{
  /// <summary>
  /// Thrown for failed <see cref="Assert"/>
  /// </summary>
  [Serializable]
  public class AssertionException : Exception
  {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    public AssertionException(string message) : base(message)
    {
    }
    /// <summary>
    /// 
    /// </summary>
    public AssertionException() : base("Assertion failed.")
    {
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="methodName"></param>
    /// <param name="message"></param>
    public AssertionException(string methodName, string message) : base(methodName + " failed. " + message)
    {
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="methodName"></param>
    /// <param name="message"></param>
    /// <param name="inner"></param>
    public AssertionException(string methodName, string message, Exception inner) : base(methodName + " failed. " + message, inner)
    {
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    public AssertionException(string message, Exception innerException) : base(message, innerException) { }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    protected AssertionException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
      //MyCustomProperty = info.GetString(nameof(MyCustomProperty));
    }

    /// <summary>
    /// Implementation of <see cref="Exception.GetObjectData(SerializationInfo, StreamingContext)"/>.
    /// </summary>
    public override void GetObjectData(SerializationInfo info!!, StreamingContext context)
    {
      //info.AddValue(nameof(MyCustomProperty), MyCustomProperty);
      base.GetObjectData(info, context);
    }

       
  }
}
