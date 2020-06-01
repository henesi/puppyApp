using AuthService.DataAccess.NHibernate;
using AuthService.Domain;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using NHibernate.Bytecode;
using NHibernate.Cfg;
using NHibernate.Connection;
using NHibernate.Dialect;
using NHibernate.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using library = NHibernate;

namespace PuppyApp.DataAccess.NHibernate
{
    public static class NHibernateInstaller
    {
        public static IServiceCollection AddNHibernate(this IServiceCollection services, string cnString)
        {
            //Serilog
            NHibernateLogger.SetLoggersFactory(new library.Logging.Serilog.SerilogLoggerFactory());

            var cfg = new Configuration();

            cfg.DataBaseIntegration(db =>
            {
                db.Dialect<PostgreSQL83Dialect>();
                db.Driver<NpgsqlDriver>();
                db.ConnectionProvider<DriverConnectionProvider>();
                db.BatchSize = 500;
                db.IsolationLevel = System.Data.IsolationLevel.ReadCommitted;
                db.ConnectionString = cnString;
                db.Timeout = 30;/*seconds*/
                db.LogFormattedSql = false;
                db.LogSqlInConsole = true;
                db.SchemaAction = SchemaAutoAction.Update;
            });

            cfg.Proxy(p => p.ProxyFactoryFactory<StaticProxyFactoryFactory>());

            cfg.Cache(c => c.UseQueryCache = false);

            cfg.AddAssembly(typeof(NHibernateInstaller).Assembly);

            services.AddSingleton(cfg.BuildSessionFactory());

            services.AddScoped(s => s.GetService<ISessionFactory>().OpenSession());

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
