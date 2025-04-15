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
        static void Main(string[] args)
        {


            Test2 Test = new()
            {
                A = 1,
                B = "Test"
            };

            string json = JsonConvert.SerializeObject(Test);
            Test testB = JsonConvert.DeserializeObject<Test>(json);
            Console.WriteLine(testB.C);

        }
    }
}
