using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PW.IO.FileSystemObjects
{
    public partial class FileSystem
    {

		/// <summary>
		/// Enumerates a directory on disk and returns a <see cref="DirectoryPath"/> object for each sub-directory./>.
		/// </summary>
		public static IEnumerable<DirectoryPath> EnumerateDirectories(this DirectoryPath directory, System.IO.SearchOption searchOption)
		{
			if (!directory.Exists) throw new DirectoryNotFoundException("Directory not found: " + directory.Path);

			foreach (var dir in directory.ToDirectoryInfo().EnumerateDirectories("*", searchOption))
			{
				yield return (DirectoryPath)dir;
			}

		}


		/// <summary>
		/// Enumerates a directory on disk and returns a <see cref="FilePath"/> object for each file.
		/// </summary>
		public static IEnumerable<FilePath> EnumerateFiles(this DirectoryPath directory, System.IO.SearchOption searchOption)
		{
			if (!directory.Exists) throw new DirectoryNotFoundException("Directory not found: " + directory.Path);

			foreach (var file in directory.ToDirectoryInfo().EnumerateFiles("*.*", searchOption))
			{
				yield return (FilePath)file;
			}
		}

		/// <summary>
		/// Enumerates all files which match <paramref name="searchPattern"/>.
		/// </summary>
		public static IEnumerable<FilePath> EnumerateFiles(this DirectoryPath directory, string searchPattern) =>
			EnumerateFiles(directory, searchPattern, System.IO.SearchOption.TopDirectoryOnly);



		/// <summary>
		/// Enumerates all files with the specified extension.
		/// </summary>
		public static IEnumerable<FilePath> EnumerateFiles(this DirectoryPath directory, FileExtension ofType, System.IO.SearchOption searchOption) =>
			directory.EnumerateFiles(ofType.CreateMask(), searchOption);


		/// <summary>
		/// Enumerates a directory on disk and returns a <see cref="FilePath"/> object for each file.
		/// </summary>
		public static IEnumerable<FilePath> EnumerateFiles(this DirectoryPath directory)
		{
			if (!directory.Exists) throw new DirectoryNotFoundException("Directory not found: " + directory.Path);

			foreach (var file in directory.ToDirectoryInfo().EnumerateFiles())
			{
				yield return (FilePath)file;
			}
		}


		public station IEnumerable<FilePath> EnumerateFilesWithoutExtension(this DirectoryPath directory, FileExtension ofType , System.IO.SearchOption searchOption)
		{
      if (!directory.Exists) throw new DirectoryNotFoundException("Directory not found: " + directory.Path);

      foreach (FilePath file in System.IO.Directory.EnumerateFiles(ofType.CreateMask(), searchOption)
      {
        yield return (FileNameWithoutExtension)file;
      }

    }



  }
}
