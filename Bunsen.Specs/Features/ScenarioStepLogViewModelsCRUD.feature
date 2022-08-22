Feature: ScenarioStepLog ViewModels CRUD

    Scenario: A Standard User adds a ScenarioStepLog 
        When a Standard User clicks Add 
        Then a new ScenarioStepLog is available 

    Scenario: A Standard User saves a new ScenarioStepLog 
        Given a new ScenarioStepLog 
        And the ScenarioStepLog Id is 0 
        And the ScenarioStepLog Name is <InitialName>
        And the ScenarioStepLog IsPassing is false 
        And the ScenarioStepLog StartOfStep is <StartOfStep>
        And the ScenarioStepLog EndOfStep is <EndOfStep>
        When a Standard User clicks Save 
        Then a saved ScenarioStepLog is available 
        And the saved ScenarioStepLog is not new 
        And the ScenarioStepLog Name is <InitialName>
        And the ScenarioStepLog IsPassing is false 
        And the saved ScenarioStepLog StartOfStep is <StartOfStep>
        And the saved ScenarioStepLog EndOfStep is <EndOfStep>

    Scenario: A Standard User lists all ScenarioStepLogs 
        Given there are 2 ScenarioStepLog that are not new 
        When a Standard User opens the main view 
        Then 2 ScenarioStepLogs are listed 

    Scenario: A Standard User saves an existing ScenarioStepLog 
        Given an existing ScenarioStepLog
        And the ScenarioStepLog Id is 1
        And the ScenarioStepLog Name is <UpdatedName>
        And the ScenarioStepLog IsPassing is toggled to true 
        And the ScenarioStepLog StartOfStep is <StartOfStep>
        And the ScenarioStepLog EndOfStep is <EndOfStep>
        When a Standard User clicks Save 
        Then the ScenarioStepLog is available 
        And the ScenarioStepLog Id is 1
        And the ScenarioStepLog Name is <UpdatedName>
        And the ScenarioStepLog IsPassing is true 
        And the saved ScenarioStepLog StartOfStep is <StartOfStep>
        And the saved ScenarioStepLog EndOfStep is <EndOfStep>

    Scenario: A Standard User deletes an existing ScenarioStepLog 
        Given an existing ScenarioStepLog 
        And the ScenarioStepLog Id is 1
        And the ScenarioStepLog Name is <UpdatedName>
        And the ScenarioStepLog IsPassing is false 
        And the ScenarioStepLog StartOfStep is <StartOfStep>
        And the ScenarioStepLog EndOfStep is <EndOfStep>
        When a Standard User clicks Delete 
        Then the ScenarioStepLog is gone 

    Examples:
        | InitialName | UpdatedName       | StartOfStep           | EndOfStep             |
        | 'TestStep'  | 'UpdatedTestStep' | '2022-08-14 08:25:30' | '2022-08-14 08:26:18' |

