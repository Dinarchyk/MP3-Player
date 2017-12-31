using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MP3_Player
{
    public partial class HelpForm : Form
    {
        private static double Time; 
        public HelpForm()
        {
            InitializeComponent();
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Time++;
            if (Time == 1)
            {
                BackColor = Color.Aqua;
                label1.Visible = true;
            }
            if (Time == 2)
            {
                BackColor = Color.Firebrick;
                label2.Visible = true;
            }
            if (Time == 3)
            {
                BackgroundImage = global::MP3_Player.Properties.Resources.Help;
                label3.Visible = true;
                timer1.Stop();
                Time = 0;
            }
        }

        private void HelpForm_Load(object sender, EventArgs e)
        {

        }
    }
}
