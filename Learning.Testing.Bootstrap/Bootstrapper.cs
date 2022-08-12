using Autofac;
using Learning.Testing.API;
using Learning.Testing.AppLib;
using Learning.Testing.Data;
using Learning.Testing.Utils;
using Microsoft.Data.Sqlite;
using System;
using System.Data;

namespace Learning.Testing.Bootstrap
{
    public class Bootstrapper : IDisposable
    {
        private ILifetimeScope _scope;

        public Bootstrapper()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<FeedbackService>().As<IFeedbackService>();
            builder.RegisterType<SqliteConnection>().As<IDbConnection>()
                .WithParameter("connectionString", "Data Source=..\\..\\..\\..\\Database.sqlite3;");
            builder.RegisterType<DataService>().As<IDataService>();
            builder.RegisterType<App>().As<IApp>();
            _scope = builder.Build().BeginLifetimeScope();
        }

        public IApp App => _scope.Resolve<IApp>();

        public void Dispose()
        {
            _scope.Dispose();
        }
    }
}
