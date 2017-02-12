using ConsoleApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Station
{
    class Buggy
    {
        private int ID;
        private Direction direction;
        private Communications comms;

        public Buggy(int ID, Direction direction, Communications comms)
        {
            this.ID = ID;
            this.direction = direction;
            this.comms = comms;

            sendDirection();
        }
        public void go()
        {
            comms.send(ID, "GO");
        }
        public void stop()
        {
            comms.send(ID, "STOP");
        }
        public void sendPing()
        {
            comms.send(ID, "PING");
        }
        public void sendPong()
        {
            comms.send(ID, "PONG");
        }

        private void sendDirection()
        {
            if (direction == Direction.AntiClockwise)
            {
                comms.send(ID, "ACLOCK");
            } else
            {
                comms.send(ID, "CLOCK");
            }
        }

        public void pingRecieved()
        {
            Program.print("PING recieved");
        }
        public void pongRecieved()
        {
            Program.print("PONG recieved");
        }
    }
}
