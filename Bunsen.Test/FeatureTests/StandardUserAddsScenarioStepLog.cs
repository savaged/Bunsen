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
    public void StandardUserAddsAndSavesModel()
    {
        // Given
        var model = GetNewModel();
        var saved = GetNewModel();
        saved.Id = 1;
        _mockDbConnection.SetupDapperAsync(c => c.ExecuteScalarAsync<int>(
            It.IsAny<string>(), null, null, null, null)).ReturnsAsync(1);
        // When
        _mainViewModel?.ScenarioStepLogsViewModel.AddCmd.Execute(null);
        // Then
        Assert.IsNotNull(_mainViewModel?.ScenarioStepLogViewModel.SelectedItem);

        // Given
        _mainViewModel.ScenarioStepLogViewModel.SelectedItem!.Name = model.Name;
        _mainViewModel.ScenarioStepLogViewModel.SelectedItem!.StartOfStep  = model.StartOfStep;
        _mainViewModel.ScenarioStepLogViewModel.SelectedItem!.EndOfStep  = model.EndOfStep;
        // When
        _mainViewModel.ScenarioStepLogViewModel.SaveCmd.Execute(null);
        // Then
        Assert.IsNotNull(_mainViewModel.ScenarioStepLogViewModel.SelectedItem);
        saved = _mainViewModel.ScenarioStepLogViewModel.SelectedItem;
        Assert.AreNotEqual(model.Id, saved.Id);
        Assert.AreEqual(model.Name, saved.Name);
        Assert.AreEqual(model.StartOfStep, saved.StartOfStep);
        Assert.AreEqual(model.EndOfStep, saved.EndOfStep);
    }


    [TestMethod]
    public async Task StandardUserListsModels()
    {
        // Given
        _mockDbConnection.SetupDapperAsync(c => c.QueryAsync<ScenarioStepLog>(
            It.IsAny<string>(), null, null, null, null))
            .ReturnsAsync(new ScenarioStepLog[] { GetNewModel(), GetNewModel() });

        // When
        await _mainViewModel?.LoadAsync()!;
        // Then
        Assert.AreEqual(2, _mainViewModel?.ScenarioStepLogsViewModel.Index.Count);
    }

    [TestMethod]
    public void StandardUserUpdatesModel()
    {
        // Given
        var model = GetNewModel();
        model.Id = 1;
        model.Name = "UpdatedTestStep";
        _mainViewModel!.ScenarioStepLogViewModel.SelectedItem = model;
        // When
        _mainViewModel?.ScenarioStepLogViewModel.SaveCmd.Execute(null);
        // Then
        Assert.IsNotNull(_mainViewModel?.ScenarioStepLogViewModel.SelectedItem);
        var saved = _mainViewModel!.ScenarioStepLogViewModel.SelectedItem;
        Assert.AreEqual(model.Id, saved.Id);
        Assert.AreEqual(model.Name, saved.Name);
    }

    [TestMethod]
    public void StandardUserDeletesModel()
    {
        // Given
        _mockDbConnection.SetupDapperAsync(c => c.QueryAsync<ScenarioStepLog>(
            It.IsAny<string>(), null, null, null, null));

        var model = GetNewModel();
        model.Id = 1;
        model.Name = "DeleteMe";
        _mainViewModel!.ScenarioStepLogViewModel.SelectedItem = model;
        // When
        _mainViewModel?.ScenarioStepLogViewModel.DeleteCmd.Execute(null);
        // Then
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