using System;
using System.Collections.Generic;
using System.Text;

namespace JobsityChat.Data.Entities
{
    public class UserEntity
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
