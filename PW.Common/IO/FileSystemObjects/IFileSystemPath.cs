namespace PW.IO.FileSystemObjects;

/// <summary>
/// Enables polymorphism for classes that inherit from <see cref="FileSystemPath"/>
/// </summary>
public interface IFileSystemPath : IReadOnlyValue<string>
{
  public bool Exists { get; }
}

