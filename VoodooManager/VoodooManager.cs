using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Dynamifier;
using Messaging;
using DynamicProxy;
using Newtonsoft.Json;

namespace VoodooManager
{
    public class VoodooManager<T> : IMessageHandler
    {
        private readonly T m_UnderlyingObject;

        private readonly IMessageSink m_MessageSink;

        private readonly Dictionary<string, Action<dynamic>> m_Paths = new Dictionary<string, Action<dynamic>>(); 

        public VoodooManager(T underlyingObject, IMessageSink messageSink)
        {
            m_UnderlyingObject = underlyingObject;
            m_MessageSink = messageSink;
        }

        public void DefinePath<TTarget>(string pathName, Expression<TTarget> expression)
        {
            m_Paths.Add(pathName, expression.AllowDynamicArguments());
        }

        public void HandleMessage(string message)
        {
            var messageBinding = JsonConvert.DeserializeObject<VoodooMessageBinding>(message);

            switch (messageBinding.Type)
            {
                case MessageType.RequestGetMember:
                    typeof (T).GetField(messageBinding.Payload).GetValue(m_UnderlyingObject);
                    break;

                case MessageType.RespondGetMember:
                    messageBinding.ProxyBinding.KnownMemberValues.Enqueue(messageBinding.Payload);
                    ExecutePath(messageBinding);
                    break;

                default:
                    throw new InvalidOperationException("Invalid message type");
            }
        }

        private void ExecutePath(VoodooMessageBinding messageBinding)
        {
            dynamic proxy = new VoodooProxy(messageBinding.ProxyBinding, m_MessageSink);

            try
            {
                m_Paths[messageBinding.ProxyBinding.PathName](proxy);
            }
            catch
            {
            }
        }
    }
}
