using AnimalSearchService.DataAccess;
using Autofac;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalSearchService.Configuration.IoC
{
    public class ElasticSearchModule : Module
    {
        public ConfigurationOptions ConfigurationOptions { get; set; }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance<ElasticClient>(CreateElasticClient(ConfigurationOptions.ELASTICSEARCH_SERVER));
            builder.RegisterType<AnimalRepository>();
        }
        private static ElasticClient CreateElasticClient(string cnString)
        {
            var settings = new ConnectionSettings(new Uri($"http://{cnString}"))
                .DefaultIndex("puppys_animal");
            var client = new ElasticClient(settings);
            return client;
        }
    }
}
