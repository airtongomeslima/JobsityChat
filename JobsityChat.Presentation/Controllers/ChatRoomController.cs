using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobsityChat.Domain.Api;
using JobsityChat.Domain.Entities;
using JobsityChat.Domain.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace JobsityChat.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatRoomController : BaseController
    {
        IChatRoomApi chatRoomApi;

        public ChatRoomController(IChatRoomApi chatRoomApi)
        {
            this.chatRoomApi = chatRoomApi;
        }

        [HttpGet]
        public async Task<IEnumerable<ChatRoom>> GetChatRoomsAsync()
        {
            return await chatRoomApi.GetChatRooms();
        }

        [HttpPost]
        public async Task<IEnumerable<ChatRoom>> Post(string chatRoomName)
        {
            //TODO: get user info to fill userid
            var userId = "";
            return await chatRoomApi.CreateChatRoom(chatRoomName, userId);
        }

    }
}