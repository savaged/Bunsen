using Autofac;
using Bunsen.API;
using Bunsen.AppLib;
using Bunsen.Utils;

namespace Bunsen.CLI.IoC
{
    public class ConsoleModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ConsoleFeedbackService>().As<IFeedbackService>();
            builder.RegisterType<App>().As<IApp>();
        }
    }
}
