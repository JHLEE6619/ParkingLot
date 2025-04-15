using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model
{
    // DB에서 받을때 사용
    public class Entry_exit_record
    {
        public string VehicleNum { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime ExitDate { get; set; }
        public DateTime ParkingTime { get; set; }
        public int Fee { get; set; }
        public byte Classification { get; set; } // 차량 분류(0 : 일반 차량, 1 : 정기 등록 차량, 2 : 사전 정산 차량)
    }
}
