using RabbitMQ.Client;
using Search_Service.Dtos;
using System.Text;
using System.Text.Json;

namespace Search_Service.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection? _connection;
        private readonly IModel? _channel;

        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;

            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQLocalHost"],
                Port = int.Parse(_configuration["RabbitMQLocalPort"] ??
                    throw new NullReferenceException("RabbitMQLocalPort is NULL"))
            };

            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                //_channel.ExchangeDeclare(exchange: "trigger", ExchangeType.Fanout);

                _connection.ConnectionShutdown += RabbitMQ_ConnnectionShutdown; //From Video

                Console.WriteLine("--> Connected to Message Bus in MessageBusClient");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Couldn't connect to Message Bus: {ex.Message}");
            }
        }

        public void SendNewCustomer(CustomerPublishDto customerPublishDto)
        {
            var message = JsonSerializer.Serialize(customerPublishDto);

            if (_connection!.IsOpen)
            {
                Console.WriteLine("--> RabbitMQ Connection Open, sending message...");
                SendMessage(message);
            }
            else
            {
                Console.WriteLine("--> RabbitMQ Connection is Closed, NOT sending");
            }
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            _channel!.BasicPublish(exchange: "amq.direct",
                                   routingKey: "direct",
                                   basicProperties: null,
                                   body: body);

            Console.WriteLine($"--> We have sent: {message}");
        }

        public void Dispose()  //Implement IDisposable (In tutorial)
        {
            Console.WriteLine("MessageBus Disposed");
            if (_channel!.IsOpen)
            {
                _channel.Close(); //Also has dispose method 
                _connection!.Close();
            }
        }


        private void RabbitMQ_ConnnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> RabbitMQ Connection Shutdown");
        }
    }
}
