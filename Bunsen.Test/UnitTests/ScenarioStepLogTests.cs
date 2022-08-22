using Bunsen.Models;

namespace Bunsen.Test.UnitTests;

[TestClass]
public class ScenarioStepLogTests
{
    [TestMethod]
    public void CalculateDurationOfStepTest()
    {
        var model = new ScenarioStepLog
        {
            Name = "TestStep",
            StartOfStep = new DateTime(2022, 8, 14, 8, 25, 30),
            EndOfStep = new DateTime(2022, 8, 14, 8, 26, 18),
        };
        var result = model.CalculateSecondsTakenOfStep();
        Assert.AreEqual(48, result);
    }

    [TestMethod]
    public void Temp()
    {
        var _ = DateTime.TryParse("2022-08-14 08:25:30", out var dt);
        Assert.AreEqual(new DateTime(2022, 8, 14, 8, 25, 30), dt);
    }

}