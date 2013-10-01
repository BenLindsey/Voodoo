using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Messaging;
using DynamicProxy;
using Newtonsoft.Json;

namespace Manager
{
    public class VoodooManager<T>
    {
        private readonly T m_UnderlyingObject;

        private readonly IMessageSink m_MessageSink;

        private readonly Dictionary<string, Action<VoodooProxy>> m_Paths = new Dictionary<string, Action<VoodooProxy>>(); 

        public VoodooManager(T underlyingObject, IMessageSink messageSink)
        {
            m_UnderlyingObject = underlyingObject;
            m_MessageSink = messageSink;
        }

        public void DefinePath<TTarget>(string pathName, Expression<Action<TTarget>> expression)
        {
            m_Paths.Add(pathName, expression.AllowProxyArgument());
        }

        public void StartPath(string pathName, string remoteAddress, string localAddress)
        {
            dynamic proxy = new VoodooProxy(remoteAddress, localAddress, pathName, m_MessageSink);

            try
            {
                m_Paths[pathName](proxy);
            }
            catch
            {
            }
        }

        public void HandleMessage(string payload)
        {
            var messageBinding = JsonConvert.DeserializeObject<VoodooMessageBinding>(payload);

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
                    throw new InvalidOperationException("Invalid payload type");
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
