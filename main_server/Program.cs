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
            FileReceiveServer.Clients.Add(1, null);
            FileReceiveServer.Clients.Add(2, null);
            FileReceiveServer.Clients.Add(4, null);
            FileReceiveServer entrance_Server = new(10001);
            FileReceiveServer exit_Server = new(10003);
            MainServer mainServer = new();
            await mainServer.StartMainServer();
        }
    }
}
