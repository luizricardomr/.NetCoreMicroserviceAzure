using Azure.Messaging.ServiceBus;
using Mango.Services.Reward.Api.Messages;
using Mango.Services.RewardsApi.Services;
using Newtonsoft.Json;
using System.Text;

namespace Mango.Services.RewardsApi.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string serviceBusConnectionString;
        private readonly string orderCreatedTopic;
		private readonly string orderCreatedRewardsSubscription;
		private readonly IConfiguration _configuration;
        private readonly RewardsService _rewardsService;

        private ServiceBusProcessor _rewardProcessor;
		public AzureServiceBusConsumer(IConfiguration configuration, RewardsService rewardsService)
		{
			_configuration = configuration;
            _rewardsService = rewardsService;

			serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");

            orderCreatedTopic = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreatedTopic");
            orderCreatedRewardsSubscription = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreated_Rewards_Subscription");


			var client = new ServiceBusClient(serviceBusConnectionString);
            _rewardProcessor = client.CreateProcessor(orderCreatedTopic, orderCreatedRewardsSubscription);
			
		}

		public async Task Start()
        {
            _rewardProcessor.ProcessMessageAsync += OnNewOrderRewardsRequestReceived;
            _rewardProcessor.ProcessErrorAsync += ErrorHandler;
            await _rewardProcessor.StartProcessingAsync();
		}

		public async Task Stop()
        {
            _rewardProcessor.StopProcessingAsync();
            _rewardProcessor.DisposeAsync();
		}

        private async Task OnNewOrderRewardsRequestReceived(ProcessMessageEventArgs args)
        {
            //Aqui é onde receberá a mensagem

            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            RewardsMessage objMessage = JsonConvert.DeserializeObject<RewardsMessage>(body);
            try
            {
                await _rewardsService.UpdateReward(objMessage);
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
