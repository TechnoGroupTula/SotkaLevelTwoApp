using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SotkaLevelTwoCore.Base
{
    public delegate void ServerHandler(object? sender, ServerEventArgs e);
    public delegate void ClientHandler(object? sender, ClientEventArgs e);

    public class ServerEventArgs
    {
        BaseServer server;
        public ServerEventArgs(BaseServer server) => this.server = server;

        public BaseServer Server => this.server;
        public EndPoint? EndPoint => server.EndPoint?.Point;
        public IPAddress? IPAddress => server.EndPoint?.Address;
        public int? Port => server.EndPoint?.Port;
        
    }

    public class ClientEventArgs
    {
        BaseClient client;
        public ClientEventArgs(BaseClient client) => this.client = client;
        public EndPoint? EndPoint => client.EndPoint?.Point;
        public IPAddress? IPAddress => client.EndPoint?.Address;
        public int? Port => client.EndPoint?.Port;
        
    }

    public struct Port
    {
        public int Value { set; get; }

        public static implicit operator int(Port port)
        {
            return port.Value;
        }

        public static implicit operator Port(int value)
        {
            return new Port() { Value = value };
        }
    }
}
