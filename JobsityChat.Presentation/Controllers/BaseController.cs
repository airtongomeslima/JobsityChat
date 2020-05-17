using JobsityChat.Presentation.Helpers;
using JobsityChat.Presentation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobsityChat.Presentation.Controllers
{
    public class BaseController : Controller
    {
        private UserInformationModel user;

        private void FillUser(IConfiguration configuration)
        {
            

            var token = Request.Headers["Authorization"].ToString();

            Task<UserInformationModel> task = Task.Run<UserInformationModel>(async () => await UserInformationHelper.GetUserInfo(token, configuration["Authentication:BaseAddress"]));

            user = task.Result;


        }

        public string GetCurrentUserId(IConfiguration configuration)
        {
            if (user == null)
            {
                FillUser(configuration);
            }
            return user.UserId;
        }

        public string GetCurrentUserName(IConfiguration configuration)
        {
            if (user == null)
            {
                FillUser(configuration);
            }

            return user.UserName;
        }

        public bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
