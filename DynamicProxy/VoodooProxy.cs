using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Messaging;

using Newtonsoft.Json;

namespace DynamicProxy
{
    public class VoodooProxy : DynamicObject
    {
        private readonly string m_RemoteAddress;

        private readonly string m_ProxyAddress;

        private readonly string m_PathName;

        private readonly IMessageSink m_MessageSink;

        private readonly Queue<string> m_KnownMemberValues;

        public VoodooProxy(string remoteAddress, string proxyAddress, string pathName, IMessageSink messageSink)
        {
            m_RemoteAddress     = remoteAddress;

            m_ProxyAddress      = proxyAddress;

            m_PathName          = pathName;

            m_KnownMemberValues = new Queue<string>();

            m_MessageSink       = messageSink;
        }

        public VoodooProxy(VoodooProxyBinding binding, IMessageSink messageSink)
        {
            m_RemoteAddress     = binding.RemoteAddress;

            m_ProxyAddress      = binding.ProxyAddress;

            m_PathName          = binding.PathName;

            m_KnownMemberValues = binding.KnownMemberValues;

            m_MessageSink       = messageSink;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (m_KnownMemberValues.Count > 0)
            {
                result = JsonConvert.DeserializeObject(m_KnownMemberValues.Dequeue(), binder.ReturnType);
                return true;
            }
            else
            {
                Request(MessageType.RequestGetMember, binder.Name);
                result = null;
                return false;
            }
        }

        private void Request(MessageType type, string request)
        {
            m_MessageSink.Enqueue(m_RemoteAddress, SerializeRequest(type, request));
        }

        private string SerializeRequest(MessageType type, string request)
        {
            return JsonConvert.SerializeObject(new VoodooMessageBinding
            {
                Type         = type,

                Payload      = request,
                       
                ProxyBinding = new VoodooProxyBinding
                {
                    RemoteAddress = m_RemoteAddress,

                    ProxyAddress = m_ProxyAddress,

                    PathName = m_PathName,

                    KnownMemberValues = m_KnownMemberValues
                }
            });
        }
    }
}
