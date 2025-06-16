namespace AspNet_CQRS.Infrastructure.Messages
{
    public interface IRabbitMQProducer
    {
        void SendMemberMessage<T>(T message);
    }
}
