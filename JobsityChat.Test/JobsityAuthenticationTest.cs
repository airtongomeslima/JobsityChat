using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace JobsityChat.Test
{
    [TestClass]
    public class JobsityAuthenticationTest
    {
        private string baseAddress;
        public JobsityAuthenticationTest()
        {

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var configuration = builder.Build();

            baseAddress = configuration["RabbitMQ:Address"];
        }

        [TestMethod]
        public async Task CreateUserAsync()
        {
            try
            {
                string userToRegister = "{\"Email\": \"testuser@mailinator.com\",\"Password\": \"@Ai123456789\",\"ConfirmPassword\": \"@Ai123456789\"}";

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseAddress);

                    var content = new StringContent(userToRegister, Encoding.UTF8, "application/json");

                    var result = await client.PostAsync("api/Account/Register", content);

                    string resultContent = await result.Content.ReadAsStringAsync();

                    if (result.StatusCode != HttpStatusCode.OK)
                    {
                        Assert.Fail(resultContent);
                    }
                }
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }
    }
}
