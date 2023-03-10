using AutoMapper;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Search_Service.EventProcessing;
using Server_Service.Data;
using Server_Service.Dtos;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace Server_Service.AsyncDataServices
{
    public class MessageBusSubscriber : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IEventProcessor _eventProcessor;
        private IConnection _connection;
        private IModel _channel;

        public MessageBusSubscriber(
            IConfiguration configuration,
            IEventProcessor eventProcessor)
        {
            _configuration = configuration;
            _eventProcessor = eventProcessor;

            InitializeRabbitMQ();
        }

        private void InitializeRabbitMQ()
        {
            //var factory = new ConnectionFactory() { Uri = new Uri(_configuration["AmqpUri"]) };
            var factory = new ConnectionFactory() { HostName = _configuration["RabbitMQLocalHost"],
                                                    Port = int.Parse(_configuration["RabbitMQLocalPort"]) };
            //var factory = new ConnectionFactory() { HostName = _configuration["RabbitMQLocalHost"],
            //                                        Port = 15672 };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "searchqueue",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

            //'prefetchCount'-queue won't send next message before the first one isn't 'acked' 
            _channel.BasicQos(prefetchSize: 0, prefetchCount: 2, global: false);

            _connection.ConnectionShutdown += RabbitMQ_ConnnectionShutdown;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            _channel.BasicConsume(queue: "searchqueue",
                                 autoAck: false,     //'false' for waiting
                                 consumer: consumer);
            Console.WriteLine("--> Awaiting RabbitMQ RPC requests");

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();

                //For ID
                var props = ea.BasicProperties;
                var replyProps = _channel.CreateBasicProperties();
                replyProps.CorrelationId = props.CorrelationId;
                
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"--> Received Name: {message}");

                var response = _eventProcessor.ProcessEvent(message);      //Finding customers

                //After processing the event, we 'ack' the received message 
                _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);

                //Sending response message
                var jsonmessage = JsonSerializer.Serialize(response);  //??
                var responseBytes = Encoding.UTF8.GetBytes(jsonmessage);
                _channel.BasicPublish(exchange: string.Empty,
                             routingKey: props.ReplyTo,
                             basicProperties: replyProps,
                             body: responseBytes);
            };

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Console.WriteLine("MessageBus Disposed");
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
            base.Dispose();
        }
        private void RabbitMQ_ConnnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> RabbitMQ Connection Shutdown");
        }
    }
}
