using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Client1.Model;
using Newtonsoft.Json;

namespace Client1.ViewModel
{
    public class Network
    {
        public TcpClient Clnt{ get; set; }
        public NetworkStream Stream { get; set; }
        public ObservableCollection<Record> Record { get; set; } = [];
        public Window Window { get; set; }

        public enum MsgId
        {
            ENTRY_RECORD, PAYMENT, REGISTRATION, PERIOD_EXTENSION, INIT_PARKING_LIST, ENTER_VEHICLE, EXIT_VEHICLE
        }

        public Network(Window window)
        {
            this.Window = window;
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
            this.Window.Dispatcher.BeginInvoke(() =>
            {
                rcv_msg.ParkingList.ForEach(x => Record.Add(x));
            });
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
                    this.Window.Dispatcher.BeginInvoke(() =>
                    {
                        Record.Add(msg.Record);
                    });
                }
            }
            catch
            {
                Stream.Close();
                Clnt.Close();
            }
        }
    }
}
