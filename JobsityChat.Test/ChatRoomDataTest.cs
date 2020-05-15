using JobsityChat.Domain.Mappers;
using JobsityChat.Domain.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Configuration;
using JobsityChat.Domain.Entities;
using JobsityChat.Test.Helpers;

namespace JobsityChat.Test
{
    [TestClass]
    public class ChatRoomDataTest
    {
        private Domain.UnitOfWork.IUnitOfWork unitOfWork;
        private static TestDatabase testDatabase;
        private static IMockTestDatabase mockMemoryDatabase;

        private static ChatRoomEntity baseChatRoom = new ChatRoomEntity
        {
            CreatorUserId = 2,
            Title = "Test ChatRoom",
            UsersCount = 0
        };

        public ChatRoomDataTest()
        {

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var configuration = builder.Build();

            string _con = configuration["ConnectionStrings:DefaultConnection"];

            testDatabase = new TestDatabase(_con);
            mockMemoryDatabase = new MockTestDatabase(testDatabase);

            unitOfWork = new Data.UnitOfWork.UnitOfWork(_con);
        }

        [TestMethod]
        public async Task CreateChatRoom()
        {
            var userId = await mockMemoryDatabase.CreateUser();
            baseChatRoom.CreatorUserId = userId;

            var chatRoomId = await unitOfWork.ChatRoomRepository.InsertAsync(baseChatRoom);

            baseChatRoom.ChatRoomId = chatRoomId;

            var chatRooms = await unitOfWork.ChatRoomRepository.GetAllAsync();
            Assert.IsNotNull(chatRooms);
            Assert.IsTrue(chatRooms.Any(c =>
                                        c.ChatRoomId == baseChatRoom.ChatRoomId &&
                                        c.CreatorUserId == baseChatRoom.CreatorUserId &&
                                        c.Title == baseChatRoom.Title &&
                                        c.UsersCount == baseChatRoom.UsersCount)
                         );
        }

        [TestMethod]
        public async Task UpdateChatRoom()
        {
            var mockedChatRooms = await mockMemoryDatabase.CreateManyChatRoomAsync();
            var chatRoomId = mockedChatRooms.ToList()[0].ChatRoomId;

            var chatRoom = await unitOfWork.ChatRoomRepository.GetAsync(chatRoomId);

            Assert.IsNotNull(chatRoom);

            chatRoom.Title = "Updated ChatRoom";
            chatRoom.UsersCount = 1;


            await unitOfWork.ChatRoomRepository.UpdateAsync(chatRoom);

            chatRoom = await unitOfWork.ChatRoomRepository.GetAsync(chatRoomId);

            Assert.IsTrue(chatRoom.Title == "Updated ChatRoom");
            Assert.IsTrue(chatRoom.UsersCount == 1);

            baseChatRoom = chatRoom;
        }

        [TestMethod]
        public async Task GetChatRoomsAsync()
        {
            var mockedChatRooms = await mockMemoryDatabase.CreateManyChatRoomAsync();

            var chatRooms = await unitOfWork.ChatRoomRepository.GetAllAsync();
            Assert.IsNotNull(chatRooms);

            foreach (var chatRoom in mockedChatRooms)
            {
                Assert.IsTrue(chatRooms.Any(c =>
                                            c.ChatRoomId == chatRoom.ChatRoomId &&
                                            c.CreatorUserId == chatRoom.CreatorUserId &&
                                            c.Title == chatRoom.Title &&
                                            c.UsersCount == chatRoom.UsersCount)
                             );
            }
        }

        [TestMethod]
        public async Task GetChatRoomMessageBoard()
        {
            var chatRoomMessages = (await mockMemoryDatabase.ChatRoomMessageBoardMockedDataAsync()).ToList();

            var chatRoom = await unitOfWork.ChatRoomMessageRepository.GetAllAsync();
            chatRoom = chatRoom.Where(c => c.ChatRoomId == chatRoomMessages[0].ChatRoomId).ToList();

            Assert.IsNotNull(chatRoom);

            foreach (var message in chatRoom)
            {
                var hasMockedChatMessage = chatRoomMessages.Any(
                    m =>
                    m.ChatRoomMessageId == message.ChatRoomMessageId &&
                    m.ChatRoomId == message.ChatRoomId &&
                    m.PostDate.ToShortDateString() == message.PostDate.ToShortDateString() &&
                    m.Message == message.Message &&
                    m.UserId == message.UserId
                    );

                Assert.IsTrue(hasMockedChatMessage);
            }

        }


    }
}
