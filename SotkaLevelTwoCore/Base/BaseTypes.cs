using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SotkaLevelTwoCore.Base
{
    #region Handlers for Base Server and Client
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ServerHandler(object? sender, ServerEventArgs e);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ClientHandler(object? sender, ClientEventArgs e);
    #endregion


    public delegate void ExceptionHandler(object? sender, ExceptionEventArgs e);

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
        public EndPoint? EndPoint => client.ServerEndPoint?.Point;
        public IPAddress? IPAddress => client.ServerEndPoint?.Address;
        public int? Port => client.ServerEndPoint?.Port;
        
    }
    public class ExceptionEventArgs
    {
        Exception exception;
        public ExceptionEventArgs(Exception exception) => this.exception = exception;
        public Exception Exception => exception;
        public string Message => exception.Message;
    }
    public class BaseServerException : Exception
    {
        public BaseServerException() : base("Undefined error with server") { }
        public BaseServerException(string message) : base(message) { }

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
