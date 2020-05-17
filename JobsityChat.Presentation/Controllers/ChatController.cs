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
    [Route("[controller]")]
    public class ChatController : Controller
    {
        IChatRoomApi chatRoomApi;

        public ChatController(IChatRoomApi chatRoomApi)
        {
            this.chatRoomApi = chatRoomApi;
        }

        [HttpGet]
        public async Task<IEnumerable<ChatRoom>> GetChatRoomsAsync()
        {
            return await chatRoomApi.GetChatRooms();
        }

        [HttpGet("{chatRoomId}")]
        public async Task<IEnumerable<Message>> GetChatRoomMessageBoardAsync(int chatRoomId)
        {
            return await chatRoomApi.GetChatRoomMessageBoardAsync(chatRoomId);
        }

        [HttpPost]
        public async Task<IEnumerable<Message>> Post(Message post)
        {
            //TODO: get user info to fill userid
            return await chatRoomApi.SendMessageAsync(post);
        }

    }
}