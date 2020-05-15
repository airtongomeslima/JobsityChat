using JobsityChat.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobsityChat.Test.Helpers
{
    public class MockTestDatabase : IMockTestDatabase
    {
        private TestDatabase testDatabase;

        public MockTestDatabase(TestDatabase testDatabase)
        {
            this.testDatabase = testDatabase;
        }

        public async Task<int> CreateUser()
        {
            var iduser = await testDatabase.ExecuteScalarAsync<UserEntity>("User",
      "INSERT INTO [USER] (Name,Email,Password) " +
      "VALUES ('TestUser1', 'testuser1@mailinator.com', '123456'); SELECT CAST(SCOPE_IDENTITY() as int);");

            return (int)iduser;
        }



        public async Task<ChatRoomEntity> CreateChatRoomAsync()
        {

            var iduser = await CreateUser();

            var chatRoom = new ChatRoomEntity
            {
                CreatorUserId = iduser,
                Title = $"ChatRoom {RandomString(15)}",
                UsersCount = 0
            };

            var chatRoomId = await testDatabase.ExecuteScalarAsync<ChatRoomEntity>("ChatRoom",
                    "INSERT INTO ChatRoom (CreatorUserId,Title,UsersCount) " +
                    $"VALUES ({chatRoom.CreatorUserId.ToString()},'{chatRoom.Title}',{chatRoom.UsersCount}); SELECT CAST(SCOPE_IDENTITY() as int);");

            chatRoom.ChatRoomId = (int)chatRoomId;

            return chatRoom;
        }

        public async Task<IEnumerable<ChatRoomEntity>> CreateManyChatRoomAsync()
        {

            var iduser = await CreateUser();

            var values = new ChatRoomEntity[]
            {
                await CreateChatRoomAsync(),
                await CreateChatRoomAsync(),
                await CreateChatRoomAsync()
            };

            foreach (var value in values)
            {
                var chatRoomId = await testDatabase.ExecuteScalarAsync<ChatRoomEntity>("ChatRoom",
                        "INSERT INTO ChatRoom (CreatorUserId,Title,UsersCount) " +
                        $"VALUES ({value.CreatorUserId.ToString()},'{value.Title}',{value.UsersCount}); SELECT CAST(SCOPE_IDENTITY() as int);");

                value.ChatRoomId = (int)chatRoomId;
            }

            return values;
        }

        public async Task<IEnumerable<ChatRoomMessageEntity>> ChatRoomMessageBoardMockedDataAsync()
        {

            var chatRoom = await CreateChatRoomAsync();
            var userA = await CreateUser();
            var userB = await CreateUser();

            var values = new ChatRoomMessageEntity[]
            {
            new ChatRoomMessageEntity { ChatRoomId = chatRoom.ChatRoomId, UserId = userA, Message = RandomString(20), PostDate = DateTime.Now.AddHours(-1)  },
            new ChatRoomMessageEntity { ChatRoomId = chatRoom.ChatRoomId, UserId = userB, Message = RandomString(20), PostDate = DateTime.Now.AddHours(-1)  },
            new ChatRoomMessageEntity { ChatRoomId = chatRoom.ChatRoomId, UserId = userA, Message = RandomString(20), PostDate = DateTime.Now.AddHours(-1)  },
            new ChatRoomMessageEntity { ChatRoomId = chatRoom.ChatRoomId, UserId = userB, Message = RandomString(20), PostDate = DateTime.Now.AddHours(-1)  }
            };

            foreach (var value in values)
            {
                var chatRoomMessageId = await testDatabase.ExecuteScalarAsync<ChatRoomMessageEntity>("ChatRoomMessage",
                        "INSERT INTO ChatRoomMessage (ChatRoomId,UserId,Message,PostDate) " +
                        $"VALUES ({value.ChatRoomId},{value.UserId},'{value.Message}','{value.PostDate.ToString("yyyy-MM-dd HH:mm:ss.fff")}'); SELECT CAST(SCOPE_IDENTITY() as int);");

                value.ChatRoomMessageId = (int)chatRoomMessageId;
            }

            return values;
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
