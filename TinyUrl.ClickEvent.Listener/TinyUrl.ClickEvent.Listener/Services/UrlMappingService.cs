using MongoDB.Driver;
using TinyUrl.ClickEvent.Listener.Context;
using TinyUrl.ClickEvent.Listener.Interfaces;
using TinyUrl.ClickEvent.Listener.Models;

namespace TinyUrl.ClickEvent.Listener.Services
{
    public class UrlMappingService : IUrlMappingService
    {
        private readonly MongoDbContext _database;

        public UrlMappingService(MongoDbContext database)
        {
            _database = database;   
        }

        public async Task UpdateUrlClicksAsync(string shortUrl)
        {
            var filter = Builders<UrlMapping>.Filter.Regex(e => e.ShortUrl, new MongoDB.Bson.BsonRegularExpression(shortUrl));

            var update = Builders<UrlMapping>.Update
               .Inc(e => e.Clicks, 1);

            await _database.UrlMappings.UpdateOneAsync(filter, update);
        }
    }
}
