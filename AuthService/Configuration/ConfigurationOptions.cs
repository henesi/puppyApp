using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Configuration
{
    public class ConfigurationOptions
    {
        public string DEFAULT_CONNECTION { get;set; }
        public string SECRET { get; set; }
        public string ALLOWED_AUTH_ORIGINS { get; set; }
    }
}
