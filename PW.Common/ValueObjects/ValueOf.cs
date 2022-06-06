 

using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

// See: https://github.com/mcintyre321/ValueOf/blob/master/ValueOf/ValueOf.cs
// This class is from version 'Commits on May 10, 2019'

// https://github.com/mcintyre321/ValueOf/issues/5

// NB !!!! DO NOT implement implicit conversion operator, back to  type TValue.
// Otherwise it is possible to inadvertently compare equality for two different ValueOf types.
// This occurs because the compiler will call the implicit operator.

namespace PW.ValueObjects
{
  /// <summary>
  /// Base class for primitive data types. Avoid 'primitive obsession'
  /// </summary>
  /// <example>class FilePath : DataPrimitive{string, FilePath}</example> 
  public abstract class ValueOf<TValue, TSelf> where TSelf : ValueOf<TValue, TSelf>, new()
  {
    private static readonly Func<TSelf> Factory;

    /// <summary>
    /// Optionally override to perform validation.
    /// </summary>
    protected virtual void Validate() { }


    static ValueOf()
    {
      var ctor = typeof(TSelf).GetTypeInfo().DeclaredConstructors.First();
      var argsExp = Array.Empty<Expression>();
      var newExp = Expression.New(ctor, argsExp);
      var lambda = Expression.Lambda(typeof(Func<TSelf>), newExp);
      Factory = (Func<TSelf>)lambda.Compile();
    }

    /// <summary>
    /// The value of the underlying data type.
    /// </summary>
    public TValue Value { get; protected set; } = default!;



    /// <summary>
    /// Explicitly converts the ValueOf back to the underlying <typeparamref name="TValue"/> type."/>
    /// </summary>
    /// <param name="dataType"></param>
    public static explicit operator TValue(ValueOf<TValue, TSelf> dataType) =>
      dataType is null ? throw new ArgumentNullException(nameof(dataType)) : dataType.Value;


    /// <summary>
    /// Explicitly convert <typeparamref name="TValue"/> into a new instance of <see cref="ValueOf{TData, TDataPrimitive}"/>
    /// </summary>
    /// <exception cref="InvalidCastException">Thrown if validation of <paramref name="value"/> fails.</exception>
    public static explicit operator ValueOf<TValue, TSelf>(TValue value) => From(value);


    /// <summary>
    /// Creates a new instance of <see cref="ValueOf{TData, TDataPrimitive}"/>
    /// </summary>
    public static TSelf From(TValue value)
    {
      if (EqualityComparer<TValue>.Default.Equals(value, default!)) throw new ArgumentNullException(nameof(value));
      var x = Factory();
      x.Value = value;
      x.Validate();
      return x;
    }


    /// <summary>
    /// Attempts to convert <paramref name="value"/> into <typeparamref name="TSelf"/>.
    /// </summary>
    public static Result<TSelf, ErrorMessage> TryFrom(TValue value)
    {
      var x = Factory();

      try
      {
        x.Validate();
        x.Value = value;
        return Result.Success<TSelf, ErrorMessage>(x);
      }
      catch (Exception ex)
      {
        return Result.Failure<TSelf, ErrorMessage>((ErrorMessage)ex.Message);
      }
    }


    /// <summary>
    /// Performs equality comparison of the two instances
    /// </summary>
    /// <returns></returns>
    public virtual bool Equals(ValueOf<TValue, TSelf>? other) => other is not null && EqualityComparer<TValue>.Default.Equals(Value!, other.Value!);


    /// <summary>
    /// Performs equality comparison of the two instances
    /// </summary>
    /// <returns></returns>
    public override bool Equals(object? obj) =>
      obj is not null && (ReferenceEquals(this, obj) || obj.GetType() == GetType() && Equals((ValueOf<TValue, TSelf>)obj));

    /// <summary>
    /// Returns the hash code for <typeparamref name="TValue"/>
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => EqualityComparer<TValue>.Default.GetHashCode(Value!);


    /// <summary>
    /// Performs equality comparison of the two instances
    /// </summary>
    /// <returns></returns>
    public static bool operator ==(ValueOf<TValue, TSelf> a, ValueOf<TValue, TSelf> b) => 
      a is null && b is null || a is not null && b is not null && a.Equals(b);


    /// <summary>
    /// Performs negative-equality comparison of the two instances
    /// </summary>
    /// <returns></returns>
    public static bool operator !=(ValueOf<TValue, TSelf> a, ValueOf<TValue, TSelf> b) => !(a == b);


    /// <summary>
    /// Returns ToString() for <typeparamref name="TValue"/>
    /// </summary>
    /// <returns></returns>
    public override string ToString() => Value?.ToString() ?? string.Empty;
  }
}
