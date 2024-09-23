using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SotkaLevelTwoCore.Base
{
    public class BaseClient
    {
        private Socket? _socket;
        private SocketEndPoint? _endPopint;
        private SocketProtocolStack? _protocolStack;

        BaseServer? _server;
    }
}
