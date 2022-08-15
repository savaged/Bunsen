using Bunsen.API;
using System;

namespace Bunsen.Utils
{
    public class FeedbackService : IFeedbackService
    {
        public void Feedback(string output)
        {
            Console.WriteLine(output);
        }
    }
}
