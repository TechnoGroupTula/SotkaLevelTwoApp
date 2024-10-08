using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using SotkaLevelTwoCore.Types;

namespace SotkaLevelTwoCore.Base
{
    #region BaseServer Class
    /// <summary>
    /// Base Server - базовый класс сервера
    /// </summary>
    /// <remarks>
    /// для Master - Publisher  - принять подписку от Slave
    /// для Slave  - DataServer - для получения запросов от Master
    /// </remarks>
    public class BaseServer
    {

        #region Private fields
        /// <summary>
        /// базовые поля
        /// </summary>
        /// <param name="_socket">Сокет для сервера</param>
        /// <param name="_active">Сервер запущен или нет</param>
        /// <param name="_protocolStack">Стек протоколов для сервера</param>
        /// <param name="_endPoint">Адрес и порт привязки сервера</param>
        private Socket? _socket;
        private bool _active;
        private SocketProtocolStack? _protocolStack;
        private SocketEndPoint? _endPoint;

        /// <summary>
        /// read/write потоки данных для сервера
        /// </summary>
        private NetworkStream? _readerStream;
        private NetworkStream? _writerStream;

        /// <summary>
        /// токен для остановки
        /// </summary>
        private volatile CancellationTokenSource? _cancellationTokenSource;
        private CancellationToken? _token;
        #endregion

        #region Events
        /// <summary>
        /// события для сервера
        /// </summary>
        public event ServerHandler? Started;
        public event ServerHandler? Stoped;
        public event ServerHandler? Canceled;
        public event ServerHandler? ClientAcceped;

        //public event ExceptionHandler? SocketError;
        #endregion

        #region Constructors
        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="protocolStack">
        /// входящий набор протоколов для сокета
        /// </param>
        /// <exception cref="BaseServerException">
        /// Ошибка создания объекта сервера
        /// </exception>
        public BaseServer(SocketProtocolStack protocolStack)
        {
            this._protocolStack = protocolStack;
            this._active = false;
            this._cancellationTokenSource = new CancellationTokenSource();
            this._token = _cancellationTokenSource.Token;

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
        #endregion

        #region Properties
        public SocketEndPoint? EndPoint
        {
            get => _endPoint;
            set => _endPoint = value;
        }
        public bool Active
        {
            get => this._active;
        }

        public CancellationTokenSource? CancellationTokenSource
        {
            set
            {
                if(value is not null)
                {
                    _cancellationTokenSource = value;
                    _token = _cancellationTokenSource.Token;
                }
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Старт сервера
        /// </summary>
        /// <exception cref="BaseServerException">
        /// Выброс исключения, при ошибке подключения
        /// </exception>
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

        /// <summary>
        /// Старт сервера
        /// </summary>
        /// <param name="endPoint">
        /// Адрес привязки сервера
        /// </param>
        /// /// <exception cref="BaseServerException">
        /// Выброс исключения, при ошибке подключения
        /// </exception>
        public void Start(SocketEndPoint endPoint)
        {
            _endPoint = endPoint;
            this.Start();
        }

        /// <summary>
        /// Подключение клиента
        /// </summary>
        /// <returns>
        /// Возвращение подключенного клиента
        /// </returns>
        public BaseClient Accept()
        {
            Socket? _client = _socket?.Accept();
            ClientAcceped?.Invoke(this, new ServerEventArgs(this));

            return new BaseClient();
        }

        /// <summary>
        /// Подключение клиента
        /// </summary>
        /// <returns>
        /// Возвращение подключенного клиента
        /// </returns>
        public async Task<BaseClient> AcceptAsyc()
        {
            Socket? _client = await _socket?.AcceptAsync()!;
            ClientAcceped?.Invoke(this, new ServerEventArgs(this));

            return new BaseClient();
        }

        /// <summary>
        /// Остановка сервера
        /// </summary>
        public void Stop()
        {
            _socket?.Close();
            Stoped?.Invoke(this, new ServerEventArgs(this));
        }

        /// <summary>
        /// Возвращение фабрики сервера
        /// </summary>
        public static BaseServerFactory Factory()
        {
            return new BaseServerFactory();
        }
        #endregion
    }
    #endregion

    #region Factory of BaseServer
    /// <summary>
    /// Фабрика объектов класса сервера
    /// </summary>
    public class BaseServerFactory
    {
        /// <summary>
        /// конструкторы
        /// </summary>
        public BaseServerFactory() { }

        /// <summary>
        /// Фабричный метод
        /// </summary>
        /// <param name="stack">
        /// Строитель стека протоколов
        /// </param>
        /// <returns>
        /// Сервер BaseServer
        /// </returns>
        public BaseServer Server(Func<SocketProtocolStackBuilder, SocketProtocolStack> stack)
        {
            return new BaseServer(stack(new SocketProtocolStackBuilder()));
        }

        /// <summary>
        /// Статический метод возвращает сервер по умолчанию
        /// </summary>
        /// <returns>
        /// Сервер TCP/IP
        /// </returns>
        public static BaseServer DefaultServer()
        {
            var defaultStack = new SocketProtocolStackBuilder()
                                    .SetFamily(AddressFamily.InterNetwork)
                                    .SetType(SocketType.Stream)
                                    .SetProtocol(ProtocolType.Tcp)
                                    .GetSocketProtocolStack();
            
            return new BaseServer(defaultStack);
        }
    }
    #endregion
}
