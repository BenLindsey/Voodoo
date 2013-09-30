using System.Collections.Generic;

namespace DynamicProxy
{
    /// <summary>
    /// A serializable binding used to maintain a proxy's state over continuations.
    /// </summary>
    public class VoodooProxyBinding
    {
        public string        RemoteAddress { get; set; }

        public string        ProxyAddress  { get; set; }

        public string        PathName      { get; set; }

        public Queue<string> KnownMemberValues { get; set; }
    }
}
