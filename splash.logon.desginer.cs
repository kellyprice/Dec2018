////class: Splash
////Purpose: Splash Form 
////Author: Rich
////Date: 03/2012

////Version history: 
////YYYY-MM-DD
////Author:
////Description:

using System;
using System.Windows.Forms;
using FAB.Classes;

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

            AddWarning();
            
            Application.DoEvents();

            if (!ProcessLDAPLogin())
                Environment.Exit(1);

            timer1.Enabled = true;
        }

        private void AddWarning()
        {
            lblWarning.Text = "*********************************************************************\r\n";
            lblWarning.Text += "*                                                                   *\r\n";
            lblWarning.Text += "*                          WARNING    ! ! !                         *\r\n";
            lblWarning.Text += "*                                                                   *\r\n";
            lblWarning.Text += "*   Unauthorized access to this system is forbidden and will be     *\r\n";
            lblWarning.Text += "*                                                                   *\r\n";
            lblWarning.Text += "*  prosecuted by law. By accessing this system, you agree that your *\r\n";
            lblWarning.Text += "*                                                                   *\r\n";
            lblWarning.Text += "*   actions may be monitored if unauthorized usage is suspected.    *\r\n";
            lblWarning.Text += "*                                                                   *\r\n";
            lblWarning.Text += "*********************************************************************";
        }

        private bool ProcessLDAPLogin()
        {
            if (!IsInLDAPGroup())
            {
                MessageBox.Show("You do not belong to the correct User Group to be able to use this application. Contact your Administrator if you require access.", "Authentication Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }

            return true;
        }

        private bool IsInLDAPGroup()
        {
            var user = Environment.UserName;
            var dsam = new WKSAM("");
            var up = dsam.FindUser(user);
            var groups = up.GetGroups();

            foreach (var group in groups)
            {
                if (group.ToString().ToLower() == "ldn_apps_cas_approver")
                    return true;

                if (group.ToString().ToLower() == "ldn_apps_cas_inputter")
                    return true;

                if (group.ToString().ToLower() == "ldn_apps_cas_admin")
                    return true;

                if (group.ToString().ToLower() == "ldn_apps_cas_viewer")
                    return true;
            }

            return false;
        }

        private void timer1_Tick(object sender, System.EventArgs e)
        {
            Form MyForm = new Login();
            //Screen screen = Screen.FromPoint(Cursor.Position);
            Screen screen = Screen.FromControl(this);
            MyForm.Location = screen.Bounds.Location;
            MyForm.Show();
            timer1.Enabled = false;
            this.Dispose(false);
        }
    }
}




namespace FAB
{
    partial class Login
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.panel1 = new System.Windows.Forms.Panel();
            this.LoginAdmin = new System.Windows.Forms.Button();
            this.LoginApprover = new System.Windows.Forms.Button();
            this.LoginViewer = new System.Windows.Forms.Button();
            this.LoginInput = new System.Windows.Forms.Button();
            this.lblWarning = new System.Windows.Forms.Label();
            this.llblChangeEnviron = new System.Windows.Forms.LinkLabel();
            this.ddnEnvironment = new System.Windows.Forms.ComboBox();
            this.lblApplicationTitle = new System.Windows.Forms.Label();
            this.PictureBox1 = new System.Windows.Forms.PictureBox();
            this.Cancel = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.LoginAdmin);
            this.panel1.Controls.Add(this.LoginApprover);
            this.panel1.Controls.Add(this.LoginViewer);
            this.panel1.Controls.Add(this.LoginInput);
            this.panel1.Controls.Add(this.lblWarning);
            this.panel1.Controls.Add(this.llblChangeEnviron);
            this.panel1.Controls.Add(this.ddnEnvironment);
            this.panel1.Controls.Add(this.lblApplicationTitle);
            this.panel1.Controls.Add(this.PictureBox1);
            this.panel1.Controls.Add(this.Cancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(600, 600);
            this.panel1.TabIndex = 4;
            // 
            // LoginAdmin
            // 
            this.LoginAdmin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(48)))), ((int)(((byte)(91)))));
            this.LoginAdmin.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.LoginAdmin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LoginAdmin.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LoginAdmin.ForeColor = System.Drawing.Color.White;
            this.LoginAdmin.Location = new System.Drawing.Point(301, 529);
            this.LoginAdmin.Name = "LoginAdmin";
            this.LoginAdmin.Size = new System.Drawing.Size(111, 23);
            this.LoginAdmin.TabIndex = 28;
            this.LoginAdmin.Text = "Login as Admin";
            this.LoginAdmin.UseVisualStyleBackColor = false;
            this.LoginAdmin.Click += new System.EventHandler(this.LoginAdmin_Click);
            // 
            // LoginApprover
            // 
            this.LoginApprover.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(48)))), ((int)(((byte)(91)))));
            this.LoginApprover.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.LoginApprover.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LoginApprover.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LoginApprover.ForeColor = System.Drawing.Color.White;
            this.LoginApprover.Location = new System.Drawing.Point(301, 500);
            this.LoginApprover.Name = "LoginApprover";
            this.LoginApprover.Size = new System.Drawing.Size(111, 23);
            this.LoginApprover.TabIndex = 27;
            this.LoginApprover.Text = "Login as Approver";
            this.LoginApprover.UseVisualStyleBackColor = false;
            this.LoginApprover.Click += new System.EventHandler(this.LoginApprover_Click);
            // 
            // LoginViewer
            // 
            this.LoginViewer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(48)))), ((int)(((byte)(91)))));
            this.LoginViewer.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.LoginViewer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LoginViewer.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LoginViewer.ForeColor = System.Drawing.Color.White;
            this.LoginViewer.Location = new System.Drawing.Point(174, 529);
            this.LoginViewer.Name = "LoginViewer";
            this.LoginViewer.Size = new System.Drawing.Size(111, 23);
            this.LoginViewer.TabIndex = 26;
            this.LoginViewer.Text = "Login as Viewer";
            this.LoginViewer.UseVisualStyleBackColor = false;
            this.LoginViewer.Click += new System.EventHandler(this.LoginViewer_Click);
            // 
            // LoginInput
            // 
            this.LoginInput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(48)))), ((int)(((byte)(91)))));
            this.LoginInput.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.LoginInput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LoginInput.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LoginInput.ForeColor = System.Drawing.Color.White;
            this.LoginInput.Location = new System.Drawing.Point(174, 500);
            this.LoginInput.Name = "LoginInput";
            this.LoginInput.Size = new System.Drawing.Size(111, 23);
            this.LoginInput.TabIndex = 25;
            this.LoginInput.Text = "Login as Input";
            this.LoginInput.UseVisualStyleBackColor = false;
            this.LoginInput.Click += new System.EventHandler(this.LoginInput_Click);
            // 
            // lblWarning
            // 
            this.lblWarning.AutoSize = true;
            this.lblWarning.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.lblWarning.ForeColor = System.Drawing.Color.DimGray;
            this.lblWarning.Location = new System.Drawing.Point(35, 164);
            this.lblWarning.Name = "lblWarning";
            this.lblWarning.Size = new System.Drawing.Size(53, 15);
            this.lblWarning.TabIndex = 24;
            this.lblWarning.Text = "Warning";
            this.lblWarning.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // llblChangeEnviron
            // 
            this.llblChangeEnviron.ActiveLinkColor = System.Drawing.Color.ForestGreen;
            this.llblChangeEnviron.AutoSize = true;
            this.llblChangeEnviron.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.llblChangeEnviron.Location = new System.Drawing.Point(41, 300);
            this.llblChangeEnviron.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.llblChangeEnviron.Name = "llblChangeEnviron";
            this.llblChangeEnviron.Size = new System.Drawing.Size(106, 13);
            this.llblChangeEnviron.TabIndex = 23;
            this.llblChangeEnviron.TabStop = true;
            this.llblChangeEnviron.Text = "Change Environment";
            this.toolTip1.SetToolTip(this.llblChangeEnviron, "Change Environment. Default PROD");
            this.llblChangeEnviron.Visible = false;
            this.llblChangeEnviron.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llblChangeEnviron_LinkClicked);
            // 
            // ddnEnvironment
            // 
            this.ddnEnvironment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddnEnvironment.Font = new System.Drawing.Font("Calibri", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ddnEnvironment.FormattingEnabled = true;
            this.ddnEnvironment.Items.AddRange(new object[] {
            "PRODUCTION",
            "RECOVERY"});
            this.ddnEnvironment.Location = new System.Drawing.Point(19, 270);
            this.ddnEnvironment.Margin = new System.Windows.Forms.Padding(2);
            this.ddnEnvironment.Name = "ddnEnvironment";
            this.ddnEnvironment.Size = new System.Drawing.Size(109, 21);
            this.ddnEnvironment.TabIndex = 22;
            this.ddnEnvironment.Visible = false;
            // 
            // lblApplicationTitle
            // 
            this.lblApplicationTitle.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblApplicationTitle.AutoSize = true;
            this.lblApplicationTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblApplicationTitle.Font = new System.Drawing.Font("Calibri", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblApplicationTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.lblApplicationTitle.Location = new System.Drawing.Point(104, 112);
            this.lblApplicationTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblApplicationTitle.Name = "lblApplicationTitle";
            this.lblApplicationTitle.Size = new System.Drawing.Size(399, 39);
            this.lblApplicationTitle.TabIndex = 20;
            this.lblApplicationTitle.Text = "Credit Administration System";
            this.lblApplicationTitle.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // PictureBox1
            // 
            this.PictureBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(48)))), ((int)(((byte)(91)))));
            this.PictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.PictureBox1.Location = new System.Drawing.Point(0, 0);
            this.PictureBox1.Name = "PictureBox1";
            this.PictureBox1.Size = new System.Drawing.Size(598, 81);
            this.PictureBox1.TabIndex = 19;
            this.PictureBox1.TabStop = false;
            // 
            // Cancel
            // 
            this.Cancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(48)))), ((int)(((byte)(91)))));
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Cancel.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Cancel.ForeColor = System.Drawing.Color.White;
            this.Cancel.Location = new System.Drawing.Point(260, 452);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(66, 23);
            this.Cancel.TabIndex = 15;
            this.Cancel.Text = "&Exit";
            this.Cancel.UseVisualStyleBackColor = false;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(1, 1);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(155, 80);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox2.TabIndex = 38;
            this.pictureBox2.TabStop = false;
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(600, 600);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Login_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Login_MouseDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        internal System.Windows.Forms.Button Cancel;
        internal System.Windows.Forms.PictureBox PictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        internal System.Windows.Forms.Label lblApplicationTitle;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ComboBox ddnEnvironment;
        private System.Windows.Forms.LinkLabel llblChangeEnviron;
        private System.Windows.Forms.Label lblWarning;
        internal System.Windows.Forms.Button LoginAdmin;
        internal System.Windows.Forms.Button LoginApprover;
        internal System.Windows.Forms.Button LoginViewer;
        internal System.Windows.Forms.Button LoginInput;
    }
}
