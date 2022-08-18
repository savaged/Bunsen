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
}