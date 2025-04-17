using System.Collections.ObjectModel;
using System.Drawing;
using System.Net.Sockets;
using Newtonsoft.Json;

namespace test
{
    public class Test
    {
        public int A { get; set; }
        public string B { get; set; }
        public string C { get; set; }
    }

    public class Test2
    {
        public int A { get; set; }
        public string B { get; set; }
    }

    public class Program
    {
        public static ObservableCollection<Test> Color { get; set; } = [];

        static void Main(string[] args)
        {
            DateTime dateTime = new(new DateOnly(2025, 3, 17), new TimeOnly(19, 0)); 
            DateTime now = DateTime.Now;
            int parkingTime = Dif_date(dateTime, now);
            Console.WriteLine(Cal_totalFee(parkingTime));


        }

        public static int Dif_date(DateTime entryDate, DateTime exitDate)
        {
            TimeSpan timeDiff = exitDate - entryDate;
            int min = (int)timeDiff.TotalMinutes;
            return min;
        }

        public static int Cal_totalFee(int parkingTime)
        {
            int totalFee = (500 * parkingTime / 10);
            return totalFee;
        }
    }
}
