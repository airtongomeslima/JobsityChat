using JobsityChat.Data.Entities;
using JobsityChat.Domain.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JobsityChat.Data.Mappers
{
    public class ChatRoomMessageEntityMapper : IMapper<IEnumerable<Message>, IEnumerable<ChatRoomMessageEntity>>
    {
        public IEnumerable<ChatRoomMessageEntity> MapFrom(IEnumerable<Message> input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            return input.Select(c => new ChatRoomMessageEntity
            {
                UserId = c.UserId,
                ChatRoomId = c.ChatRoomId,
                Message = c.Text,
                PostDate = c.PostDate
            });
        }
    }
}
