using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using TinyUrl.ClickEvent.Listener.Interfaces;

namespace TinyUrl.ClickEvent.Listener.Functions
{
    public class ClieckEventListenerFunction
    {
        private readonly ILogger _logger;
        private readonly IDatabaseUpdaterService _databaseUpdaterService;

        public ClieckEventListenerFunction(ILoggerFactory loggerFactory, IDatabaseUpdaterService databaseUpdaterService)
        {
            _logger = loggerFactory.CreateLogger<ClieckEventListenerFunction>();
            _databaseUpdaterService = databaseUpdaterService;
        }

        [Function("ClieckEventListenerFunction")]
        public async Task Run([RabbitMQTrigger("tinyUrl", ConnectionStringSetting = "RabbitMQConnection")] string item)
        {
            _logger.LogInformation($"C# Queue trigger function processed: {item}");

            await _databaseUpdaterService.UpdateUrlClicksAsync(item);
        }
    }
}
