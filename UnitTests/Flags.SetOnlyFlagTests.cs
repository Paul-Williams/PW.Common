using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PW.Flags.UnitTests;

[TestClass]
public class SetOnlyFlagTests
{

  private SetOnlyFlag FlagAsProperty { get; } = new SetOnlyFlag();

  [TestMethod]
  public void AsProperty_IsSet_True()
  {
    //var flag = new SetOnlyFlag();
    FlagAsProperty.Set();
    Assert.IsTrue(FlagAsProperty.IsSet);
  }

  [TestMethod]
  public void AsField_IsSet_True()
  {
    var flag = new SetOnlyFlag();
    flag.Set();
    Assert.IsTrue(flag.IsSet);
  }

  [TestMethod]
  public void Set_Twice_Ok()
  {
    var flag = new SetOnlyFlag();
    flag.Set();
    Assert.IsTrue(flag.IsSet);
    flag.Set();
    Assert.IsTrue(flag.IsSet);
  }


}
