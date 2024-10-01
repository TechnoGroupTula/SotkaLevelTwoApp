using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SotkaLevelTwoCore.Base
{
    public class BaseSubscriber
    {
        private BaseClient _subsriber;

        private SocketEndPoint? _publisherEndPoint;
        private BaseServer? _publisher;

        public BaseSubscriber()
        {
            _subsriber = BaseClientFactory.DefaultClient();
        }
        public BaseSubscriber(SocketEndPoint publisherEndPoint) : this()
        {
            _publisherEndPoint = publisherEndPoint;
        }

        public bool Subscribe()
        {
            try
            {
                if (_publisherEndPoint is not null)
                    _subsriber.Connect(_publisherEndPoint);

            }
            catch(Exception ex)
            {
            }
            
            return true;
        }
    }

    
}
