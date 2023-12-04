using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using System.Threading.Channels;
using System;
using System.Threading.Tasks;
using Mc2.CrudTest.Shared.Models.Logs;

namespace Mc2.CrudTest.Presentation.Server.Services
{
    public class RabbitMQProducerService
    {
        private readonly ILogger<RabbitMQProducerService> _logger;

        protected ConnectionFactory _factory;
        protected IConnection _connection;
        protected IModel _channel;
        public RabbitMQProducerService(ILogger<RabbitMQProducerService> logger)
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

                _channel.ExchangeDeclare(exchange: "LogExchange", type: "direct");

                _channel.QueueDeclare(queue: "LogQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
                _channel.QueueBind("LogQueue", "LogExchange", "LogQueue");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException?.Message ?? ex.Message);
            }
        }
        public Task AddToLogQueueAsync(LogDto model)
        {
            string message = JsonSerializer.Serialize(model);
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "LogExchange",
                                 routingKey: "LogQueue",
                                 basicProperties: null,
                                 body: body);
            _logger.LogInformation($"add to log queue:{model.TableName}-{model.UserId}-{model.RowId}");
            return Task.CompletedTask;
        }

    }
}
