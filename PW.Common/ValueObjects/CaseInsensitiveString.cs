#nullable enable 

using CSharpFunctionalExtensions;
using PW.FailFast;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

// TODO: Look at example on GitHub to see if it can replace this class.

// See: https://github.com/mcintyre321/ValueOf/blob/master/ValueOf/ValueOf.cs
// This class is from version 'Commits on May 10, 2019'

// https://github.com/mcintyre321/ValueOf/issues/5

namespace PW.ValueObjects
{
  /// <summary>
  /// A type of string which can be compared ignoring case.
  /// </summary>
  public sealed class CaseInsensitiveString : ValueOf<string, CaseInsensitiveString>
  {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public override bool Equals(ValueOf<string, CaseInsensitiveString>? other)
    {
      return string.Equals(Value, other?.Value, StringComparison.OrdinalIgnoreCase);
    }
  }


  // 2022-02-15 -- This does not appear to work. Unfinished?

  //  /// <summary>
  //  /// Base class for case-insensitive string data types. Avoid 'primitive obsession'
  //  /// </summary>
  //  /// <example>class FilePath : DataPrimitive{string, FilePath}</example> 

  //  public abstract class CaseInsensitiveString<TSelf> where TSelf : CaseInsensitiveString<TSelf>, new()
  //  {
  //    private static readonly Func<TSelf> Factory;
  //    private int _hashCode;//= 0;

  //    /// <summary>
  //    /// Optionally override to perform validation.
  //    /// </summary>
  //    /// <returns>
  //    /// </returns>
  //    protected virtual ValidationResult Validate(string value) => ValidationResult.Ok;

  //    static CaseInsensitiveString()
  //    {
  //      var ctor = typeof(TSelf).GetTypeInfo().DeclaredConstructors.First();
  //      var argsExp = Array.Empty<Expression>();
  //      var newExp = Expression.New(ctor, argsExp);
  //      var lambda = Expression.Lambda(typeof(Func<TSelf>), newExp);
  //      Factory = (Func<TSelf>)lambda.Compile();
  //    }

  //#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
  //    /// <summary>
  //    /// The value of the underlying data type.
  //    /// </summary>
  //    public string Value { get; protected set; }
  //#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

  //    /// <summary>
  //    /// Explicitly converts the <see cref="ValueOf{TData, TDataPrimitive}"/> back to string type."/>
  //    /// </summary>
  //    /// <param name="dataType"></param>
  //    public static explicit operator string(CaseInsensitiveString<TSelf> dataType) => dataType.Value;



  //    /// <summary>
  //    /// Casts <paramref name="value"/> to <typeparamref name="TSelf"/>. If validation fails then <see cref="InvalidCastException"/> is thrown.
  //    /// </summary>
  //    /// <exception cref="InvalidCastException">Thrown if validation of <paramref name="value"/> fails.</exception>    
  //    public static explicit operator CaseInsensitiveString<TSelf>(string value)
  //    {
  //      value.NullGuard(nameof(value));
  //      var x = Factory();
  //      if (x.Validate(value) is ValidationResult result && result.IsFailure) throw new InvalidCastException(result.Error.Value);
  //      x.Value = value;
  //      x._hashCode = value.ToLower().GetHashCode();
  //      return x;
  //    }


  //    ///// <summary>
  //    ///// Creates a new instance of <see cref="ValueOf{TData, TDataPrimitive}"/>
  //    ///// </summary>
  //    ///// <param name="value">Value to convert.</param>
  //    ///// <param name="validateValue">Option to skip validation of value.</param>
  //    ///// <exception cref="ArgumentException">Thrown if validation of <paramref name="value"/> fails.</exception>
  //    //[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "<Pending>")]
  //    //[Obsolete("Use version of From() without validation option argument. NB: Argument is now ignored!")]
  //    //public static TSelf From(string value, bool validateValue = true) =>(TSelf)value;

  //    /// <summary>
  //    /// Casts <paramref name="value"/> to <typeparamref name="TSelf"/>. If validation fails then <see cref="InvalidCastException"/> is thrown.
  //    /// </summary>
  //    public static TSelf From(string value) => (TSelf)value;



  //    /// <summary>
  //    /// Attempts to convert <paramref name="value"/> into <typeparamref name="TSelf"/>.
  //    /// </summary>
  //    public static Result<TSelf, ErrorMessage> TryFrom(string value)
  //    {
  //      var x = Factory();
  //      var validation = x.Validate(value);
  //      if (validation.IsSuccess) x.Value = value;
  //      return validation.IsSuccess ? Result.Success<TSelf, ErrorMessage>(x) : Result.Failure<TSelf, ErrorMessage>(validation.Error);
  //    }


  //    /// <summary>
  //    /// Performs equality comparison of the two instances
  //    /// </summary>
  //    /// <returns></returns>
  //    public virtual bool Equals(CaseInsensitiveString<TSelf> other) =>
  //     ReferenceEquals(Value, other?.Value) || string.Equals(Value, other?.Value, StringComparison.OrdinalIgnoreCase);

  //    /// <summary>
  //    /// Performs equality comparison of the two instances
  //    /// </summary>
  //    /// <returns></returns>
  //    public override bool Equals(object? obj)
  //    {
  //      return obj is not null && (ReferenceEquals(this, obj) || (obj.GetType() == GetType() && Equals((CaseInsensitiveString<TSelf>)obj)));
  //    }

  //    /// <summary>
  //    /// Returns the hash code for string
  //    /// </summary>
  //    /// <returns></returns>
  //    public override int GetHashCode() => _hashCode;

  //    /// <summary>
  //    /// Performs equality comparison of the two instances
  //    /// </summary>
  //    /// <returns></returns>
  //    public static bool operator ==(CaseInsensitiveString<TSelf> a, CaseInsensitiveString<TSelf> b)
  //    {
  //      return a is null && b is null || a is not null && b is not null && a.Equals(b);
  //    }

  //    /// <summary>
  //    /// Performs negative-equality comparison of the two instances
  //    /// </summary>
  //    /// <returns></returns>
  //    public static bool operator !=(CaseInsensitiveString<TSelf> a, CaseInsensitiveString<TSelf> b) => !(a == b);

  //    /// <summary>
  //    /// Returns ToString() for string
  //    /// </summary>
  //    /// <returns></returns>
  //    public override string ToString()
  //    {
  //      // Value should never be null. This is checked in From method
  //      return Value != null ? Value.ToString() : string.Empty;
  //    }
  //  }
}
