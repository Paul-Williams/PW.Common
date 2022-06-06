using Microsoft.VisualStudio.TestTools.UnitTesting;
using PW.Extensions;

namespace PW.IO.FileSystemObjects.UnitTests;

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


}
