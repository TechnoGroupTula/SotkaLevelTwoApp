using Microsoft.Extensions.Configuration;
using SotkaLevelTwoCore.Types;
using System;
using System.Net;

namespace SotkaLevelTwoCore.Base
{
    public abstract class BaseConfigurationLoader
    {
        /// <summary>
        /// Загружает конфигурацию с использованием указанного делегата для добавления источников конфигурации.
        /// </summary>
        /// <param name="configureSource">Делегат, который добавляет источники конфигурации в <see cref="ConfigurationBuilder"/>.</param>
        /// <returns>Возвращает объект <see cref="IConfigurationRoot"/> с загруженной конфигурацией.</returns>
        /// <exception cref="FileNotFoundException">Выбрасывается, если файл конфигурации не найден.</exception>
        protected IConfigurationRoot LoadConfiguration(Action<IConfigurationBuilder> configureSource)
        {
            var builder = new ConfigurationBuilder();
            configureSource(builder);
            return builder.Build();
        }

        /// <summary>
        /// Проверяет корректность конфигурации (IP-адрес и порт).
        /// </summary>
        protected void ValidateConfiguration(IConfiguration configuration, string ipKey, string portKey)
        {
            string? ipAddress = configuration[ipKey];
            string? port = configuration[portKey];

            if (!IPAddress.TryParse(ipAddress, out _))
            {
                throw new MissingConfigurationException($"Некорректный IP-адрес по ключу: {ipKey}");
            }

            if (string.IsNullOrEmpty(port) || !ushort.TryParse(port, out _))
            {
                throw new MissingConfigurationException($"Некорректный порт по ключу: {portKey}");
            }
        }
    }

    public class ServerConfigurationLoader : BaseConfigurationLoader
    {
        /// <summary>
        /// Загружает конфигурацию сервера из XML-файла и создает объект <see cref="SocketEndPoint"/>.
        /// </summary>
        /// <returns>Возвращает сконфигурированный объект <see cref="SocketEndPoint"/> из файла конфигурации.</returns>
        /// <exception cref="FileNotFoundException">
        /// Выбрасывается, если файл конфигурации не найден по указанному пути.
        /// </exception>
        /// <exception cref="MissingConfigurationException">
        /// Выбрасывается, если конфигурация содержит некорректный IP-адрес или порт.
        /// </exception>
        /// <remarks>
        /// Метод ищет файл конфигурации по пути <c>Configurations/config.xml</c>, проверяет корректность параметров IP-адреса и порта,
        /// и создает объект <see cref="SocketEndPoint"/> на основе этих параметров.
        /// </remarks>
        public SocketEndPoint LoadXmlConfiguration()
        {
            string configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configurations", "config.xml");

            var configuration = LoadConfiguration(builder => builder.AddXmlFile(configFilePath));

            ValidateConfiguration(configuration, "Server:IpAddress", "Server:Port");

            return new SocketEndPointBuilder()
                .SetAddress(configuration["Server:IpAddress"]!)
                .SetPort(int.Parse(configuration["Server:Port"]!))
                .GetSocketEndPoint();
        }
    }

    public class ClientConfigurationLoader : BaseConfigurationLoader
    {
        /// <summary>
        /// Загружает конфигурацию клиента из XML-файла и создает объект <see cref="SocketEndPoint"/>.
        /// </summary>
        /// <returns>Возвращает сконфигурированный объект <see cref="SocketEndPoint"/> из файла конфигурации.</returns>
        /// <exception cref="FileNotFoundException">
        /// Выбрасывается, если файл конфигурации не найден по указанному пути.
        /// </exception>
        /// <exception cref="MissingConfigurationException">
        /// Выбрасывается, если конфигурация содержит некорректный IP-адрес или порт.
        /// </exception>
        /// <remarks>
        /// Метод ищет файл конфигурации по пути <c>Configurations/config.xml</c>, проверяет корректность параметров IP-адреса и порта,
        /// и создает объект <see cref="SocketEndPoint"/> на основе этих параметров.
        /// </remarks>
        public SocketEndPoint LoadXmlConfiguration()
        {
            string configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configurations", "config.xml");

            var configuration = LoadConfiguration(builder => builder.AddXmlFile(configFilePath));

            ValidateConfiguration(configuration, "Client:IpAddress", "Client:Port");

            return new SocketEndPointBuilder()
                .SetAddress(configuration["Client:IpAddress"]!)
                .SetPort(int.Parse(configuration["Client:Port"]!))
                .GetSocketEndPoint();
        }
    }

    public class MissingConfigurationException : Exception
    {
        public MissingConfigurationException(string message) : base(message) { }
    }
}
