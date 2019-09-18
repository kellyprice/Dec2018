
////class: Splash
////Purpose: Splash Form 
////Author: Rich
////Date: 03/2012

////Version history: 
////YYYY-MM-DD
////Author:
////Description:

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace FAB
{
    public partial class Splash : Form
    {
        
        public Splash()
        {
            InitializeComponent();
            //string s = this.GetType().Assembly.GetName().Version.ToString();
            //lblVersionNo.Text = "Version " + s;
            //GlobalVariables.ApplicationWorkingDirectory = Directory.GetCurrentDirectory();
                

            //Application title
            //lblApplicationTitle.Text = this.GetType().Assembly.GetName().Name.ToString();
                
            lblVersion.Text = "Version: " + this.GetType().Assembly.GetName().Version.ToString();

            //Copyright info
            lblCopyright.Text = "";// "Copyright © FAB 2014";
                                    //lblCopyright.Text = this.GetType().Assembly.GetName().Copyright.ToString();

            label2.Text = "Unauthorized access to this system is forbidden and will be\r\nprosecuted by law. By accessing the system, you agree that your\r\nactions may be monitored if unauthorized usage is suspected.";

            Application.DoEvents();
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, System.EventArgs e)
        {
            Form MyForm = new Login ();
            //Screen screen = Screen.FromPoint(Cursor.Position);
            Screen screen = Screen.FromControl(this);
            MyForm.Location = screen.Bounds.Location;
            MyForm.Show(); 
            timer1.Enabled = false;
            this.Dispose(false);
        }
    }
}
