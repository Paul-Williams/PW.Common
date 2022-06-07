namespace PW.IO;

/// <summary>
/// Provides base functionality to classes designed to 'group' files into sub-directories.
/// </summary>
public abstract class FileGrouperBase : IOperation
{
  /// <summary>
  /// Creates a new instance. Called from the sub-class
  /// </summary>
  public FileGrouperBase(DirectoryInfo directory, string fileMask)
  {
    Directory = directory;
    FileMask = fileMask;
  }

  /// <summary>
  /// Creates a new instance. Called from the sub-class
  /// </summary>
  public FileGrouperBase(DirectoryInfo directory)
  {
    Directory = directory;
  }

  /// <summary>
  /// Logic to determine whether to include a file in the grouping operation.
  /// </summary>
  protected abstract bool FileFilter(FileInfo file);

  /// <summary>
  /// Logic to map (move) a file to a new directory.
  /// </summary>
  protected abstract (FileInfo File, DirectoryInfo MoveToDirectory) Map(FileInfo file);

  /// <summary>
  /// Directory containing files to be 'grouped' into sub-directories.
  /// </summary>
  protected DirectoryInfo? Directory { get; set; }

  /// <summary>
  /// Mask used to obtain list of files within <see cref="Directory"/>
  /// </summary>
  protected string FileMask { get; } = "*.*";

  /// <summary>
  /// Performs the 'grouping' of sequentially named files into sub-directories.
  /// </summary>
  public void Perform()
  {
    if (Directory is null) throw new Exception(nameof(Directory) + " is null.");
    // Ensure the directory still exists.
    // Could create a lock-file at this point, to ensure the directory is not deleted during processing.
    if (!Directory.Exists) throw new DirectoryNotFoundException(Directory.FullName);

    // Creates 'file -> (file,sub-directory)' mappings for all files to be included in the grouping operation.
    var mappings = CreateMappings();

    //// Create all required sub-directories.
    //Directory.CreateSubdirectories(mappings.Select(x => x.Subdirectory).Distinct());

    mappings.ForEach(x => x.MoveToDirectory.EnsureExists()); //.Select(x=> x.MoveToDirectory).en


    mappings.ForEach(x => x.File.MoveTo(x.MoveToDirectory));

    // Move files to their new sub-directory. 
    //mappings.Select(x => (x.File, NewDirectory: Directory.Append(x.Subdirectory)))
    //  .ForEach(x => x.File.MoveTo(x.NewDirectory));

  }

  /// <summary>
  /// Creates an array of mappings: 'file -> sub-directory-name'.
  /// </summary>
  private (FileInfo File, DirectoryInfo MoveToDirectory)[] CreateMappings()
  {
    if (Directory is null) throw new Exception(nameof(Directory) + " is null.");

    return Directory.GetFiles(FileMask)  // File-mask provided by the sub-class
            .Where(FileFilter)           // Predicate provided by the sub-class
            .Select(Map)                 // Mapper provided by the sub-class
            .ToArray();                  // NB: If return type where IEnumerable, then the GetFiles() might be called multiple times.
  }
}
