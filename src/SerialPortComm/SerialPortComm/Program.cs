using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SerialPortComm
{
    class Program
    {
        static void Main(string[] args)
        {
            SerialPort com = new SerialPort("COM12", 9600, Parity.None, 8, StopBits.One);
            com.Open();

            for (int x = 0; true; x++)
            {
                // Thread.Sleep(2000);
                // Console.WriteLine("Data " + x);
                // com.Write("Data " + x);
                Console.Write("Message : ");
                com.Write(Console.ReadLine());
            }
        }
    }
}
