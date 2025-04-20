using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using Server.Model;
using Newtonsoft.Json;
using System.IO;

namespace Server.Controller
{
    public class MainServer : Date
    {

        public DBC Dbc {get;set;}
        public enum MsgId
        {
            ENTRY_RECORD, PAYMENT, REGISTRATION, PERIOD_EXTENSION, PREPAYMENT, UPDATE_CLASSIFICATION = 8
        }

        public async Task StartMainServer()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 10000);
            Console.WriteLine(" 메인서버 시작 ");
            listener.Start();
            while (true)
            {
                TcpClient clnt =
                    await listener.AcceptTcpClientAsync().ConfigureAwait(false);

                Task.Run(() => ServerMainAsync(clnt));
            }
        }

        private async Task ServerMainAsync(TcpClient clnt)
        {
            this.Dbc = new();
            Console.WriteLine("클라이언트3/4 연결");
            Console.WriteLine("DB 연결");
            byte[] buf = new byte[1024];
            Receive_msg msg;
            NetworkStream stream = clnt.GetStream();
            while (true)
            {
                int len = await stream.ReadAsync(buf).ConfigureAwait(false);
                string json = Encoding.UTF8.GetString(buf);
                msg = JsonConvert.DeserializeObject<Receive_msg>(json);
                handler(msg, stream);
                
            }
        }

        private void handler(Receive_msg rcv_msg, NetworkStream stream)
        {
            Send_msg send_msg = null;
            DateTime exitDate;
            int totalFee;
            switch (rcv_msg.MsgId)
            {
                // 사전 / 출차 정산 전
                case (byte)MsgId.ENTRY_RECORD:
                    send_msg = Show_paymentInfo(rcv_msg);
                    Send_messageAsync(send_msg, stream);
                    break;
                //  출차 정산 후
                case (byte)MsgId.PAYMENT:
                    exitDate = Str_to_date(rcv_msg.Record.ExitDate);
                    totalFee = int.Parse(rcv_msg.Record.TotalFee.Replace("원", ""));
                    Dbc.Update_exitRecord(rcv_msg.Record.VehicleNum, exitDate, totalFee);
                    break;
                case (byte)MsgId.REGISTRATION:
                    Dbc.Insert_regInfo(rcv_msg.User);
                    Dbc.Update_classification(rcv_msg.User.VehicleNum, 2);
                    Update_classification(rcv_msg, 2);
                    break;
                case (byte)MsgId.PERIOD_EXTENSION:
                    Dbc.Update_regInfo(rcv_msg.User);
                    break;
                // 사전정산 후
                case (byte)MsgId.PREPAYMENT:
                    Dbc.Update_classification(rcv_msg.Record.VehicleNum, rcv_msg.Record.Classification);
                    exitDate = Str_to_date(rcv_msg.Record.ExitDate);
                    totalFee = int.Parse(rcv_msg.Record.TotalFee.Replace("원", ""));
                    Dbc.Update_exitRecord(rcv_msg.Record.VehicleNum, exitDate, totalFee);
                    Update_classification(rcv_msg, 1);
                    break;

            }
        }


        private async Task Send_messageAsync(Send_msg msg, NetworkStream stream)
        {
            if (msg == null || stream == null)
            {
                Console.WriteLine("메인 서버 : 메세지 또는 stream이 null입니다.");
                return;
            }
            string json = JsonConvert.SerializeObject(msg);
            byte[] buf = Encoding.UTF8.GetBytes(json);
            await stream.WriteAsync(buf).ConfigureAwait(false);
        }

        private Send_msg Show_entryRecord(Receive_msg rcv_msg)
        {
            byte cls = Dbc.Select_expDate(rcv_msg.Record.VehicleNum);
            Dbc.Insert_Entry_record(cls, rcv_msg.Record.VehicleNum);
            Entry_exit_record record = Dbc.Select_record(rcv_msg.Record.VehicleNum);
            string entry_date = Date_to_str(record.EntryDate);
            Send_msg send_msg = new()
            {
                MsgId = (byte)MsgId.ENTRY_RECORD,
                Record = new()
                {
                    VehicleNum = record.VehicleNum,
                    EntryDate = entry_date,
                    Classification = record.Classification
                }
            };

            return send_msg;
        }

        private Send_msg Show_paymentInfo(Receive_msg rcv_msg)
        {
            Entry_exit_record record = Dbc.Select_record(rcv_msg.Record.VehicleNum);

            string entry_date = Date_to_str(record.EntryDate);
            DateTime now = DateTime.Now;
            string exit_Date = Date_to_str(now);
            int parkingTime = Dif_date(record.EntryDate, now);
            string parking_time = parkingTime + "분";
            int totalFee = Cal_totalFee(parkingTime);
            string total_fee = totalFee.ToString() + "원";
            Send_msg send_msg = new()
            {
                MsgId = (byte)MsgId.PAYMENT,
                Record = new()
                {
                    VehicleNum = record.VehicleNum,
                    EntryDate = entry_date,
                    Classification = record.Classification,
                    ExitDate = exit_Date,
                    ParkingTime = parking_time,
                    TotalFee = total_fee
                }
            };

            return send_msg;
        }

        private void Update_classification(Receive_msg rcv_msg, byte cls)
        {
            Send_msg msg = new()
            {
                MsgId = (byte)MsgId.UPDATE_CLASSIFICATION,
                Record = new()
                {
                    VehicleNum = rcv_msg.Record.VehicleNum,
                    Classification = cls
                }
            };

            Send_messageAsync(msg, Program.Clients[1]);
        }
    }
}
