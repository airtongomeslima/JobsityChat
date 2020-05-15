using JobsityChat.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobsityChat.Domain.Api
{
    public interface IApi
    {
        IEnumerable<ChatRoom> GetChatRooms();
        IEnumerable<Message> GetChatRoomMessageBoard(int chatRoomId);
    }
}
