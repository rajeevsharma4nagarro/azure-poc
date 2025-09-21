using Azure.Messaging.ServiceBus;
using Azure.Storage.Queues;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SCD.EmailProcessorFunction.Models;
using SCD.EmailProcessorFunction.Services;
using System.Text.Json;

namespace SCD.EmailProcessorFunction
{
    public class ReadQueueMessage
    {
        private readonly ILogger<ReadQueueMessage> _logger;
        private readonly EmailService emailService;
        private readonly QueueClient _queueClient;

        public ReadQueueMessage(ILogger<ReadQueueMessage> logger, ServiceBusClient serviceBusClient, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            emailService = new EmailService(httpClientFactory);

            var AzureWebStorageConnectionName = Environment.GetEnvironmentVariable("AzureWebStorageConnectionName");
            var AzureWebStorageQueueName = Environment.GetEnvironmentVariable("AzureWebStorageQueueName");
            _queueClient = new QueueClient(AzureWebStorageConnectionName, AzureWebStorageQueueName);
            _queueClient.CreateIfNotExists();
        }

        [Function(nameof(ReadQueueMessage))]
        public async Task Run(
            [ServiceBusTrigger("scd-emailshoppingcart", Connection = "ServiceBusConnectionName")]
            ServiceBusReceivedMessage message,
            ServiceBusMessageActions messageActions)
        {
            _logger.LogInformation("Message ID: {id}", message.MessageId);
            _logger.LogInformation("Message Body: {body}", message.Body);
            _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

            OrderHeader order = JsonSerializer.Deserialize<OrderHeader>(message.Body);
            ResponseDto result = await emailService.SendEmailAsync(order);
            //ResponseDto result = new() { IsSuccess = true  }; 
            if (result.IsSuccess)
            {
                if (order.Status == "Approved")
                {
                    await _queueClient.SendMessageAsync(message.Body);
                }

                // Complete the message
                await messageActions.CompleteMessageAsync(message);
            }
        }
    }
}
