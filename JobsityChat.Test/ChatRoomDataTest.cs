using JobsityChat.Data.Mappers;
using JobsityChat.Data.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JobsityChat.Test
{
    [TestClass]
    public class ChatRoomDataTest
    {
        private Domain.Repository.IRepository repository;

        public ChatRoomDataTest()
        {
            var mockedApi = new Mock<Data.Api.IApi>();
            mockedApi.Setup(a => a.GetChatRoomsData()).Returns(TestBase.ChatRoomMockedData);
            repository = new JobsityRepository(mockedApi.Object, new ChatRoomEntityMapper());
        }

        [TestMethod]
        public void GetChatRooms()
        {
            var chatRooms = repository.GetChatRooms();
            Assert.IsNotNull(chatRooms);

            foreach (var chatRoom in chatRooms)
            {
                var mockedChatRoom = TestBase.ChatRoomMockedData.FirstOrDefault(c => c.ChatRoomId == chatRoom.ChatRoomId);
                Assert.IsNotNull(mockedChatRoom);

                Assert.AreEqual(chatRoom.ChatRoomId, mockedChatRoom.ChatRoomId);
                Assert.AreEqual(chatRoom.Title, mockedChatRoom.Title);
                Assert.AreEqual(chatRoom.UsersCount, mockedChatRoom.UsersCount);

            }
        }


    }
}
