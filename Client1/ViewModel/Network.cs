using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
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

        public Network(MainWindow mainWindow, VM_Main vm_main)
        {
            this.MainWindow = mainWindow;
            this.VM_Main = vm_main;
            this.Clnt = new TcpClient("127.0.0.1", 10001);
            this.Stream = this.Clnt.GetStream();
            Init_Parking_list();
            Task.Run(() => Receive_message());
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

        private void Init_Parking_list()
        {
            Send_msg send_msg = new()
            {
                MsgId = (byte)MsgId.INIT_PARKING_LIST,
            };
            byte[] buf = Serialize_to_json(send_msg);
            Stream.Write(buf);
            byte[] read_buf = new byte[1024];
            int len = Stream.Read(read_buf);
            Receive_msg rcv_msg = Deserialize_to_json(read_buf, len);
            rcv_msg.ParkingList.ForEach(x => VM_Main.Record.Add(x));
        }

        private void Receive_message()
        {
            Receive_msg msg = new();
            byte[] buf = new byte[1024];
            try
            {
                while (true)
                {
                    int len = Stream.Read(buf, 0, buf.Length);
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
                case (byte)MsgId.EXIT_VEHICLE:
                    Delete_Record(msg);
                    break;
                case (byte)MsgId.SEAT_INFO:
                    Update_seatInfo(msg);
                    break;
            }
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
