using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client3.Model
{
    public class Record
    {
        public string VehicleNum {  get; set; }
        public string EntryDate { get; set; }
        public string ExitDate { get; set; }
        public string ParkingTime { get; set; }
        public string TotalFee { get; set; }
        public byte Classification { get; set; }
    }
}
