using JobsityChat.Domain.Entities;
using JobsityChat.Domain.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JobsityChat.Domain.Mappers
{
    public class ChatRoomEntityMapper : IMapper<IEnumerable<ChatRoom>, IEnumerable<ChatRoomEntity>>
    {
        public IEnumerable<ChatRoomEntity> MapFrom(IEnumerable<ChatRoom> input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            return input.Select(c => new ChatRoomEntity
            {
                ChatRoomId = c.ChatRoomId,
                Title = c.Title,
                UsersCount = c.UsersCount
            });
        }
    }
}
