using Microsoft.Extensions.Configuration;
using SotkaLevelTwoCore.Base;
using System.Net;
using System.Net.Sockets;

namespace SotkaLevelTwoMaster
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SocketProtocolStackBuilder stackBuilder = new SocketProtocolStackBuilder();
            SocketProtocolStack socketProtocolStack = stackBuilder.SetFamily(AddressFamily.InterNetwork)
                                                                  .SetType(SocketType.Stream)
                                                                  .SetProtocol(ProtocolType.Tcp)
                                                                  .GetSocketProtocolStack();

            BaseClient client = new BaseClient(socketProtocolStack);


            SocketEndPoint? socketEndPoint = null;
            try
            {
                 socketEndPoint = new ClientConfigurationLoader().LoadXmlConfiguration();
            }
            catch (FileNotFoundException ex)
            {
                //Обрабатываем исключение
            }
            catch (MissingConfigurationException ex)
            {
                //Обрабатываем исключение
            }

            if (socketEndPoint is not null)
                client.Connect(socketEndPoint);
        }
    }
}
