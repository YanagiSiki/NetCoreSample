using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreSample.Helper
{
    public class ConfigurationHelper
    {
        static IConfiguration _configuration;
        public ConfigurationHelper()
        {
            _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        }

        public string GetValue(string key)
        {
            return _configuration.GetSection(key).Value ?? string.Empty;
        }
    }
}
