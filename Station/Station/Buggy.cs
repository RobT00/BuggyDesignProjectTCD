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
            int x = last_gantry;
            if (last_gantry == 0)
                x = earlyAction();
            trackState("Go", x);
        }
        public void stop()
        {
            comms.send(ID, "STOP");
            int x = last_gantry;
            if (last_gantry == 0)
                x = earlyAction();
            trackState("Stop", x);
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
            if (((direction == Direction.Clockwise) && (gantry_num == 3)) || ((direction == Direction.AntiClockwise) && (gantry_num == 1)))
                laps++; //Wizard of OZ Demo requires travelling in clockwise direction
            go();
            trackState("Gantry", gantry_num);
            if (laps == 2) //Wizard of OZ Demo requires to park after 2 laps
                goPark();
        }
        private void goPark()
        {
            if (direction == Direction.Clockwise)
                comms.send(ID, "PARKRIGHT"); //Left override will need to be triggered
            else
                comms.send(ID, "PARKLEFT"); //Trigger right override
            
            //Alternatively could send "RIGHT"/"LEFT"?
            //Perhaps there should be a delay here?
            //comms.send(ID, "STOP");
            
            /*
            We could also let the buggy Park/Stop itself, when goPark is triggered, the byggy will turn/override in the desired
            direction when both eyes see black (override should stop when an eye sees white again, it has turned)
            At the next instance of both eyes seeing black, stop the buggy, the buggy is now parked
             */
            //Wait for message from buggy that it has parked
            //}
            //else
                //Program.print("Invalid Command! Not safe to park!");
        }
        public void buggyParked()
        {
            Program.print("Buggy " + ID + " parked! " + laps + " completed!");
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
        private void buggyAction()
        {
            Program.print("Buggy number: " + ID);
        }
        private void trackState(string call, int num)
        {
            //Is there a way to tell what function has called this?
            buggyAction();
            if (direction == Direction.AntiClockwise)
            {
                if (num == 1)
                    num = num + 2;
                else
                    num--;
            }
            if (call == "Gantry") {
                Program.print(" stopped at gantry " + last_gantry + " Entering track section: ");
                if (laps == 2) {
                    Program.print("Park Lane");
                    return;
                }
                Program.print(num.ToString()); 
            }
            else if (call == "Stop")
            {
                Program.print(" has stopped in section " + num);
            }
            else if (call == "Go")
            {
                Program.print(" is on the move in section " + num);
            }
            //I will edit this function so that it will be capable of printing out the track state information for both buggis when the time arises
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
