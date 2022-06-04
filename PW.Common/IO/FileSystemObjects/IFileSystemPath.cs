namespace PW.IO.FileSystemObjects;

/// <summary>
/// Enables polymorphism for classes that inherit from <see cref="FileSystemPath{T}"/>
/// </summary>
public interface IFileSystemPath : IValue<string>
{
  public bool Exists { get; }
}

