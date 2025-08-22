using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Text;

namespace Search_Service.AsyncDataServices
{
    public class RpcBusClient : IRpcBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection? _connection;
        private readonly IModel? _channel;
        private readonly string _requestQueueName = "searchqueue";
        private readonly string _responseQueueName = "rpc_reply";
        private readonly ConcurrentDictionary<string, TaskCompletionSource<string>> callbackMapper = new(); //??

        public RpcBusClient(IConfiguration configuration)
        {
            _configuration = configuration;

            //var factory = new ConnectionFactory() { Uri = new Uri(_configuration["AmqpUri"]) };
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

                _connection.ConnectionShutdown += RabbitMQ_ConnnectionShutdown; //From Video

                Console.WriteLine("--> Connected to Message Bus in RpcBusClient");

                // declare a server-named queue (Response handling)(from tutorial)
                _channel.QueueDeclare(_responseQueueName, true, false, false, null);

                var consumer = new EventingBasicConsumer(_channel);
                //var consumer = new AsyncDefaultBasicConsumer(_channel);

                consumer.Received += (model, ea) =>
                {
                    if (!callbackMapper.TryRemove(ea.BasicProperties.CorrelationId, out var tcs))
                        return;
                    var body = ea.Body.ToArray();
                    var responsemessage = Encoding.UTF8.GetString(body.ToArray());

                    Console.WriteLine($"--> Received Message: {responsemessage}");
                    tcs.TrySetResult(responsemessage);    //
                };

                _channel.BasicConsume(consumer: consumer,
                                     queue: _responseQueueName,
                                     autoAck: true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Couldn't connect to Message Bus: {ex.Message}");
            }
        }

        public Task<string> SendNameToBusAsync(string message, CancellationToken cancellationToken)
        {
            _channel!.QueueDeclare(queue: _requestQueueName,
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

            IBasicProperties props = _channel.CreateBasicProperties();

            var correlationId = Guid.NewGuid().ToString();
            props.CorrelationId = correlationId;
            props.ReplyTo = _responseQueueName;        //sending repling queue name

            var messageBytes = Encoding.UTF8.GetBytes(message);

            var tcs = new TaskCompletionSource<string>();
            callbackMapper.TryAdd(correlationId, tcs);

            _channel.BasicPublish(exchange: string.Empty,
                     routingKey: "searchqueue",
                     basicProperties: props,
                     body: messageBytes);

            Console.WriteLine($"--> We have sent: {message}");

            cancellationToken.Register(() => callbackMapper.TryRemove(correlationId, out _));
            return tcs.Task;
        }


        public void Dispose()
        {
            Console.WriteLine("MessageBus Disposed");
            if (_channel!.IsOpen)
            {
                _channel.Close(); 
                _connection!.Close();
            }
        }

        private void RabbitMQ_ConnnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> RabbitMQ Connection Shutdown");
        }
    }
}
