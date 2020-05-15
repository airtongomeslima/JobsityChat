using JobsityChat.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JobsityChat.Test.Helpers
{
    public interface IMockTestDatabase
    {
        public Task<int> CreateUser();
        public Task<ChatRoomEntity> CreateChatRoomAsync();
        public Task<IEnumerable<ChatRoomEntity>> CreateManyChatRoomAsync();
        public Task<IEnumerable<ChatRoomMessageEntity>> ChatRoomMessageBoardMockedDataAsync();
    }
}
