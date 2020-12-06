using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DisplayController
{
    public partial class Form1 : Form
    {
        Bitmap bitmap = new Bitmap(@"D:\Sam\Documents\Git\XiaoDisplay\spotify-logo-png-7053.png");
        SerialDevice serialDevice;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            int blockSize = 60;
            int xOffset = 0;
            int yOffset = 0;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    string cmd = string.Format("DISPLAY {0} {1} {2} {3}", xOffset, yOffset, blockSize, blockSize);
                    serialDevice.SendExecuteCommand(cmd);

                    Int16[] pixArray = new Int16[blockSize * blockSize];
                    int index = 0;
                    int xFinish = xOffset + blockSize;
                    int yFinish = yOffset + blockSize;

                    for (int y = yOffset; y < yFinish; y++)
                    {
                        for (int x = xOffset; x < xFinish; x++)
                        {
                            Color clr = bitmap.GetPixel(x, y);

                            // convert the colour to r5g6b5
                            pixArray[index] |= (Int16)((clr.R & 0b11111000) << 8);
                            pixArray[index] |= (Int16)((clr.G & 0b11111100) << 3);
                            pixArray[index] |= (Int16)((clr.B & 0b11111000) >> 3);
                            index++;
                        }
                    }

                    for(int a = 0; a < pixArray.Length; a++)
                    {
                        pixArray[a] = SwitchEndianness(pixArray[a]);
                    }

                    byte[] result = new byte[pixArray.Length * sizeof(Int16)];                    
                    Buffer.BlockCopy(pixArray, 0, result, 0, result.Length);

                    serialDevice.SendBytes(result);
                    //MessageBox.Show(serialDevice.ReadLine());

                    xOffset += 60;
                }
                yOffset += 60;
                xOffset = 0;
            }
        }

        public Int16 SwitchEndianness(Int16 i)
        {
            return (Int16)((i << 8) + (i >> 8));
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            serialDevice = new SerialDevice();
            bool success = serialDevice.Connect("COM20");
            if (!success)
            {
                MessageBox.Show("Failed");
                return;
            }
        }
        private void btnRead_Click(object sender, EventArgs e)
        {
            string response = serialDevice.ReadLine();
            MessageBox.Show(response);
        }

        private void btnIDN_Click(object sender, EventArgs e)
        {
            string response = serialDevice.SendRequestCommand("*IDN?");

            MessageBox.Show(response);
        }

        private void btnGetBuff_Click(object sender, EventArgs e)
        {
            string response = serialDevice.SendRequestCommand("BUFFER? 0 100");
            MessageBox.Show(response);
        }
    }
}
