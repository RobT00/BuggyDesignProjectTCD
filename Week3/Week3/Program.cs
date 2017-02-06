using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;

namespace Week3
{
    class Program
    {
        static String message = "";
        static List<String> Messages = new List<String>();
        static int count = 0;
        static void Main(string[] args)
        {
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

            while (true) {
                Console.Write("> ");
                message = Console.ReadLine();
                if (message == "print")
                {
                    for (int i = 0; i < count; i++)
                    {
                        string output = Messages[i];
                        Console.WriteLine(output);
                    }
                }
                else
                {
                    port.Write(message + "\n");
                    Messages.Add(message);
                    count++;
                    Thread.Sleep(200);
                }
            }
        }

        private static void recievedData(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort test = (SerialPort) sender;
            String message2 = test.ReadLine();
            Console.WriteLine("> " + message2);
            Messages.Add(message2);
            count++;
        }
    }
}
