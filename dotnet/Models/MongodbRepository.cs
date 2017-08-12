using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using dotnet.Helper;

namespace dotnet.Models
{
    public class MongodbRepository
    {
        public IMongoClient _client;
        public IMongoDatabase _database;

        public MongodbRepository() {
            ConfigurationHelper configurationHelper = new ConfigurationHelper();
            Console.WriteLine(configurationHelper.GetValue("connectionStrings:mongodb"));
            Console.WriteLine(configurationHelper.GetValue("connectionStrings:mongodbName"));
            try {

                _client = new MongoClient(configurationHelper.GetValue("connectionStrings:mongodb"));
                _database = _client.GetDatabase(configurationHelper.GetValue("connectionStrings:mongodbName"));
            }
            catch (Exception e) {

            }
        }
    }
}
