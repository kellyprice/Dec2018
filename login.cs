////class: Login
////Purpose: Login form. Select access based on permission level
////Author: Various
////Date: 07/2012

////Version history: 
////YYYY-MM-DD
////Author:
////Description:

using System;
using System.Configuration;
using System.Windows.Forms;
using System.Security.Principal;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Net;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Collections.Generic;
using DataAccess;
using FAB.Classes;



namespace FAB
{
    public partial class Login : Form
    {
        private bool hasInput = false;
        private bool hasApprover = false;
        private bool hasAdmin = false;
        private bool hasViewer = false;

        public Login()
        {
            InitializeComponent();

            ddnEnvironment.SelectedIndex = 0;//PROD

            Globals.GlobalEnviron = Environ.PROD;

            DataAccess.DataAccess.DAGlobals.GlobEnviron = DataAccess.DataAccess.Environ.PROD;

            PopulateWarning();

            IsInLDAPGroup();

            //txtUsername.Text = WindowsIdentity.GetCurrent().Name.ToUpper();
            //ddnEnvironment.SelectedIndex = 0;
            //txtUsername.Text = "Richard page";

            //DateTime Expiry = new DateTime(2016, 10, 31);
            //if (DateTime.Today > Expiry)
            //{
            //    MessageBox.Show("Application has expired", "Trial period expired", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    Application.Exit();
            //}

        }

        private void PopulateWarning()
        {
            lblWarning.Text = "WARNING\r\n\r\nYOU ARE LOGGING INTO A FIRST ABU DHABI BANK APPLICATION\r\n\r\n";
            lblWarning.Text += "THE PROGRAMS AND DATA STORED ON THIS SYSTEM ARE LICENSED\r\nTO OR ARE THE PROPERTY OF FIRST ABU DHABI BANK. THIS IS A PRIVATE\r\nCOMPUTING SYSTEM FOR USE ONLY BY AUTHORIZED USERS.\r\n\r\n";
            lblWarning.Text += "UNAUTHORIZED ACCESS TO ANY PROGRAM OR DATA ON THIS SYSTEM\r\nIS NOT PERMITTED. BY ACCESSING AND USING THIS SYSTEM YOU ARE CONSENTING\r\nTO SYSTEM MONITORING FOR LAW ENFORCEMENT AND OTHER PURPOSES.\r\n\r\n";
            lblWarning.Text += "UNAUTHORIZED USE OF THIS SYSTEM MAY SUBJECT YOU TO CRIMINAL\r\nPROSECUTION, PENALTIES, AS WELL AS COMPANY INITIATED DISCIPLINARY\r\nPROCEEDINGS.YOU FURTHER ACKNOWLEDGE THAT BY ENTRY INTO THIS SYSTEM, YOU\r\nEXPECT NO PRIVACY FROM MONITORING.\r\n\r\n";
            lblWarning.Text += "IF YOU ARE NOT AN AUTHORISED USER, DISCONNECT IMMEDIATELY.";
        }

        //Dictionary<string, int> dictionary = new Dictionary<string, int>();
        //private void OK_Click(object sender, EventArgs e)
        //{
        //    if (ddnEnvironment.SelectedIndex == -1)
        //    {
        //        MessageBox.Show("Please select an environment and try again", "Select Environment", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        //        return;
        //    }

        //    switch (ddnEnvironment.SelectedIndex)
        //    {
        //        case 0:
        //            Globals.GlobalEnviron = Environ.PROD;
        //            DataAccess.DataAccess.DAGlobals.GlobEnviron = DataAccess.DataAccess.Environ.PROD;
        //            break;
        //        case 1:
        //            Globals.GlobalEnviron = Environ.UAT;
        //            DataAccess.DataAccess.DAGlobals.GlobEnviron = DataAccess.DataAccess.Environ.UAT;
        //            break;
        //    }


        //    Globals.gUserType = DataManager.AuthenticateUser(txtUsername.Text, txtPassword.Text);
        //    Globals._gUserName = txtUsername.Text;

        //    switch (Globals.gUserType)
        //    {
        //        case -1: //No connection
        //            MessageBox.Show("Unable to open a connection to the database", "Database Connection Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            break;
        //        case -2: //user not in table
        //            MessageBox.Show("Username entered does not exist in the database ", "Username does not exist in database", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            break;
        //        case -3: //user locked
        //            MessageBox.Show("Username entered is not Active", "Username is not an Active User in database", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            break;
        //        case 1: //Input
        //            dictionary.Clear();
        //            LoginAudit();
        //            Globals.gReadOnly = false;
        //            Form MyForm = new frmSelect();
        //            //Screen screen = Screen.FromPoint(Cursor.Position);
        //            Screen screen = Screen.FromControl(this);
        //            MyForm.Location = screen.Bounds.Location;
        //            MyForm.Show();
        //            this.Dispose(false);
        //            break;
        //        case 2: //Approver
        //            dictionary.Clear();
        //            LoginAudit();
        //            Globals.gReadOnly = true;
        //            Form MyFrm = new frmSelect();
        //            //Screen screen = Screen.FromPoint(Cursor.Position);
        //            Screen scrn = Screen.FromControl(this);
        //            MyFrm.Location = scrn.Bounds.Location;
        //            MyFrm.Show();
        //            this.Dispose(false);
        //            break;
        //        case 3: //Admin
        //            dictionary.Clear();
        //            LoginAudit();
        //            Form MyFm = new frmUsers();
        //            //Screen screen = Screen.FromPoint(Cursor.Position);
        //            Screen scr = Screen.FromControl(this);
        //            MyFm.Location = scr.Bounds.Location;
        //            MyFm.Show();
        //            this.Dispose(false);
        //            break;
        //        case 4: //Viewer
        //            dictionary.Clear();
        //            LoginAudit();
        //            Globals.gReadOnly = true;
        //            Form MyF = new frmSelect();
        //            //Screen screen = Screen.FromPoint(Cursor.Position);
        //            Screen scn = Screen.FromControl(this);
        //            MyF.Location = scn.Bounds.Location;
        //            MyF.Show();
        //            this.Dispose(false);
        //            break;

        //        default:
        //            MessageBox.Show("Username Password Combination Incorrect", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            // See whether Dictionary contains this string.
        //            if (dictionary.ContainsKey(txtUsername.Text))
        //            {
        //                dictionary[txtUsername.Text] = dictionary[txtUsername.Text] + 1;
        //            }
        //            else
        //            {
        //                dictionary.Add(txtUsername.Text, 1);
        //            }

        //            if (dictionary[txtUsername.Text] > 4)
        //            {
        //                //lock account
        //                List<DAField> fields = new List<DAField>();
        //                DAField f = new DAField();
        //                f.Field = "Username";
        //                f.dType = "String";
        //                f.Value = txtUsername.Text;
        //                fields.Add(f);
        //                bool locked = DataManager.SaveData(fields, "LockUserAccount", null, null, txtUsername.Text);
        //                if(locked)
        //                    MessageBox.Show("User Account locked for user " + txtUsername.Text, "Account Locked", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //                else
        //                    MessageBox.Show("Failed to lock User Account - " + txtUsername.Text + " after 5 failed log in attempts", "User does not exist in database", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            }

        //            break;
        //    }

        //}

        private void LoginAudit()
        {
            bool SaveLoginAudit = DataManager.SaveLoginAudit("Login", Environment.UserName);
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to exit the application?", "FAB - Credit Administration System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                bool SaveLoginAudit = DataManager.SaveLoginAudit("Exit", Environment.UserName);
                Application.Exit();
            }
        }



        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        private void Login_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void Login_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(Color.Black, 3),
                                     this.DisplayRectangle);
        }

        //private void Login_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.KeyCode == Keys.Enter) 
        //    {
        //        OK_Click(this, null);
        //    }
        //    else if (e.KeyCode == Keys.Escape) 
        //    {
        //        Cancel_Click(this, null);
        //    }
        //}

        private void llblChangePassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form MyForm = new frmChangePassword();
            //Screen screen = Screen.FromPoint(Cursor.Position);
            Screen screen = Screen.FromControl(this);
            MyForm.Location = screen.Bounds.Location;
            MyForm.Show();

            this.Close();
        }

        private void llblChangeEnviron_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            llblChangeEnviron.Visible = false;
            ddnEnvironment.Visible = true;
            ddnEnvironment.Top = llblChangeEnviron.Top;
            ddnEnvironment.Left = llblChangeEnviron.Left;
        }

        private void IsInLDAPGroup()
        {
            var user = Environment.UserName;
            var dsam = new WKSAM("");
            var up = dsam.FindUser(user);
            var groups = up.GetGroups();

            foreach (var group in groups)
            {
                if (group.ToString().ToLower() == "ldn_apps_cas_approver")
                    hasApprover = true;

                if (group.ToString().ToLower() == "ldn_apps_cas_inputter")
                    hasInput = true;

                if (group.ToString().ToLower() == "ldn_apps_cas_admin")
                    hasAdmin = true;

                if (group.ToString().ToLower() == "ldn_apps_cas_viewer")
                    hasViewer = true;
            }

            LoginInput.Enabled = hasInput;
            LoginAdmin.Enabled = hasAdmin;
            LoginViewer.Enabled = hasViewer;
            LoginApprover.Enabled = hasApprover;
        }

        private string GetConnectionString()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["CAS_PROD"].ConnectionString;

            return new Crypto().Decrypt(connectionString);
        }

        private void ProcessUser(int userTypeId)
        {
            var connectionString = GetConnectionString();
            var user = Environment.UserName;
            var doesUserExist = DoesUserExist(connectionString, user);

            if (doesUserExist == -1)
                ConnectionError();
            else if (doesUserExist == 0)
                if (!CreateUser(connectionString, user, userTypeId))
                    ConnectionError();
            else
                if (!UpdateUser(connectionString, user, userTypeId))
                    ConnectionError();
        }

        private int DoesUserExist(string connectionString, string user)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    var query = $"select count(*) from NBAD_User where Username = '{user}'";
                    var command = new SqlCommand(query, connection);
                    var noUsers = "0";

                    connection.Open();

                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        noUsers = reader[0].ToString();
                    }

                    return noUsers != "0" ? 1 : 0;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);

                    return -1;
                }
            }
        }

        private bool CreateUser(string connectionString, string user, int userTypeId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    var query = $"insert into NBAD_User select '{user}', null, {userTypeId}, 1";
                    var command = new SqlCommand(query, connection);

                    connection.Open();

                    command.ExecuteNonQuery();

                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        private bool UpdateUser(string connectionString, string user, int userTypeId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    var query = $"update NBAD_User set User_Type = {userTypeId} where Username = '{user}'";
                    var command = new SqlCommand(query, connection);

                    connection.Open();

                    command.ExecuteNonQuery();

                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        private static void ConnectionError()
        {
            MessageBox.Show("There has been an error connecting to the database. The application will now close.", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            Environment.Exit(1);
        }

        private void LoginInput_Click(object sender, EventArgs e)
        {
            ProcessUser(1);

            LoginAudit();

            Globals._gUserName = Environment.UserName;
            Globals.gUserType = 1;

            Globals.gReadOnly = false;
            Form MyForm = new frmSelect();
            Screen screen = Screen.FromControl(this);
            MyForm.Location = screen.Bounds.Location;
            MyForm.Show();
            this.Dispose(false);
        }

        private void LoginApprover_Click(object sender, EventArgs e)
        {
            ProcessUser(2);

            LoginAudit();

            Globals._gUserName = Environment.UserName;
            Globals.gUserType = 2;

            Globals.gReadOnly = true;
            Form MyFrm = new frmSelect();
            Screen scrn = Screen.FromControl(this);
            MyFrm.Location = scrn.Bounds.Location;
            MyFrm.Show();
            this.Dispose(false);
        }

        private void LoginAdmin_Click(object sender, EventArgs e)
        {
            ProcessUser(3);

            LoginAudit();

            Globals._gUserName = Environment.UserName;
            Globals.gUserType = 3;

            Form MyFm = new frmUsers();
            Screen scr = Screen.FromControl(this);
            MyFm.Location = scr.Bounds.Location;
            MyFm.Show();
            this.Dispose(false);
        }

        private void LoginViewer_Click(object sender, EventArgs e)
        {
            ProcessUser(4);

            LoginAudit();

            Globals._gUserName = Environment.UserName;
            Globals.gUserType = 4;

            Globals.gReadOnly = true;
            Form MyF = new frmSelect();
            Screen scn = Screen.FromControl(this);
            MyF.Location = scn.Bounds.Location;
            MyF.Show();
            this.Dispose(false);
        }
    }
}
