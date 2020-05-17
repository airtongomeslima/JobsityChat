using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobsityChat.Presentation.Models
{
    public class UserInformationModel
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public bool HasRegistered { get; set; }
        public string UserId { get; set; }
        public object LoginProvider { get; set; }
    }
}
