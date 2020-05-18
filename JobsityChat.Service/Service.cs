using JobsityChat.MessageBroker;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
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
        private static IMessageBroker messageBroker;
        public Service()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            var port = ConfigurationManager.AppSettings["RabbitMQPort"];
            var hostName = ConfigurationManager.AppSettings["RabbitMQAddress"];
            var userName = ConfigurationManager.AppSettings["RabbitMQUserName"];
            var password = ConfigurationManager.AppSettings["RabbitMQPassword"];


            messageBroker = new MessageBroker.MessageBroker(hostName, port, userName, password);
            Task.Run(() =>
            {
                try
                {
                    messageBroker.Subscribe(
                        "JobsityChatService",
                        "quote",
                        (string message) =>
                        {
                            var code = message.Split('\t')[0];
                            var chatRoomId = message.Split('\t')[1];
                            var result = GetQuotes(code);


                            if (result.Count == 0)
                            {
                                var text = $"Sorry, but there no quote avaliable for {code} at this moment.\t{chatRoomId}";
                                messageBroker.SendMessage("JobsityChatServiceQuotes", "quote", text);
                            }

                            foreach (var item in result)
                            {
                                var text = $"{item.Symbol.ToUpper()} quote is ${item.Close} per share.\t{chatRoomId}";
                                messageBroker.SendMessage("JobsityChatServiceQuotes", "quote", text);
                            }

                        });

                }
                catch (Exception e)
                {
                    EventLog.WriteEntry(
                        "JobsityChat.Service",
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


        protected override void OnStop()
        {
            messageBroker.Dispose();
        }
    }
}
