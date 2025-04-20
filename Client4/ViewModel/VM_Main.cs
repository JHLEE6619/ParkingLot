using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client1.Model;
using Client4.Model;
using Client4.ViewModel.Commands;

namespace Client4.ViewModel
{
    public class VM_Main : INotifyPropertyChanged
    {
        public Record _record = new();
        public Record Record
        {
            get => _record;
            set
            {
                _record = value;
                OnPropertyChanged(nameof(Record));
            }
        }
        private Network Network { get; set; }
        public Command Payment_ { get; set; }
        private string _btn_content = "결제 하기";
        public string Btn_content
        {
            get => _btn_content;
            set
            {
                _btn_content = value;
                OnPropertyChanged(nameof(Btn_content));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private readonly object recordLock = new object();
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public VM_Main() 
        {
            Network = new Network(this);
            Payment_ = new(Payment);
            Task.Run(() => Network.Receive_messageAsync());
        }
        
        private void Payment()
        {
            lock (recordLock) 
            {
                Send_msg msg = new()
                {
                    MsgId = (byte)Network.MsgId.PAYMENT,
                    Record = this.Record
                };
                Network.Send_messageAsync(msg);
            }

        }
    }
}
