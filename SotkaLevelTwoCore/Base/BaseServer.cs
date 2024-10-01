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
        #region Private fields
        /// <summary>
        /// base data for server
        /// </summary>
        private Socket? _socket;
        private bool _active;
        private SocketProtocolStack? _protocolStack;
        private SocketEndPoint? _endPoint;

        /// <summary>
        /// read/write streams for server
        /// </summary>
        private NetworkStream? _readerStream;
        private NetworkStream? _writerStream;

        /// <summary>
        /// 
        /// </summary>
        private volatile CancellationTokenSource? _cancellationTokenSource;
        #endregion

        /// <summary>
        /// events for server
        /// </summary>
        public event ServerHandler? Started;
        public event ServerHandler? Stoped;
        public event ServerHandler? ClientAcceped;

        //public event ExceptionHandler? SocketError;

        /// <summary>
        /// constructors
        /// </summary>
        /// <param name="protocolStack"></param>
        public BaseServer(SocketProtocolStack protocolStack)
        {
            this._protocolStack = protocolStack;
            this._active = false;
            this._cancellationTokenSource = new CancellationTokenSource();

            try
            {
                this._socket = new Socket(_protocolStack.Family,
                                      _protocolStack.Type,
                                      _protocolStack.Protocol);

                this._readerStream = new NetworkStream(this._socket);
                this._writerStream = new NetworkStream(this._socket);
                
            }
            catch(Exception)
            {
                //SocketError?.Invoke(this, new ExceptionEventArgs(ex));
                throw new BaseServerException("Error of server instancing");
            }
            finally
            {

            }
        }

        public SocketEndPoint? EndPoint
        {
            get => _endPoint;
            set => _endPoint = value;
        }
        public bool Active
        {
            get => this._active;
        }

        public void Start()
        {
            try
            {
                _socket?.Bind(_endPoint?.Point!);
                _socket?.Listen();
                _active = true;
                Started?.Invoke(this, new ServerEventArgs(this));
            }
            catch(Exception)
            {
                throw new BaseServerException("Error of server starting");
            }
        }

        public void Start(SocketEndPoint endPoint)
        {
            _endPoint = endPoint;
            this.Start();
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

        static public BaseServer DefaultServer()
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
