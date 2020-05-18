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
        public async Task<BaseResponseModel<IEnumerable<ChatRoom>>> GetChatRoomsAsync()
        {
            IEnumerable<ChatRoom> result;
            try
            {
                result = await chatRoomApi.GetChatRooms();

                return new BaseResponseModel<IEnumerable<ChatRoom>>
                {
                    Success = true,
                    Response = result
                };
            }
            catch (Exception e)
            {
                return new BaseResponseModel<IEnumerable<ChatRoom>>
                {
                    Success = false
                };
            }
        }

        [HttpPost]
        public async Task<BaseResponseModel<IEnumerable<ChatRoom>>> Post(ChatRoom newChatRoom)
        {
            IEnumerable<ChatRoom> result;
            try
            {
                var accessToken = Request.Headers[HeaderNames.Authorization];
                var userId = GetCurrentUserId(configuration);
                result = await chatRoomApi.CreateChatRoom(newChatRoom.Title, userId);

                return new BaseResponseModel<IEnumerable<ChatRoom>>
                {
                    Success = true,
                    Response = result
                };
            }
            catch (Exception e)
            {
                return new BaseResponseModel<IEnumerable<ChatRoom>>
                {
                    Success = false
                };
            }
        }

    }
}