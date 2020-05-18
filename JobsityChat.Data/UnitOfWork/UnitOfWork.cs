using JobsityChat.Data.Repository;
using JobsityChat.Domain.Entities;
using JobsityChat.Domain.Mappers;
using JobsityChat.Domain.Repository;
using JobsityChat.Domain.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobsityChat.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly string connectionString;
        private IGenericRepository<ChatRoomEntity> chatRoomRepository;
        private IGenericRepository<ChatRoomMessageEntity> chatRoomMessageRepository;
        private IGenericRepository<AspNetUser> usersRepository;

        public UnitOfWork(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IGenericRepository<ChatRoomEntity> ChatRoomRepository
        {
            get
            {
                if (chatRoomRepository == null)
                {
                    chatRoomRepository = new GenericRepository<ChatRoomEntity>("ChatRoom", connectionString);
                }

                return chatRoomRepository;
            }
        }

        public IGenericRepository<ChatRoomMessageEntity> ChatRoomMessageRepository
        {
            get
            {
                if (chatRoomMessageRepository == null)
                {
                    chatRoomMessageRepository = new GenericRepository<ChatRoomMessageEntity>("ChatRoomMessage", connectionString);
                }

                return chatRoomMessageRepository;
            }
        }

        public IGenericRepository<AspNetUser> UsersRepository
        {
            get
            {
                if (usersRepository == null)
                {
                    usersRepository = new GenericRepository<AspNetUser>("AspNetUser", connectionString);
                }

                return usersRepository;
            }
        }
    }
}
