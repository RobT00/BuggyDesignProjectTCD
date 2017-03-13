﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;

namespace Station
{
    class Communications
    {
        private Dictionary<string, Action<int>> buggyhash = new Dictionary<string, Action<int>>();
        private Action<int, string> defaultHandler = null;
        private SerialPort port = new SerialPort();
        private bool recieved = false;
        public Communications()
        {
            port.PortName = "COM4";
            port.BaudRate = 9600;
            port.Open();

            port.Write("+++");
            Thread.Sleep(1100);
            port.WriteLine("ATID 3308, CH C, CN");
            Thread.Sleep(10000);

            port.DiscardInBuffer();
            port.DataReceived += recievedData;
        }
        public void send(int buggy_id, string command)
        {
            int reps = 0;
            int sender_id = 0;
            recieved = false;
            while (!recieved && reps < 10)
            {
                port.Write(sender_id + " " + buggy_id + " " + command + "\n");
                //Thread.Sleep(100);
                Task.Delay(100);
                reps++;
            }
            if (reps == 10)
                Console.WriteLine("Command: " + command + "\nNot being recieved by buggy: " + buggy_id);
        }
        public void recievedData(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort test = (SerialPort)sender;
            String message = test.ReadLine();
            if (message.Length < 5)
                return;
            int sender_id = message[0] - '0';
            int reciever_id = message[2] - '0';
            string command = message.Substring(4);
            if (reciever_id != 0)
                return;
            if (sender_id != 1 && sender_id != 2)
                return;
            recieved = true;
            if (!buggyhash.ContainsKey(command))
                defaultHandler?.Invoke(sender_id, command);
            else
            {
                Console.WriteLine("> Buggy" + sender_id + ": " + command);
                buggyhash[command](sender_id);
            }
        }
        public void addCommand(string command, Action<int> handler)
        {
            buggyhash.Add(command, handler);
        }
        public void setDefaultHandler(Action<int, string> handler)
        {
            defaultHandler = handler;
        }
    }
}
