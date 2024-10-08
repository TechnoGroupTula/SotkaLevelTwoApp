using SotkaLevelTwoCore.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SotkaLevelTwoCore.Base
{
    public class BasePublisher
    {
        private BaseServer _server;
        private IList<BaseSubscriber> _subscribers = new List<BaseSubscriber>();
        public BasePublisher() 
        {
            _server = BaseServerFactory.DefaultServer();
        }

        public BasePublisher(BaseServer server)
        {
            _server = server;
        }

        public BasePublisher(SocketEndPoint endpoint) : this()
        {
            _server.EndPoint = endpoint;
        }

        public BasePublisher(BaseServer server, SocketEndPoint endpoint)
        {
            _server = server;
            _server.EndPoint = endpoint;
        }

        public async Task StartSubscription()
        {
            try
            {
                _server.Start();
                while (true)
                {
                    BaseClient client = await _server.AcceptAsyc();
                    BaseSubscriber subscriber = new BaseSubscriber(client, this);

                    _subscribers.Add(subscriber);
                }
            }
            catch (Exception ex)
            { }
            finally 
            { }
        }

    }
}
