﻿using SotkaLevelTwoCore.Base;
using System.Net.Sockets;
using System.Net;
using SotkaLevelTwoCore.Types;

namespace SotkaLevelTwoSlave
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

            BaseServer server = new BaseServer(socketProtocolStack);

            SocketEndPoint? socketEndPoint = null;
            try
            {
                socketEndPoint = new ServerConfigurationLoader().LoadXmlConfiguration();
            }
            catch (FileNotFoundException ex)
            {
                //Обрабатываем исключение
            }
            catch (MissingConfigurationException ex)
            {
                //Обрабатываем исключение
            }

            Console.WriteLine("Server start");
            server.Start(socketEndPoint);

            var client = server.Accept();

            server.Stop();
            Console.WriteLine("Server stop");

            //BaseServerFactory factory = BaseServer.Factory();

            //BaseServer baseServer = factory.Server( 
            //    b => b.SetFamily(AddressFamily.InterNetwork)
            //          .SetProtocol(ProtocolType.Tcp)
            //          .SetType(SocketType.Stream)
            //          .GetSocketProtocolStack()
            //);

        }
    }
}
