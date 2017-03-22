using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Station
{
    class Station
    {
        private Buggy buggy1 = null;
        private Buggy buggy2 = null;
        private Communications comms;
        private int number_of_buggies = 0;

        public Station()
        {
            comms = new Communications();

            comms.setDefaultHandler(defaultCommandHandler);
            comms.addCommand("PING", (int ID) => getBuggyForID(ID)?.pingRecieved());
            comms.addCommand("PONG", (int ID) => getBuggyForID(ID)?.pongRecieved());
            comms.addCommand("GOING", (int ID) => getBuggyForID(ID)?.going());
            comms.addCommand("STOPPED", (int ID) => getBuggyForID(ID)?.stopped());
            comms.addCommand(new Regex(@"^GANTRY(?<GantryID>[123])$"), (int ID, GroupCollection groups) => getBuggyForID(ID)?.onGantry(Int32.Parse(groups["GantryID"].Value)));
            comms.addCommand("GANTRY_INVALID", (int ID) => getBuggyForID(ID)?.onGantry(-10));
            comms.addCommand("PARKED", (int ID) => getBuggyForID(ID)?.buggyParked());
            comms.addCommand("OBSTACLE", (int ID) => getBuggyForID(ID)?.stopped()); //may make new function to state the obstacle caused stop
            comms.addCommand("PATHCLEAR", (int ID) => getBuggyForID(ID)?.going()); //same as above
            comms.addCommand(new Regex(@"^IRLength: (?<Length>\d+)$"), (int ID, GroupCollection groups) => Program.print("Buggy " + ID + " Pulse length: " + groups["Length"].Value));
            comms.addCommand(new Regex(@"^INVALID: (?<Command>.*)$"), (int ID, GroupCollection groups) => Program.print("Buggy " + ID + "received invalid command: " + groups["Command"].Value));
            setUp();
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
            else if (ID == 2)
                getBuggyForID(1)?.go();
            else
                Program.print("Something goofed...", ConsoleColor.Magenta);
        }
        public void setNumberOfLabs(int laps)
        {
            if (getNumberOfBuggies() == 1)
                buggy1.setRequiredLaps(laps);
            else
            {
                buggy1.setRequiredLaps(laps + 1);
                buggy2.setRequiredLaps(laps);
            }
        }
        public void setNumberOfBuggies(int buggies)
        {
            number_of_buggies = buggies;
        }
        public int getNumberOfBuggies()
        {
            return number_of_buggies;
        }
        public void setUp()
        {
            if (buggy1 != null)
                buggy1.stopOnlineCheck();
            if (buggy2 != null)
                buggy2.stopOnlineCheck();

            int buggies = 3;
            int laps = 0;
            while (buggies > 2 || buggies <= 0)
            {
                Console.WriteLine("How many buggies are you using? ");
                Int32.TryParse(Console.ReadLine(), out buggies);
            }
            while (laps <= 0)
            {
                Console.WriteLine("How many laps would you like to do? ");
                Int32.TryParse(Console.ReadLine(), out laps);
            }
            buggy1 = new Buggy(1, Direction.Clockwise, this, comms);
            buggy1.syn(silent: true);
            Program.print("Buggy 1 OK", buggy1.getColour());
            buggy1.startOnlineCheck();
            if (buggies == 2)
            {
                buggy2 = new Buggy(2, Direction.AntiClockwise, this, comms);
                buggy2.syn(silent: true);
                Program.print("Buggy 2 OK", buggy2.getColour());
                buggy2.startOnlineCheck();
            }
            else
                buggy2 = null;
            setNumberOfBuggies(buggies);
            setNumberOfLabs(laps);
        }
    }
}
