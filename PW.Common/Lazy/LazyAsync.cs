using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

// Source: https://blogs.msdn.microsoft.com/pfxteam/2011/01/15/asynclazyt/

// Note: 
// Both constructors on this class originally used 'Task.Factory.StartNew'.
// This was causing the warning 'CA2008	Do not create tasks without passing a TaskScheduler'
// Have not tested this class since changing to Task.Run() !!!

// Original constructors:
// public AsyncLazy(Func<T> valueFactory) : base(() => Task.Factory.StartNew(valueFactory)) { }
// public AsyncLazy(Func<Task<T>> taskFactory) : base(() => Task.Factory.StartNew(() => taskFactory()).Unwrap())

/* Example usage:

class MarsLanderStats
{
  // We have method which may take a while, or something.
  static string GetRemoteData() => { //whirr, whirr, whirr }

  // A lazy instance to wrap that slowness within an awaitable Task
  static readonly AsyncLazy<string> _data = new AsyncLazy<string>(GetRemoteData);
   
  void ProcessData()
  {
    // Method awaits the lazy-task to get the string from far-far-away or something...
    string data = await _data;
  }
}

*/

namespace PW.Lazy;

/// <summary>
/// Provides support for asynchronous lazy initialization. E.g. var t = await AsyncLazy{T}
/// </summary>
/// <typeparam name="T">The type of object that is being asynchronously initialized.</typeparam>
public class LazyAsync<T> : Lazy<Task<T>>
{
  /// <summary>
  /// Initializes a new instance which invokes '<paramref name="valueFactory"/>' within an awaitable Task.
  /// </summary>
  /// <param name="valueFactory">A function which returns <typeparamref name="T"/>.</param>
  public LazyAsync(Func<T> valueFactory) : base(() => Task.Run(valueFactory)) { }

  /// <summary>
  /// Initializes a new instance.
  /// </summary>
  /// <param name="taskFactory">A function which is invoked to return a Task. This Task is then run to return <typeparamref name="T"/>.</param>
  public LazyAsync(Func<Task<T>> taskFactory) : base(() => Task.Run(() => taskFactory())) { }

  /// <summary>
  /// Asynchronous infrastructure support. This method permits instances of this class to be awaited.
  /// </summary>
  public TaskAwaiter<T> GetAwaiter() => Value.GetAwaiter();
}