using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Station
{
    class Program
    {
        public static int buggies = 3;
        static void Main(string[] args)
        {
            int laps = 0;
            Station station = new Station();
            while (buggies > 2 && buggies <= 0)
            {
                Console.WriteLine("How many buggies are you using? ");
                Int32.TryParse(Console.ReadLine(), out buggies);
            }
            station.setNumberOfBuggies(buggies);
            Console.WriteLine("How many laps would you like to do? ");
            Int32.TryParse(Console.ReadLine(), out laps);
            station.setNumberOfLabs(laps);
            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine();
                if (input.Length < 3)
                {
                    Console.WriteLine("Station: Message too short");
                    continue;
                }
                if (!Char.IsDigit(input[0]))
                {
                    Console.WriteLine("Station: Start message with reciever ID");
                    continue;
                }
                int ID = input[0] - '0';
                if (station.getBuggyForID(ID) == null)
                {
                    Console.WriteLine("Station: No buggy with given ID");
                }
                string command = input.Substring(2);
                switch (command)
                {
                    case "PING":
                        station.getBuggyForID(ID)?.sendPing();
                        break;
                    case "PONG":
                        station.getBuggyForID(ID)?.sendPong();
                        break;
                    case "GO":
                        station.getBuggyForID(ID)?.go();
                        break;
                    case "STOP":
                        station.getBuggyForID(ID)?.stop();
                        break;
                    //case "GANTRY1":
                    //    station.getBuggyForID(ID)?.onGantry(1);
                    //    break;
                    //case "GANTRY2":
                    //    station.getBuggyForID(ID)?.onGantry(2);
                    //    break;
                    //case "GANTRY3":
                    //    station.getBuggyForID(ID)?.onGantry(3);
                    //    break;
                    case "PARK":
                        station.getBuggyForID(ID)?.goPark();
                        break;
                }
            }
        }

        public static void print(string message)
        {
            Console.WriteLine(message);
            Console.Write("> ");
        }
    }
}
