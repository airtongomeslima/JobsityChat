using JobsityChat.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobsityChat.Domain.Repository
{
    public interface IRepository
    {
        IEnumerable<ChatRoomEntity> GetChatRooms();
        IEnumerable<ChatRoomMessageEntity> GetChatRoom(int chatRoomId);
    }
}
