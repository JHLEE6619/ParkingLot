using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model
{
    public class Date
    {
        public const int fee = 500;
        public const int perTenMin = 10;

        public int Dif_date(DateTime entryDate, DateTime exitDate)
        {
            TimeSpan timeDiff = exitDate - entryDate;
            int min = (int)timeDiff.TotalMinutes;
            return min;
        }

        public int Cal_totalFee(int parkingTime)
        {
            int totalFee = (fee * parkingTime / perTenMin);
            return totalFee;
        }

        public string Date_to_str(DateTime dateTime)
        {
            string dateForamt = "MM월 dd일 HH시 mm분";
            return dateTime.ToString(dateForamt);
        }

        public DateTime Str_to_date(string dateString)
        {
            string format = "MM월 dd일 HH시 mm분";
            CultureInfo provider = CultureInfo.InvariantCulture;

            return DateTime.ParseExact(dateString, format, provider);
        }
    }
}
