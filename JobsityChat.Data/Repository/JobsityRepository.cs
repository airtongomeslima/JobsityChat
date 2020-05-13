using JobsityChat.Data.Api;
using JobsityChat.Data.Entities;
using JobsityChat.Domain.Mappers;
using JobsityChat.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobsityChat.Data.Repository
{
    public class JobsityRepository : IRepository
    {
        private readonly IApi api;
        private readonly IMapper<IEnumerable<ChatRoom>, IEnumerable<ChatRoomEntity>> chatRoomMapper;
        private readonly IMapper<IEnumerable<Message>, IEnumerable<ChatRoomMessageEntity>> chatRoomMessageMapper;

        public JobsityRepository(IApi api, IMapper<IEnumerable<ChatRoom>, IEnumerable<ChatRoomEntity>> mapper)
        {
            this.api = api ?? throw new ArgumentNullException(nameof(api));
            this.chatRoomMapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public IEnumerable<ChatRoomEntity> GetChatRooms()
        {

            return chatRoomMapper.MapFrom(api.GetChatRoomsData());
        }

        public IEnumerable<ChatRoomMessageEntity> GetChatRoom(int chatRoomId)
        {
            if (chatRoomId == 0)
                throw new ArgumentNullException(nameof(chatRoomId));

            return chatRoomMessageMapper.MapFrom(api.GetChatRoomData(chatRoomId));
        }
    }
}
