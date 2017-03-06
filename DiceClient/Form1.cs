using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace DiceClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Prior checks

            if (User_Name.Text.Length <= 3)
            {
                MessageBox.Show("Name too short!");
                return;
            }

            try 
            {
                IPAddress.Parse(IP_Address.Text);
            }
            catch
            {
                MessageBox.Show("IP address not valid");
                return;
            }

            // end of checks

            // Start calculation
            string R = GetResult(DiceCombo.SelectedIndex);
            //R will contain "index|results"

            //show results
            label1.Text = "Results: " + R.Split('|')[1];

            //Send results to server; format "name|index|results"
            DataExchange D = new DataExchange(IP_Address.Text, 44455);
            if (!D.Send(User_Name.Text + "|" + R)) MessageBox.Show("connection error");
            else
            {
                button1.Enabled = false;
                timer1.Start();
            }

            return;
        }

        private int GetValue()
        {
            int a;
            int min = 10;
            int tot = 0;

            for (int x = 0; x < 4; x++)
            {
                a = Convert.ToInt32(Roll(6, 1));
                if (a < min) min = a;
                tot += a;
            }

            tot -= min;
            System.Threading.Thread.Sleep(Convert.ToInt32(Roll(80, 1)));

            return tot;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            button1.Enabled = true;
            timer1.Stop();
        }

        private void F1_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            timer1.Interval = 2000;
            DiceCombo.SelectedIndex = 0;
        }

        private string GetResult(int index)
        {
            string res = index + "|";
            Random R = new Random();

            switch (index)
            {
                //flip a coin
                case 0:
                    if (R.Next(0, 2) == 0) res += "Cross";
                    else res += "Head";
                    break;
                //Roll 1d3
                case 1:
                    res += Roll(3, 1);
                    break;
                //Roll 1d4
                case 2:
                    res += Roll(4, 1);
                    break;
                //Roll 1d6
                case 3:
                    res += Roll(6, 1);
                    break;
                //Roll 1d8
                case 4:
                    res += Roll(8, 1);
                    break;
                //Roll 1d10
                case 5:
                    res += Roll(10, 1);
                    break;
                //Roll 1d12
                case 6:
                    res += Roll(12, 1);
                    break;
                //Roll 1d20
                case 7:
                    res += Roll(20, 1);
                    break;
                //Roll 1d30
                case 8:
                    res += Roll(30, 1);
                    break;
                //Roll 1d100
                case 9:
                    res += Roll(100, 1);
                    break;
                //Roll all characteristics
                case 10:
                    for (int cycle = 0; cycle < 6; cycle++)
                    {
                        if (cycle > 0) res += " - ";
                        res += GetValue();
                    }
                    break;
                //Roll 2d4
                case 11:
                    res += Roll(4, 2);
                    break;
                //Roll 3d4
                case 12:
                    res += Roll(4, 3);
                    break;
                //Roll 2d6
                case 13:
                    res += Roll(6, 2);
                    break;
                //Roll 3d6
                case 14:
                    res += Roll(6, 3);
                    break;
                //Roll 4d6
                case 15:
                    res += Roll(6, 4);
                    break;
                //Roll 5d6
                case 16:
                    res += Roll(6, 5);
                    break;
                //Roll 6d6
                case 17:
                    res += Roll(6, 6);
                    break;
            }

            return res;
        }

        private string Roll (int dice, int rolls)
        {
            Random R = new Random();
            string res = "";

            int[] a = new int[rolls];
            int tot = 0;
            for (int cycle = 0; cycle < rolls; cycle++)
            {
                a[cycle] = R.Next(1, (dice + 1));
                if (cycle > 0) res += " + ";
                res += a[cycle];
                tot += a[cycle];
            }

            if (rolls > 1) res += " = " + tot;
            return res;
        }
    }
}
