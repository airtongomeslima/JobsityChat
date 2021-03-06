﻿using JobsityChat.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JobsityChat.Domain.Api
{
    public interface IChatRoomApi
    {
        Task<IEnumerable<ChatRoom>> GetChatRooms();
        Task<IEnumerable<Message>> GetChatRoomMessageBoardAsync(int chatRoomId);
        Task<IEnumerable<Message>> SendMessageAsync(Message message);
        Task<IEnumerable<ChatRoom>> CreateChatRoom(string name, string userId);
    }
}
