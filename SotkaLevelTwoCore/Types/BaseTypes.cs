using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SotkaLevelTwoCore.Base;

namespace SotkaLevelTwoCore.Types
{
    #region Handlers for Base Server and Client
    /// <summary>
    /// Делегат обработчиков событий сервера
    /// </summary>
    /// <param name="sender">
    /// Источник события
    /// </param>
    /// <param name="e">
    /// Аргументы события
    /// </param>
    public delegate void ServerHandler(object? sender, ServerEventArgs e);

    /// <summary>
    /// Делегат обработчиков событий клиента
    /// </summary>
    /// <param name="sender">
    /// Источник события
    /// </param>
    /// <param name="e">
    /// Аргументы события
    /// </param>
    public delegate void ClientHandler(object? sender, ClientEventArgs e);

    /// <summary>
    /// Делегат обработчиков событий исключений
    /// </summary>
    /// <param name="sender">
    /// Источник события
    /// </param>
    /// <param name="e">
    /// Аргументы события
    /// </param>
    public delegate void ExceptionHandler(object? sender, ExceptionEventArgs e);
    #endregion

    #region Arguments of Event Classes
    /// <summary>
    /// ServerEventArgs - набор аргументов для событий сервера
    ///     Server - ссылка на сервер источник события
    ///     EndPoint - адрес сервера
    /// </summary>
    public class ServerEventArgs
    {
        BaseServer server;
        public ServerEventArgs(BaseServer server) => this.server = server;

        public BaseServer Server => server;
        public EndPoint? EndPoint => server.EndPoint?.Point;
        public IPAddress? IPAddress => server.EndPoint?.Address;
        public int? Port => server.EndPoint?.Port;

    }

    /// <summary>
    /// ClientEventArgs - набор аргументов для событий клиента
    ///     Server - ссылка на клиент источник события
    ///     EndPoint - адрес клиента
    /// </summary>
    public class ClientEventArgs
    {
        BaseClient client;
        public ClientEventArgs(BaseClient client) => this.client = client;
        public EndPoint? EndPoint => client.ServerEndPoint?.Point;
        public IPAddress? IPAddress => client.ServerEndPoint?.Address;
        public int? Port => client.ServerEndPoint?.Port;

    }

    /// <summary>
    /// 
    /// </summary>
    public class ExceptionEventArgs
    {
        Exception exception;
        public ExceptionEventArgs(Exception exception) => this.exception = exception;
        public Exception Exception => exception;
        public string Message => exception.Message;
    }
    #endregion

    #region Exceptions
    public class BaseServerException : Exception
    {
        public BaseServerException() : base("Undefined error with server") { }
        public BaseServerException(string message) : base(message) { }

    }
    public class BaseClientException : Exception
    {
        public BaseClientException() : base("Undefined error with client") { }
        public BaseClientException(string message) : base(message) { }

    }
    #endregion

    public struct Port
    {
        public ushort Value { set; get; }

        public static implicit operator ushort(Port port)
        {
            return port.Value;
        }

        public static implicit operator Port(ushort value)
        {
            return new Port() { Value = value };
        }
    }
}
