using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ADExplorer
{
    public partial class ADExplorer : Form
    {
        private string _connectionString;
        public string ConnectionString
        {
            get
            {
                if(String.IsNullOrEmpty(_connectionString))
                {
                    _connectionString = textBoxForConnectionString.Text;
                }

                return _connectionString;
            }
        }

        public ADExplorer()
        {
            InitializeComponent();
            var uname = Environment.UserDomainName.ToLower() + @"\" + Environment.UserName.ToLower();
            //var uname = Environment.UserName + "@" + Environment.UserDomainName;
            textBoxForUserName.Text = "richard.cranium";
        }

        private void buttonForFindUser_Click(object sender, EventArgs e)
        {
            FindUser(textBoxForUserName.Text);
        }

        private void FindUser(string username)
        {
            var dsam = new WKDSAM(ConnectionString);
            var up = dsam.FindUser(username);
            if(up != null)
            {
                textBoxForUserInfo.Text = up.DisplayName + ";" + up.Description + ";" + up.DistinguishedName;
            } else
            {
                textBoxForUserInfo.Text = "User not found";
            }
            
        }

        private void buttonForConnect_Click(object sender, EventArgs e)
        {
            TestConnection();
        }

        private void TestConnection()
        {
            var dsam = new WKDSAM(ConnectionString);
            if (dsam.CanConnect())
            {
                textBoxForConnectResult.Text = "Success!";
            }
            else
            {
                textBoxForConnectResult.Text = dsam.ProcessError.Message;
            }
        }

        private void buttonForCheckMemberShip_Click(object sender, EventArgs e)
        {
            CheckForMembership(textBoxForUserName.Text, textBoxForGroupName.Text);
        }

        private void CheckForMembership(string username, string groupname)
        {
            textBoxForMemberCheck.Text = "Checking...";
            List<string[]> groups = new List<string[]>();
            var processingError = "";

            var bw = new BackgroundWorker();
            bw.DoWork += (s, a) =>
            {
                var dsam = new WKDSAM(ConnectionString);
                var up = dsam.FindUser(username);
                groups = dsam.ListGroups(up);

                if(dsam.ProcessError != null)
                {
                    processingError = dsam.ProcessError.Message + "; " + dsam.ProcessError.StackTrace;
                }

            };

            bw.RunWorkerCompleted += (s, a) =>
            {
                if (String.IsNullOrEmpty(processingError))
                {

                    if (groups.Where(g => g[0].Equals(groupname)).Count() > 0)
                    {
                        textBoxForMemberCheck.Text = "yes";
                    }
                    else
                    {
                        textBoxForMemberCheck.Text = "no";
                    }
                } else
                {

                    textBoxForMemberCheck.Text = processingError;
                }
            };


            bw.RunWorkerAsync();
            
        }
    }
}
