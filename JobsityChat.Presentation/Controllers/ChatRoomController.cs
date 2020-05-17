using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobsityChat.Domain.Api;
using JobsityChat.Domain.Entities;
using JobsityChat.Domain.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

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
        [Authorize()]
        public async Task<IEnumerable<ChatRoom>> GetChatRoomsAsync()
        {
            return await chatRoomApi.GetChatRooms();
        }

        [HttpPost]
        [Authorize()]
        public async Task<IEnumerable<ChatRoom>> Post(string chatRoomName)
        {
            var accessToken = Request.Headers[HeaderNames.Authorization];
            //TODO: get user info to fill userid
            var userId = "";
            return await chatRoomApi.CreateChatRoom(chatRoomName, userId);
        }

    }
}