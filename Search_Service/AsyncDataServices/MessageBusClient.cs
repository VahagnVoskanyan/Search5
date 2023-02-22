using RabbitMQ.Client;
using System.Text;
using System.Threading.Channels;

namespace Search_Service.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;
            var factory = new ConnectionFactory() { Uri = new Uri(_configuration["AmqpUri"]) };
            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _connection.ConnectionShutdown += RabbitMQ_ConnnectionShutdown; //From Video

                Console.WriteLine("--> Connected to Message Bus");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Couldn't connect to Message Bus: {ex.Message}");
            }
        }

        public void SendNameToBus(string name)
        {
            if (_connection.IsOpen)
            {
                Console.WriteLine("--> RabbitMQ Connectioin is Open, sending message...");
                SendMessage(name);
            }
            else
            {
                Console.WriteLine("--> RabbitMQ Connectioin is Closed, Not sending");
            }
        }

        private void SendMessage(string message)
        {
            _channel.QueueDeclare(queue: "searchqueue",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: string.Empty,
                     routingKey: "searchqueue",
                     basicProperties: null,
                     body: body);

            Console.WriteLine($"We have sent: {message}");
        }
        public void Dispose()
        {
            Console.WriteLine("MessageBus Disposed");
            if (_channel.IsOpen)
            {
                _channel.Close(); //Also has dispose method 
                _connection.Close();
            }
        }

        private void RabbitMQ_ConnnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> RabbitMQ Connection Shutdown");
        }
    }
}
