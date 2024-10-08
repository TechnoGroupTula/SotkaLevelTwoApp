using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SotkaLevelTwoCore.Types;

namespace SotkaLevelTwoCore.Base
{
    #region BaseClient Class
    /// <summary>
    /// Base Client
    ///     для Master - Data Query - клиент запросов у Slave
    ///     для Slave - Subscriber - подписчик у Master
    ///     
    ///     Socket - сокет Listener
    ///     Active - флаг активности клиента
    ///     SocketProtocolStack - стек протоколов для сокета сервера
    ///     SocketEndPoint - адрес првязки для сервера (ip адрес и номер порта)
    /// </summary>
    public class BaseClient
    {
        #region Private Fields
        /// <summary>
        /// базовые данные
        /// </summary>
        private Socket? _socket;
        private SocketProtocolStack? _protocolStack;
        private SocketEndPoint? _serverEndPoint;
        private bool _active;
        /// <summary>
        /// write/read потоки данных для клиента
        /// </summary>
        private NetworkStream? _readerStream;
        private NetworkStream? _writerStream;
        #endregion

        #region Events
        /// <summary>
        /// События клиента
        /// </summary>
        public event ClientHandler? Connected;
        public event ClientHandler? Disconnected;
        #endregion

        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        public BaseClient()
        {
        }
        public BaseClient(SocketProtocolStack protocolStack)
        {
            try
            {
                this._protocolStack = protocolStack;
                this._socket = new Socket(_protocolStack.Family,
                                          _protocolStack.Type,
                                          _protocolStack.Protocol);
                this._readerStream = new NetworkStream(this._socket);
                this._writerStream = new NetworkStream(this._socket);
            }
            catch (Exception)
            {
                throw new BaseClientException("");
            }

        }
        #endregion

        #region Properties
        /// <summary>
        /// свойства
        /// </summary>
        public SocketEndPoint? ServerEndPoint
        {
            get => _serverEndPoint;
            set => _serverEndPoint = value;
        }

        public NetworkStream ReaderStream
        {

        }

        #region Methods
        /// <summary>
        /// Подключение к серверу
        /// </summary>
        /// <exception cref="BaseClientException">
        /// Ошибка подключения к серверу
        /// </exception>
        public void Connect()
        {
            try
            {
                _socket?.Connect(_serverEndPoint!.Address!, _serverEndPoint!.Port);
                Connected?.Invoke(this, new ClientEventArgs(this));
            }
            catch(Exception)
            {
                throw new BaseClientException("Server connection error");
            }

        }

        /// <summary>
        /// Подключение к серверу
        /// </summary>
        /// <param name="endPoint">
        /// Адрес удаленного сервера
        /// </param>
        /// /// <exception cref="BaseClientException">
        /// Ошибка подключения к серверу
        /// </exception>
        public void Connect(SocketEndPoint endPoint)
        {
            _serverEndPoint = endPoint;

            this.Connect();
        }

        /// <summary>
        /// Подключение к серверу
        /// </summary>
        /// <returns></returns>
        /// <exception cref="BaseClientException">
        /// Ошибка подключения к серверу
        /// </exception>
        public async Task ConnectAsync()
        {
            try
            {
                await _socket!.ConnectAsync(_serverEndPoint!.Address!, _serverEndPoint!.Port);
                Connected?.Invoke(this, new ClientEventArgs(this));
            }
            catch(Exception)
            {
                throw new BaseClientException("Server connection error");
            }
        }

        /// <summary>
        /// Подключение к серверу
        /// </summary>
        /// <param name="endPoint">
        /// Адрес удаленного сервера
        /// </param>
        /// <returns></returns>
        /// /// <exception cref="BaseClientException">
        /// Ошибка подключения к серверу
        /// </exception>
        public async Task ConnectAsync(SocketEndPoint endPoint)
        {
            _serverEndPoint = endPoint;

            await this.ConnectAsync(endPoint);
        }

        /// <summary>
        /// Отключение от сервера
        /// </summary>
        public void Disconnect()
        {
            _socket!.Disconnect(false);
            Disconnected?.Invoke(this, new ClientEventArgs(this));
        }

        /// <summary>
        /// Отключение от сервера
        /// </summary>
        /// <returns></returns>
        public async Task DisconnectAsync()
        {
            await _socket!.DisconnectAsync(false);
            Disconnected?.Invoke(this, new ClientEventArgs(this));
        }

        /// <summary>
        /// Получение фабрики клиента
        /// </summary>
        /// <returns>
        /// Возвращается объект фабрики
        /// </returns>
        public BaseClientFactory Factory()
        {
            return new BaseClientFactory();
        }
        #endregion
    }
    #endregion

    #region Factory of BaseClient Class
    /// <summary>
    /// Фабрика создания объекта клиента
    /// </summary>
    public class BaseClientFactory
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public BaseClientFactory() { }
        
        /// <summary>
        /// Фабричный метод
        /// </summary>
        /// <param name="stack">
        /// Строитель стека протоколов для сокета клиентпа
        /// </param>
        /// <returns>
        /// Объект клиента
        /// </returns>
        public BaseClient Client(Func<SocketProtocolStackBuilder, SocketProtocolStack> stack)
        {
            return new BaseClient(stack(new SocketProtocolStackBuilder()));
        }

        /// <summary>
        /// Статический метод для возврата клиента по умолчанию
        /// </summary>
        /// <returns>
        /// Клиент стека протоколов TCP/IP
        /// </returns>
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
    #endregion
}
