using Bunsen.API;
using Bunsen.Bootstrap;
using Bunsen.CLI.IoC;

using var bootstrapper = new Bootstrapper(new ConsoleModule());
await bootstrapper.Resolve<IApp>().RunAsync();
