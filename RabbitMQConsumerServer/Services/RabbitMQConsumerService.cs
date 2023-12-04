using Microsoft.AspNetCore.Connections;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;
using System.Text;
using Mc2.CrudTest.Shared.Models.Logs;
using MongoDB.Driver;

namespace RabbitMQConsumerServer.Services
{
    public class RabbitMQConsumerService
    {
        private readonly ILogger<RabbitMQConsumerService> _logger;
        protected ConnectionFactory _factory;
        protected IConnection _connection;
        protected IModel _channel;
        MongoClient? dbClient;

        public RabbitMQConsumerService(ILogger<RabbitMQConsumerService> logger)
        {
            _logger = logger;
            try
            {
                _factory = new ConnectionFactory()
                {
                    HostName = "127.0.0.1"
   ,
                    Port = 5672
   ,
                    UserName = "admin"
   ,
                    Password = "admin"
                };
                _connection = _factory.CreateConnection();
                _channel = _connection.CreateModel();
                dbClient = new MongoClient("mongodb://localhost");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException?.Message ?? ex.Message);
            }
        }
        public void StartLogConsumer()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += _logConsumer;

            _channel?.BasicConsume(queue: $"LogQueue",
                                 autoAck: true,
                                 consumer: consumer);
        }
        protected async void _logConsumer(object? model, BasicDeliverEventArgs ea)
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var logDto = JsonSerializer.Deserialize<LogDto>(message) ?? throw new Exception("invalid rabbitmq message recieved");
            try
            {
                IMongoDatabase? db = dbClient?.GetDatabase("Logs");
                var collection = db?.GetCollection<LogDto>("LogTransactions");
                collection?.InsertOne(logDto);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.InnerException?.Message ?? ex.Message);
            }

            _logger.LogInformation(message);
        }
    }
}
