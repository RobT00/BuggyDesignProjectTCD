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
