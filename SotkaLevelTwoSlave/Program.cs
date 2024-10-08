using SotkaLevelTwoCore.Base;
using System.Net.Sockets;
using System.Net;
using SotkaLevelTwoCore.Types;

namespace SotkaLevelTwoSlave
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SocketEndPointBuilder builder = new SocketEndPointBuilder();
            SocketEndPoint socketEndPoint = builder.SetAddress(IPAddress.Any)
                                                   .SetPort(30000)
                                                   .GetSocketEndPoint();

            SocketProtocolStackBuilder stackBuilder = new SocketProtocolStackBuilder();
            SocketProtocolStack socketProtocolStack = stackBuilder.SetFamily(AddressFamily.InterNetwork)
                                                                  .SetType(SocketType.Stream)
                                                                  .SetProtocol(ProtocolType.Tcp)
                                                                  .GetSocketProtocolStack();

            BaseServer server = new BaseServer(socketProtocolStack);
            Console.WriteLine("Server start");
            server.Start(socketEndPoint);

            var client = server.Accept();

            server.Stop();
            Console.WriteLine("Server stop");

            //BaseServerFactory factory = BaseServer.Factory();

            //BaseServer baseServer = factory.Server( 
            //    b => b.SetFamily(AddressFamily.InterNetwork)
            //          .SetProtocol(ProtocolType.Tcp)
            //          .SetType(SocketType.Stream)
            //          .GetSocketProtocolStack()
            //);

        }
    }
}
