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
    Directory = directory ?? throw new ArgumentNullException(nameof(directory));
    FileMask = fileMask ?? throw new ArgumentNullException(nameof(fileMask));
  }

  /// <summary>
  /// Creates a new instance. Called from the sub-class
  /// </summary>
  public FileGrouperBase(DirectoryInfo directory) =>
    Directory = directory ?? throw new ArgumentNullException(nameof(directory));

  /// <summary>
  /// Logic to determine whether to include a file in the grouping operation.
  /// </summary>
  protected abstract bool FileFilter(FileInfo file);

  /// <summary>
  /// Logic to map (move) a file to a new directory.
  /// </summary>
  protected abstract (FileInfo File, FileInfo NewLocation) Map(FileInfo file);

  /// <summary>
  /// Directory containing files to be 'grouped' into sub-directories.
  /// </summary>
  protected DirectoryInfo Directory { get; set; }

  /// <summary>
  /// Mask used to obtain list of files within <see cref="Directory"/>
  /// </summary>
  protected string FileMask { get; } = "*.*";

  /// <summary>
  /// Performs the 'grouping' of sequentially named files into sub-directories.
  /// </summary>
  public void Perform()
  {
    // Ensure the directory still exists.
    // Could create a lock-file at this point, to ensure the directory is not deleted during processing.
    if (!Directory.Exists) throw new DirectoryNotFoundException("Directory not found: " + Directory.FullName);

    // Creates 'file -> (file, file)' mappings for all files to be included in the grouping operation.
    var mappings = CreateMappings();


    // HACK: Added ! in Select in the code below to remove warning: CS8620
    // The .NET5 declaration is FileInfo.Directory? making it clear that the property may return null. 
    // This will still throw an exception if a returned DirectoryInfo is indeed null.

    // Create any required sub-directories.
    // This is done separately from the move operation so we can
    // work with a distinct list of directories and save some hits to the disk.      
    mappings.Select(x => x.NewLocation.Directory!)
      .Distinct(DirectoryInfoPathEqualityComparer.Instance)
      .ForEach(x => x.Create());

    // Move each file to its new location.
    mappings.ForEach(x => x.CurrentLocation.MoveTo(x.NewLocation));

  }

  /// <summary>
  /// Creates an array of mappings: 'file -> sub-directory-name'.
  /// </summary>
  private (FileInfo CurrentLocation, FileInfo NewLocation)[] CreateMappings()
  {
    if (!Directory.Exists) throw new DirectoryNotFoundException("Directory not found: " + Directory.FullName);

    return Directory.EnumerateFiles(FileMask)   // File-mask provided by the sub-class
            .Where(FileFilter)                  // Predicate provided by the sub-class
            .Select(Map)                        // Mapper provided by the sub-class
            .ToArray();                         // NB: If IEnumerable were returned, the call may inadvertently enumerate the directory multiple times.
  }
}






// Version before restoring modified code from 'FilesToSubdirectories' project.

///// <summary>
///// Provides base functionality to classes designed to 'group' files into sub-directories.
///// </summary>
//public abstract class FileGrouperBase : IOperation
//{
//  /// <summary>
//  /// Creates a new instance. Called from the sub-class
//  /// </summary>
//  public FileGrouperBase(DirectoryInfo directory, string fileMask)
//  {
//    Directory = directory;
//    FileMask = fileMask;
//  }

//  /// <summary>
//  /// Creates a new instance. Called from the sub-class
//  /// </summary>
//  public FileGrouperBase(DirectoryInfo directory)
//  {
//    Directory = directory;
//  }

//  /// <summary>
//  /// Logic to determine whether to include a file in the grouping operation.
//  /// </summary>
//  protected abstract bool FileFilter(FileInfo file);

//  /// <summary>
//  /// Logic to map (move) a file to a new directory.
//  /// </summary>
//  protected abstract (FileInfo File, DirectoryInfo MoveToDirectory) Map(FileInfo file);

//  /// <summary>
//  /// Directory containing files to be 'grouped' into sub-directories.
//  /// </summary>
//  protected DirectoryInfo? Directory { get; set; }

//  /// <summary>
//  /// Mask used to obtain list of files within <see cref="Directory"/>
//  /// </summary>
//  protected string FileMask { get; } = "*.*";

//  /// <summary>
//  /// Performs the 'grouping' of sequentially named files into sub-directories.
//  /// </summary>
//  public void Perform()
//  {
//    if (Directory is null) throw new Exception(nameof(Directory) + " is null.");
//    // Ensure the directory still exists.
//    // Could create a lock-file at this point, to ensure the directory is not deleted during processing.
//    if (!Directory.Exists) throw new DirectoryNotFoundException(Directory.FullName);

//    // Creates 'file -> (file,sub-directory)' mappings for all files to be included in the grouping operation.
//    var mappings = CreateMappings();

//    //// Create all required sub-directories.
//    //Directory.CreateSubdirectories(mappings.Select(x => x.Subdirectory).Distinct());

//    mappings.ForEach(x => x.MoveToDirectory.EnsureExists()); //.Select(x=> x.MoveToDirectory).en


//    mappings.ForEach(x => x.File.MoveTo(x.MoveToDirectory));

//    // Move files to their new sub-directory. 
//    //mappings.Select(x => (x.File, NewDirectory: Directory.Append(x.Subdirectory)))
//    //  .ForEach(x => x.File.MoveTo(x.NewDirectory));

//  }

//  /// <summary>
//  /// Creates an array of mappings: 'file -> sub-directory-name'.
//  /// </summary>
//  private (FileInfo File, DirectoryInfo MoveToDirectory)[] CreateMappings()
//  {
//    if (Directory is null) throw new Exception(nameof(Directory) + " is null.");

//    return Directory.GetFiles(FileMask)  // File-mask provided by the sub-class
//            .Where(FileFilter)           // Predicate provided by the sub-class
//            .Select(Map)                 // Mapper provided by the sub-class
//            .ToArray();                  // NB: If return type where IEnumerable, then the GetFiles() might be called multiple times.
//  }
//}
