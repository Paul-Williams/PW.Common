 

using System;
using PW.FailFast;


// This class could be made static for use as an extension method as:
// TOutput =  Tinput.Convert(Func<TInput, TOutput> func);

namespace PW
{
  /// <summary>
  /// Converts from TSource to TResult using function delegates.
  /// </summary>
  /// <typeparam name="TInput">The input type</typeparam>
  /// <typeparam name="TOutput">The output type</typeparam>
  public class Converter<TInput, TOutput>
  {
    private readonly Func<TInput, TOutput> _convertionCallback;
    private readonly Func<TInput> _inputProvider;

    /// <summary>
    /// Creates a new instance of the class
    /// </summary>
    /// <param name="inputProvider">A function delegate which provides the input to be converted</param>
    /// <param name="convertionCallback">A function delegate which performs the conversion from <typeparamref name="TInput"/> to  <typeparamref name="TOutput"/></param>
    
    public Converter(Func<TInput> inputProvider!!, Func<TInput, TOutput> convertionCallback!!)
    {
      _convertionCallback = convertionCallback;
      _inputProvider = inputProvider;
    }

    /// <summary>
    /// Performs the conversion from <typeparamref name="TInput"/> to <typeparamref name="TOutput"/>
    /// </summary>
    /// <returns></returns>
    public TOutput Convert() => _convertionCallback(_inputProvider());
  }
}
