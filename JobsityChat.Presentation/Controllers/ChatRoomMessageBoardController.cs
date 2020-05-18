using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobsityChat.Domain.Api;
using JobsityChat.Domain.Entities;
using JobsityChat.Domain.UnitOfWork;
using JobsityChat.Presentation.Models;
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
        public async Task<BaseResponseModel<IEnumerable<Message>>> GetMessages(int chatRoomId)
        {
            IEnumerable<Message> result;
            try
            {
                result = await chatRoomApi.GetChatRoomMessageBoardAsync(chatRoomId);

                return new BaseResponseModel<IEnumerable<Message>>
                {
                    Success = true,
                    Response = result
                };
            }
            catch (Exception e)
            {
                return new BaseResponseModel<IEnumerable<Message>>
                {
                    Success = false
                };
            }
        }

        [HttpPost]
        public async Task<BaseResponseModel<IEnumerable<Message>>> Post(Message post)
        {
            IEnumerable<Message> result;
            try
            {
                post.UserId = GetCurrentUserId(configuration);
                post.UserName = GetCurrentUserName(configuration);

                result = await chatRoomApi.SendMessageAsync(post);

                return new BaseResponseModel<IEnumerable<Message>>
                {
                    Success = true,
                    Response = result
                };
            }
            catch (Exception e)
            {
                return new BaseResponseModel<IEnumerable<Message>>
                {
                    Success = false
                };
            }
        }

    }
}