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

namespace JobsityChat.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize()]
    public class ChatRoomMessageBoardController : BaseController
    {
        IChatRoomApi chatRoomApi;
        IConfiguration configuration;

        public ChatRoomMessageBoardController(IChatRoomApi chatRoomApi, IConfiguration configuration)
        {
            this.chatRoomApi = chatRoomApi;
            this.configuration = configuration;
        }

        [HttpGet("{chatRoomId}")]
        public async Task<IEnumerable<Message>> GetChatRoomMessageBoardAsync(int chatRoomId)
        {
            return await chatRoomApi.GetChatRoomMessageBoardAsync(chatRoomId);
        }

        [HttpPost]
        public async Task<IEnumerable<Message>> Post(Message post)
        {
            post.UserId = GetCurrentUserId(configuration);
            post.UserName = GetCurrentUserName(configuration);

            return await chatRoomApi.SendMessageAsync(post);
        }

    }
}