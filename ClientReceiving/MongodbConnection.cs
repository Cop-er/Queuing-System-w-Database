using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;

namespace ClientReceiving
{
    class MongodbConnection
    {
        public static string CollectionName { get; } = "forEntryControl";
        public static string DatabaseConnection { get; } = "mongodb://192.168.4.56:27017";

        public MongoClient Client { get; private set; }
        public IMongoDatabase Database { get; private set; }

        public MongoClient GetClient()
        {
            return Client;
        }

        public IMongoDatabase GetDatabase()
        {
            return Database;
        }

        public async Task Run()
        {
            var collection = Database.GetCollection<BsonDocument>(CollectionName);
            var filter = Builders<BsonDocument>.Filter.Eq("OwnerOfSystemAndDatabase", "Floi");
            var documents = await collection.Find(filter).ToListAsync();
            foreach (var document in documents)
            {
                Console.WriteLine(document.ToString());
            }
        }

        public async Task<bool> DateCheck(string dt, string col)
        {
            var collection = Database.GetCollection<BsonDocument>(col);
            var filter = Builders<BsonDocument>.Filter.Eq("DateString", dt);
            var documents = await collection.Find(filter).ToListAsync();
            return documents.Any();
        }

        public async Task SaveData()
        {
            DateTime dt = DateTime.Now;
            bool x = await DateCheck(dt.ToShortDateString(), CollectionName);
            if (!x)
            {
                var collection = Database.GetCollection<BsonDocument>(CollectionName);
                var mld = new BsonDocument
                {
                    { "Province", "Surigao Del Sur" },
                    { "City", "City of Bislig" },
                    { "OwnerOfSystemAndDatabase", "Floi" },
                    { "Date", dt },
                    { "DateString", dt.ToShortDateString() },
                    { "EmesilBirth", 0 },
                    { "JomaryDeath", 0 },
                    { "HelenMarriage", 0 },
                    { "NikkiCTC", 0 },
                    { "DonCourt", 0 },
                    { "NikkiLegitimationEdorsementsLegitimation", 0 },
                    { "FrechieCorrection", 0 }
                };
                await collection.InsertOneAsync(mld);
            }
        }

        public void ConnectToMongoDB()
        {
            try
            {
                var settings = MongoClientSettings.FromConnectionString(DatabaseConnection);
                Client = new MongoClient(settings);
                Database = Client.GetDatabase("myDatabase");
                Console.WriteLine("Connected to MongoDB successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while connecting to MongoDB: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateData(string dt)
        {
            var collection = Database.GetCollection<BsonDocument>(CollectionName);
            var filter = Builders<BsonDocument>.Filter.Eq("DateString", dt);
            var update = Builders<BsonDocument>.Update
                .Set("EmesilBirth", 5)
                .Set("JomaryDeath", 5)
                .Set("HelenMarriage", 5)
                .Set("NikkiCTC", 5)
                .Set("DonCourt", 5);

            var result = await collection.UpdateOneAsync(filter, update);
            if (result.IsAcknowledged && result.ModifiedCount > 0)
            {
                Console.WriteLine("Document updated successfully.");
            }
            else
            {
                Console.WriteLine("No documents matched the filter or no modifications were made.");
            }
        }
    }
}
