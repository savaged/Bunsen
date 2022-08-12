using Learning.Testing.API;
using Learning.Testing.Models;
using Learning.Testing.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learning.Testing.AppLib
{
    public class App : IApp
    {
        private readonly IFeedbackService _feedbackService;
        private readonly IDataService _dataService;

        public App(
            IFeedbackService feedbackService,
            IDataService dataService)
        {
            _feedbackService = feedbackService ??
                throw new ArgumentNullException(nameof(feedbackService));

            _dataService = dataService ??
                throw new ArgumentNullException(nameof(dataService));
        }

        public async Task RunAsync()
        {
            IEnumerable<ScenarioStepLog> index = new List<ScenarioStepLog>();
            try
            {
                var model = await _dataService.StoreAsync(new ScenarioStepLog
                {
                    Name = "Step 1",
                    IsPassing = false
                });
                _feedbackService.Feedback("Stored a step. Now listing...");
                index = await _dataService.IndexAsync<ScenarioStepLog>();
                _feedbackService.Feedback(ToJson(index));
                model = index.FirstOrDefault();
                model.IsPassing = true;
                await _dataService.UpdateAsync(model);
                _feedbackService.Feedback("Updated the step. Now showing...");
                model = await _dataService.ShowAsync<ScenarioStepLog>(model.Id);
                await _dataService.DeleteAsync<ScenarioStepLog>(index.FirstOrDefault().Id);
                _feedbackService.Feedback("Deleted the step.");
            }
            catch (Exception e)
            {
                _feedbackService.Feedback(e.Message);
            }
        }

        private string ToJson(IEnumerable<ScenarioStepLog> index)
        {
            var sb = new StringBuilder("[");
            foreach (var scenarioStepLog in index)
            {
                sb.Append(scenarioStepLog.ToDictionary().ToJson());
                sb.Append(",");
            }
            sb.Append("]");
            return sb.ToString();
        }
    }
}
