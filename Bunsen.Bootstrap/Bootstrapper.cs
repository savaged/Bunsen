using Autofac;
using Bunsen.API;
using Bunsen.AppLib;
using Bunsen.Data;
using Bunsen.Utils;
using Microsoft.Data.Sqlite;
using System;
using System.Data;

namespace Bunsen.Bootstrap
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
