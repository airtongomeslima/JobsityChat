using JobsityChat.MessageBroker;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JobsityChat.Test
{
    [TestClass]
    public class JobsityChatServiceTest
    {
        string address, port, user, password;

        public JobsityChatServiceTest()
        {

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var configuration = builder.Build();

            address = configuration["RabbitMQ:Address"];
            port = configuration["RabbitMQ:Port"];
            user = configuration["RabbitMQ:UserName"];
            password = configuration["RabbitMQ:Password"];

        }

        [TestMethod]
        public void Subscribe()
        {
            using (IMessageBroker messageBroker = new MessageBroker.MessageBroker(address, port, user, password))
            {
                try
                {

                    Task.Run(() =>
                    {
                        messageBroker.Subscribe("JobsityChatServiceQuotes", "quote", (string message) =>
                        {
                            Assert.IsFalse(string.IsNullOrEmpty(message));
                        });
                    }).Wait();

                }
                catch (Exception e)
                {
                    Assert.Fail(e.Message);
                }

            }
        }

        [TestMethod]
        public void GetQuote()
        {

            using (IMessageBroker messageBroker = new MessageBroker.MessageBroker(address, port, user, password))
            {
                try
                {
                    messageBroker.SendMessage("JobsityChatService", "quote", "amd.us");

                }
                catch (Exception e)
                {
                    Assert.Fail(e.Message);
                }

            }
        }
    }
}
