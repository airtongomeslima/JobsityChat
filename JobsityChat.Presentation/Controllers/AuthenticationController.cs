using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using JobsityChat.Presentation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace JobsityChat.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : BaseController
    {
        IConfiguration configuration;

        public AuthenticationController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpPost("Login")]
        public async Task<BaseResponseModel<AuthenticationModel>> Post(LoginModel login)
        {
            try
            {
                var data = new Dictionary<string, string>();
                data.Add("grant_type", "password");
                data.Add("username", login.Email);
                data.Add("password", login.Password);

                using (var client = new HttpClient())
                {
                    using (var content = new FormUrlEncodedContent(data))
                    {
                        content.Headers.Clear();
                        content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                        client.BaseAddress = new Uri(configuration["Authentication:BaseAddress"]);

                        var result = await client.PostAsync("token", content);

                        string resultContent = await result.Content.ReadAsStringAsync();

                        if (result.StatusCode != HttpStatusCode.OK)
                        {
                            //Add log
                            return new BaseResponseModel<AuthenticationModel>
                            {
                                Success = false,
                                Message = result.StatusCode == HttpStatusCode.Unauthorized ?  "User or password incorrect." : "Fail to login."
                            };
                        }

                        return new BaseResponseModel<AuthenticationModel>
                        {
                            Success = true,
                            Response = JsonConvert.DeserializeObject<AuthenticationModel>(resultContent)
                        };
                    }
                }
            }
            catch (Exception e)
            {
                //Add log
                return new BaseResponseModel<AuthenticationModel>
                {
                    Success = false,
                    Message = "Fail to login."
                };
            }
        }


        [HttpPost("Register")]
        public async Task<BaseResponseModel<bool>> Register(RegisterModel userToRegister)
        {
            try
            {
                //string userToRegister =  "{\"Email\": \"testuser@mailinator.com\",\"Password\": \"@Ai123456789\",\"ConfirmPassword\": \"@Ai123456789\"}";
                if (userToRegister.Password != userToRegister.ConfirmPassword)
                {
                    return new BaseResponseModel<bool>
                    {
                        Success = false,
                        Message = "The passwords must match."
                    };
                }

                if (!IsValidEmail(userToRegister.Email))
                {
                    return new BaseResponseModel<bool>
                    {
                        Success = false,
                        Message = "You must provide a valid email address."
                    };
                }

                if (string.IsNullOrEmpty(userToRegister.UserName) ||
                    string.IsNullOrEmpty(userToRegister.Email) ||
                    string.IsNullOrEmpty(userToRegister.Password) ||
                    string.IsNullOrEmpty(userToRegister.ConfirmPassword))
                {
                    return new BaseResponseModel<bool>
                    {
                        Success = false,
                        Message = "All fields are required."
                    };
                }

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(configuration["Authentication:BaseAddress"]);

                    var content = new StringContent(JsonConvert.SerializeObject(userToRegister), Encoding.UTF8, "application/json");

                    var result = await client.PostAsync("api/Account/Register", content);

                    string resultContent = await result.Content.ReadAsStringAsync();

                    if (result.StatusCode != HttpStatusCode.OK)
                    {
                        return new BaseResponseModel<bool>
                        {
                            Success = false,
                            Message = resultContent
                        };
                    }

                    return new BaseResponseModel<bool>
                    {
                        Success = true
                    };
                }
            }
            catch (Exception e)
            {
                //Add log
                return new BaseResponseModel<bool>
                {
                    Success = false,
                    Message = "Fail to login."
                };
            }
        }
    }
}