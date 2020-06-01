using Autofac;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDistributorService.Configuration.IoC
{
    public class ElasticSearchModule : Module
    {
        public ConfigurationOptions ConfigurationOptions { get; set; }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance<ElasticClient>(CreateElasticClient(ConfigurationOptions.ELASTICSEARCH_SERVER));
        }
        private static ElasticClient CreateElasticClient(string cnString)
        {
            var settings = new ConnectionSettings(new Uri(cnString))
                .DefaultIndex("puppy_animal");
            var client = new ElasticClient(settings);
            return client;
        }
    }
}
