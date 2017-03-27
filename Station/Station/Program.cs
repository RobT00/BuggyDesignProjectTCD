﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Station
{
    class Program
    {
        private static object printLock = new Object();
        private static List<string> inputBuffer = new List<string>();
        private static int inputBufferIndex = -1;
        private static ConsoleColor inputColour = ConsoleColor.DarkGreen;
        private static ConsoleColor emptyColour = ConsoleColor.Black;

        static void Main(string[] args)
        {
            Console.WriteLine("Initialising...");
            Station station = new Station();
            while (true)
            {
                string input = readInput();

                if (input == "EXIT")
                    Environment.Exit(0);
                if (input == "RESET")
                {
                    station.setUp();
                    continue;
                }

                if (input.Length < 3)
                {
                    print("Station: Message too short");
                    continue;
                }
                if (!Char.IsDigit(input[0]))
                {
                    print("Station: Start message with reciever ID");
                    continue;
                }

                int ID = input[0] - '0';
                if (station.getBuggyForID(ID) == null)
                {
                    print("Station: No buggy with given ID");
                }

                string command = input.Substring(2);
                if (!executeCommand(station, command, ID))
                {
                    print("Station: No such command");
                }
            }
        }

        private static bool executeCommand(Station station, string command, int ID)
        {
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
                case "PARK":
                    station.getBuggyForID(ID)?.goPark();
                    break;

                default:
                    return false;
            }
            return true;
        }
        private static void printInput()
        {
            clearInput();
            lock (printLock)
            {
                Console.CursorLeft = 0;
                Console.BackgroundColor = inputColour;
                Console.Write("> ");
                if (inputBufferIndex != -1)
                    Console.Write(inputBuffer[inputBufferIndex]);
                Console.BackgroundColor = emptyColour;
            }
        }

        /// <summary>
        /// Clear line with black, so the current input is cleared if printed message is shorter
        /// </summary>
        private static void clearInput()
        {
            lock (printLock)
            {
                Console.CursorLeft = 0;
                Console.BackgroundColor = emptyColour;
                int length = 2;
                if (inputBufferIndex != -1)
                    length += inputBuffer[inputBufferIndex].Length;
                for (int i = 0; i < length; i++)
                    Console.Write(" ");
                Console.CursorLeft = 0;
            }
        }

        private static string readInput()
        {
            clearInput();

            inputBuffer.Add("");
            inputBufferIndex = inputBuffer.Count - 1;
            printInput();
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                clearInput();
                if ((key.Modifiers & ConsoleModifiers.Alt) == ConsoleModifiers.Alt)
                    continue;
                if ((key.Modifiers & ConsoleModifiers.Control) == ConsoleModifiers.Control)
                    continue;
                if (key.Key == ConsoleKey.UpArrow)
                {
                    inputBufferIndex = Math.Max(0, inputBufferIndex - 1);
                }
                else if (key.Key == ConsoleKey.DownArrow)
                {
                    inputBufferIndex = Math.Min(inputBuffer.Count - 1, inputBufferIndex + 1);
                }
                else
                {
                    // Change last, not index
                    if (inputBufferIndex != inputBuffer.Count - 1) {
                        inputBuffer[inputBuffer.Count - 1] = inputBuffer[inputBufferIndex];
                        inputBufferIndex = inputBuffer.Count - 1;
                    }

                    // Separate inner if statement necessary so that 'Enter' and 'Backspace' keys are never handled by the last branch
                    if (key.Key == ConsoleKey.Enter)
                    {
                        if (inputBuffer.Last().Length != 0)
                        {
                            print(inputBuffer.Last(), inputColour);
                            break;
                        }
                    }
                    else if (key.Key == ConsoleKey.Backspace)
                    {
                        if (inputBuffer.Last().Length != 0) {
                            inputBuffer[inputBuffer.Count - 1] = inputBuffer.Last().Substring(0, inputBuffer.Last().Length - 1);
                        }
                    }
                    else if (key.KeyChar != '\u0000')
                    {
                        inputBuffer[inputBuffer.Count - 1] += key.KeyChar;
                    }
                }
                printInput();
            }

            clearInput();
            return inputBuffer.Last();
        }
        public static void print(string message)
        {
            print(message, emptyColour);
        }
        public static void print(string message, ConsoleColor backgroundColor)
        {
            lock (printLock)
            {
                clearInput();
                Console.CursorLeft = 0;
                Console.BackgroundColor = backgroundColor;
                Console.WriteLine(message);
                printInput();
                Console.BackgroundColor = emptyColour;
            }
        }
    }
}
