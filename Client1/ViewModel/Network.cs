using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using Client1.Model;
using Newtonsoft.Json;

namespace Client1.ViewModel
{
    public class Network 
    {
        public TcpClient Clnt{ get; set; }
        public NetworkStream Stream { get; set; }
        public VM_Main VM_Main { get; set; }
        public MainWindow MainWindow { get; set; }

        public enum MsgId
        {
            ENTRY_RECORD, PAYMENT, REGISTRATION, PERIOD_EXTENSION, INIT_PARKING_LIST, ENTER_VEHICLE, EXIT_VEHICLE, SEAT_INFO
        }

        public Network(MainWindow mainWindow, VM_Main vm_main, int port)
        {
            this.MainWindow = mainWindow;
            this.VM_Main = vm_main;
            this.Clnt = new TcpClient("127.0.0.1", port);
            this.Stream = this.Clnt.GetStream();
            if (port == 10001) { 
                byte[] buf = [1];
                Stream.Write(buf);
            }
            Task.Run(() => Receive_messageAsync());
        }


        public byte[] Serialize_to_json(object msg)
        {

            string json = JsonConvert.SerializeObject(msg);
            byte[] buf = Encoding.UTF8.GetBytes(json);
            return buf;
        }

        public Receive_msg Deserialize_to_json(byte[] buf, int len)
        {
            string json = Encoding.UTF8.GetString(buf, 0, len);
            var msg = JsonConvert.DeserializeObject<Receive_msg>(json);

            return msg;
        }

        private async Task Receive_messageAsync()
        {
            Receive_msg msg = new();
            byte[] buf = new byte[4096];
            try
            {
                while (true)
                {
                    int len = await Stream.ReadAsync(buf, 0, buf.Length).ConfigureAwait(false);
                    msg = Deserialize_to_json(buf, len);
                    Handler(msg);
                }
            }
            catch
            {
                Stream.Close();
                Clnt.Close();
            }
        }

        private void Handler(Receive_msg msg)
        {
            switch (msg.MsgId)
            {
                case (byte)MsgId.ENTRY_RECORD:
                    Add_Record(msg);
                    break;
                case (byte)MsgId.INIT_PARKING_LIST:
                    Init_Parking_list(msg);
                    break;
                case (byte)MsgId.EXIT_VEHICLE:
                    Delete_Record(msg);
                    break;
                case (byte)MsgId.SEAT_INFO:
                    Update_seatInfo(msg);
                    break;
            }
        }

        private void Init_Parking_list(Receive_msg rcv_msg)
        {
            MainWindow.Dispatcher.Invoke(() =>
            {
                rcv_msg.ParkingList.ForEach(x => VM_Main.Record.Add(x));
            });
        }

        private void Add_Record(Receive_msg msg)
        {
            MainWindow.Dispatcher.Invoke(() =>
            {
                VM_Main.Record.Add(msg.Record);
            });
        }

        private void Delete_Record(Receive_msg msg)
        {
            MainWindow.Dispatcher.Invoke(() =>
            {
                VM_Main.Record.Remove(msg.Record);
            });
        }

        private void Update_seatInfo(Receive_msg msg)
        {
            int idx = 0;
            foreach (var info in msg.SeatInfo)
            {
                // 주차된 자리

                if (info == 1)
                {
                    MainWindow.Dispatcher.Invoke(() =>
                    {
                        VM_Main.Color.RemoveAt(idx);
                        VM_Main.Color.Insert(idx, Brushes.Red);
                    });
                }
                // 빈자리
                else
                {
                    MainWindow.Dispatcher.Invoke(() =>
                    {
                        VM_Main.Color.RemoveAt(idx);
                        VM_Main.Color.Insert(idx, Brushes.Lime);
                    });
                }
                idx++;
            }
        }
    }
}
