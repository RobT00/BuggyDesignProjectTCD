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
        private bool muted = false;
        private bool motion = false;
        private Thread onlineThread;

        public Buggy(int ID, Direction direction, Station station, Communications comms)
        {
            this.ID = ID;
            this.direction = direction;
            this.comms = comms;
            this.station = station;

            onlineThread = new Thread(new ThreadStart(() =>
            {
                while (true)
                {
                    Thread.Sleep(1000);
                    bool firstTry = syn();
                    if (!firstTry)
                    {
                        buggyAction("Back online");
                        if (motion)
                        {
                            go();
                        }
                    }
                }
            }));
            onlineThread.Start();
        }
        public void setRequiredLaps(int laps)
        {
            requiredLaps = laps;
        }
        public void mute()
        {
            muted = true;
        }
        public void unmute()
        {
            muted = false;
        }
        public void go()
        {
            motion = true;
            comms.send(ID, "GO");
        }
        public void stop()
        {
            motion = false;
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
        public bool syn()
        {
            return comms.send(ID, "SYN", () => { buggyAction("Offline"); });
        }
        public void onGantry(int gantry_num)
        {
            stop();
            Thread.Sleep(1000);

            if (((direction == Direction.Clockwise) && (gantry_num == 2)) || ((direction == Direction.AntiClockwise) && (gantry_num == 1)))
                laps++;
            else if (((direction == Direction.Clockwise) && (last_gantry == 1) && (gantry_num != 2)) 
                || ((direction == Direction.AntiClockwise) && (last_gantry == 2) && (gantry_num != 1))) {
                laps++;
            }
            go();
            last_gantry = gantry_num;
            trackState("Gantry", gantry_num);
            if (station.getNumberOfBuggies() < 2)
            {
                if (laps >= requiredLaps && gantry_num == 2)
                    goPark();
            }
            else
            {
                if (direction == Direction.Clockwise)
                {
                    if (laps >= requiredLaps && gantry_num == 2) //Based on our system, clockwise will be counted as doing one extra lap
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
            }
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
                Program.print("Buggy " + ID + " is in the park lane");
            }
            else
            {
                if (station.getNumberOfBuggies() == 1)
                    Program.print("Buggy " + ID + " parked! " + (laps) + " lap(s) completed!");
                else
                    Program.print("Buggy " + ID + " parked! " + (laps - 1) + " lap(s) completed!");
            }
        }
        public void pingRecieved()
        {
            buggyAction("PING recieved");
        }
        public void pongRecieved()
        {
            buggyAction("PONG recieved");
        }
        public void going()
        {
            buggyAction("GOING");
        }
        public void stopped()
        {
            buggyAction("STOPPED");
        }
        private void buggyAction(String command = null)
        {
            if (!muted)
            {
                if (command == null)
                    Console.Write("> Buggy " + ID + ": " + command);
                else
                    Console.WriteLine("> Buggy " + ID + ": " + command);
            }
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
                onLap();
                if (num == -10)
                    Console.Write(" gantry interpreted as invalid");
                else
                    Console.Write((" stopped at gantry " + last_gantry + " Entering track section: "));
                if ((laps >= requiredLaps && last_gantry == 2) 
                    || (direction == Direction.AntiClockwise && last_gantry == 1)) {
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
        private void onLap()
        {
            buggyAction();
            Console.WriteLine("is on lap " + laps);
        }
    }
}
