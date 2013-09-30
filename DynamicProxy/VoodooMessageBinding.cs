using System;

namespace DynamicProxy
{
    public class VoodooMessageBinding
    {
        public MessageType        Type         { get; set; }

        public string             Payload      { get; set; }

        public VoodooProxyBinding ProxyBinding { get; set; }
    }

    public enum MessageType
    {
        RequestGetMember, RespondGetMember
    }
}