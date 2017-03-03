using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Station
{
    class Station
    {
        private Buggy buggy1;
        private Buggy buggy2;
        private Communications comms;

        public Station()
        {
            comms = new Communications();
            buggy1 = new Buggy(1, Direction.Clockwise, this ,comms);
            buggy2 = new Buggy(2, Direction.AntiClockwise, this, comms);

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
                return buggy1;
            }
            if (ID == 2)
                return buggy2;
            return null;
        }

        public void defaultCommandHandler(int ID, string command)
        {
            Program.print("Invalid message recieved from buggy " + ID + ": " + command);
        }
        public void buggySwitch(int ID)
        {
            if (ID == 1)
                getBuggyForID(2)?.go();
            else
                getBuggyForID(1)?.go();
        }
        public void setNumberOfLabs(int laps)
        {
            buggy1.setRequiredLaps(laps + 1);
            buggy2.setRequiredLaps(laps);
        }
    }
}
