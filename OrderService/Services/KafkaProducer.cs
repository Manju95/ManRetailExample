using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using OrderService.Interfaces;
using OrderService.Models;

namespace OrderService.Services;

public class KafkaProducer : IKafkaProducer
{
    private readonly IProducer<Null, string> _producer;
    private readonly KafkaSettings _settings;
    private readonly ILogger<KafkaProducer> _logger;

    public KafkaProducer(IOptions<KafkaSettings> settings, ILogger<KafkaProducer> logger)
    {
        _settings = settings.Value;
        _logger = logger;

        var config = new ProducerConfig
        {
            BootstrapServers = _settings.BootstrapServers,
            Acks = Acks.All,
            EnableIdempotence = true
        };

        _producer = new ProducerBuilder<Null, string>(config).Build();
    }

    public async Task PublishAsync<T>(T message)
    {
        var jsonMessage = JsonSerializer.Serialize(message);

        try
        {
            var result = await _producer.ProduceAsync(
                _settings.Topic,
                new Message<Null, string> { Value = jsonMessage }
            );

            _logger.LogInformation(
                "Message sent to Kafka Topic: {Topic}, Partition: {Partition}, Offset: {Offset}",
                result.Topic, result.Partition, result.Offset
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing message to Kafka");
            throw;
        }
    }
}