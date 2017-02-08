using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;

namespace ConsoleApplication
{
    class Program
    {
        static String message = "";
        static void Main(string[] args)
        {
            var HashMap = new Dictionary<object, int, int, string>();
            SerialPort port = new SerialPort();
            port.PortName = "COM6";
            port.BaudRate = 9600;
            port.Open();

            port.Write("+++");
            Thread.Sleep(1100);
            port.WriteLine("ATID 6968, CH C, CN");
            Thread.Sleep(10000);
            port.DiscardInBuffer();
            port.DataReceived += recievedData;

            while (true)
            {
                Console.Write("> ");
                message = Console.ReadLine();
                port.Write(message + "\n");
                Thread.Sleep(200);
            }
        }

        private static void recievedData(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort test = (SerialPort)sender;
            String message = test.ReadLine();
            Console.WriteLine("> " + message);
        }
    }
}