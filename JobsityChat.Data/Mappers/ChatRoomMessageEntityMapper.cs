using JobsityChat.Domain.Entities;
using JobsityChat.Domain.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JobsityChat.Domain.Mappers
{
    public class ChatRoomMessageEntityMapper : IMapper<IEnumerable<ChatRoomMessageEntity>, IEnumerable<Message>>
    {
        public IEnumerable<Message> MapFrom(IEnumerable<ChatRoomMessageEntity> input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            return input.Select(c => new Message
            {
                UserId = c.UserId,
                ChatRoomId = c.ChatRoomId,
                Text = c.Message,
                PostDate = c.PostDate
            });
        }
    }
}
