using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;

namespace ConsoleApplication
{
    class Communications
    {
        static String message = "";
        private Dictionary<string, Action<int>> buggyhash = new Dictionary<string, Action<int>>();
        private SerialPort port = new SerialPort();
        public Communications()
        {
            port.PortName = "COM6";
            port.BaudRate = 9600;
            port.Open();

            port.Write("+++");
            Thread.Sleep(1100);
            port.WriteLine("ATID 6968, CH C, CN");
            Thread.Sleep(10000);

            port.DiscardInBuffer();
            port.DataReceived += recievedData;
        }
        public void send(int buggy_id, string command)
        {
            int sender_id = 0;
            Console.Write("> ");
            port.Write(sender_id + " " + buggy_id + " " + command + "\n");
            Thread.Sleep(200);
        }
        public void recievedData(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort test = (SerialPort)sender;
            String message = test.ReadLine();
            Console.WriteLine("> " + message);
            if (message.Length < 5)
                return;
            int sender_id = message[0] - '0';
            int reciever_id = message[2] - '0';
            string command = message.Substring(4);
            if (reciever_id != 0)
                return;
            if (sender_id != 1 && sender_id != 2)
                return;
            if (!buggyhash.ContainsKey(command))
                return;
            buggyhash[command](sender_id);
        }
    }
}
