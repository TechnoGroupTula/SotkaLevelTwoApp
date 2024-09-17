using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SotkaLevelTwoCore.Base
{
    public class BaseAddress
    {
        public IPAddress IPAddress { get; set; }
        public int Port { get; set; }
        public IPEndPoint IPAddressEndPoint { get; set; }

        public BaseAddress() : this(IPAddress.None, 0) { }

        public BaseAddress(IPAddress iPAddress, int port)
        {
            IPAddress = iPAddress;
            Port = port;

            IPAddressEndPoint = new IPEndPoint(IPAddress, Port);
        }
    }

    class AddressBuilder
    {
        private BaseAddress _address;
        public AddressBuilder()
        {
            _address = new BaseAddress();
        }

        public AddressBuilder SetIp
    }
}
