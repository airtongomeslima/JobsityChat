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
    public class ChatRoomMessageBoardController : BaseController
    {
        IChatRoomApi chatRoomApi;

        public ChatRoomMessageBoardController(IChatRoomApi chatRoomApi)
        {
            this.chatRoomApi = chatRoomApi;
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