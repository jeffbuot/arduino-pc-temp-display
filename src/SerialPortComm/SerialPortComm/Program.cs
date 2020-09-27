using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenHardwareMonitor.Hardware;

namespace SerialPortComm
{
    class Program
    {
        public class UpdateVisitor : IVisitor
        {
            public void VisitComputer(IComputer computer)
            {
                computer.Traverse(this);
            }
            public void VisitHardware(IHardware hardware)
            {
                hardware.Update();
                foreach (IHardware subHardware in hardware.SubHardware) subHardware.Accept(this);
            }
            public void VisitSensor(ISensor sensor) { }
            public void VisitParameter(IParameter parameter) { }
        }
        static string GetSystemInfo()
        {
            UpdateVisitor updateVisitor = new UpdateVisitor();
            Computer computer = new Computer();
            computer.Open();
            computer.CPUEnabled = true;
            computer.GPUEnabled = true;
            computer.RAMEnabled = true;
            computer.Accept(updateVisitor);
            float cl = 0, ct = 0, gl = 0, gt = 0, rl = 0;
            for (int i = 0; i < computer.Hardware.Length; i++)
            {
                if (computer.Hardware[i].HardwareType == HardwareType.CPU)
                {
                    for (int j = 0; j < computer.Hardware[i].Sensors.Length; j++)
                    {
                        if (computer.Hardware[i].Sensors[j].SensorType == SensorType.Temperature)
                        {
                            if (computer.Hardware[i].Sensors[j].Name == "CPU Package")
                            {
                                ct = (float)computer.Hardware[i].Sensors[j].Value;
                            }
                        }
                        if (computer.Hardware[i].Sensors[j].SensorType == SensorType.Load)
                        {
                            if (computer.Hardware[i].Sensors[j].Name == "CPU Total")
                            {
                                cl = (float)computer.Hardware[i].Sensors[j].Value;
                            }
                        }
                    }
                }
                if (computer.Hardware[i].HardwareType == HardwareType.GpuNvidia || computer.Hardware[i].HardwareType == HardwareType.GpuAti)
                {
                    for (int j = 0; j < computer.Hardware[i].Sensors.Length; j++)
                    {
                        if (computer.Hardware[i].Sensors[j].SensorType == SensorType.Temperature)
                        {
                            if (computer.Hardware[i].Sensors[j].Name == "GPU Core")
                            {
                                gt = (float)computer.Hardware[i].Sensors[j].Value;
                            }
                        }
                        if (computer.Hardware[i].Sensors[j].SensorType == SensorType.Load)
                        {
                            if (computer.Hardware[i].Sensors[j].Name == "GPU Core")
                            {
                                gl = (float)computer.Hardware[i].Sensors[j].Value;
                            }
                        }
                    }
                }
                if (computer.Hardware[i].HardwareType == HardwareType.RAM)
                {
                    for (int j = 0; j < computer.Hardware[i].Sensors.Length; j++)
                    {
                        if (computer.Hardware[i].Sensors[j].SensorType == SensorType.Load)
                        {
                            if (computer.Hardware[i].Sensors[j].Name == "Memory")
                            {
                                rl = (float)computer.Hardware[i].Sensors[j].Value;
                            }
                        }
                    }
                }
            }
            computer.Close();
           // return $"CPU {cl:0.##}% {ct:0.##}C\nGPU {gl:0.##}% {gt:0.##}C\nRAM {rl:0.##}%";
            return $"CPU {ct:0.##}C\nGPU {gt:0.##}C\nRAM {rl:0.##}%";
        }
        static void Main(string[] args)
        {
            Console.Write("Please enter port name (e.g. COM12): ");
            SerialPort com = new SerialPort(Console.ReadLine(), 9600, Parity.None, 8, StopBits.One);
            com.Open();

            ;
            for (; true;)
            {
                Thread.Sleep(2000);
                // Console.WriteLine("Data " + x);
                // com.Write("Data " + x);
                // Console.Write("Message : ");
                string inf = GetSystemInfo();
                Console.Clear();
                Console.WriteLine(inf);
                com.Write(inf);
            }
        }
    }
}
