using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging
{
    public interface IMessageSink
    {
        void Enqueue(string destination, string payload);
    }
}
