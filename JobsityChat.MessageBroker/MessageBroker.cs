using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace JobsityChat.MessageBroker
{
    public class MessageBroker : IMessageBroker
    {
        private static ConnectionFactory _factory;
        private static IConnection _connection;
        private static IModel _channel;
        private static string hostName, userName, password;
        private int port;

        public MessageBroker(string rabbitMQAddress, string rabbitMQPort, string rabbitMQUserName, string rabbitMQPassword)
        {
            int.TryParse(rabbitMQPort, out var port);
            this.port = port == 0 ? 5672 : port;

            hostName = rabbitMQAddress;
            userName = rabbitMQUserName;
            password = rabbitMQPassword;

        }

        public void SendMessage(string exchange, string routingKey, string message, string type = "direct")
        {
            var factory = new ConnectionFactory()
            {
                HostName = hostName,
                Port = port,
                UserName = userName,
                Password = password
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: exchange,
                                        type: type);

                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: exchange,
                                     routingKey: routingKey,
                                     basicProperties: null,
                                     body: body);
            }
        }

        public void Subscribe(string exchange, string routingKey, Action<string> onReceive, string type = "direct")
        {
            try
            {
                _factory = new ConnectionFactory()
                {
                    HostName = hostName,
                    Port = port,
                    UserName = userName,
                    Password = password
                };
                _connection = _factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.ExchangeDeclare(exchange: exchange,
                                            type: type);
                var queueName = _channel.QueueDeclare().QueueName;

                _channel.QueueBind(queue: queueName,
                                      exchange: exchange,
                                      routingKey: routingKey);


                var consumer = new EventingBasicConsumer(_channel);

                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body.ToArray());
                    onReceive(message);
                };
                _channel.BasicConsume(queue: queueName,
                                     autoAck: true,
                                     consumer: consumer);



            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Dispose()
        {
            if (_channel != null && _channel.IsOpen)
                _channel.Close();

            if (_connection != null && _connection.IsOpen)
                _connection.Close();

            if (_channel != null)
                _channel.Dispose();

            if (_connection != null)
                _connection.Dispose();
        }
    }
}
