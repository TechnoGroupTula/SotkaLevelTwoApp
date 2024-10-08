using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using SotkaLevelTwoCore.Types;

namespace SotkaLevelTwoCore.Base
{
    public class BaseSubscriber
    {
        private BaseClient _client;

        private SocketEndPoint? _publisherEndPoint;
        private BasePublisher? _publisher;

        private IPAddress _localIpAddress;

        public BaseSubscriber()
        {
            _client = BaseClientFactory.DefaultClient();
        }
        public BaseSubscriber(SocketEndPoint publisherEndPoint) : this()
        {
            _publisher = new BasePublisher(publisherEndPoint);
        }
        public BaseSubscriber(BaseClient client, BasePublisher publisher)
        {
            _client = client;
            _publisher = publisher;
        }

        public bool Subscribe()
        {
            try
            {
                if (_publisherEndPoint is not null)
                    _client.Connect(_publisherEndPoint);

            }
            catch(Exception ex)
            {
            }
            
            return true;
        }
    }

    
}
