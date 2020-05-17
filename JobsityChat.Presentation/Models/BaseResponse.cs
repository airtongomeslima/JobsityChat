using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobsityChat.Presentation.Models
{
    public class BaseResponseModel<T>
    {
        public T Response { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
