using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace JobsityChat.Domain.Entities
{
    public class ChatRoomEntity
    {
        [Description("id")]
        public int ChatRoomId { get; set; }
        public string CreatorUserId { get; set; }
        public string Title { get; set; }
        public int UsersCount { get; set; }
    }
}
