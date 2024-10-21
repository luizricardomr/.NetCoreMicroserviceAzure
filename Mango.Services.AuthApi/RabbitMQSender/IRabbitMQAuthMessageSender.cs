namespace Mango.Services.AuthApi.RabbitMQSender
{
    public interface IRabbitMQAuthMessageSender
    {
        void SendMessage(string message, string queueName);
    }
}
