using Bunsen.Utils;

namespace Bunsen.Test.UnitTests;

[TestClass]
public class ExtensionsTests
{
    [TestMethod]
    public void CloneTest()
    {
        var eg = new Example { Field = "test" };
        var clone = eg.Clone();
        Assert.IsNotNull(clone);
        Assert.AreEqual(eg.Field, clone.Field);
        Assert.AreNotEqual(eg, clone);
    }

}

internal class Example
{
    public string? Field { get; set; }
}
