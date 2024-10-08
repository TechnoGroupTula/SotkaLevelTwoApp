using SotkaLevelTwoCore.Base;
using SotkaLevelTwoCore.Types;
using System.Net;
using System.Net.Sockets;

namespace SotkaLevelTwoMaster
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SocketEndPointBuilder builder = new SocketEndPointBuilder();
            SocketEndPoint socketEndPoint = builder.SetAddress(IPAddress.Loopback)
                                                   .SetPort(30000)
                                                   .GetSocketEndPoint();

            SocketProtocolStackBuilder stackBuilder = new SocketProtocolStackBuilder();
            SocketProtocolStack socketProtocolStack = stackBuilder.SetFamily(AddressFamily.InterNetwork)
                                                                  .SetType(SocketType.Stream)
                                                                  .SetProtocol(ProtocolType.Tcp)
                                                                  .GetSocketProtocolStack();

            BaseClient client = new BaseClient(socketProtocolStack);
            client.Connect(socketEndPoint);
        }
    }
}
