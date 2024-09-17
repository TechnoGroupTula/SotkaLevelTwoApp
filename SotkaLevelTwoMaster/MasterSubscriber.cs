using SotkaLevelTwoCore.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SotkaLevelTwoMaster
{
    public class MasterSubscriber
    {
        Socket socket;
        IPAddress address;
        int port;

        public MasterSubscriber()
        {
            socket = new(AddressFamily.InterNetwork, 
                         SocketType.Stream, 
                         ProtocolType.Tcp);
            address = IPAddress.Loopback;
            port = 30000;
        }

        public async Task Subscribe()
        {
            IPEndPoint point = new(address, port);
            socket.Bind(point);
            socket.Listen();

            while(true)
            {
                var remoteSlave = await socket.AcceptAsync();
            }
        }
    }
}
