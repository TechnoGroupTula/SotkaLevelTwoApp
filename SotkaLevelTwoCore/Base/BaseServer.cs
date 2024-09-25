using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;

namespace SotkaLevelTwoCore.Base
{
    /// <summary>
    /// Base Listener 
    ///     для Master - издатель для Master
    ///     для Slave  - для получения запросов от Master
    ///     
    ///     Socket - сокет Listener
    ///     Active - флаг активности сервера
    ///     
    /// </summary>
    public class BaseServer
    {
        private Socket? _socket;
        private bool _active;
        private SocketProtocolStack? _protocolStack;
        private SocketEndPoint? _endPoint;
        private volatile CancellationTokenSource? _cancellationTokenSource;

        public event ServerHandler? Started;
        public event ServerHandler? Stoped;
        public event ServerHandler? ClientAcceped;

        public BaseServer(SocketProtocolStack protocolStack)
        {
            this._protocolStack = protocolStack;
            this._active = false;
            this._cancellationTokenSource = new CancellationTokenSource();

            this._socket = new Socket(_protocolStack.Family,
                                      _protocolStack.Type,
                                      _protocolStack.Protocol);
        }

        public SocketEndPoint? EndPoint
        {
            get => _endPoint;
            set => _endPoint = value;
        }

        public void Start()
        {
            _socket?.Bind(_endPoint?.Point!);
            _socket?.Listen();
            Started?.Invoke(this, new ServerEventArgs(this));
        }

        public void Start(SocketEndPoint endPoint)
        {
            _endPoint = endPoint;

            _socket?.Bind(_endPoint?.Point!);
            _socket?.Listen();
            Started?.Invoke(this, new ServerEventArgs(this));
        }

        public BaseClient Accept()
        {
            Socket? _client = _socket?.Accept();
            ClientAcceped?.Invoke(this, new ServerEventArgs(this));

            return new BaseClient();
        }

        public async Task<BaseClient> AcceptAsyc()
        {
            Socket? _client = await _socket?.AcceptAsync()!;
            ClientAcceped?.Invoke(this, new ServerEventArgs(this));

            return new BaseClient();
        }

        public void Stop()
        {
            _socket?.Close();
            Stoped?.Invoke(this, new ServerEventArgs(this));
        }

        public static BaseServerFactory Factory()
        {
            return new BaseServerFactory();
        }
    }

    public class BaseServerFactory
    {
        public BaseServerFactory() { }

        public BaseServer Server(Func<SocketProtocolStackBuilder, SocketProtocolStack> stack)
        {
            return new BaseServer(stack(new SocketProtocolStackBuilder()));
        }

        public BaseServer DefaultServer()
        {
            var defaultStack = new SocketProtocolStackBuilder()
                                    .SetFamily(AddressFamily.InterNetwork)
                                    .SetType(SocketType.Stream)
                                    .SetProtocol(ProtocolType.Tcp)
                                    .GetSocketProtocolStack();
            
            return new BaseServer(defaultStack);
        }

    }
}
