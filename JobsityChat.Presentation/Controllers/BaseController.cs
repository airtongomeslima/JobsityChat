using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobsityChat.Presentation.Controllers
{
    public class BaseController : Controller
    {
        private int userId = 0;

        public int getCurrentUserId()
        {
            return userId;
        }
    }
}
