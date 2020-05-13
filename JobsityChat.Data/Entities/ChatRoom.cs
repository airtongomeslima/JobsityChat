using System;
using System.Collections.Generic;
using System.Text;

namespace JobsityChat.Data.Entities
{
    public class ChatRoom
    {
        public int ChatRoomId { get; set; }
        public string Title { get; set; }
        public int UsersCount { get; set; }

        public IList<Message> Messages { get; set; }
    }
}
