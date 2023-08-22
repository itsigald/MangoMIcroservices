namespace Mango.Services.ShoppingCartAPI.Dtos
{
    public class AppSettings
    {
        public string Secret { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public Services Services { get; set; } = new();
        public TopicQueueName TopicQueueName { get; set; } = new();
    }
}
