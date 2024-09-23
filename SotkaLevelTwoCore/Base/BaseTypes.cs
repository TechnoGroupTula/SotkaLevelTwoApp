using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SotkaLevelTwoCore.Base
{
    public delegate void ServerHandler(object? sender, ServerEventArgs e);
    public delegate void ClientHandler(object? sender, ClientEventArgs e);

    public class ServerEventArgs
    {

    }

    public class ClientEventArgs
    {

    }
}
