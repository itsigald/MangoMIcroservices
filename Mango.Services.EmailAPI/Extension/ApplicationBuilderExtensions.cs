using Mango.Services.EmailAPI.Messaging;
using System.Reflection.Metadata;

namespace Mango.Services.EmailAPI.Extension
{
    public static class ApplicationBuilderExtensions
    {
        private static IAzureServiceBusConsumer? azureServiceBusConsumer { get; set; }
        public static IApplicationBuilder UseAzureServiceBusConsumer(this IApplicationBuilder appBuilder)
        {
            azureServiceBusConsumer = appBuilder.ApplicationServices.GetService<IAzureServiceBusConsumer>();
            var hostApplicationLife = appBuilder.ApplicationServices.GetService<IHostApplicationLifetime>();

            if(hostApplicationLife != null)
            {
                hostApplicationLife.ApplicationStarted.Register(OnStart);
                hostApplicationLife.ApplicationStopping.Register(OnStop);
            }

            return appBuilder;
        }

        private static void OnStart()
        {
            azureServiceBusConsumer?.Start();
        }

        private static void OnStop()
        {
            azureServiceBusConsumer?.Stop();
        }
    }
}
