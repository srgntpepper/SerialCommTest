using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Runtime.CompilerServices;

namespace SerialCommTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {

                Console.WriteLine("Welcome to the SerialCommTest Application");
                Console.WriteLine("Please follow the prompts.");
                Console.WriteLine("You may exit and any time by typing exit. . .\n\n\n");

                string[] ports = SerialPort.GetPortNames();
                if (ports.Length == 0)
                {
                    Console.WriteLine("No COM Ports have been detected. Check your connections.");
                    Console.WriteLine("Do you want to check again? (Yes/Y or No/N)");
                    string res = Console.ReadLine().Trim().ToLower();

                    if (res[0] == 'n' || res[0] == 'e')
                    {
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }

                Console.WriteLine("\nAvailable COM Ports: ");
                foreach (var port in ports)
                {
                    Console.WriteLine(" > " + port);
                }


                SerialPort serialPort = null;
                while (serialPort == null)
                {

                    Console.WriteLine("Enter COM Port (e.g., COM1) or type 'exit' to end the program: ");
                    string portName = Console.ReadLine().Trim();

                    if (portName.ToLower() == "exit")
                    {
                        return;
                    }

                    if (Array.IndexOf(ports, portName.ToUpper()) != -1)
                    {
                        serialPort = new SerialPort(portName, 9600, Parity.None, 8, StopBits.One);
                    }
                    else
                    {
                        Console.WriteLine("Invalid Entry, please try again.");
                    }
                }

                try
                {
                    serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                    serialPort.Open();
                    Console.WriteLine("Port opened successfully!");

                    Console.WriteLine("Type a command to send to the device or exit to quit: ");
                    while (true)
                    {
                        string command = Console.ReadLine().Trim();
                        if (command.ToLower().Trim().First() == 'e')
                        {
                            break;
                        }
                        serialPort.WriteLine(command + "\n\r");
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }

            Console.WriteLine("Press any key to exit. . .");
            Console.ReadKey();     
        }

        private static void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string inData = sp.ReadExisting();
            Console.WriteLine("Data Received: ");
            Console.WriteLine(inData);
        }
    }
}
