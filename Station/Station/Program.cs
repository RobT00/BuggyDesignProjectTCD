using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Station
{
    class Program
    {

        /// <summary>
        /// Parses raw string into command and message
        /// </summary>
        /// <param name="s">The input string</param>
        /// <param name="command">Command contatined in the string</param>
        /// <param name="message">Message contatined in the string</param>
        /// <param name="count">Number of times the command is to be executed</param>
        /// <returns>Whether parsing was successful</returns>
        static bool parseMessage(string s, out string command, out string message, out int count)
        {
            count = 1;
            command = s;
            message = "";
            if (s == null || s.Length == 0 || !s.Contains(":"))
            {
                return false;
            }
            command = s.Substring(0, s.IndexOf(':'));
            message = s.Substring(s.IndexOf(':') + 1);
            count = Math.Max(1, message.TakeWhile(char.IsDigit)
                    .Reverse()
                    .Select((char c, int index) => ((int)Math.Pow(10, index)) * (c - '0'))
                    .Sum());
            message = message.Substring(message.TakeWhile((char c) => char.IsDigit(c) || char.IsWhiteSpace(c)).Count());
            return true;
        }

        static void Main(string[] args)
        {
            bool isRunning = true;
            while (isRunning)
            {
                Console.WriteLine("Enter command:##message (valid commands: 'print', 'reversePrint', 'stop')");
                string command, message;
                int count;
                if (!parseMessage(Console.ReadLine(), out command, out message, out count))
                {
                    Console.WriteLine("Invalid command: " + command);
                }
                else if (command == "print")
                {
                    for (int i = 0; i < count; i++)
                    {
                        Console.WriteLine(message);
                    }
                }
                else if (command == "reversePrint")
                {
                    for (int i = 0; i < count; i++)
                    {
                        Console.WriteLine(string.Concat<char>(message.Reverse()));
                    }
                }
                else if (command == "stop")
                {
                    Console.WriteLine("Stopping");
                    isRunning = false;
                }
                else
                {
                    Console.WriteLine("Invalid command: " + command);
                }

                // Hold execution of the loop until user input
                Console.ReadLine();
            }
        }
    }
}
