namespace PW.IO.FileSystemObjects;

public interface IDirectoryPath
{
  bool Exists { get; }
  DirectoryName Name { get; }
  DirectoryPath? Parent { get; }

  DirectoryPath Append(DirectoryName subDirectory);
  FilePath Append(FileName file);
  FilePath File(string file);
  bool IsBelow(DirectoryPath directory);
  DirectoryInfo ToDirectoryInfo();
}