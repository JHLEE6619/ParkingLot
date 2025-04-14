using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client3.Model
{
    public class Send_msg
    {
        public byte MsgId { get; set; }
        public User User { get; set; }
        public Record Record { get; set; }
    }
}
