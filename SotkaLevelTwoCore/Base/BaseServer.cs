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
    public abstract class BaseServer
    {
        BaseAddress? _address;
        AddressBuilder? _addressBuilder;
        
        AddressFamily _family = AddressFamily.Unknown;
        SocketType _type = SocketType.Unknown;
        ProtocolType _protocol = ProtocolType.Unknown;

        Socket? _socket = null;

        public BaseServer(BaseAddress? address = null)
        {
            this._address = address;
        }

        
    }

    /// <summary>
    /// OPC DA (Data Access)
    /// </summary>
    public class DaServer : BaseServer
    {

    }

    /// <summary>
    /// OPC HDA (Historical Data Access)
    /// </summary>
    public class HdaServer : BaseServer
    {

    }
    /// <summary>
    /// OPC AE (Alarms & Events)
    /// </summary>
    public class AeServer : BaseServer
    {

    }
}
