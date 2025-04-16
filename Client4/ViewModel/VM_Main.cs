using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client1.Model;

namespace Client4.ViewModel
{
    public class VM_Main
    {
        public Record Record { get; set; } = new();
        public VM_Main() 
        {
            Network.Vm_Main = this;
            Task.Run(() => Network.Receive_message());
        }
    }
}
