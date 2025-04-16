using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Client1.Model
{
    public class Receive_msg
    {
        public byte MsgId { get; set; }
        public User User { get; set; }
        public Record Record { get; set; }
        public List<Record> ParkingList { get; set; }
        public List<int> SeatInfo { get; set; }
    }
}
