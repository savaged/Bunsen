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
            set
            {
                _name = value;
                NotifyPropertyChanged();
            }
        }

        public DateTime? StartOfStep
        {
            get => _startOfStep;
            set
            {
                _startOfStep = value;
                NotifyPropertyChanged();
            }
        }
        
        public DateTime? EndOfStep
        {
            get => _endOfStep;
            set
            {
                _endOfStep = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsPassing
        {
            get => _isPassing;
            set
            {
                _isPassing = value;
                NotifyPropertyChanged();
            }
        }

        public double CalculateSecondsTakenOfStep()
        {
            return (EndOfStep - StartOfStep)?.TotalSeconds ?? 0;
        }
        
    }
}
