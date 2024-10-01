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
        /// <summary>
        /// private fields
        /// </summary>
        private Socket? _socket;
        private SocketProtocolStack? _protocolStack;

        private SocketEndPoint? _serverEndPoint;

        private NetworkStream? _readerStream;
        private NetworkStream? _writerStream;

        public event ClientHandler? Connected;
        public event ClientHandler? Disconnected;

        /// <summary>
        /// constructors
        /// </summary>
        public BaseClient()
        {
        }

        public BaseClient(SocketProtocolStack protocolStack)
        {
            this._protocolStack = protocolStack;
            this._socket = new Socket(_protocolStack.Family,
                                      _protocolStack.Type,
                                      _protocolStack.Protocol);
            this._readerStream = new NetworkStream(this._socket);
            this._writerStream = new NetworkStream(this._socket);
        }

        /// <summary>
        /// properties
        /// </summary>
        public SocketEndPoint? ServerEndPoint
        {
            get => _serverEndPoint;
            set => _serverEndPoint = value;
        }

        public NetworkStream ReaderStream
        {

        }

        /// <summary>
        /// methods
        /// </summary>
        public void Connect()
        {
            _socket?.Connect(_serverEndPoint!.Address!, _serverEndPoint!.Port);
            Connected?.Invoke(this, new ClientEventArgs(this));
        }

        public void Connect(SocketEndPoint endPoint)
        {
            _serverEndPoint = endPoint;

            _socket?.Connect(_serverEndPoint!.Address!, _serverEndPoint!.Port);
            Connected?.Invoke(this, new ClientEventArgs(this));
        }

        public async Task ConnectAsync()
        {
            await _socket!.ConnectAsync(_serverEndPoint!.Address!, _serverEndPoint!.Port);
            Connected?.Invoke(this, new ClientEventArgs(this));
        }

        public async Task ConnectAsync(SocketEndPoint endPoint)
        {
            _serverEndPoint = endPoint;

            await _socket!.ConnectAsync(_serverEndPoint!.Address!, _serverEndPoint!.Port);
            Connected?.Invoke(this, new ClientEventArgs(this));
        }

        public void Disconnect()
        {
            _socket!.Disconnect(false);
            Disconnected?.Invoke(this, new ClientEventArgs(this));
        }

        public async Task DisconnectAsync()
        {
            await _socket!.DisconnectAsync(false);
            Disconnected?.Invoke(this, new ClientEventArgs(this));
        }

        public BaseClientFactory Factory()
        {
            return new BaseClientFactory();
        }
    }

    public class BaseClientFactory
    {
        public BaseClientFactory() { }

        public BaseClient Client(Func<SocketProtocolStackBuilder, SocketProtocolStack> stack)
        {
            return new BaseClient(stack(new SocketProtocolStackBuilder()));
        }

        static public BaseClient DefaultClient()
        {
            var defaultStack = new SocketProtocolStackBuilder()
                                    .SetFamily(AddressFamily.InterNetwork)
                                    .SetType(SocketType.Stream)
                                    .SetProtocol(ProtocolType.Tcp)
                                    .GetSocketProtocolStack();

            return new BaseClient(defaultStack);
        }
    }
}
