using Microsoft.VisualStudio.TestTools.UnitTesting;
using PW.Extensions;
using System.Linq;

namespace PW.IO.FileSystemObjects.Tests;

[TestClass]
public class FilePathTests
{

  [TestMethod]
  public void TraverseParents1()
  {
    var FilePath1 = (FilePath)@"C:\One\Two\Three\SomeFile.txt";

    // Assumes: File path is three directories deep.
    Assert.IsNotNull(FilePath1.DirectoryPath.Parent);
    Assert.IsNotNull(FilePath1.DirectoryPath.Parent.Parent);
    Assert.IsNotNull(FilePath1.DirectoryPath.Parent.Parent.Parent);

    // This attempts to retrieve the parent of the top-level directory, which will not exist.
    Assert.IsNull(FilePath1.DirectoryPath.Parent.Parent.Parent.Parent);

  }

  [TestMethod]
  public void TestJoin()
  {
    var seq =  new[]  {"one","two","three"};

    Assert.AreEqual("one,two,three", seq.Join(','));
    Assert.AreEqual("one,two,three", seq.JoinWithCommas());
    Assert.AreEqual("one two three", seq.JoinWithSpaces());

    Assert.AreEqual("\"one\",\"two\",\"three\"", seq.Join(',',true));
    Assert.AreEqual("\"one\",\"two\",\"three\"", seq.JoinWithCommas(true));
    Assert.AreEqual("\"one\" \"two\" \"three\"", seq.JoinWithSpaces(true));


  }

  [TestMethod]
  public void StringExtensionTests()
  {
    Assert.AreEqual("On", "One".RemoveLastCharacter());

    Assert.IsTrue("\"value\"".IsEnquoted());
    Assert.IsTrue("\"\"".IsEnquoted());
    Assert.IsFalse("\"value".IsEnquoted());
    Assert.IsFalse("value\"".IsEnquoted());
    Assert.IsFalse("s".IsEnquoted());

    Assert.AreEqual("value", "\"value\"".Unenquote());
    Assert.AreEqual("", "\"\"".Unenquote());


  }

  [TestMethod]
  public void TestPathEquality()
  {
    Assert.IsTrue(new FilePath(@"C:\Temp\Test.txt").DirectoryPath == new FilePath(@"C:\temp\Test.txt").DirectoryPath);
  }

  [TestMethod]
  public void TestDirectoryPathToHashTable()
  {
    var d = new DirectoryPath[] { new DirectoryPath(@"c:\temp") };

    var h = d[0].GetHashCode();
    var h1 = System.StringComparer.OrdinalIgnoreCase.GetHashCode(d[0].Path);
    var h2 = System.StringComparer.OrdinalIgnoreCase.GetHashCode(@"c:\temp\");

    var Directories = d.ToHashSet();
  }


}
