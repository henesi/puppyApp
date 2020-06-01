using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGateway.Configuration
{
    public class ConfigurationOptions
    {
        public string SECRET { get; set; }
        public string ALLOWED_AUTH_ORIGINS { get; set; }
    }
}
