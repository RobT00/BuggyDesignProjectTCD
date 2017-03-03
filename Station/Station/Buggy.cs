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
        private Station station;
        private int requiredLaps = 0;

        public Buggy(int ID, Direction direction, Station station, Communications comms)
        {
            this.ID = ID;
            this.direction = direction;
            this.comms = comms;
            this.station = station;

            sendDirection();
        }
        public void setRequiredLaps(int laps)
        {
            requiredLaps = laps;
        }
        public void go()
        {
            //if (laps == requiredLaps)
            //    return;
            //else
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
            //buggyAction();
            //Program.print(" stopped at Gantry: " + gantry_num);
            Thread.Sleep(1000);
            last_gantry = gantry_num;
            if (((direction == Direction.Clockwise) && (gantry_num == 2)) || ((direction == Direction.AntiClockwise) && (gantry_num == 1)))
                laps++;
            
            go();
            trackState("Gantry", gantry_num);
            if (direction == Direction.Clockwise)
            {
                if (laps == requiredLaps) //Based on our system, clockwise will be counted as doing one extra lap
                    goPark();
                else if (last_gantry == 3)
                {
                    stop();
                    station.buggySwitch(ID);
                }
            }
            else if (direction == Direction.AntiClockwise)
            {
                if (gantry_num == 1)
                {
                    goPark();
                }
            }
            //For Bronze
            //if (laps == 2)
            //    goPark();
        }
        public void goPark()
        {
            comms.send(ID, "PARK");    
        }
        public void buggyParked()
        {
            if (direction == Direction.AntiClockwise)
            {
                station.buggySwitch(ID);
                //if (laps < requiredLaps)
                Program.print("Buggy " + ID + " is in the park lane");
            }
            else
                Program.print("Buggy " + ID + " parked! " + (laps - 1) + " lap(s) completed!");
            //Environment.Exit(0); This exits the program
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
        public void going()
        {
            //int x = last_gantry;
            //if (last_gantry == 0)
            //    x = earlyAction();
            //trackState("Go", x);
        }
        public void stopped()
        {
            //int x = last_gantry;
            //if (last_gantry == 0)
            //    x = earlyAction();
            //trackState("Stop", x);
        }
        private void buggyAction()
        {
            Console.Write("Buggy number: " + ID);
        }
        private void trackState(string call, int num)
        {
            buggyAction();
            if (direction == Direction.AntiClockwise)
            {
                if (num == 1)
                    num = num + 2;
                else
                    num--;
            }
            if (call == "Gantry") {
                Console.Write((" stopped at gantry " + last_gantry + " Entering track section: "));
                if (laps == requiredLaps || (direction == Direction.AntiClockwise && last_gantry == 1)) {
                    Console.WriteLine(("Park Lane"));
                    return;
                }
                Console.WriteLine((num.ToString())); 
            }
            else if (call == "Stop")
            {
                Console.WriteLine((" has stopped in section " + num));
            }
            else if (call == "Go")
            {
                Console.WriteLine((" is on the move in section " + num));
            }
         }
        private int earlyAction()
        {
            int a;
            {
                if (direction == Direction.AntiClockwise)
                    a = 2;
                else
                    a = 3;
            }
            return a;
        }
    }
}
