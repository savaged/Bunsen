using Bunsen.API;
using Bunsen.Models;
using Moq;

namespace Bunsen.Test.FeatureTests;

[TestClass]
public class FeatureScenarioStepLogCRUD
{
    private IDataService? _dataService;
    
    [TestInitialize]
    public void Init()
    {
        // TODO switch to mocking the dbconnection
        Mock<IDataService> mock = new Mock<IDataService>();
        mock.Setup(s => s.StoreAsync(It.IsAny<ScenarioStepLog>()).Result).Returns<ScenarioStepLog>(m => m);
        _dataService = mock.Object;
    }

    [TestMethod]
    public async Task StandardUserAddsScenarioStepLog()
    {
        var model = new ScenarioStepLog
        {
            Name = "TestStep1",
            StartOfStep = new DateTime(2022, 8, 14, 8, 25, 30),
            EndOfStep = new DateTime(2022, 8, 14, 8, 26, 18),
        };
        var saved = await _dataService?.StoreAsync(model)!;
        Assert.AreEqual(model, saved);
    }
}