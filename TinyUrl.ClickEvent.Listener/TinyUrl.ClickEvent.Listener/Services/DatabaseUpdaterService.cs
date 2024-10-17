using MongoDB.Driver;
using TinyUrl.ClickEvent.Listener.Context;
using TinyUrl.ClickEvent.Listener.Interfaces;
using TinyUrl.ClickEvent.Listener.Models;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace TinyUrl.ClickEvent.Listener.Services
{
    public class DatabaseUpdaterService : IDatabaseUpdaterService
    {
        private readonly MongoDbContext _database;

        public DatabaseUpdaterService(MongoDbContext database)
        {
            _database = database;   
        }

        public async Task ResetDailyLimitAsync()
        {
            var update = Builders<UserLimit>.Update.Set(u => u.DailyCalls, 0);

            await _database.UsersLimit.UpdateManyAsync(Builders<UserLimit>.Filter.Empty, update);
        }

        public async Task UpdateUrlClicksAsync(string shortUrl)
        {
            var filter = Builders<UrlMapping>.Filter.Regex(e => e.ShortUrl, new MongoDB.Bson.BsonRegularExpression(shortUrl));

            var update = Builders<UrlMapping>.Update
               .Inc(e => e.Clicks, 1);

            var options = new FindOneAndUpdateOptions<UrlMapping>
            {
                ReturnDocument = ReturnDocument.After
            };

            var updatedUrlMapping = await _database.UrlMappings.FindOneAndUpdateAsync(filter, update, options);

            await UpdateUserLimitAsync(updatedUrlMapping.UserId).ConfigureAwait(false);
        }

        private async Task UpdateUserLimitAsync(int userIdToUpdate)
        {
            var filter = Builders<UserLimit>.Filter.Eq(e => e.UserId, userIdToUpdate);

            var update = Builders<UserLimit>.Update
               .Inc(e => e.DailyCalls, 1);

            await _database.UsersLimit.UpdateOneAsync(filter, update);
        }
    }
}
