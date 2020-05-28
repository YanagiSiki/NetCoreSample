using Microsoft.Extensions.Configuration;
using System.IO;

namespace NetCoreSample.Helper
{
    public class ConfigurationHelper
    {
        IConfiguration _configuration;
        public ConfigurationHelper()
        {
            GetJson("appsettings.json");
        }

        public ConfigurationHelper(string jsonName)
        {
            GetJson(jsonName);

        }

        private void GetJson(string jsonName)
        {
            try
            {
                _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"{jsonName}.json")
                .Build();
            }
            catch
            {
                _configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();
            }
        }

        public string GetValue(string key)
        {
            return _configuration.GetSection(key).Value ?? string.Empty;
        }

        public T GetValue<T>(string key)
        {
            return _configuration.GetValue<T>(key);
        }
    }
}
