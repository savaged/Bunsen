using Learning.Testing.API;
using System;

namespace Learning.Testing.Utils
{
    public class FeedbackService : IFeedbackService
    {
        public void Feedback(string output)
        {
            Console.WriteLine(output);
        }
    }
}
