using AutoMapper;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Search_Service.EventProcessing;
using Server_Service.Data;
using Server_Service.Dtos;
using System.Text;
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
            var factory = new ConnectionFactory() { Uri = new Uri(_configuration["AmqpUri"]) };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "searchqueue",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

            //'prefetchCount'-queue won't send next message before the first one isn't 'acked' 
            _channel.BasicQos(prefetchSize: 0, prefetchCount: 2, global: false);
            Console.WriteLine("--> Listening on the Message Bus...");

            _connection.ConnectionShutdown += RabbitMQ_ConnnectionShutdown;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"--> Received Name: {message}");

                _eventProcessor.ProcessEvent(message);      //Finding customers and sending

                //After processing the event and sending message to Bus, we 'ack' the received message 
                _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            _channel.BasicConsume(queue: "searchqueue",
                                 autoAck: false,     //'false' for waiting
                                 consumer: consumer);

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
