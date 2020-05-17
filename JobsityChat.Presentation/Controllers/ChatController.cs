using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobsityChat.Domain.Api;
using JobsityChat.Domain.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace JobsityChat.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatController : Controller
    {
        IChatRoomApi chatRoomApi;

        public ChatController(IChatRoomApi chatRoomApi)
        {

        }

        public IActionResult Index()
        {
            return View();
        }
    }
}