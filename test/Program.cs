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
            Color.Add(new() { A = 1 });
            Color.Add(new() { A = 2 });
            Color.Add(new() { A = 3 });

            Color.RemoveAt(0);
            Console.WriteLine(Color[0].A);
            Console.WriteLine(Color[1].A);
            Color.Insert(0, new() { A = 1 });
            Console.WriteLine(Color[0].A);
            Console.WriteLine(Color[1].A);

        }
    }
}
