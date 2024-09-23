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
        private SocketEndPoint? _endPoint;
        private SocketProtocolStack? _protocolStack;

        BaseServer? _server;

        public BaseClient(SocketProtocolStack protocolStack)
        {
            this._protocolStack = protocolStack;
            this._socket = new Socket(_protocolStack.Family,
                                      _protocolStack.Type,
                                      _protocolStack.Protocol);
        }

        public SocketEndPoint? EndPoint
        {
            get => _endPoint;
            set => _endPoint = value;
        }

        public void Connect()
        {
            _socket?.Connect(_endPoint!.Address!, _endPoint!.Port);
        }

        public void Connect(SocketEndPoint endPoint)
        {
            _endPoint = endPoint;

            _socket?.Connect(_endPoint!.Address!, _endPoint!.Port);
        }

        public async Task ConnectAsync()
        {
            await _socket!.ConnectAsync(_endPoint!.Address!, _endPoint!.Port);
        }

        public async Task ConnectAsync(SocketEndPoint endPoint)
        {
            _endPoint = endPoint;

            await _socket!.ConnectAsync(_endPoint!.Address!, _endPoint!.Port);
        }

        public void Disconnect()
        {
            _socket!.Disconnect(false);
        }

        public async Task DisconnectAsync()
        {
            await _socket!.DisconnectAsync(false);
        }
    }
}
