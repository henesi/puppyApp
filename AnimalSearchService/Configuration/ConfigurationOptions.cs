using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalSearchService.Configuration
{
    public class ConfigurationOptions
    {
        public string MESSAGEBROKER_PASSWORD { get; set; }
        public string MESSAGEBROKER_SERVER { get; set; }
        public string MESSAGEBROKER_USERNAME { get; set; }
        public string SECRET { get; set; }
        public string FILE_SERVER { get; set; }
        public string FILE_SERVER_ACCESS_KEY { get; set; }
        public string FILE_SERVER_SECRET_KEY { get; set; }
        public string APIGATEWAY_SERVER { get; set; }
        public string ALLOWED_AUTH_ORIGINS { get; set; }
        public string CONNECTION_STRING { get; set; }
        public string ELASTICSEARCH_SERVER { get; set; }
    }
}
