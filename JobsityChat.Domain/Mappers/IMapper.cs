using System;
using System.Collections.Generic;
using System.Text;

namespace JobsityChat.Domain.Mappers
{

    public interface IMapper<in T, out TO>
    {
        TO MapFrom(T input);
    }
}
