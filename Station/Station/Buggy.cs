using ConsoleApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Station
{
    class Buggy
    {
        private int ID;
        private Direction direction;
        private Communications comms;
        private int last_gantry = 0;
        private int laps = 0;

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
        public void onGantry(int gantry_num)
        {
            stop();
            buggyAction();
            Program.print(" stopped at Gantry: " + gantry_num);
            Thread.Sleep(1000);
            last_gantry = gantry_num;
            if (((direction == Direction.Clockwise) && (last_gantry == 3)) || ((direction == Direction.AntiClockwise) && (last_gantry == 1)))
                laps++; //Wizard of OZ Demo requires travelling in clockwise direction
            go();
            if (laps == 2) //Wizard of OZ Demo requires to park after 2 laps
                goPark();
        }
        public void goPark()
        {
            //if (((direction == Direction.Clockwise) && (last_gantry == 3)) || ((direction == Direction.AntiClockwise) && (last_gantry == 1)))
            //if (laps == 2)
            //{
            buggyAction();
            Program.print(" going to park lane");
            //comms.send(ID, "PARK");
            comms.send(ID, "STOP");
            Program.print("Buggy " + ID + " stopped! " + laps + " completed!");
            //}
            //else
                //Program.print("Invalid Command! Not safe to park!");
        }
        private void sendDirection()
        {
            if (direction == Direction.AntiClockwise)
            {
                comms.send(ID, "ACLOCK");
            }
            else
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
        private void buggyAction()
        {
            Program.print("Buggy number: " + ID);
        }
    }
}
