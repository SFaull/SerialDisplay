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
        DisplayController device;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            device.WriteImage(@"D:\Sam\Documents\Git\XiaoDisplay\Capture.PNG");
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            device = new DisplayController();
            bool success = device.Connect("COM20");
            if (!success)
            {
                MessageBox.Show("Failed");
                return;
            }
        }
        private void btnRead_Click(object sender, EventArgs e)
        {
            string response = device.ReadLine();
            MessageBox.Show(response);
        }

        private void btnIDN_Click(object sender, EventArgs e)
        {
            string response = device.SendRequestCommand("*IDN?");

            MessageBox.Show(response);
        }

        private void btnGetBuff_Click(object sender, EventArgs e)
        {
            string response = device.SendRequestCommand("BUFFER? 0 100");
            MessageBox.Show(response);
        }
    }
}
