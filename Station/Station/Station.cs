using ConsoleApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Station
{
    class Station
    {
        private Buggy buggy;
        private Communications comms;

        public Station()
        {
            comms = new Communications();
            buggy = new Buggy(1, Direction.Clockwise, comms);

            comms.setDefaultHandler(defaultCommandHandler);
            comms.addCommand("PING", (int ID) => getBuggyForID(ID)?.pingRecieved());
            comms.addCommand("PONG", (int ID) => getBuggyForID(ID)?.pongRecieved());
            comms.addCommand("GOING", (int ID) => getBuggyForID(ID)?.going());
            comms.addCommand("STOPPED", (int ID) => getBuggyForID(ID)?.stopped());
            comms.addCommand("GANTRY1", (int ID) => getBuggyForID(ID)?.onGantry(1));
            comms.addCommand("GANTRY2", (int ID) => getBuggyForID(ID)?.onGantry(2));
            comms.addCommand("GANTRY3", (int ID) => getBuggyForID(ID)?.onGantry(3));
            comms.addCommand("PARKED", (int ID) => getBuggyForID(ID)?.buggyParked());
            comms.addCommand("OBSTACLE", (int ID) => getBuggyForID(ID)?.stopped()); //may make new function to state the obstacle caused stop
            comms.addCommand("PATHCLEAR", (int ID) => getBuggyForID(ID)?.going()); //same as above
        }
        public Buggy getBuggyForID(int ID)
        {
            if (ID == 1)
            {
                return buggy;
            }
            return null;
        }

        public void defaultCommandHandler(int ID, string command)
        {
            Program.print("Invalid message recieved from buggy " + ID + ": " + command);
        }
    }
}
