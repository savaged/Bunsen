using Learning.Testing.Bootstrap;

using (var bootstrapper = new Bootstrapper())
{
    await bootstrapper.App.RunAsync();
}
