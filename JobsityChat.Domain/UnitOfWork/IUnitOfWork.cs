using JobsityChat.Domain.Entities;
using JobsityChat.Domain.Repository;
using System.Collections.Generic;

namespace JobsityChat.Domain.UnitOfWork
{
    public interface IUnitOfWork
    {
        IGenericRepository<ChatRoomEntity> ChatRoomRepository { get; }
        IGenericRepository<ChatRoomMessageEntity> ChatRoomMessageRepository { get; }
        IGenericRepository<AspNetUser> UsersRepository { get; }
    }
}
