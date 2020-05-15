using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace JobsityChat.Domain.Entities
{
    public class ChatRoomMessageEntity
    {
        [Description("id")]
        public int ChatRoomMessageId { get; set; }
        public int ChatRoomId { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; }
        public DateTime PostDate { get; set; }
    }
}
