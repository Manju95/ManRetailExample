using System.Text.Json;
using Confluent.Kafka;
using InventoryService.DTO;
using InventoryService.Interfaces;
using InventoryService.Models;
using Microsoft.Extensions.Options;

namespace InventoryService.Services;

public class InventoryKafkaConsumer : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly KafkaSettings _settings;
    private readonly ILogger<InventoryKafkaConsumer> _logger;

    public InventoryKafkaConsumer(IOptions<KafkaSettings> settings, ILogger<InventoryKafkaConsumer> logger,IServiceScopeFactory scopeFactory)
    {
        _settings = settings.Value ?? throw new ArgumentNullException(nameof(settings));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = _settings.BootstrapServers,
            GroupId = _settings.GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = true
        };

        using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        consumer.Subscribe(_settings.Topic);

        _logger.LogInformation("Inventory Service listening to Kafka topic: {Topic}", _settings.Topic);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var inventoryService = scope.ServiceProvider
                    .GetRequiredService<IInventoryServices>();
                
                var result = consumer.Consume(stoppingToken);

                var orderEvent = JsonSerializer.Deserialize<OrderCreatedEvent>(result.Message.Value);

                _logger.LogInformation("Received OrderCreatedEvent: {OrderId}", orderEvent.OrderId);
                
                foreach (var item in orderEvent.Items)
                {
                    var product = await inventoryService.GetProductById(item.ProductId);

                    if (product == null)
                    {
                        _logger.LogWarning("Product not found: {ProductId}", item.ProductId);
                        continue;
                    }

                    if (product.Quantity < item.Quantity)
                    {
                        _logger.LogWarning("Not enough stock for Product {ProductId}", item.ProductId);
                        continue;
                    }

                    product.Quantity -= item.Quantity;

                    await inventoryService.UpdateStock(item.ProductId, product);
                }
                _logger.LogInformation("Stock updated for products");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error consuming Kafka message");
            }
        }
    }
}