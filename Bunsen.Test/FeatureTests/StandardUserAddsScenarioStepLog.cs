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
    private Mock<DbConnection>? _mockDbConnection;
    
    [TestInitialize]
    public void Init()
    {
        _mockDbConnection = new Mock<DbConnection>();
        _mockDbConnection.SetupDapperAsync(
            c => c.ExecuteAsync(It.IsAny<string>(), It.IsAny<ScenarioStepLog>(), null, null, null))
            .ReturnsAsync(1);
        _mockDbConnection.SetupDapperAsync(c => c.QuerySingleOrDefaultAsync<ScenarioStepLog>(
            It.IsAny<string>(), null, null, null, null)).ReturnsAsync(GetNewModel());
        _mockDbConnection.SetupDapperAsync(c => c.ExecuteScalarAsync<int>(
            It.IsAny<string>(), null, null, null, null)).ReturnsAsync(1);

        _dataService = new DataService(_mockDbConnection.Object);
    }

    [TestMethod]
    public async Task StandardUserAddsModel()
    {
        var model = GetNewModel();
        var saved = GetNewModel();
        saved.Id = 1;
        _mockDbConnection.SetupDapperAsync(c => c.QuerySingleOrDefaultAsync<ScenarioStepLog>(
            It.IsAny<string>(), null, null, null, null)).ReturnsAsync(saved);
        saved = await _dataService?.StoreAsync(model)!;
        Assert.AreNotEqual(model.Id, saved.Id);
        Assert.AreEqual(model.Name, saved.Name);
        Assert.AreEqual(model.StartOfStep, saved.StartOfStep);
        Assert.AreEqual(model.EndOfStep, saved.EndOfStep);
    }

    [TestMethod]
    public async Task StandardUserAddsModelTwice()
    {
        var model1 = GetNewModel();
        var saved1 = GetNewModel();
        saved1.Id = 1;
        _mockDbConnection.SetupDapperAsync(c => c.QuerySingleOrDefaultAsync<ScenarioStepLog>(
            It.IsAny<string>(), null, null, null, null)).ReturnsAsync(saved1);
        saved1 = await _dataService?.StoreAsync(model1)!;
        Assert.AreNotEqual(model1.Id, saved1.Id);
        Assert.AreEqual(model1.Name, saved1.Name);
        Assert.AreEqual(model1.StartOfStep, saved1.StartOfStep);
        Assert.AreEqual(model1.EndOfStep, saved1.EndOfStep);

        _mockDbConnection.SetupDapperAsync(c => c.ExecuteScalarAsync<int>(
            It.IsAny<string>(), null, null, null, null)).ReturnsAsync(2);

        var model2 = GetNewModel();
        var saved2 = GetNewModel();
        saved2.Id = 2;
        _mockDbConnection.SetupDapperAsync(c => c.QuerySingleOrDefaultAsync<ScenarioStepLog>(
            It.IsAny<string>(), null, null, null, null)).ReturnsAsync(saved2);
        saved2 = await _dataService?.StoreAsync(model2)!;
        Assert.AreNotEqual(model2.Id, saved2.Id);
        Assert.AreEqual(model2.Name, saved2.Name);
        Assert.AreEqual(model2.StartOfStep, saved2.StartOfStep);
        Assert.AreEqual(model2.EndOfStep, saved2.EndOfStep);

        Assert.AreNotEqual(saved1.Id, saved2.Id);
    }

    [TestMethod]
    public async Task StandardUserListsModels()
    {
        _mockDbConnection.SetupDapperAsync(c => c.QueryAsync<ScenarioStepLog>(
            It.IsAny<string>(), null, null, null, null))
            .ReturnsAsync(new ScenarioStepLog[] { GetNewModel(), GetNewModel() });

        var index = await _dataService?.IndexAsync<ScenarioStepLog>()!;
        Assert.AreEqual(2, index.Count());
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