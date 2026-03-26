namespace OrderService.Interfaces;

public interface IKafkaProducer
{
    Task PublishAsync<T>(T message);
}