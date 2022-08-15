using Bunsen.API;
using Bunsen.Models;
using Moq;
using Moq.Dapper;
using Dapper;
using Bunsen.Data;
using System.Data.Common;

namespace Bunsen.Test.FeatureTests;

[TestClass]
public class FeatureScenarioStepLogCRUD
{
    private IDataService? _dataService;
    
    [TestInitialize]
    public void Init()
    {
        var mockDbConnection = new Mock<DbConnection>();
        mockDbConnection.SetupDapperAsync(
            c => c.ExecuteAsync(It.IsAny<string>(), It.IsAny<ScenarioStepLog>(), null, null, null))
            .ReturnsAsync(1);
        mockDbConnection.SetupDapperAsync(c => c.QuerySingleOrDefaultAsync<ScenarioStepLog>(
            It.IsAny<string>(), null, null, null, null))
            .ReturnsAsync(GetNewModel());

        _dataService = new DataService(mockDbConnection.Object);
    }

    [TestMethod]
    public async Task StandardUserAddsModelLog()
    {
        var model = GetNewModel();
        var saved = await _dataService?.StoreAsync(model)!;
        Assert.AreEqual(model.Id, saved.Id);
        Assert.AreEqual(model.Name, saved.Name);
        Assert.AreEqual(model.StartOfStep, saved.StartOfStep);
        Assert.AreEqual(model.EndOfStep, saved.EndOfStep);
    }

    private ScenarioStepLog GetNewModel()
    {
        return new ScenarioStepLog
        {
            Name = "TestStep",
            StartOfStep = new DateTime(2022, 8, 14, 8, 25, 30),
            EndOfStep = new DateTime(2022, 8, 14, 8, 26, 18),
        };
    }
}