using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Queuing_System
{
    class mongodb_connection
    {
        public MongoClient client;
        public IMongoDatabase database;
        private readonly string _collectionName = "QueuingSystemDatabase";
        private readonly string _databaseconnection = "mongodb://192.168.4.103:27017";

        public async Task Run()
        {
            var collection = database.GetCollection<BsonDocument>(_collectionName);
            var filter = Builders<BsonDocument>.Filter.Eq("OwnerOfSystemAndDatabase", "Floi");
            var documents = await collection.Find(filter).ToListAsync();
            foreach (var document in documents)
            {
                Console.WriteLine(document.ToString());
            }
        }

        public async Task<bool> DateCheck(String dt, string col)
        {
            var collection = database.GetCollection<BsonDocument>(col);
            var filter = Builders<BsonDocument>.Filter.Eq("DateString", dt);
            var documents = await collection.Find(filter).ToListAsync();
            return documents.Any();
        }

        public async Task SaveData()
        {
            DateTime dt = DateTime.Now;
            bool x = await DateCheck(dt.ToShortDateString(), _collectionName);
            if (x == false)
            {
                var collection = database.GetCollection<BsonDocument>(_collectionName);
                var mld = new BsonDocument
                {
                  { "Province", "Surigao Del Sur" },
                  { "City", "City of Bislig" },
                  { "OwnerOfSystemAndDatabase", "Floi" },
                  {"Date", dt},
                  {"DateString", dt.ToShortDateString()},

                  {"EmesilBirth", 0},
                  {"JomaryDeath", 0},
                  {"HelenMarriage", 0},
                  {"NikkiCTC", 0},
                  {"DonCourt", 0},
                  {"NikkiLegitimationEdorsementsLegitimation", 0},
                  {"FrechieCorrection", 0},
                };


                Console.WriteLine("1");
                await collection.InsertOneAsync(mld);
            }
            Console.WriteLine("2");

        }

        public void ConnectToMongoDB()
        {
            try
            {
                var settings = MongoClientSettings.FromConnectionString(_databaseconnection);
                client = new MongoClient(settings);
                database = client.GetDatabase("myDatabase");
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
            dt = "5/24/2024";
            var collection = database.GetCollection<BsonDocument>(_collectionName);
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
