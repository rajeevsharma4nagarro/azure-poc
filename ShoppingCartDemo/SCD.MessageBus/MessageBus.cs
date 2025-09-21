using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SCD.MessageBus
{
    public class MessageBus : iMessageBus
    {
        private readonly EmailQueue _emailQueue;

        public MessageBus(IOptions<EmailQueue> emailQueue)
        {
            _emailQueue = emailQueue.Value;
        }
        public async Task PublishMessage(object message)
        {
            await using var client = new ServiceBusClient(_emailQueue.ConnectionString);
            ServiceBusSender sender = client.CreateSender(_emailQueue.Name);
            var jsonMessage = JsonConvert.SerializeObject(message);
            ServiceBusMessage finalMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage))
            {
                CorrelationId = Guid.NewGuid().ToString()
            };
            await sender.SendMessageAsync(finalMessage);
            await client.DisposeAsync();
        }
    }
}
