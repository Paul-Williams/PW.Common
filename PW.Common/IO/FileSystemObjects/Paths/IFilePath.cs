namespace PW.IO.FileSystemObjects;

public interface IFilePath
{
  DirectoryName DirectoryName { get; }
  DirectoryPath DirectoryPath { get; }
  bool Exists { get; }
  FileExtension Extension { get; }
  FileName Name { get; }
  FileNameWithoutExtension NameWithoutExtension { get; }

  FilePath ChangeDirectory(DirectoryPath directory);
  FilePath ChangeExtension(FileExtension newExtension);
  FilePath ChangeName(FileName newName);
  FilePath ChangeName(FileNameWithoutExtension newName);
  FilePath ChangeName(Func<string, string> f);
  FileInfo ToFileInfo();
}