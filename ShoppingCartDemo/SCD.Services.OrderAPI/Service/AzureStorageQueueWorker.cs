using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using SCD.Services.OrderAPI.Data;
using SCD.Services.OrderAPI.Models;
using SCD.Services.OrderAPI.Models.Dto;
using SCD.Services.OrderAPI.Service.IService;
using SCD.Services.OrderAPI.Utilitiy;
using System.Text.Json;

namespace SCD.Services.OrderAPI.Service
{
    public class AzureStorageQueueWorker : BackgroundService
    {
        private readonly ILogger<AzureStorageQueueWorker> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly QueueClient _queueClient;
        private readonly IConfiguration _configuration;
        public AzureStorageQueueWorker(
            ILogger<AzureStorageQueueWorker> logger,
            IServiceProvider serviceProvider,
            IConfiguration configuration
            )
        {
            _configuration = configuration;
            _logger = logger;
            _serviceProvider = serviceProvider;

            var AzureWebStorageConnectionName = _configuration.GetValue<string>("AzureStorage:StorageConnectionName");
            var AzureWebStorageQueueName = _configuration.GetValue<string>("AzureStorage:StorageQueueName");
            _queueClient = new QueueClient(AzureWebStorageConnectionName, AzureWebStorageQueueName);
            _queueClient.CreateIfNotExists();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                QueueMessage[] messages = await _queueClient.ReceiveMessagesAsync(maxMessages: 10, visibilityTimeout: TimeSpan.FromMinutes(1));
                foreach(QueueMessage message in messages)
                {
                    try
                    {
                        OrderHeader orderHeader = JsonSerializer.Deserialize<OrderHeader>(message.Body.ToString());
                        orderHeader.Status = OrderStatus.Delivered;
                        
                        using (var scope = _serviceProvider.CreateScope())
                        {
                            var _db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                            _db.Update(orderHeader);
                            _db.SaveChanges();


                            //Send mail to LogiApp
                            var _emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                            string to = orderHeader.Email;
                            string subject = "SCD - Order confirmation";
                            string mailbody = $@"Hi {orderHeader.Name},<br/><br/>
                            Your order no {orderHeader.OrderHeaderId} for the amount of Rs. {orderHeader.OrderTotal}/- has been <b>{orderHeader.Status}</b> to your specified address.
                            <br/>You can check order status on SCD portal. 
                            <br/><br/><br/>--<br/>Thanks<br/>Shopping Cart Demo (SCD)<br/>";
                            var resp = await _emailService.SendEmailAsync(to, subject, mailbody);
                        }

                        

                        await _queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error processing message {0}", message.MessageId);
                    }
                }
            }
            await Task.Delay(5000, stoppingToken);
        }
    }
}
