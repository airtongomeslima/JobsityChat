using System;
using System.Collections.Generic;
using System.Text;

namespace JobsityChat.Domain.Entities
{
    public class Message
    {
        public int UserId { get; set; }
        public int ChatRoomId { get; set; }
        public string UserName { get; set; }
        public string Text { get; set; }
        public DateTime PostDate { get; set; }
    }
}
