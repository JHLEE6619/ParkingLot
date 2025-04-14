using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model
{
    public class User
    {
        public string VehicleNum { get; set; }
        public string Pw { get; set; }
        public int Reg_period { get; set; }
        public int Fee { get; set; }
    }
}
