using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client1.Model;

namespace Client4.ViewModel
{
    public class VM_Main : INotifyPropertyChanged
    {
        public Record _record = new();
        Network Network { get; set; }
        public Record Record
        {
            get => _record;
            set
            {
                _record = value;
                OnPropertyChanged(nameof(Record));
            }
        }
        public VM_Main() 
        {
            Network = new Network(this);
            Task.Run(() => Network.Receive_message());
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
