using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SotkaLevelTwoCore.Types
{
    #region Protocol Stack for Socket
    /// <summary>
    /// Configuration protocols for socket
    /// </summary>
    public class SocketProtocolStack
    {
        private AddressFamily _family;
        private SocketType _type;
        private ProtocolType _protocol;

        public AddressFamily Family
        {
            set => _family = value;
            get => _family;
        }

        public SocketType Type
        {
            set => _type = value;
            get => _type;
        }

        public ProtocolType Protocol
        {
            set => _protocol = value;
            get => _protocol;
        }

        public SocketProtocolStackBuilder GetBuilder()
        {
            return new SocketProtocolStackBuilder();
        }
    }
    /// <summary>
    /// Builder for class protocols stack 
    /// </summary>
    public class SocketProtocolStackBuilder
    {
        private SocketProtocolStack _address = null!;

        public SocketProtocolStackBuilder()
        {
            _address = new SocketProtocolStack();
        }

        public SocketProtocolStackBuilder SetFamily(AddressFamily family)
        {
            _address.Family = family;
            return this;
        }

        public SocketProtocolStackBuilder SetType(SocketType type)
        {
            _address.Type = type;
            return this;
        }

        public SocketProtocolStackBuilder SetProtocol(ProtocolType protocol)
        {
            _address.Protocol = protocol;
            return this;
        }

        public SocketProtocolStack GetSocketProtocolStack()
        {
            return _address;
        }

    }
    #endregion

    #region End Point for Binding
    /// <summary>
    /// Configuration for end point
    /// </summary>
    public class SocketEndPoint
    {
        private EndPoint? _endPoint;
        private IPAddress? _ipAddress;
        private Port _port;

        public EndPoint? Point
        {
            get => _endPoint;
            set => _endPoint = value;
        }

        public IPAddress? Address
        {
            get => _ipAddress;
            set => _ipAddress = value;
        }

        public Port Port
        {
            get => _port;
            set => _port = value;
        }
        public SocketEndPointBuilder Builder() => new SocketEndPointBuilder();

    }

    /// <summary>
    /// Builder for class end point
    /// </summary>
    public class SocketEndPointBuilder
    {
        private SocketEndPoint _socketEndPoint = null!;
        public SocketEndPointBuilder()
        {
            _socketEndPoint = new SocketEndPoint();
        }
        public SocketEndPointBuilder SetAddress(IPAddress address)
        {
            _socketEndPoint.Address = address;
            return this;
        }
        public SocketEndPointBuilder SetAddress(string address)
        {
            _socketEndPoint.Address = IPAddress.Parse(address);
            return this;
        }
        public SocketEndPointBuilder SetPort(Port port)
        {
            _socketEndPoint.Port = port;
            return this;
        }
        public SocketEndPoint GetSocketEndPoint()
        {
            if (_socketEndPoint.Address is not null && _socketEndPoint.Port != 0)
                _socketEndPoint.Point = new IPEndPoint(_socketEndPoint.Address, _socketEndPoint.Port);
            else
                throw new NullReferenceException();

            return _socketEndPoint;
        }

    }
    #endregion
}
