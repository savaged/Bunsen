using Bunsen.Models;
using Moq;
using Moq.Dapper;
using Dapper;
using Bunsen.Data;
using System.Data.Common;
using Bunsen.ViewModels;
using Bunsen.ViewModels.Core;

namespace Bunsen.Test.FeatureTests;

[TestClass]
public class FeatureScenarioStepLogCRUD
{
    private Mock<DbConnection>? _mockDbConnection;
    private MainViewModel? _mainViewModel;
    
    [TestInitialize]
    public void Init()
    {
        _mockDbConnection = new Mock<DbConnection>();
        _mockDbConnection.SetupDapperAsync(
            c => c.ExecuteAsync(It.IsAny<string>(), It.IsAny<ScenarioStepLog>(), null, null, null))
            .ReturnsAsync(1);
        _mockDbConnection.SetupDapperAsync(c => c.QuerySingleOrDefaultAsync<ScenarioStepLog>(
            It.IsAny<string>(), null, null, null, null)).ReturnsAsync(GetNewModel());
        _mockDbConnection.SetupDapperAsync(c => c.ExecuteAsync(
            It.IsAny<string>(), It.IsAny<ScenarioStepLog>(), null, null, null));

        _mainViewModel = new MainViewModel(
            BusyStateManager.Instance,
            new DataService(_mockDbConnection.Object));
    }

    [TestMethod]
    public void StandardUserAddsModel()
    {
        var model = GetNewModel();
        var saved = GetNewModel();
        saved.Id = 1;
        _mockDbConnection.SetupDapperAsync(c => c.QuerySingleOrDefaultAsync<ScenarioStepLog>(
            It.IsAny<string>(), null, null, null, null)).ReturnsAsync(saved);
        _mockDbConnection.SetupDapperAsync(c => c.ExecuteScalarAsync<int>(
            It.IsAny<string>(), null, null, null, null)).ReturnsAsync(1);
        
        _mainViewModel?.ScenarioStepLogsViewModel.AddCmd.Execute(null);
        Assert.IsNotNull(_mainViewModel?.ScenarioStepLogViewModel.SelectedItem);
        _mainViewModel.ScenarioStepLogViewModel.SelectedItem!.Name = model.Name;
        _mainViewModel.ScenarioStepLogViewModel.SelectedItem!.StartOfStep  = model.StartOfStep;
        _mainViewModel.ScenarioStepLogViewModel.SelectedItem!.EndOfStep  = model.EndOfStep;
        _mainViewModel.ScenarioStepLogViewModel.SaveCmd.Execute(null);
        Assert.IsNotNull(_mainViewModel.ScenarioStepLogViewModel.SelectedItem);
        saved = _mainViewModel.ScenarioStepLogViewModel.SelectedItem;
        Assert.AreNotEqual(model.Id, saved.Id);
        Assert.AreEqual(model.Name, saved.Name);
        Assert.AreEqual(model.StartOfStep, saved.StartOfStep);
        Assert.AreEqual(model.EndOfStep, saved.EndOfStep);
    }

    [TestMethod]
    public void StandardUserAddsModelTwice()
    {
        var model1 = GetNewModel();
        var model2 = GetNewModel();
        var saved1 = GetNewModel();
        saved1.Id = 1;
        var saved2 = GetNewModel();
        saved2.Id = 2;

        _mockDbConnection.SetupDapperAsync(c => c.QuerySingleOrDefaultAsync<ScenarioStepLog>(
            It.IsAny<string>(), null, null, null, null)).ReturnsAsync(
            new Queue<ScenarioStepLog>(new ScenarioStepLog[] { saved1, saved2 }).Dequeue);
        _mockDbConnection.SetupDapperAsync(c => c.ExecuteScalarAsync<int>(
            It.IsAny<string>(), null, null, null, null)).ReturnsAsync(
            new Queue<int>( new int[] { 1, 2 }).Dequeue);
        
        _mainViewModel?.ScenarioStepLogsViewModel.AddCmd.Execute(null);
        Assert.IsNotNull(_mainViewModel?.ScenarioStepLogViewModel.SelectedItem);
        _mainViewModel.ScenarioStepLogViewModel.SelectedItem!.Name = model1.Name;
        _mainViewModel.ScenarioStepLogViewModel.SelectedItem!.StartOfStep  = model1.StartOfStep;
        _mainViewModel.ScenarioStepLogViewModel.SelectedItem!.EndOfStep  = model1.EndOfStep;
        _mainViewModel.ScenarioStepLogViewModel.SaveCmd.Execute(null);
        Assert.IsNotNull(_mainViewModel?.ScenarioStepLogViewModel.SelectedItem);
        saved1 = _mainViewModel.ScenarioStepLogViewModel.SelectedItem;
        Assert.AreNotEqual(model1.Id, saved1.Id);
        Assert.AreEqual(model1.Name, saved1.Name);
        Assert.AreEqual(model1.StartOfStep, saved1.StartOfStep);
        Assert.AreEqual(model1.EndOfStep, saved1.EndOfStep);

        _mainViewModel?.ScenarioStepLogsViewModel.AddCmd.Execute(null);
        Assert.IsNotNull(_mainViewModel?.ScenarioStepLogViewModel.SelectedItem);
        _mainViewModel.ScenarioStepLogViewModel.SelectedItem!.Name = model2.Name;
        _mainViewModel.ScenarioStepLogViewModel.SelectedItem!.StartOfStep  = model2.StartOfStep;
        _mainViewModel.ScenarioStepLogViewModel.SelectedItem!.EndOfStep  = model2.EndOfStep;
        Assert.IsNotNull(_mainViewModel?.ScenarioStepLogViewModel.SelectedItem);
        saved2 = _mainViewModel.ScenarioStepLogViewModel.SelectedItem;
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

        await _mainViewModel?.LoadAsync()!;
        Assert.AreEqual(2, _mainViewModel?.ScenarioStepLogsViewModel.Index.Count);
    }

    [TestMethod]
    public void StandardUserUpdatesModel()
    {
        var model = GetNewModel();
        model.Id = 1;
        model.Name = "UpdatedTestStep";
        _mainViewModel!.ScenarioStepLogViewModel.SelectedItem = model;
        _mainViewModel?.ScenarioStepLogViewModel.SaveCmd.Execute(null);
        Assert.IsNotNull(_mainViewModel?.ScenarioStepLogViewModel.SelectedItem);
        var saved = _mainViewModel!.ScenarioStepLogViewModel.SelectedItem;
        Assert.AreEqual(model.Id, saved.Id);
        Assert.AreEqual(model.Name, saved.Name);
    }

    [TestMethod]
    public void StandardUserDeletesModel()
    {
        _mockDbConnection.SetupDapperAsync(c => c.QueryAsync<ScenarioStepLog>(
            It.IsAny<string>(), null, null, null, null));

        var model = GetNewModel();
        model.Id = 1;
        model.Name = "DeleteMe";
        _mainViewModel!.ScenarioStepLogViewModel.SelectedItem = model;
        _mainViewModel?.ScenarioStepLogViewModel.DeleteCmd.Execute(null);
        Assert.IsNull(_mainViewModel?.ScenarioStepLogViewModel.SelectedItem);
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