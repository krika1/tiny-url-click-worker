using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using TinyUrl.ClickEvent.Listener.Interfaces;

namespace TinyUrl.ClickEvent.Listener.Functions
{
    public class ClieckEventListenerFunction
    {
        private readonly ILogger _logger;
        private readonly IUrlMappingService _urlMappingService;

        public ClieckEventListenerFunction(ILoggerFactory loggerFactory, IUrlMappingService urlMappingService)
        {
            _logger = loggerFactory.CreateLogger<ClieckEventListenerFunction>();
            _urlMappingService = urlMappingService;
        }

        [Function("ClieckEventListenerFunction")]
        public async Task Run([RabbitMQTrigger("tinyUrl", ConnectionStringSetting = "RabbitMQConnection")] string item)
        {
            _logger.LogInformation($"C# Queue trigger function processed: {item}");

            await _urlMappingService.UpdateUrlClicksAsync(item);
        }
    }
}
