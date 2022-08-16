using Autofac;
using Bunsen.API;
using Bunsen.Data;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace Bunsen.Bootstrap
{
    public class Bootstrapper : IDisposable
    {
        private readonly ILifetimeScope _scope;

        public Bootstrapper() : this(new List<Module>()) { }

        public Bootstrapper(Module module)
            : this(new List<Module> { module }) { }

        public Bootstrapper(IEnumerable<Module> modules)
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<SqliteConnection>().As<IDbConnection>()
                .WithParameter("connectionString", $"Data Source=..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}Database.sqlite3;");
            builder.RegisterType<DataService>().As<IDataService>();

            foreach (var module in modules)
            {
                builder.RegisterModule(module);
            }
            _scope = builder.Build().BeginLifetimeScope();
        }

        public T Resolve<T>() where T : notnull
        {
            return _scope.Resolve<T>();
        }

        public void Dispose()
        {
            _scope.Dispose();
        }
    }
}
