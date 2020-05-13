using System;
using System.Collections.Generic;
using System.Text;

namespace JobsityChat.Domain.UseCases
{
    internal interface IRequestHandler<in TRequest, out TResponse>
    {
        TResponse Handle(TRequest data);
    }
}
