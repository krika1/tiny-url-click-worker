using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using TinyUrl.ClickEvent.Listener.Interfaces;

namespace TinyUrl.ClickEvent.Listener.Functions
{
    public class ApiLimitReseterFunction
    {
        private readonly IDatabaseUpdaterService _databaseUpdaterService;
        private readonly ILogger<ApiLimitReseterFunction> _logger;

        public ApiLimitReseterFunction(ILogger<ApiLimitReseterFunction> logger, IDatabaseUpdaterService databaseUpdaterService)
        {
            _databaseUpdaterService = databaseUpdaterService;
            _logger = logger;
        }   

        [Function("ApiLimitReseterFunction")]
        public async Task Run([TimerTrigger("0 0 * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation($"Resetting daily call limits at: {DateTime.Now}");

            await _databaseUpdaterService.ResetDailyLimitAsync().ConfigureAwait(false);
        }
    }
}
