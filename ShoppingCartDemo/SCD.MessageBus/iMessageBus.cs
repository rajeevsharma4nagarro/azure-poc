using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCD.MessageBus
{
    public interface iMessageBus
    {
        Task PublishMessage(object message);
    }
}
