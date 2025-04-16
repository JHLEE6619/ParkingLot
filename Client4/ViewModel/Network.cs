using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Client1.Model;
using Client4.View;
using Newtonsoft.Json;

namespace Client4.ViewModel
{
    public static class Network
    {
        public static TcpClient clnt = new TcpClient("127.0.0.1",10001);
        public static NetworkStream stream = clnt.GetStream();
        public static Main main;

        public static VM_Main Vm_Main { get; set; }

        public static void Receive_message()
        {
            byte[] buf = new byte[1024];
            while (true)
            {
                stream.Read(buf);
                string json = Encoding.UTF8.GetString(buf);
                Receive_msg msg = JsonConvert.DeserializeObject<Receive_msg>(json);
                main.Dispatcher.Invoke(() => {
                    Vm_Main.Record.VehicleNum = msg.Record.VehicleNum;
                    Vm_Main.Record.Classification = msg.Record.Classification;
                    Vm_Main.Record.ExitDate = msg.Record.ExitDate;
                    Vm_Main.Record.ParkingTime = msg.Record.ParkingTime;
                    Vm_Main.Record.TotalFee = msg.Record.TotalFee;
                    // 사전 정산 차량이거나 정기등록 차량이면
                    if (Vm_Main.Record.Classification >= 1)
                        Navigate_goodByePage();
                });
            }
        }

        public static void Navigate_goodByePage()
        {
            Thread.Sleep(3000);
            GoodBye goodBye = new();
            main.NavigationService.Navigate(goodBye);
        }

    }
}
