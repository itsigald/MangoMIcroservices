namespace Mango.Services.ShoppingCartAPI.Dtos
{
    public class MessageBusSetting : IMessageBusSetting
    {
        private readonly TopicQueueName _topicQueueName;

        public MessageBusSetting(AppSettings? appSettings)
        {
            if(appSettings == null)
                throw new ArgumentNullException(nameof(appSettings));

            _topicQueueName = appSettings.TopicQueueName; 
        }

        public TopicQueueName TopicQueueName => _topicQueueName;
    }
}
