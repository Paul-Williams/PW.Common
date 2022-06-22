using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace PW.IO.FileInfoExtensions.Tests;

[TestClass]
public class FileInfoExtensionsTests
{

  [TestMethod]
  public void WaitForAccess_IsNotNull()
  {
    var TestFile = new FileInfo(Path.Combine(
      Environment.GetEnvironmentVariable("TEMP"), "WaitForAccess" + DateTime.Now.Ticks.ToString() + ".TestFile"));

    using var cfs = File.Create( TestFile.FullName);
    cfs?.Close();
    cfs?.Dispose();


    using var fs = TestFile.WaitForAccess(TimeSpan.FromMilliseconds(100));
    Assert.IsNotNull(fs);
    fs?.Close();
    fs?.Dispose();

    File.Delete(TestFile.FullName);

  }

  [TestMethod]
  public void WaitForAccess_IsNull()
  {
    var TestFile = new FileInfo(Path.Combine(
      Environment.GetEnvironmentVariable("TEMP"), "WaitForAccess" + DateTime.Now.Ticks.ToString() + ".TestFile"));

    var cfs = File.Create(TestFile.FullName);

    using var fs = TestFile.WaitForAccess(TimeSpan.FromMilliseconds(100));
    Assert.IsNull(fs);
    fs?.Close();
    fs?.Dispose();

    cfs?.Close();
    cfs?.Dispose();
    File.Delete(TestFile.FullName);

  }

  [TestMethod]
  public void WaitForAccess_ThrowsIOException()
  {
    var TestFile = new FileInfo(Path.Combine(
      Environment.GetEnvironmentVariable("TEMP"), "WaitForAccess" + DateTime.Now.Ticks.ToString() + ".TestFile"));

    Assert.ThrowsException<IOException>(()=>TestFile.WaitForAccess(TimeSpan.FromMilliseconds(100)));


  }


}
