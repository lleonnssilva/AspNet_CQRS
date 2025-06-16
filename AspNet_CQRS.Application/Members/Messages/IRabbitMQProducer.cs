namespace AspNet_CQRS.Application.Members.Messages
{
    public interface IRabbitMQProducer
    {
        void SendMemberMessage<T>(T message);
    }
}
