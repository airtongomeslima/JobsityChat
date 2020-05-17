using JobsityChat.Domain.Api;
using JobsityChat.Data.UnitOfWork;
using JobsityChat.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using JobsityChat.Domain.Mappers;
using JobsityChat.Domain.UnitOfWork;
using JobsityChat.MessageBroker;
using System.Text.RegularExpressions;

namespace JobsityChat.Data.Api
{
    public class ChatRoomApi : IChatRoomApi
    {
        protected List<Message> botMessageList = new List<Message>();
        protected IUnitOfWork uow;
        protected static IMessageBroker messageBroker;

        public ChatRoomApi(string connectionString, string mqHostName, string mqPort, string mqUserName, string mqPassword)
        {
            uow = new UnitOfWork.UnitOfWork(connectionString);
            messageBroker = new MessageBroker.MessageBroker(mqHostName, mqPort, mqUserName, mqPassword);
            messageBroker.Subscribe("JobsityChatServiceQuotes", "quote", (string message) =>
            {
                if(message.Contains("\t"))
                {
                    var messageContent = message.Split('\t');
                    int.TryParse(messageContent[1], out var chatRoomId);

                    botMessageList.Add(new Message
                    {
                        ChatRoomId = chatRoomId,
                        PostDate = DateTime.Now,
                        Text = messageContent[0],
                        UserId = "-1",
                        UserName = "ChatBot"
                    });
                }
                
            });
        }
        public async Task<IEnumerable<Message>> GetChatRoomMessageBoardAsync(int chatRoomId)
        {
            var repositoryList = await uow.ChatRoomMessageRepository.GetAllAsync();
            var dbList = repositoryList.Where(c => c.ChatRoomId == chatRoomId).ToList();
            var mapper = new ChatRoomMessageEntityMapper();
            var result = mapper.MapFrom(dbList).ToList();
            result.AddRange(botMessageList.Where(c => c.ChatRoomId == chatRoomId).ToList());

            return result.OrderBy(c => c.PostDate).Take(50);
        }

        public async Task<IEnumerable<ChatRoom>> GetChatRooms()
        {
            var list = await uow.ChatRoomRepository.GetAllAsync();
            var mapper = new ChatRoomEntityMapper();

            return mapper.MapFrom(list);
        }

        public async Task<IEnumerable<Message>> SendMessageAsync(int chatRoomId, string userId, string message)
        {
            if (message.ToLower().Contains("/stock="))
            {
                var pattern = @"(?<=\/stock=).*(?=>$|\s)";

                Regex regex = new Regex(pattern);

                Match match = regex.Match(message);

                if (match.Success)
                {
                    messageBroker.SendMessage("JobsityChatService", "quote", match.Value.ToLower().Replace(" ", ""));
                }
            }

            await uow.ChatRoomMessageRepository.InsertAsync(new ChatRoomMessageEntity
            {
                ChatRoomId = chatRoomId,
                Message = message,
                UserId = userId,
                PostDate = DateTime.Now
            });

            return await GetChatRoomMessageBoardAsync(chatRoomId);
        }
    }
}
