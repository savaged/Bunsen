using Bunsen.Data;
using Bunsen.Models;
using Bunsen.Utils;
using Bunsen.ViewModels;
using Bunsen.ViewModels.Core;
using Dapper;
using Moq;
using Moq.Dapper;
using System.Data.Common;
using System.Reflection;
using TechTalk.SpecFlow.Infrastructure;

namespace Bunsen.Specs.StepDefinitions;

[Binding]
public class ScenarioStepLogViewModelsCRUDStepDefinitions
{
    private readonly ISpecFlowOutputHelper _specFlowOutputHelper;
    private Mock<DbConnection>? _mockDbConnection;
    private MainViewModel? _mainViewModel;

    public ScenarioStepLogViewModelsCRUDStepDefinitions(ISpecFlowOutputHelper specFlowOutputHelper)
    {
        _specFlowOutputHelper = specFlowOutputHelper;

        GivenAScenarioStepLogDatabaseIsLoaded();
        GivenTheMainViewIsOpen();
    }

    [Then(@"a new ScenarioStepLog is available")]
    public void ThenANewScenarioStepLogIsAvailable()
    {
        Assert.IsNotNull(_mainViewModel?.ScenarioStepLogViewModel.SelectedItem,
            MethodBase.GetCurrentMethod()?.Name);
    }

    [Given(@"a new ScenarioStepLog")]
    public void GivenANewScenarioStepLog()
    {
        var model = new ScenarioStepLog();
        if (_mainViewModel != null)
        {
            _mainViewModel.ScenarioStepLogViewModel.SelectedItem = model;
        }
    }

    [Given(@"the ScenarioStepLog Name is (.*)")]
    public void GivenTheScenarioStepLogNameIs(string value)
    {
        if (_mainViewModel != null && _mainViewModel.ScenarioStepLogViewModel.SelectedItem != null)
        {
            _mainViewModel.ScenarioStepLogViewModel.SelectedItem.Name = value;
        }
    }

    [Given(@"the ScenarioStepLog StartOfStep is (.*)")]
    public void GivenTheScenarioStepLogStartOfStepIs(string value)
    {
        if (_mainViewModel != null && _mainViewModel.ScenarioStepLogViewModel.SelectedItem != null)
        {
            var _ = DateTime.TryParse(value, out var dt);
            _mainViewModel.ScenarioStepLogViewModel.SelectedItem.StartOfStep = dt;
        }
    }

    [Given(@"the ScenarioStepLog EndOfStep is (.*)")]
    public void GivenTheScenarioStepLogEndOfStepIs(string value)
    {
        if (_mainViewModel != null && _mainViewModel.ScenarioStepLogViewModel.SelectedItem != null)
        {
            var _ = DateTime.TryParse(value, out var dt);
            _mainViewModel.ScenarioStepLogViewModel.SelectedItem.EndOfStep = dt;
        }
    }

    [Then(@"a saved ScenarioStepLog is available")]
    public void ThenASavedScenarioStepLogIsAvailable()
    {
        Assert.IsNotNull(_mainViewModel?.ScenarioStepLogViewModel.SelectedItem,
            MethodBase.GetCurrentMethod()?.Name);
    }

    [Then(@"the saved ScenarioStepLog is not new")]
    public void ThenTheSavedScenarioStepLogIsNotNew()
    {
        Assert.IsTrue(_mainViewModel?.ScenarioStepLogViewModel.SelectedItem?.Id > 0,
            MethodBase.GetCurrentMethod()?.Name);
    }

    [Then(@"the ScenarioStepLog Name is (.*)")]
    public void ThenTheScenarioStepLogNameIs(string value)
    {
        Assert.AreEqual(value, _mainViewModel?.ScenarioStepLogViewModel.SelectedItem?.Name,
            MethodBase.GetCurrentMethod()?.Name);
    }

    [Then(@"the ScenarioStepLog IsPassing is true")]
    public void ThenTheScenarioStepLogIsPassingIsTrue()
    {
        Assert.IsTrue(_mainViewModel?.ScenarioStepLogViewModel.SelectedItem?.IsPassing,
            MethodBase.GetCurrentMethod()?.Name);
    }

    [When(@"a Standard User clicks Add")]
    public void WhenAStandardUserClicksAdd()
    {
        _mainViewModel?.ScenarioStepLogsViewModel.AddCmd.Execute(null);
    }

    [Given(@"the ScenarioStepLog IsPassing is false")]
    public void GivenTheScenarioStepLogIsPassingIsFalse()
    {
        if (_mainViewModel != null && _mainViewModel.ScenarioStepLogViewModel.SelectedItem != null)
        {
            _mainViewModel.ScenarioStepLogViewModel.SelectedItem.IsPassing = false;
        }
    }

    [When(@"a Standard User clicks Save")]
    public void WhenAStandardUserClicksSave()
    {
        _mockDbConnection.SetupDapperAsync(c => c.ExecuteScalarAsync<int>(
            It.IsAny<string>(), null, null, null, null)).ReturnsAsync(1);
        
        _mainViewModel?.ScenarioStepLogViewModel.SaveCmd.Execute(null);
    }

    [Then(@"the ScenarioStepLog IsPassing is false")]
    public void ThenTheScenarioStepLogIsPassingIsFalse()
    {
        Assert.IsFalse(_mainViewModel?.ScenarioStepLogViewModel.SelectedItem?.IsPassing,
            MethodBase.GetCurrentMethod()?.Name);
    }

    [Then(@"the saved ScenarioStepLog StartOfStep is (.*)")]
    public void ThenTheSavedScenarioStepLogStartOfStepIs(string value)
    {
        var _ = DateTime.TryParse(value, out var dt);
        Assert.AreEqual(dt, _mainViewModel?.ScenarioStepLogViewModel.SelectedItem?.StartOfStep,
            MethodBase.GetCurrentMethod()?.Name);
    }

    [Then(@"the saved ScenarioStepLog EndOfStep is (.*)")]
    public void ThenTheSavedScenarioStepLogEndOfStepIs(string value)
    {
        var _ = DateTime.TryParse(value, out var dt);
        Assert.AreEqual(dt, _mainViewModel?.ScenarioStepLogViewModel.SelectedItem?.EndOfStep,
            MethodBase.GetCurrentMethod()?.Name);
    }

    [Given(@"there are (.*) ScenarioStepLog that are not new")]
    public void GivenThereAreScenarioStepLogThatAreNotNew(int value)
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

    [When(@"a Standard User opens the main view")]
    public async Task WhenAStandardUserOpensTheMainView()
    {
        await _mainViewModel?.LoadAsync()!;
    }

    [Then(@"(.*) ScenarioStepLogs are listed")]
    public void ThenScenarioStepLogsAreListed(int value)
    {
        Assert.AreEqual(value, _mainViewModel?.ScenarioStepLogsViewModel.Index.Count,
            MethodBase.GetCurrentMethod()?.Name);
    }

    [Given(@"an existing ScenarioStepLog")]
    public void GivenAnExistingScenarioStepLog()
    {
        var model = new ScenarioStepLog { Id = 1 };
        if (_mainViewModel != null)
        {
            _mainViewModel.ScenarioStepLogViewModel.SelectedItem = model;
        }
    }

    [Given(@"the ScenarioStepLog Id is (.*)")]
    public void GivenTheScenarioStepLogIdIs(int value)
    {
        if (_mainViewModel != null && _mainViewModel.ScenarioStepLogViewModel.SelectedItem != null)
        {
            _mainViewModel.ScenarioStepLogViewModel.SelectedItem.Id = value;
        }
    }

    [Given(@"the ScenarioStepLog IsPassing is toggled to true")]
    public void GivenTheScenarioStepLogIsPassingIsToggledToTrue()
    {
        if (_mainViewModel != null && _mainViewModel.ScenarioStepLogViewModel.SelectedItem != null)
        {
            _mainViewModel.ScenarioStepLogViewModel.SelectedItem.IsPassing = true;
        }
    }

    [Then(@"the ScenarioStepLog is available")]
    public void ThenTheScenarioStepLogIsAvailable()
    {
        Assert.IsNotNull(_mainViewModel?.ScenarioStepLogViewModel.SelectedItem,
            MethodBase.GetCurrentMethod()?.Name);
    }

    [Then(@"the ScenarioStepLog Id is (.*)")]
    public void ThenTheScenarioStepLogIdIs(int value)
    {
        Assert.AreEqual(value, _mainViewModel?.ScenarioStepLogViewModel.SelectedItem?.Id,
            MethodBase.GetCurrentMethod()?.Name);
    }

    [When(@"a Standard User clicks Delete")]
    public void WhenAStandardUserClicksDelete()
    {
        _mockDbConnection.SetupDapperAsync(c => c.QueryAsync<ScenarioStepLog>(
            It.IsAny<string>(), null, null, null, null));

        _mainViewModel?.ScenarioStepLogViewModel.DeleteCmd.Execute(null);
    }

    [Then(@"the ScenarioStepLog is gone")]
    public void ThenTheScenarioStepLogIsGone()
    {
        Assert.IsNull(_mainViewModel?.ScenarioStepLogViewModel.SelectedItem,
            MethodBase.GetCurrentMethod()?.Name);
    }




    private void GivenAScenarioStepLogDatabaseIsLoaded()
    {
        _mockDbConnection = new Mock<DbConnection>();
        _mockDbConnection.SetupDapperAsync(
            c => c.ExecuteAsync(It.IsAny<string>(), It.IsAny<ScenarioStepLog>(), null, null, null))
            .ReturnsAsync(1);
        _mockDbConnection.SetupDapperAsync(
            c => c.ExecuteAsync(It.IsAny<string>(), It.IsAny<ScenarioStepLog>(), null, null, null))
            .ReturnsAsync(1);
    }

    private void GivenTheMainViewIsOpen()
    {
        if (_mockDbConnection != null)
        {
            _mainViewModel = new MainViewModel(
                BusyStateManager.Instance,
                new DataService(_mockDbConnection.Object));
        }
    }

}

