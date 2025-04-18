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
        public static Dictionary<byte, NetworkStream> Clients { get; set; } = [];
        static async Task Main(string[] args)
        {
            Clients.Add(1, null);
            Clients.Add(2, null);
            Clients.Add(4, null);
            FileReceiveServer entrance_Server = new(10001);
            FileReceiveServer exit_Server = new(10003);
            MainServer mainServer = new();
            await mainServer.StartMainServer();
        }
    }
}
