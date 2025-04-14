using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model
{
    // 클라이언와 주고 받을 때 사용
    public class Record
    {
        public string VehicleNum { get; set; }
        public string EntryDate { get; set; }
        public string ExitDate { get; set; }
        public string ParkingTime { get; set; }
        public string TotalFee { get; set; }
        public byte Classification { get; set; }
    }
}
