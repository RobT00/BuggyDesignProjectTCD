using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static String message = "";
        static bool isRunning = true;
        public static string Reverse(string s)
        {
            //Console.WriteLine("Test");
            //Console.WriteLine("test" + s);
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
        public static int Times(string s)
        {
            int times = 0;
            if (char.IsDigit(message[0])) {
                while ((message.Length > 0) && (char.IsDigit(message[0]))){
                    //Console.WriteLine("11 " + times);
                    //Console.WriteLine(char.IsDigit(message[0]));
                    times *= 10;
                    times += int.Parse(message.Substring(0,1));
                    //Console.WriteLine(times);
                    //Console.WriteLine(message[0]);
                    message = message.Substring(1, message.Length-1);
                    //Console.WriteLine(message[0]);
                    //Console.WriteLine(message);
                }
                //Console.WriteLine(message);
                //Console.WriteLine(times);
            }
            //Console.WriteLine(times);
            return times;
        }
        static void Main(string[] args)
        {
            while (isRunning)
            {
                int number = 0;
                Console.WriteLine("Enter control message: print:, reversePrint: or stop:  ");
                message = Console.ReadLine();
                //determine message

                if (message.StartsWith("print:"))
                {
                    //int number = Int32.Parse(message.Substring(6, 1));
                    message = message.Substring(6);
                   // Console.WriteLine(message);
                    number = Times(message);
                    //Console.WriteLine(number);
                    for (int i = 0; i < number; i++)
                    {
                        Console.WriteLine("print message: " + message);
                    }
                    Console.WriteLine("Message was printed " + number + " times");
                }
                else if (message.StartsWith("stop:"))
                {
                    Console.WriteLine("message: stop");
                    isRunning = false;
                }
                else if (message.StartsWith("reversePrint:"))
                {
                    //number = Int32.Parse(message.Substring(13, 1));
                    //Console.WriteLine("test");
                    message = message.Substring(13);
                    number = Times(message);
                    //Console.WriteLine(message);
                    message = Reverse(message);
                    for (int i = 0; i < number; i++)
                    {
                        Console.WriteLine("reversePrint message: " + message);
                    }
                    Console.WriteLine("Message was printed " + number + " times");
                }
                else
                {
                    Console.WriteLine("message " + message + " is not valid");
                }
            }
            //Console.WriteLine("You wrote: " + message);

            //Stop program ending immeditately from running with F5
            //Console.ReadLine();
        }
    }
}
