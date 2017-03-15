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
        private object[] receiveLocks = { new object(), new object() };
        private SerialPort port = new SerialPort();
        private bool[] received = { false, false };
        private object sendingLock = new object();
        public Communications()
        {
            port.PortName = "COM11";
            port.BaudRate = 9600;
            port.Open();

            port.Write("+++");
            Thread.Sleep(1100);
            port.WriteLine("ATID 3308, CH C, CN");
            Thread.Sleep(10000);

            port.DiscardInBuffer();
            port.DataReceived += recievedData;

            addCommand("ACK", (int ID) => { });
        }
        public bool send(int buggy_id, string command, Action offlineHandler = null)
        {
            int reps = 0;
            int sender_id = 0;
            lock (receiveLocks[buggy_id])
            {
                received[buggy_id] = false;
                while (!received[buggy_id])
                {
                    reps++;
                    
                    if (reps == 2 && !received[buggy_id])
                    {
                        if (offlineHandler != null)
                        {
                            offlineHandler();
                        } else
                        {
                            Console.WriteLine("Command: " + command + "\nnot being recieved by buggy: " + buggy_id +
                            "\nWill keep sending command");
                        }
                        
                    }
                    lock (sendingLock)
                    {
                        port.Write(sender_id + " " + buggy_id + " " + command + "\n");
                    }
                    Monitor.Wait(receiveLocks[buggy_id], 100);
                }
            }
            if (reps > 1)
            {
                if (offlineHandler == null)
                {
                    Console.WriteLine(" Command: " + command + " received after " + reps + " attempts"); 
                }
                return false;
            }
            return true;
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

            lock (receiveLocks[sender_id])
            {
                received[sender_id] = true;
            }
            if (!buggyhash.ContainsKey(command))
                defaultHandler?.Invoke(sender_id, command);
            else
                buggyhash[command](sender_id);
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
