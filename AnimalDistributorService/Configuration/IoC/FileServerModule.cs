using AnimalDistributorService.Api.Services;
using Autofac;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDistributorService.Configuration.IoC
{
    public class FileServerModule : Module
    {
        public ConfigurationOptions ConfigurationOptions { get; set; }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register((c, p) =>
                 new ServerStorageService(ConfigurationOptions.FILE_SERVER, ConfigurationOptions.APIGATEWAY_SERVER, ConfigurationOptions.FILE_SERVER_ACCESS_KEY, ConfigurationOptions.FILE_SERVER_SECRET_KEY))
               .As<IStorageService>();
        }
    }
}
