namespace Mango.Services.EmailAPI.Dtos
{
    public class MessageBus
    {
        public string? MessageBusConnectionString { get; set; }

        public TopicQueueName? TopicQueueName { get; set; }
    }
}
