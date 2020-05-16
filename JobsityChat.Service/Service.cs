using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace JobsityChat.Service
{
    public partial class Service : ServiceBase
    {
        private static ConnectionFactory _factory;
        private static IConnection _connection;
        private static IModel _channel;
        private static string eventSource = "JobsityChatService";
        public Service()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {

            Task.Run(() =>
            {

                try
                {
                    _factory = new ConnectionFactory()
                    {
                        HostName = "192.168.0.117",
                        Port = 5672,
                        UserName = "guest",
                        Password = "guest"
                    };
                    _connection = _factory.CreateConnection();
                    _channel = _connection.CreateModel();
                    _channel.ExchangeDeclare(exchange: "JobsityChatService",
                                                type: "direct");
                    var queueName = _channel.QueueDeclare().QueueName;

                    _channel.QueueBind(queue: queueName,
                                          exchange: "JobsityChatService",
                                          routingKey: "quote");


                    var consumer = new EventingBasicConsumer(_channel);

                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body.ToArray());
                        var routingKey = ea.RoutingKey;
                        var result = GetQuotes(message);
                        foreach (var item in result)
                        {
                            var text = $"{item.Symbol.ToUpper()} quote is ${item.Close} per share";
                            SendResponse(text);
                        }
                    };
                    _channel.BasicConsume(queue: queueName,
                                         autoAck: true,
                                         consumer: consumer);



                }
                catch (Exception e)
                {
                    EventLog.WriteEntry(
                        eventSource,
                        e.Message,
                        EventLogEntryType.Error,
                        100
                    );
                }
            });
        }


        public static List<StockQuote> GetQuotes(string stockCode)
        {
            var lines = new List<string>();
            var result = new List<StockQuote>();

            HttpWebRequest req =
                (HttpWebRequest)WebRequest
                    .Create("https://stooq.com/q/l/?e=csv&f=sd2t2c&s=" + stockCode);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

            using (StreamReader reader = new StreamReader(resp.GetResponseStream()))
            {
                lines.AddRange(reader.ReadToEnd().Split(Environment.NewLine.ToCharArray()).Where(l => l != ""));
            }

            if (lines.Count > 0)
            {
                foreach (var line in lines)
                {
                    var quoteData = line.Split(',');

                    if (quoteData.Length < 4)
                    {
                        continue;
                    }

                    DateTime.TryParse($"{quoteData[1]} {quoteData[2]}", out var quoteDate);
                    decimal.TryParse(quoteData[3],
                            NumberStyles.AllowDecimalPoint,
                            new CultureInfo("en-US"),
                            out var closeValue);

                    if (quoteDate == null || closeValue == 0)
                    {
                        continue;
                    }


                    result.Add(new StockQuote
                    {
                        Symbol = quoteData[0],
                        Date = quoteDate,
                        Close = closeValue
                    });
                }
            }


            return result;
        }

        static void SendResponse(string response)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "192.168.0.117",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };


            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "JobsityChatServiceQuotes",
                                        type: "direct");

                var body = Encoding.UTF8.GetBytes(response);
                channel.BasicPublish(exchange: "JobsityChatServiceQuotes",
                                     routingKey: "quote",
                                     basicProperties: null,
                                     body: body);
            }
        }

        protected override void OnStop()
        {

            _channel.Close();
            _connection.Close();
            _channel.Dispose();
            _connection.Dispose();
        }
    }
}
