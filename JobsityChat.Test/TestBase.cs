using JobsityChat.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobsityChat.Test
{
    public static class TestBase
    {
        public static IEnumerable<Data.Entities.ChatRoom> ChatRoomMockedData => new Data.Entities.ChatRoom[]
        {
            new ChatRoom{ ChatRoomId = 1, Title = "ChatRoom 1", UsersCount = 2 }
        };

        public static IEnumerable<Data.Entities.ChatRoomMessageEntity> ChatRoomMessageBoardMockedData => new Data.Entities.ChatRoomMessageEntity[]
        {
            new Data.Entities.ChatRoomMessageEntity { ChatRoomId = 1, ChatRoomMessageId = 1, UserId = 1, Message = "Test Message 1", PostDate = new DateTime(2020,05,13,9,30,1)  },
            new Data.Entities.ChatRoomMessageEntity { ChatRoomId = 1, ChatRoomMessageId = 2, UserId = 2, Message = "Test Message 2", PostDate = new DateTime(2020,05,13,9,30,10)  },
            new Data.Entities.ChatRoomMessageEntity { ChatRoomId = 1, ChatRoomMessageId = 3, UserId = 1, Message = "Test Message 3", PostDate = new DateTime(2020,05,13,9,30,15)  },
            new Data.Entities.ChatRoomMessageEntity { ChatRoomId = 1, ChatRoomMessageId = 4, UserId = 2, Message = "Test Message 4", PostDate = new DateTime(2020,05,13,9,31,1)  },
            new Data.Entities.ChatRoomMessageEntity { ChatRoomId = 1, ChatRoomMessageId = 5, UserId = 1, Message = "Test Message 5", PostDate = new DateTime(2020,05,13,10,30,1)  }
        };
        public static IEnumerable<Data.Entities.User> UsersMocked => new Data.Entities.User[]
        {
            new Data.Entities.User { Name = "User A", UserId = 1 },
            new Data.Entities.User { Name = "User B", UserId = 2 },
        };
    }
}
