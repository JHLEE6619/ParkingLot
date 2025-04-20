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
    public class Network
    {
        public TcpClient clnt;
        public NetworkStream stream;
        public VM_Main Vm_Main { get; set; }

        public Network(VM_Main vm_main)
        {
            Vm_Main = vm_main;
            clnt = new TcpClient("127.0.0.1", 10004);
            stream = clnt.GetStream();
        }

        public void Receive_message()
        {
            byte[] buf = new byte[1024];
            while (true)
            {
                stream.Read(buf);
                string json = Encoding.UTF8.GetString(buf);
                Receive_msg msg = JsonConvert.DeserializeObject<Receive_msg>(json);
                Vm_Main.Record.VehicleNum = msg.Record.VehicleNum;
                Vm_Main.Record.Classification = msg.Record.Classification;
                Vm_Main.Record.ExitDate = msg.Record.ExitDate;
                Vm_Main.Record.ParkingTime = msg.Record.ParkingTime;
                Vm_Main.Record.TotalFee = msg.Record.TotalFee;
                // 사전 정산 차량이거나 정기등록 차량이면
                if (Vm_Main.Record.Classification >= 1)
                    Navigate_goodByePage();
            }
        }

        public void Navigate_goodByePage()
        {
            Thread.Sleep(3000);
            GoodBye goodBye = new();
            main.NavigationService.Navigate(goodBye);
        }

    }
}
