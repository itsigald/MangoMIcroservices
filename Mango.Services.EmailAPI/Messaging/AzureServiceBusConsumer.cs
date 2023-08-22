using Azure.Messaging.ServiceBus;
using Mango.Services.EmailAPI.Data;
using Mango.Services.EmailAPI.Dtos;
using Mango.Services.EmailAPI.Models;
using Mango.Services.EmailAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;

namespace Mango.Services.EmailAPI.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly AppSettings? _appSettings;
        private readonly EmailService _emailService;
        private readonly ServiceBusProcessor _serviceBusProcessor;

        public AzureServiceBusConsumer(IOptions<AppSettings> optionAppSettings, EmailService emailService)
        {
            _appSettings = optionAppSettings.Value;
            _emailService = emailService;

            var client = new ServiceBusClient(_appSettings.MessageBus!.MessageBusConnectionString!);
            _serviceBusProcessor = client.CreateProcessor(_appSettings.MessageBus!.TopicQueueName!.EmailShoppingCartQueue);
        }

        public async Task Start()
        {
            _serviceBusProcessor.ProcessMessageAsync += _serviceBusProcessor_ProcessMessageAsync;
            _serviceBusProcessor.ProcessErrorAsync += _serviceBusProcessor_ProcessErrorAsync;
            
            await _serviceBusProcessor.StartProcessingAsync();
        }

        public async Task Stop()
        {
            await _serviceBusProcessor.StopProcessingAsync();
            await _serviceBusProcessor.DisposeAsync();
        }

        private async Task _serviceBusProcessor_ProcessMessageAsync(ProcessMessageEventArgs arg)
        {
            var body = Encoding.UTF8.GetString(arg.Message.Body);
            CartDto? cartDto = JsonSerializer.Deserialize<CartDto?>(body);

            try
            {
                await _emailService.EmailCartAndLog(cartDto);
                await arg.CompleteMessageAsync(arg.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private Task _serviceBusProcessor_ProcessErrorAsync(ProcessErrorEventArgs arg)
        {
            Console.WriteLine(arg.Exception.Message);
            return Task.CompletedTask;
        }
    }
}
