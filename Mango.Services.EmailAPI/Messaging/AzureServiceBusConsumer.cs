using Azure.Messaging.ServiceBus;
using Mango.Services.EmailApi.DTO;
using Mango.Services.EmailApi.Messages;
using Mango.Services.EmailApi.Services;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace Mango.Services.EmailApi.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string serviceBusConnectionString;
        private readonly string emailCartQueue;
		private readonly string registerUserQueue;
		private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;
		private readonly string orderCreated_Topic;
		private readonly string orderCreated_Email_Subscription;

        private ServiceBusProcessor _emailOrderPlacedProcessor;
        private ServiceBusProcessor _emailCartProcessor;
		private ServiceBusProcessor _registerUserProcessor;

		public AzureServiceBusConsumer(IConfiguration configuration, EmailService emailService)
		{
			_configuration = configuration;
			_emailService = emailService;

			serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
			
            emailCartQueue = _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue");
			registerUserQueue = _configuration.GetValue<string>("TopicAndQueueNames:RegisterUserQueue");
            orderCreated_Topic = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreatedTopic");
            orderCreated_Email_Subscription = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreated_Email_Subscription");

			var client = new ServiceBusClient(serviceBusConnectionString);
			_emailCartProcessor = client.CreateProcessor(emailCartQueue);
			_registerUserProcessor = client.CreateProcessor(registerUserQueue);
            _emailOrderPlacedProcessor = client.CreateProcessor(orderCreated_Topic, orderCreated_Email_Subscription);
			
		}

		public async Task Start()
        {
            _emailCartProcessor.ProcessMessageAsync += OnEmailCartRequestReceived;
            _emailCartProcessor.ProcessErrorAsync += ErrorHandler;
            await _emailCartProcessor.StartProcessingAsync();

			_registerUserProcessor.ProcessMessageAsync += OnUserRegisterRequestReceived;
			_registerUserProcessor.ProcessErrorAsync += ErrorHandler;
			await _registerUserProcessor.StartProcessingAsync();

            _emailOrderPlacedProcessor.ProcessMessageAsync += OnOrderPlacedRequestReceived;
            _emailOrderPlacedProcessor.ProcessErrorAsync += ErrorHandler;
            await _emailOrderPlacedProcessor.StartProcessingAsync();
        }

		public async Task Stop()
        {
            _emailCartProcessor.StopProcessingAsync();
            _emailCartProcessor.DisposeAsync();

			_registerUserProcessor.StopProcessingAsync();
			_registerUserProcessor.DisposeAsync();

            _emailOrderPlacedProcessor.StopProcessingAsync();
            _emailOrderPlacedProcessor.DisposeAsync();
        }

        private async Task OnEmailCartRequestReceived(ProcessMessageEventArgs args)
        {
            //Aqui é onde receberá a mensagem

            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            CartDTO objMessage = JsonConvert.DeserializeObject<CartDTO>(body);
            try
            {
                await _emailService.EmailCartAndLog(objMessage);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }

		private async Task OnUserRegisterRequestReceived(ProcessMessageEventArgs args)
		{
			//Aqui é onde receberá a mensagem

			var message = args.Message;
			var body = Encoding.UTF8.GetString(message.Body);

			string email = JsonConvert.DeserializeObject<string>(body);
			try
			{
				await _emailService.RegisterUserEmailAndLog(email);
				await args.CompleteMessageAsync(args.Message);
			}
			catch (Exception)
			{
				throw;
			}
		}

        private async Task OnOrderPlacedRequestReceived(ProcessMessageEventArgs args)
        {
            //Aqui é onde receberá a mensagem

            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            RewardsMessage messageDTO = JsonConvert.DeserializeObject<RewardsMessage>(body);
            try
            {
                await _emailService.LogOrderPlaced(messageDTO);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            await Task.CompletedTask;
        }


    }
}
