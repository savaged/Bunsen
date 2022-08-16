using Bunsen.API;
using Bunsen.Bootstrap;
using Bunsen.Models;

namespace Bunsen.Test.IntegrationTests;

[TestClass]
public class DataAccessIntegrationTests
{
    private readonly Bootstrapper _bootstrapper;
    private IDataService? _dataService;

    public DataAccessIntegrationTests()
    {
        _bootstrapper = new Bootstrapper();
    }

    [TestInitialize]
    public void Init()
    {
        _dataService = _bootstrapper.Resolve<IDataService>();
    }

    [TestMethod]
    public async Task CRUDTest()
    {
        var model = GetNewModel();
        var saved1 = await _dataService?.StoreAsync(model)!;
        Assert.IsNotNull(saved1);

        model = GetNewModel();
        var saved2 = await _dataService?.StoreAsync(model)!;
        Assert.IsNotNull(saved2);
        Assert.AreNotEqual(saved1.Id, saved2.Id);

        var index = await _dataService?.IndexAsync<ScenarioStepLog>()!;
        Assert.IsNotNull(index);
        Assert.AreEqual(2, index.Count());
        var selected = index.FirstOrDefault();
        Assert.IsNotNull(selected);
        selected.Name = $"{model.Name} updated";
        _dataService?.UpdateAsync(selected);

        var shown = await _dataService?.ShowAsync<ScenarioStepLog>(selected.Id)!;
        Assert.IsNotNull(shown);
        Assert.AreEqual(selected.Name, shown.Name);
        
        index = await _dataService?.IndexAsync<ScenarioStepLog>()!;
        Assert.IsNotNull(index);
        foreach (var m in index)
        {
            Assert.IsNotNull(m);
            await _dataService?.DeleteAsync<ScenarioStepLog>(m.Id)!;
        }
        index = await _dataService?.IndexAsync<ScenarioStepLog>()!;
        Assert.IsNotNull(index);
        Assert.AreEqual(0, index.Count());
    }

    private static ScenarioStepLog GetNewModel()
    {
        return new ScenarioStepLog
        {
            Name = "TestStep",
            StartOfStep = new DateTime(2022, 8, 14, 8, 25, 30),
            EndOfStep = new DateTime(2022, 8, 14, 8, 26, 18),
        };
    }

}
