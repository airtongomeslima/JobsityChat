using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobsityChat.Domain.Api;
using JobsityChat.Domain.Entities;
using JobsityChat.Domain.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;

namespace JobsityChat.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize()]
    public class ChatRoomController : BaseController
    {
        IChatRoomApi chatRoomApi;
        IConfiguration configuration;

        public ChatRoomController(IChatRoomApi chatRoomApi, IConfiguration configuration)
        {
            this.chatRoomApi = chatRoomApi;
            this.configuration = configuration;
        }

        [HttpGet]
        public async Task<IEnumerable<ChatRoom>> GetChatRoomsAsync()
        {
            return await chatRoomApi.GetChatRooms();
        }

        [HttpPost]
        public async Task<IEnumerable<ChatRoom>> Post(string chatRoomName)
        {
            var accessToken = Request.Headers[HeaderNames.Authorization];
            //TODO: get user info to fill userid
            var userId = GetCurrentUserId(configuration);
            return await chatRoomApi.CreateChatRoom(chatRoomName, userId);
        }

    }
}