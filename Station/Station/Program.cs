using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week_2_Lab_Z8_2
{
    class Program
    {
        static string message = "";
        
        static bool isRunning = true;
        static void Main(string[] args)
        {
           while (isRunning)
            {
                Console.WriteLine("Please enter your control message: 'print:X' or 'stop' or 'reversePrint:Xmessage'");

                message = Console.ReadLine();
                string input = message;

                if (message.StartsWith("print:"))
                {
                    int number = Int32.Parse(message.Substring(6, 1));

                    for(int i=0; i<number; i++)
                    {
                        Console.WriteLine("print message: " + message.Substring(6));

                    }
                }

                else if (message.StartsWith("stop"))
                    {
                        Console.WriteLine("message: stop");
                        isRunning = false;
                    }
                else if (message.StartsWith("reversePrint"))
                {
                    
                    string message = new string(input.ToCharArray().Reverse().ToArray());
                    Console.WriteLine("print message: " + message);
                }
                else
                {
                        Console.WriteLine("message: " + message + " not valid");
                     }
                }

                Console.ReadLine();
            }
        }
}

