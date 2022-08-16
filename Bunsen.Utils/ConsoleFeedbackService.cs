using Bunsen.API;
using System;

namespace Bunsen.Utils
{
    public class ConsoleFeedbackService : IFeedbackService
    {
        public void Feedback(string output)
        {
            Console.WriteLine(output);
        }
    }
}
