using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace JobsityChat.Presentation.Controllers
{
    public class AuthenticationController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}