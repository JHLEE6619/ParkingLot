using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Client1.Model;
using Client4.Model;
using Client4.View;
using Newtonsoft.Json;

namespace Client4.ViewModel
{
    public class Network
    {
        public TcpClient clnt;
        public NetworkStream stream;
        public VM_Main Vm_Main { get; set; }
        public static Main main { get; set; }

        public enum MsgId
        {
            ENTRY_RECORD, PAYMENT, REGISTRATION, PERIOD_EXTENSION, PREPAYMENT, UPDATE_CLASSIFICATION = 8
        }

        public Network(VM_Main vm_main)
        {
            Vm_Main = vm_main;
            clnt = new TcpClient("127.0.0.1", 10004);
            stream = clnt.GetStream();
        }

        public async Task Receive_messageAsync()
        {
            byte[] buf = new byte[1024];
            while (true)
            {
                byte cls = 0;
                int len = await stream.ReadAsync(buf).ConfigureAwait(false);
                string json = Encoding.UTF8.GetString(buf, 0, len);
                Receive_msg msg = JsonConvert.DeserializeObject<Receive_msg>(json);
                cls = msg.Record.Classification;
                main.Dispatcher.Invoke(() =>
                {
                    Vm_Main.Record.VehicleNum = msg.Record.VehicleNum;
                    Vm_Main.Record.EntryDate = msg.Record.EntryDate;
                    Vm_Main.Record.Classification = cls;
                    Vm_Main.Record.ExitDate = msg.Record.ExitDate;
                    Vm_Main.Record.ParkingTime = msg.Record.ParkingTime;
                    Vm_Main.Record.TotalFee = msg.Record.TotalFee;
                    // 사전 정산 차량이거나 정기등록 차량이면
                });
                if (cls >= 1)
                    await Navigate_goodByePageAsync(cls);
            }
        }

        public async Task Navigate_goodByePageAsync(byte cls)
        {
            if (cls == 1)
            {
                Vm_Main.Btn_content = "사전 정산 차량";
            }
            else if (cls == 2)
            {
                Vm_Main.Btn_content = "정기 등록 차량";
            }
            await Task.Delay(3000);
            Init_record();
            main.Dispatcher.Invoke(() =>
            {
                main.NavigationService.Navigate(new GoodBye());
            });
        }

        public async Task Send_messageAsync(Send_msg msg)
        {
            if (msg == null)
            {
                Console.WriteLine("메세지가 null입니다.");
                return;
            }
            string json = JsonConvert.SerializeObject(msg);
            byte[] buf = Encoding.UTF8.GetBytes(json);
            await stream.WriteAsync(buf).ConfigureAwait(false);
        }

        private void Init_record()
        {
            main.Dispatcher.Invoke(() =>
            {
                Vm_Main.Record.VehicleNum = "";
                Vm_Main.Record.EntryDate = "";
                Vm_Main.Record.ExitDate = "";
                Vm_Main.Record.ParkingTime = "";
                Vm_Main.Record.TotalFee = "";
                Vm_Main.Btn_content = "결제 하기";
            });
        }
    }
}
