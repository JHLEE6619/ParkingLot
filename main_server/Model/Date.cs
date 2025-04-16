using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model
{
    public class Date
    {
        public int Dif_date(DateTime entryDate, DateTime exitDate)
        {
            TimeSpan timeDiff = entryDate - exitDate;
            int min = (int)timeDiff.TotalMinutes;
            return min;
        }

        public int Cal_totalFee(int parkingTime)
        {
            int totalFee = (Fee.fee * parkingTime / 10);
            return totalFee;
        }

        public string Date_to_str(DateTime dateTime)
        {
            string dateForamt = "MM월 dd일 HH시 mm분";
            return dateTime.ToString(dateForamt);
        }
    }
}
