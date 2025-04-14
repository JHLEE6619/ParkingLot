using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Client3.Model;
using Newtonsoft.Json;

namespace Client3.ViewModel
{
    public static class Network
    {
        public static readonly TcpClient clnt = new TcpClient("127.0.0.1", 10001);
        public static readonly NetworkStream stream = clnt.GetStream();

        public enum MsgId
        {
            ENTRY_RECORD, PAYMENT, REGISTRATION, PERIOD_EXTENSION
        }

        public static byte[] Serialize_to_json(object msg)
        {

            string json = JsonConvert.SerializeObject(msg);
            byte[] buf = Encoding.UTF8.GetBytes(json);
            return buf;
        }

        public static Receive_msg Deserialize_to_json(byte[] buf, int len)
        {
            string json = Encoding.UTF8.GetString(buf, 0, len);
            var msg = JsonConvert.DeserializeObject<Receive_msg>(json);

            return msg;
        }

        public static Receive_msg Receive_message()
        {
            Receive_msg msg = new();
            byte[] buf = new byte[1024];
            try
            {
                int len =  stream.Read(buf, 0, buf.Length);
                msg = Deserialize_to_json(buf, len);
            }
            catch 
            {
                stream.Close();
                clnt.Close();
            }
            return msg;
        }

    }
}
