using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using Client3.Model;
using Client3.View;
using Client3.ViewModel.Commands;
using MySqlConnector;

namespace Client3.ViewModel
{
    public class VM_PrePayment
    {
        public User User { get; set; } = new();
        public Record Record { get; set; } = new();
        public Command Show_fee { get; set; }
        public Command Payment { get; set; }

        public VM_PrePayment()
        {
            Show_fee = new Command(Send_vehicleNum);
        }

        public void Send_vehicleNum()
        {
            Send_msg msg = new()
            {
                MsgId = (byte)Network.MsgId.PAYMENT,
                Record = this.Record
            };
            byte[] buf = Network.Serialize_to_json(msg);
            Network.stream.Write(buf);

            // 입/출차 일시, 주차 시간, 주차 요금 UI 업데이트
            Receive_msg rcv_msg = Network.Receive_message();
            Record.EntryDate = rcv_msg.Record.EntryDate;
            Record.ExitDate = rcv_msg.Record.ExitDate;
            Record.ParkingTime = rcv_msg.Record.ParkingTime;
            Record.TotalFee = rcv_msg.Record.TotalFee;
        }
    }
}
