using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment_1_2e10
{
    class Program
    {
        static string message = "";
        static bool isRunning = true;

        static void Main(string[] args)
        {
            while(isRunning)
            {
                Console.WriteLine("Please enter control message: 'print:X' or 'reversePrint:X' or 'end'");
                message = Console.ReadLine();
                
                if(message.StartsWith("print:"))
                {

                    int number = Int32.Parse(message.Substring(6, 1));
                    for(int i=0; i<number; i++)
                    {
                        Console.WriteLine("print message: " + message.Substring(7));
                    }
                    
                   
                }
                else if (message.StartsWith("reversePrint:"))
                {
                    int number = Int32.Parse(message.Substring(13, 1));
                    for(int i=0; i<number; i++)
                    {
                        char[] array = message.Substring(14).ToCharArray();
                        string reverse = string.Empty;
                        for (int j = array.Length - 1; j > -1; j--)
                        {
                            reverse += array[j];
                        }
                        Console.WriteLine("print message: " + reverse);
                    }
                }
                else if (message.StartsWith("end"))
                {
                    Console.WriteLine("message: end");
                    isRunning = false;
                }
                else
                {
                    Console.WriteLine("message: " + message + "not valid");
                }
            }
            Console.ReadLine();
        }
    }
}
