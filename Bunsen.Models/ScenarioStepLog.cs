using System;

namespace Bunsen.Models
{
    public class ScenarioStepLog : ModelBase
    {
        private string _name = string.Empty;
        private DateTime? _startOfStep;
        private DateTime? _endOfStep;
        private bool _isPassing;

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public DateTime? StartOfStep
        {
            get => _startOfStep;
            set => _startOfStep = value;
        }
        
        public DateTime? EndOfStep
        {
            get => _endOfStep;
            set => _endOfStep = value;
        }

        public bool IsPassing
        {
            get => _isPassing;
            set => _isPassing = value;
        }

        public double CalculateSecondsTakenOfStep()
        {
            return (EndOfStep - StartOfStep)?.TotalSeconds ?? 0;
        }
        
    }
}
