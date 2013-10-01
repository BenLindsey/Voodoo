using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Manager;
using Messaging;

namespace Demo
{
    class DemoSink : IMessageSink
    {
        public VoodooManager<DemoCat> Manager;

        public void Enqueue(string destination, string payload)
        {
            Manager.HandleMessage(payload);
        }
    }
}
