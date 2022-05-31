#nullable enable 

using PW.FailFast;
using System;
using System.Collections.Generic;
using System.IO;

namespace PW.IO
{
  public static partial class DirectoryInfoExtensions
  {

    /// <summary>
    /// Returns a list of subdirectories to which access is authorized.
    /// </summary>
    public static List<DirectoryInfo> GetAuthorizedDirectories(this DirectoryInfo directory)
    {
      Guard.NotNull(directory, nameof(directory));

      // We will not catch UnauthorizedAccessException on the initial directory.
      // The initial directory will not be returned. It is not a subdirectory of itself!

      var wd = new WalkData { TrackUnAuthorized = false };

      var subDirectories = directory.GetDirectories();

      foreach (var next in subDirectories)
      {
        wd.Current = next;
        WalkInternal(wd);
      }

      return wd.AuthorizedDirectories;

    }

    /// <summary>
    /// Returns lists of subdirectories to which access is authorized and unauthorized.
    /// </summary>
    public static (List<DirectoryInfo> Authorized, List<DirectoryInfo> Unauthorized) GetAuthorizedAndUnauthorizedDirectories(this DirectoryInfo directory!!)
    {
      var wd = new WalkData { TrackUnAuthorized = true };

      // We will not catch UnauthorizedAccessException on the initial directory
      // The initial directory will not be returned. It is not a subdirectory of itself!

      var subDirectories = directory.GetDirectories();

      foreach (var nextDirectory in subDirectories)
      {
        wd.Current = nextDirectory;
        WalkInternal(wd);
      }

      return (wd.AuthorizedDirectories, wd.UnauthorizedDirectories);

    }


    /// <summary>
    /// Recursive walk down the directory tree structure.
    /// </summary>
    /// <param name="wd"></param>
    private static void WalkInternal(WalkData wd!!)
    {
      try
      {
        var subDirectories = wd.Current!.GetDirectories();

        //If we got here, then the current directory is OK, so add it to the list
        wd.AuthorizedDirectories.Add(wd.Current);

        foreach (var nextDirectory in subDirectories)
        {
          wd.Current = nextDirectory;
          WalkInternal(wd);
        }

      }

      catch (UnauthorizedAccessException)
      {
        if (wd.TrackUnAuthorized) wd.UnauthorizedDirectories.Add(wd.Current!);
      }
    }

    /// <summary>
    /// Used to pass tracking data down the recursive call stack. <see cref="WalkInternal(WalkData)"/>.
    /// </summary>
    private class WalkData
    {
      public WalkData() : this(true) { }


      public WalkData(bool trackUnAuthorized)
      {
        TrackUnAuthorized = trackUnAuthorized;
        AuthorizedDirectories = new List<DirectoryInfo>(1000);
        UnauthorizedDirectories = trackUnAuthorized ? new List<DirectoryInfo>(1000) : new List<DirectoryInfo>();
      }

      public DirectoryInfo? Current;
      public readonly List<DirectoryInfo> AuthorizedDirectories;
      public readonly List<DirectoryInfo> UnauthorizedDirectories;
      public bool TrackUnAuthorized = true;
    }

  }

}
