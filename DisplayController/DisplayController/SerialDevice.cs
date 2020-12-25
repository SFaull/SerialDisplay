using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisplayController
{
    public class SerialDevice
    {
        private SerialPort port;

        public SerialDevice()
        {
            port = null; ;
        }

        ~SerialDevice()
        {
            this.Disconnect();
        }

        static public string[] GetComPorts()
        {
            return SerialPort.GetPortNames();
        }


        public bool Connect(string portName, int baudRate = 115200)
        {
            bool success = false;
            if (!IsConnected())
            {
                port = new SerialPort(portName);
                if (!port.IsOpen)
                {
                    port.BaudRate = baudRate;
                    port.DataBits = 8;
                    port.StopBits = StopBits.One;
                    port.Parity = Parity.None;
                    port.Handshake = Handshake.None;
                    port.DtrEnable = true;
                    port.ReadTimeout = 5000;
                    port.WriteTimeout = 5000;
                    port.Open();

                    success = this.IsConnected();
                }
            }
            return success;
        }

        public void Disconnect()
        {
            if (this.IsConnected())
                port.Close();
        }

        public bool IsConnected()
        {
            if (port != null && port.IsOpen)
                return true;
            return false;
        }


        public string ReadLine()
        {
            String str = "";

            if (IsConnected())
            {
                try
                {
                    str = port.ReadLine();
                    str = str.TrimEnd('\r', '\n');
                }
                catch   // timeout
                {
                    str = "Error";
                }
            }

            Console.WriteLine("Arduino: " + str);
            return str;
        }

        public void WriteLine(string str)
        {
            str = str + "\r";   // add a CR
            if (IsConnected())
            {
                try
                {
                    port.DiscardOutBuffer();
                    port.DiscardInBuffer();
                    Console.WriteLine("PC: " + str);
                    port.WriteLine(str);
                }
                catch   // timeout
                {
                    // write error/timeout
                    Console.WriteLine("PC: [Write Timout]");
                }
            }
        }

        public string SendRequestCommand(string cmd)
        {
            this.WriteLine(cmd);
            return this.ReadLine();
        }

        public void SendExecuteCommand(string cmd)
        {
            this.WriteLine(cmd);
        }

        public void SendBytes(byte[] byteArray)
        {
            if (IsConnected())
            {
                try
                {
                    port.Write(byteArray, 0, byteArray.Length);
                }
                catch   // timeout
                {
                    Console.WriteLine("failed");
                }
            }
        }
    }
}
