using Azure.Messaging.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Mango.MessageBus
{
    public class MessageBus : IMessageBus
    {
        private string messageBusCn = "Endpoint=sb://localhost/;SharedAccessKeyName=all;SharedAccessKey=CLwo3FQ3S39Z4pFOQDefaiUd1dSsli4XOAj3Y9Uh1E=;EnableAmqpLinkRedirect=false";

        public async Task PublishMessage(string queueTopicName, object message)
        {
            await using var client = new ServiceBusClient(messageBusCn);
            ServiceBusSender sender = client.CreateSender(queueTopicName);
            var jsonMessage = JsonSerializer.Serialize(message);

            ServiceBusMessage sbMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage))
            {
                CorrelationId = Guid.NewGuid().ToString()
            };

            await sender.SendMessageAsync(sbMessage);
            await client.DisposeAsync();
        }
    }
}
