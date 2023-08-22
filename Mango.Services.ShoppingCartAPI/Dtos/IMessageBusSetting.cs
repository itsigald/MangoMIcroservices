namespace Mango.Services.ShoppingCartAPI.Dtos
{
    public interface IMessageBusSetting
    {
        TopicQueueName TopicQueueName { get; }
    }
}
