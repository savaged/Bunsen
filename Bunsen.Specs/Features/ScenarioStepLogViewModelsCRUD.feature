Feature: ScenarioStepLog ViewModels CRUD

    @scenarioStepLogViewModelsCRUD
    Scenario: A Standard User adds a ScenarioStepLog 
        When a Standard User clicks Add 
        Then a new ScenarioStepLog is available 

    @scenarioStepLogViewModelsCRUD
    Scenario: A Standard User saves a new ScenarioStepLog 
        Given a new ScenarioStepLog 
        Given the ScenarioStepLog Id is 0 
        Given the ScenarioStepLog Name is 'TestStep'
        Given the ScenarioStepLog IsPassing is false 
        Given the ScenarioStepLog StartOfStep is '2022-08-14 08:25:30'
        Given the ScenarioStepLog EndOfStep is '2022-08-14 08:26:18'
        When a Standard User clicks Save 
        Then a saved ScenarioStepLog is available 
        Then the saved ScenarioStepLog is not new 
        Then the ScenarioStepLog Name is 'TestStep'
        Then the ScenarioStepLog IsPassing is false 
        Then the saved ScenarioStepLog StartOfStep is '2022-08-14 08:25:30'
        Then the saved ScenarioStepLog EndOfStep is '2022-08-14 08:26:18'

    @scenarioStepLogViewModelsCRUD
    Scenario: A Standard User lists all ScenarioStepLogs 
        Given there are 2 ScenarioStepLog that are not new 
        When a Standard User opens the main view 
        Then 2 ScenarioStepLogs are listed 

    @scenarioStepLogViewModelsCRUD
    Scenario: A Standard User saves an existing ScenarioStepLog 
        Given an existing ScenarioStepLog
        Given the ScenarioStepLog Id is 1
        Given the ScenarioStepLog Name is 'UpdatedTestStep'
        Given the ScenarioStepLog IsPassing is toggled to true 
        Given the ScenarioStepLog StartOfStep is '2022-08-14 08:25:30'
        Given the ScenarioStepLog EndOfStep is '2022-08-14 08:26:18'
        When a Standard User clicks Save 
        Then the ScenarioStepLog is available 
        Then the ScenarioStepLog Id is 1
        Then the ScenarioStepLog Name is 'UpdatedTestStep'
        Then the ScenarioStepLog IsPassing is true 
        Then the saved ScenarioStepLog StartOfStep is '2022-08-14 08:25:30'
        Then the saved ScenarioStepLog EndOfStep is '2022-08-14 08:26:18'

    @scenarioStepLogViewModelsCRUD
    Scenario: A Standard User deletes an existing ScenarioStepLog 
        Given an existing ScenarioStepLog 
        Given the ScenarioStepLog Id is 1
        Given the ScenarioStepLog Name is 'DeleteMe'
        Given the ScenarioStepLog IsPassing is false 
        Given the ScenarioStepLog StartOfStep is '2022-08-14 08:25:30'
        Given the ScenarioStepLog EndOfStep is '2022-08-14 08:26:18'
        When a Standard User clicks Delete 
        Then the ScenarioStepLog is gone 

