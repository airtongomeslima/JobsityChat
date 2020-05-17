using JobsityChat.Presentation.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace JobsityChat.Presentation.Helpers
{
    public static class UserInformationHelper
    {
        public static async Task<UserInformationModel> GetUserInfo(string bearerToken, string baseAuthenticationUri)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseAuthenticationUri);
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken.Replace("Bearer ", ""));
                var result = await client.GetAsync("api/Account/UserInfo");

                if (result.IsSuccessStatusCode)
                {
                    string resultContent = await result.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<UserInformationModel>(resultContent);
                }

                return null;
            }
        }
    }
}
