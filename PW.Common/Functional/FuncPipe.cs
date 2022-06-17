// TODO: XML Comments

namespace PW.Functional;

public static class FuncPipe
{
  public static FuncPipe<T> Create<T>(Func<T> func) => new(func);
  public static FuncPipe<T, TR> Create<T, TR>(Func<T, TR> func) => new(func);
}


/// <summary>
/// Creates a function which pipes the output of a function into the input of the next function, in a fluent manor.
/// </summary>
public struct FuncPipe<T>
{
  public FuncPipe(Func<T> func) => Func = func;

  public Func<T> Func { get; }

  /// <summary>
  /// Connects the next function to the pipeline.
  /// </summary>
  public FuncPipe<T2> To<T2>(Func<T, T2> func) => new(Func.Pipe(func));

  public T Invoke() => Func.Invoke();

}

/// <summary>
/// Creates a function which pipes the output of a function into the input of the next function, in a fluent manor.
/// </summary>
public struct FuncPipe<T, TR>
{
  public FuncPipe(Func<T, TR> func) => Func = func;

  public Func<T, TR> Func { get; }

  /// <summary>
  /// Connects the next function to the pipeline.
  /// </summary>
  public FuncPipe<T, TR2> To<TR2>(Func<TR, TR2> func) => new(Func.Pipe(func));

  public TR Invoke(T x) => Func.Invoke(x);
}

/// <summary>
/// Creates a function which pipes the output of a function into the input of the next function, in a fluent manor.
/// The 'S' suffix is simply to prevent a name clash with class <see cref="FuncPipe{T}"/>.
/// </summary>
public struct FuncPipeS<T>
{
  public FuncPipeS(Func<T, T> func) => Func = func;

  public Func<T, T> Func { get; }

  /// <summary>
  /// Connects the next function to the pipeline.
  /// </summary>
  public FuncPipe<T, T> To(Func<T, T> func) => new(Func.Pipe(func));

  public T Invoke(T x) => Func.Invoke(x);

  public FuncPipeS(IEnumerable<Func<T, T>> seq)
  {
    var f = seq.FirstOrDefault();
    if (f == null) throw new ArgumentException("Enumeration contains no elements.");

    foreach (var f2 in seq)
    {
      f = x => f2(f(x));
    }
    Func = f;
  }
}




/// <summary>
/// Composes (links together) a list of functions which all have both input and output of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type which each function works with.</typeparam>
[Obsolete("Use FuncPipe instead.", false)]
public class FunctionPipeline<T> //: List<Func<T, T>>
{

  private List<Func<T, T>> Funcs { get; }

  private Func<T, T>? Composed { get; set; }

  /// <summary>
  /// Creates a new instance from the sequence of functions.
  /// </summary>
  /// <param name="seq"></param>
  /// <exception cref="ArgumentNullException"></exception>
  /// <exception cref="ArgumentException">Thrown if there is a null in <paramref name="seq"/></exception>
  public FunctionPipeline(IEnumerable<Func<T, T>> seq)
  {
    Funcs = new List<Func<T, T>>(seq);
    Guard.NoNulls(Funcs, nameof(seq));
  }

  /// <summary>
  /// Static method to link together a series of functions.
  /// </summary>
  public static Func<T, T> Compose(IList<Func<T, T>> funcs)
  {
    // Handle stupid case of single function pipe-line ;)
    if (funcs.Count == 1) return funcs[0];

    var pipeline = funcs[0];
    foreach (var f in funcs.Skip(1)) pipeline = pipeline.Then(f);
    return pipeline;
  }

  ///// <summary>
  ///// Composes all the function and returns the resulting function wrapped in a <see cref="Result{V}"/>
  ///// </summary>
  ///// <returns></returns>
  //public Result<Func<T, T>> ComposeAsResult()
  //{
  //  if (Count == 0) return Fail<Func<T, T>>("There are no function in the list");
  //  if (this.Any(f => f is null)) return Fail<Func<T, T>>("There are null references in the list.");
  //  return Ok(Compose(this));
  //}

  ///// <summary>
  ///// Returns the linked-together function pipeline.
  ///// </summary>
  ///// <returns></returns>
  //public Func<T, T> Compose()
  //{
  //  if (Count == 0) throw new InvalidOperationException("There are no function in the list");
  //  if (this.Any(f => f is null)) throw new InvalidOperationException("There are null references in the list.");
  //  return Compose(this);
  //}


  /// <summary>
  /// Executes the function pipeline
  /// </summary>
  public T Execute(T input)
  {
    if (Composed is null) Composed = Compose(Funcs);
    return Composed(input);
  }


  //public Result<T> ExecuteAsResult(T input) => ComposeAsResult().Select(input);

}
