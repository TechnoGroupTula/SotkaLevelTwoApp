using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SotkaLevelTwoCore.Base
{
    public class BasePublisher : BaseClient
    {
        public BasePublisher(SocketProtocolStack protocolStack) : base(protocolStack)
        {
        }
    }

    public class BaseSubscriber : BaseServer
    {
        public BaseSubscriber(SocketProtocolStack protocolStack) : base(protocolStack)
        {
        }
    }
}
