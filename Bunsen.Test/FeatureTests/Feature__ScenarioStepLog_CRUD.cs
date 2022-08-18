using Bunsen.Models;
using Moq;
using Moq.Dapper;
using Dapper;
using Bunsen.Data;
using System.Data.Common;
using System.Reflection;
using Bunsen.Utils;
using Bunsen.ViewModels;
using Bunsen.ViewModels.Core;

namespace Bunsen.Test.FeatureTests;

[TestClass]
public class Feature__ScenarioStepLog_CRUD
{
    private Mock<DbConnection>? _mockDbConnection;
    private MainViewModel? _mainViewModel;
    
    /// <summary>
    /// Note:
    /// 
    ///     Given definition of 'not new' is the ScenarioStepLog.Id is greater than 0
    ///
    /// </summary>
    [TestInitialize]
    public void Background__()
    {
        Given_a_ScenarioStepLog_database_is_loaded();
        Given_the_Main_View_is_open();
    }

    [TestMethod]
    public void Scenario__A_Standard_User_adds_a_ScenarioStepLog()
    {
        When_a_Standard_User_clicks_Add();

        Then_a_new_ScenarioStepLog_is_available();
    }

    [TestMethod]
    public void Scenario__A_Standard_User_saves_a_new_ScenarioStepLog()
    {
        Given_a_new_ScenarioStepLog();
        Given_the_ScenarioStepLog_Id_is(0);
        Given_the_ScenarioStepLog_Name_is("TestStep");
        Given_the_ScenarioStepLog_IsPassing_is(false);
        Given_the_ScenarioStepLog_StartOfStep_is("2022-08-14 08:25:30");
        Given_the_ScenarioStepLog_EndOfStep_is("2022-08-14 08:26:18");

        When_a_Standard_User_clicks_Save();

        Then_a_new_ScenarioStepLog_is_available();
        Then_the_saved_ScenarioStepLog_is_not_new();
        Then_the_ScenarioStepLog_Name_is("TestStep");
        Then_the_ScenarioStepLog_IsPassing_is(false);
        Then_the_saved_ScenarioStepLog_StartOfStep_is("2022-08-14 08:25:30");
        Then_the_saved_ScenarioStepLog_EndOfStep_is("2022-08-14 08:26:18");
    }

    [TestMethod]
    public async Task Scenario__A_Standard_User_lists_all_ScenarioStepLogs()
    {
        Given_there_are_2_ScenarioStepLog_that_are_not_new();

        await When_a_Standard_User_opens_the_main_view();
    
        Then_2_ScenarioStepLogs_are_listed();
    }

    [TestMethod]
    public void Scenario__A_Standard_User_saves_an_existing_ScenarioStepLog()
    {
        Given_an_existing_ScenarioStepLog();
        Given_the_ScenarioStepLog_Id_is(1);
        Given_the_ScenarioStepLog_Name_is("UpdatedTestStep");
        Given_the_ScenarioStepLog_IsPassing_is_toggled_to(true);
        Given_the_ScenarioStepLog_StartOfStep_is("2022-08-14 08:25:30");
        Given_the_ScenarioStepLog_EndOfStep_is("2022-08-14 08:26:18");

        When_a_Standard_User_clicks_Save();

        Then_a_new_ScenarioStepLog_is_available();
        Then_the_ScenarioStepLog_Id_is(1);
        Then_the_ScenarioStepLog_Name_is("UpdatedTestStep");
        Then_the_ScenarioStepLog_IsPassing_is(true);
        Then_the_saved_ScenarioStepLog_StartOfStep_is("2022-08-14 08:25:30");
        Then_the_saved_ScenarioStepLog_EndOfStep_is("2022-08-14 08:26:18");
    }

    [TestMethod]
    public void Scenario__A_Standard_User_deletes_an_existing_ScenarioStepLog()
    {
        Given_an_existing_ScenarioStepLog();
        Given_the_ScenarioStepLog_Id_is(1);
        Given_the_ScenarioStepLog_Name_is("DeleteMe");
        Given_the_ScenarioStepLog_IsPassing_is(false);
        Given_the_ScenarioStepLog_StartOfStep_is("2022-08-14 08:25:30");
        Given_the_ScenarioStepLog_EndOfStep_is("2022-08-14 08:26:18");

        When_a_Standard_User_clicks_Delete();

        Then_the_ScenarioStepLog_is_gone();
    }


    private void Given_a_ScenarioStepLog_database_is_loaded()
    {
        _mockDbConnection = new Mock<DbConnection>();
        _mockDbConnection.SetupDapperAsync(
            c => c.ExecuteAsync(It.IsAny<string>(), It.IsAny<ScenarioStepLog>(), null, null, null))
            .ReturnsAsync(1);
        _mockDbConnection.SetupDapperAsync(
            c => c.ExecuteAsync(It.IsAny<string>(), It.IsAny<ScenarioStepLog>(), null, null, null))
            .ReturnsAsync(1);
    }

    private void Given_the_Main_View_is_open()
    {
        if (_mockDbConnection != null)
        {
            _mainViewModel = new MainViewModel(
                BusyStateManager.Instance,
                new DataService(_mockDbConnection.Object));
        }
    }

    private void Given_a_new_ScenarioStepLog()
    {
        var model = new ScenarioStepLog();
        if (_mainViewModel != null)
        {
            _mainViewModel.ScenarioStepLogViewModel.SelectedItem = model;
        }
    }

    private void Given_an_existing_ScenarioStepLog()
    {
        var model = new ScenarioStepLog { Id = 1 };
        if (_mainViewModel != null)
        {
            _mainViewModel.ScenarioStepLogViewModel.SelectedItem = model;
        }
    }

    private void Given_the_ScenarioStepLog_Id_is(int value)
    {
        if (_mainViewModel != null && _mainViewModel.ScenarioStepLogViewModel.SelectedItem != null)
        {
            _mainViewModel.ScenarioStepLogViewModel.SelectedItem.Id = value;
        }
    }

    private void Given_the_ScenarioStepLog_Name_is(string value)
    {
        if (_mainViewModel != null && _mainViewModel.ScenarioStepLogViewModel.SelectedItem != null)
        {
            _mainViewModel.ScenarioStepLogViewModel.SelectedItem.Name = value;
        }
    }

    private void Given_the_ScenarioStepLog_IsPassing_is(bool value)
    {
        if (_mainViewModel != null && _mainViewModel.ScenarioStepLogViewModel.SelectedItem != null)
        {
            _mainViewModel.ScenarioStepLogViewModel.SelectedItem.IsPassing = value;
        }
    }

    private void Given_the_ScenarioStepLog_StartOfStep_is(string value)
    {
        if (_mainViewModel != null && _mainViewModel.ScenarioStepLogViewModel.SelectedItem != null)
        {
            var _ = DateTime.TryParse(value, out var dt);
            _mainViewModel.ScenarioStepLogViewModel.SelectedItem.StartOfStep = dt;
        }
    }
    
    private void Given_the_ScenarioStepLog_EndOfStep_is(string value)
    {
        if (_mainViewModel != null && _mainViewModel.ScenarioStepLogViewModel.SelectedItem != null)
        {
            var _ = DateTime.TryParse(value, out var dt);
            _mainViewModel.ScenarioStepLogViewModel.SelectedItem.EndOfStep = dt;
        }
    }

    private void Given_there_are_2_ScenarioStepLog_that_are_not_new()
    {
        var one = new ScenarioStepLog
        {
            Id = 1,
            Name = "ExistingTestStep",
            StartOfStep = DateTime.Parse("2022-08-14 08:25:30"),
            EndOfStep = DateTime.Parse("2022-08-14 08:26:18")
        };
        var two = one.Clone();
        if (two == null) return;
        two.Id = 2;
        _mockDbConnection.SetupDapperAsync(c => c.QueryAsync<ScenarioStepLog>(
            It.IsAny<string>(), null, null, null, null))
            .ReturnsAsync(new[] { one, two });
    }

    private void Given_the_ScenarioStepLog_IsPassing_is_toggled_to(bool value)
    {
        Given_the_ScenarioStepLog_IsPassing_is(value);
    }


    private void When_a_Standard_User_clicks_Add()
    {
        _mainViewModel?.ScenarioStepLogsViewModel.AddCmd.Execute(null);
    }

    private void When_a_Standard_User_clicks_Save()
    {
        _mockDbConnection.SetupDapperAsync(c => c.ExecuteScalarAsync<int>(
            It.IsAny<string>(), null, null, null, null)).ReturnsAsync(1);
        
        _mainViewModel?.ScenarioStepLogViewModel.SaveCmd.Execute(null);
    }

    private async Task When_a_Standard_User_opens_the_main_view()
    {
        await _mainViewModel?.LoadAsync()!;
    }

    private void When_a_Standard_User_clicks_Delete()
    {
        _mockDbConnection.SetupDapperAsync(c => c.QueryAsync<ScenarioStepLog>(
            It.IsAny<string>(), null, null, null, null));

        _mainViewModel?.ScenarioStepLogViewModel.DeleteCmd.Execute(null);
    }


    private void Then_a_new_ScenarioStepLog_is_available()
    {
        Assert.IsNotNull(_mainViewModel?.ScenarioStepLogViewModel.SelectedItem,
            MethodBase.GetCurrentMethod()?.Name);
    }

    private void Then_the_saved_ScenarioStepLog_is_not_new()
    {
        Assert.IsTrue(_mainViewModel?.ScenarioStepLogViewModel.SelectedItem?.Id > 0,
            MethodBase.GetCurrentMethod()?.Name);
    }

    private void Then_the_ScenarioStepLog_Name_is(string value)
    {
        Assert.AreEqual(value, _mainViewModel?.ScenarioStepLogViewModel.SelectedItem?.Name,
            MethodBase.GetCurrentMethod()?.Name);
    }
    
    private void Then_the_ScenarioStepLog_IsPassing_is(bool value)
    {
        Assert.AreEqual(value, _mainViewModel?.ScenarioStepLogViewModel.SelectedItem?.IsPassing,
            MethodBase.GetCurrentMethod()?.Name);
    }

    private void Then_the_saved_ScenarioStepLog_StartOfStep_is(string value)
    {
        var _ = DateTime.TryParse(value, out var dt);
        Assert.AreEqual(dt, _mainViewModel?.ScenarioStepLogViewModel.SelectedItem?.StartOfStep,
            MethodBase.GetCurrentMethod()?.Name);
    }
    
    private void Then_the_saved_ScenarioStepLog_EndOfStep_is(string value)
    {
        var _ = DateTime.TryParse(value, out var dt);
        Assert.AreEqual(dt, _mainViewModel?.ScenarioStepLogViewModel.SelectedItem?.EndOfStep,
            MethodBase.GetCurrentMethod()?.Name);
    }

    private void Then_2_ScenarioStepLogs_are_listed()
    {
        Assert.AreEqual(2, _mainViewModel?.ScenarioStepLogsViewModel.Index.Count,
            MethodBase.GetCurrentMethod()?.Name);
    }

    private void Then_the_ScenarioStepLog_Id_is(int id)
    {
        Assert.AreEqual(id, _mainViewModel?.ScenarioStepLogViewModel.SelectedItem?.Id,
            MethodBase.GetCurrentMethod()?.Name);
    }

    private void Then_the_ScenarioStepLog_is_gone()
    {
        Assert.IsNull(_mainViewModel?.ScenarioStepLogViewModel.SelectedItem,
            MethodBase.GetCurrentMethod()?.Name);
    }

}