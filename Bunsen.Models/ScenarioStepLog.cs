namespace Bunsen.Models
{
    public class ScenarioStepLog : ModelBase
    {
        private string _name = string.Empty;

        private bool _isPassing;

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public bool IsPassing
        {
            get => _isPassing;
            set => _isPassing = value;
        }
    }
}
