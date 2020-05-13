using JobsityChat.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobsityChat.Data.Api
{
    public interface IApi
    {
        IEnumerable<ChatRoom> GetChatRoomsData();
        IEnumerable<Message> GetChatRoomData(int chatRoomId);
    }
}
