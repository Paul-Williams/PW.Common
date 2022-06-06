using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using PW.ValueObjects;

namespace UnitTests;

[TestClass]
public class PW_ValueObjects_NamedValue
{

  [TestMethod]
  public void Test_PW_ValueObjects_NamedValue()
  {
    Assert.ThrowsException<ArgumentNullException>(() => { var r = new NamedValue<string>("Name", null); }, "null value");
    Assert.ThrowsException<ArgumentNullException>(() => { var r = new NamedValue<string>(null, "value"); }, "null name");

    Assert.IsNotNull(new NamedValue<string>("Name", "value"));
    Assert.IsNotNull(new NamedValue<int>("Name", 6));

    var t = new NamedValue<int>("Name", 6);

    Assert.IsTrue(t.Value == 6);

    Assert.AreEqual(t, new NamedValue<int>("Name", 6));
    Assert.AreNotEqual(t, new NamedValue<int>("Name", 7));

    Assert.AreEqual(new NamedValue<string>("Name", "value"), new NamedValue<string>("Name", "value"));
    Assert.AreNotEqual(new NamedValue<string>("Name", "VALUE"), new NamedValue<string>("Name", "value"));

    var cis1 = CaseInsensitiveString.From("value");
    var cis2 = CaseInsensitiveString.From("VALUE");
    var cis3 = (CaseInsensitiveString)"Test";

    Assert.IsTrue(cis1 == cis2);
    Assert.IsTrue(cis1.Equals(cis2));
    Assert.IsTrue(Equals(cis1, cis2));
    Assert.IsTrue(cis1 != cis3);

    Assert.AreEqual(new NamedValue<CaseInsensitiveString>("Name", cis1), new NamedValue<CaseInsensitiveString>("Name", cis2));

  }
}
