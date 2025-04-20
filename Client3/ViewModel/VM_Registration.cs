using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client3.Model;
using Client3.ViewModel.Commands;

namespace Client3.ViewModel
{
    public class VM_Registration
    {
        public User User { get; set; } = new();
        public ObservableCollection<string> Items { get; set; }
        public string SelectedItem { get; set; }
        public Command Registration;
        public Command PeriodExtension;
        public Network Network { get; set; }
        
        public VM_Registration() 
        {
            Network = new Network();
            Registration = new Command(Send_regInfo);
            PeriodExtension = new Command(Send_extensionInfo);
            Items = ["1일 - 20,000원", "1주일 - 50,000원", "1개월(30일) - 120,000원"];
        }

        public void Send_regInfo()
        {

            Select_period();
            Send_msg msg = new()
            {
                MsgId = (byte)Network.MsgId.REGISTRATION,
                User = this.User
            };
            Send_msg(msg);
        }

        public void Send_extensionInfo()
        {
            Select_period();
            Send_msg msg = new()
            {
                MsgId = (byte)Network.MsgId.PERIOD_EXTENSION,
                User = this.User
            };
            Send_msg(msg);
        }

        public void Send_msg(Send_msg msg)
        {
            byte[] buf = Network.Serialize_to_json(msg);
            Network.stream.Write(buf);
        }

        public void Select_period()
        {
            int idx;
            idx = Items.IndexOf(SelectedItem);

            switch (idx)
            {
                case 0: User.Fee = 20000; User.Reg_period = 1; break;
                case 1: User.Fee = 50000; User.Reg_period = 7; break;
                case 2: User.Fee = 120000; User.Reg_period = 30; break;
            }
        }
    }
}
