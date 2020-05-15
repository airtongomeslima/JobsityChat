using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace JobsityChat.Domain.Entities
{
    public class UserEntity
    {
        [Description("id")]
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
