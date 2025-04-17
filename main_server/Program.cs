using System.Net.Sockets;
using System.Net;
using System.Text;
using Server.Model;
using Newtonsoft.Json;
using Server.Controller;

namespace Server
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            FileReceiveServer fServer = new();
            MainServer mainServer = new();
            fServer.StartFileRcvServer();
            await mainServer.StartMainServer();
        }
    }
}
