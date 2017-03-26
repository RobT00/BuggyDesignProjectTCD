﻿using System;
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
        private volatile bool motion = false;
        private Thread onlineThread = null;
        private volatile bool onlineThreadShouldRun = false;

        public Buggy(int ID, Direction direction, Station station, Communications comms)
        {
            this.ID = ID;
            this.direction = direction;
            this.comms = comms;
            this.station = station;
        }
        public void startOnlineCheck()
        {
            if (onlineThread != null)
                return;
            onlineThread = new Thread(new ThreadStart(() =>
            {
                while (onlineThreadShouldRun)
                {
                    try
                    {
                        Thread.Sleep(1000);
                        bool firstTry = syn();
                        if (!firstTry)
                        {
                            buggyAction("is Back Online!");
                            if (motion)
                            {
                                go();
                            }
                        }
                    } catch (ThreadInterruptedException e) {}
                }
            }));
            onlineThreadShouldRun = true;
            onlineThread.Start();
        }
        public void stopOnlineCheck()
        {
            if (onlineThread == null)
                return;
            onlineThreadShouldRun = false;
            onlineThread.Interrupt();
            onlineThread.Join();
        }
        public void setRequiredLaps(int laps)
        {
            requiredLaps = laps;
        }
        public ConsoleColor getColour()
        {
            if (ID == 1)
                return ConsoleColor.DarkBlue;
            else
                return ConsoleColor.DarkRed;
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
        public bool syn(bool silent = false)
        {
            return comms.send(ID, "SYN", () =>
                    {
                        if (!silent)
                            buggyAction("is Offline! \nWill keep pinging buggy " + ID);
                    });
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
            trackState("Gantry");
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
        public void obstacle(string condition)
        {
            trackState(condition);
        }
        public void buggyParked()
        {
            motion = false;
            if (direction == Direction.AntiClockwise)
            {
                station.buggySwitch(ID);
                buggyAction(" is in the park lane");
            }
            else
            {
                if (station.getNumberOfBuggies() == 1)
                    buggyAction("parked! " + (laps) + " lap(s) completed!");
                else
                    buggyAction("parked! " + (laps - 1) + " lap(s) completed!");
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
        private void buggyAction(String command = "")
        {
            Program.print("Buggy " + ID + " " + command, getColour());
        }
        private void trackState(string call)
        {
            int section;
            if (direction == Direction.Clockwise)
                section = last_gantry;
            else
            {
                if (last_gantry == 1)
                    section = 3;
                else
                    section = last_gantry - 1;
            }
            if (call == "Gantry") {
                onLap();
                if (last_gantry == -10)
                    buggyAction("gantry interpreted as invalid");
                else
                {
                    buggyAction("stopped at gantry " + last_gantry);

                    string sectionString;
                    if ((direction == Direction.Clockwise && laps >= requiredLaps && last_gantry == 2)
                        || (direction == Direction.AntiClockwise && last_gantry == 1))
                        sectionString = "Park Lane";
                    else
                        sectionString = section.ToString();
                    buggyAction("entering track section: " + sectionString);
                }
            }
            else if (call == "Stop")
            {
                buggyAction("has stopped in section " + section);
            }
            else if (call == "Go")
            {
                buggyAction("is on the move in section " + section);
            }
            else if (call == "OBSTACLE")
            {
                buggyAction("has detected an obstacle in section " + section);
            }
            else if (call == "PATHCLEAR")
            {
                buggyAction("is now able to progress in section " + section);
            }
        }
        private void onLap()
        {
            buggyAction("is on lap " + laps);
        }
    }
}
