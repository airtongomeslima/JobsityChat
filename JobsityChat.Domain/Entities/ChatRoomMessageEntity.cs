using System;
using System.Collections.Generic;
using System.Text;

namespace JobsityChat.Data.Entities
{
    public class ChatRoomMessageEntity
    {
        public int ChatRoomMessageId { get; set; }
        public int ChatRoomId { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; }
        public DateTime PostDate { get; set; }
    }
}
