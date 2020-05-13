using System;
using System.Collections.Generic;
using System.Text;

namespace JobsityChat.Data.Entities
{
    public class ChatRoomEntity
    {
        public int ChatRoomId { get; set; }
        public int CreatorUserId { get; set; }
        public string Title { get; set; }
        public int UsersCount { get; set; }
    }
}
