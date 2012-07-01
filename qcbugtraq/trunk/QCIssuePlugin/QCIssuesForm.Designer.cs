namespace QCIssuePlugin
{
    partial class QCIssuesForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QCIssuesForm));
            this.grp_QCList = new System.Windows.Forms.GroupBox();
            this.lv_QCIssues = new System.Windows.Forms.ListView();
            this.bt_Cancel = new System.Windows.Forms.Button();
            this.bt_Ok = new System.Windows.Forms.Button();
            this.cb_QCURL = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cb_Project = new System.Windows.Forms.ComboBox();
            this.cb_Domain = new System.Windows.Forms.ComboBox();
            this.bt_Login = new System.Windows.Forms.Button();
            this.bt_Authenticate = new System.Windows.Forms.Button();
            this.tb_Password = new System.Windows.Forms.TextBox();
            this.tb_LoginName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cb_SaveCredentials = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ckb_AuthAndLogin = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label6 = new System.Windows.Forms.Label();
            this.cb_QCFilter = new System.Windows.Forms.ComboBox();
            this.grp_QCList.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // grp_QCList
            // 
            this.grp_QCList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grp_QCList.Controls.Add(this.lv_QCIssues);
            this.grp_QCList.Location = new System.Drawing.Point(9, 187);
            this.grp_QCList.Name = "grp_QCList";
            this.grp_QCList.Size = new System.Drawing.Size(826, 317);
            this.grp_QCList.TabIndex = 0;
            this.grp_QCList.TabStop = false;
            this.grp_QCList.Text = "ALM items";
            // 
            // lv_QCIssues
            // 
            this.lv_QCIssues.CheckBoxes = true;
            this.lv_QCIssues.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lv_QCIssues.FullRowSelect = true;
            this.lv_QCIssues.Location = new System.Drawing.Point(3, 16);
            this.lv_QCIssues.Name = "lv_QCIssues";
            this.lv_QCIssues.Size = new System.Drawing.Size(820, 298);
            this.lv_QCIssues.TabIndex = 10;
            this.lv_QCIssues.UseCompatibleStateImageBehavior = false;
            this.lv_QCIssues.View = System.Windows.Forms.View.Details;
            this.lv_QCIssues.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lv_QCIssues_ColumnClick);
            // 
            // bt_Cancel
            // 
            this.bt_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bt_Cancel.Location = new System.Drawing.Point(759, 510);
            this.bt_Cancel.Name = "bt_Cancel";
            this.bt_Cancel.Size = new System.Drawing.Size(73, 22);
            this.bt_Cancel.TabIndex = 1;
            this.bt_Cancel.Text = "Cancel";
            this.bt_Cancel.UseVisualStyleBackColor = true;
            // 
            // bt_Ok
            // 
            this.bt_Ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_Ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bt_Ok.Location = new System.Drawing.Point(675, 510);
            this.bt_Ok.Name = "bt_Ok";
            this.bt_Ok.Size = new System.Drawing.Size(78, 22);
            this.bt_Ok.TabIndex = 2;
            this.bt_Ok.Text = "Ok";
            this.bt_Ok.UseVisualStyleBackColor = true;
            this.bt_Ok.Click += new System.EventHandler(this.bt_Ok_Click);
            // 
            // cb_QCURL
            // 
            this.cb_QCURL.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cb_QCURL.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cb_QCURL.FormattingEnabled = true;
            this.cb_QCURL.Location = new System.Drawing.Point(79, 13);
            this.cb_QCURL.Name = "cb_QCURL";
            this.cb_QCURL.Size = new System.Drawing.Size(243, 21);
            this.cb_QCURL.TabIndex = 1;
            this.toolTip1.SetToolTip(this.cb_QCURL, "http[s]://[QCHost]:[port]/qcbin");
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(26, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 13);
            this.label5.TabIndex = 26;
            this.label5.Text = "ALM URL";
            // 
            // cb_Project
            // 
            this.cb_Project.FormattingEnabled = true;
            this.cb_Project.Location = new System.Drawing.Point(386, 43);
            this.cb_Project.Name = "cb_Project";
            this.cb_Project.Size = new System.Drawing.Size(242, 21);
            this.cb_Project.TabIndex = 8;
            this.cb_Project.TextChanged += new System.EventHandler(this.cb_Project_TextChanged);
            // 
            // cb_Domain
            // 
            this.cb_Domain.FormattingEnabled = true;
            this.cb_Domain.Location = new System.Drawing.Point(385, 13);
            this.cb_Domain.Name = "cb_Domain";
            this.cb_Domain.Size = new System.Drawing.Size(243, 21);
            this.cb_Domain.TabIndex = 7;
            this.cb_Domain.TextChanged += new System.EventHandler(this.cb_Domain_TextChanged);
            // 
            // bt_Login
            // 
            this.bt_Login.Location = new System.Drawing.Point(385, 113);
            this.bt_Login.Name = "bt_Login";
            this.bt_Login.Size = new System.Drawing.Size(103, 24);
            this.bt_Login.TabIndex = 9;
            this.bt_Login.Text = "Retrieve items";
            this.toolTip1.SetToolTip(this.bt_Login, "Login to the QC project; You may login to multiple projects to select CRs from mu" +
        "ltiple QC projects.");
            this.bt_Login.UseVisualStyleBackColor = true;
            this.bt_Login.Click += new System.EventHandler(this.bt_RetrieveItems_Click);
            // 
            // bt_Authenticate
            // 
            this.bt_Authenticate.Location = new System.Drawing.Point(79, 115);
            this.bt_Authenticate.Name = "bt_Authenticate";
            this.bt_Authenticate.Size = new System.Drawing.Size(105, 22);
            this.bt_Authenticate.TabIndex = 6;
            this.bt_Authenticate.Text = "Authenticate";
            this.bt_Authenticate.UseVisualStyleBackColor = true;
            this.bt_Authenticate.Click += new System.EventHandler(this.bt_Authenticate_Click);
            // 
            // tb_Password
            // 
            this.tb_Password.Location = new System.Drawing.Point(79, 68);
            this.tb_Password.Name = "tb_Password";
            this.tb_Password.PasswordChar = '*';
            this.tb_Password.Size = new System.Drawing.Size(243, 20);
            this.tb_Password.TabIndex = 3;
            // 
            // tb_LoginName
            // 
            this.tb_LoginName.Location = new System.Drawing.Point(79, 40);
            this.tb_LoginName.Name = "tb_LoginName";
            this.tb_LoginName.Size = new System.Drawing.Size(243, 20);
            this.tb_LoginName.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(339, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "Project";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(336, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "Domain";
            // 
            // cb_SaveCredentials
            // 
            this.cb_SaveCredentials.AutoSize = true;
            this.cb_SaveCredentials.Location = new System.Drawing.Point(79, 94);
            this.cb_SaveCredentials.Name = "cb_SaveCredentials";
            this.cb_SaveCredentials.Size = new System.Drawing.Size(105, 17);
            this.cb_SaveCredentials.TabIndex = 4;
            this.cb_SaveCredentials.Text = "Save credentials";
            this.toolTip1.SetToolTip(this.cb_SaveCredentials, resources.GetString("cb_SaveCredentials.ToolTip"));
            this.cb_SaveCredentials.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Password:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Login Name:";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.ckb_AuthAndLogin);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.cb_QCURL);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.cb_Project);
            this.groupBox2.Controls.Add(this.cb_SaveCredentials);
            this.groupBox2.Controls.Add(this.cb_Domain);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.bt_Login);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.bt_Authenticate);
            this.groupBox2.Controls.Add(this.tb_LoginName);
            this.groupBox2.Controls.Add(this.tb_Password);
            this.groupBox2.Location = new System.Drawing.Point(12, 3);
            this.groupBox2.MinimumSize = new System.Drawing.Size(0, 146);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(823, 146);
            this.groupBox2.TabIndex = 28;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "ALM Authentication";
            // 
            // ckb_AuthAndLogin
            // 
            this.ckb_AuthAndLogin.AutoSize = true;
            this.ckb_AuthAndLogin.Location = new System.Drawing.Point(190, 94);
            this.ckb_AuthAndLogin.Name = "ckb_AuthAndLogin";
            this.ckb_AuthAndLogin.Size = new System.Drawing.Size(136, 17);
            this.ckb_AuthAndLogin.TabIndex = 5;
            this.ckb_AuthAndLogin.Text = "Authenticate and Login";
            this.toolTip1.SetToolTip(this.ckb_AuthAndLogin, "Select to authenticate and login to the last used QC project in one click.");
            this.ckb_AuthAndLogin.UseVisualStyleBackColor = true;
            this.ckb_AuthAndLogin.CheckedChanged += new System.EventHandler(this.ckb_AuthAndLogin_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(35, 158);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(57, 13);
            this.label6.TabIndex = 29;
            this.label6.Text = "ALM Filter:";
            // 
            // cb_QCFilter
            // 
            this.cb_QCFilter.FormattingEnabled = true;
            this.cb_QCFilter.Items.AddRange(new object[] {
            "1. Default: Owner and not closed",
            "2. All - not closed"});
            this.cb_QCFilter.Location = new System.Drawing.Point(91, 155);
            this.cb_QCFilter.Name = "cb_QCFilter";
            this.cb_QCFilter.Size = new System.Drawing.Size(243, 21);
            this.cb_QCFilter.TabIndex = 30;
            this.cb_QCFilter.Text = "1. Default: Owner and not closed";
            this.cb_QCFilter.SelectedIndexChanged += new System.EventHandler(this.cb_QCFilter_SelectedIndexChanged);
            // 
            // QCIssuesForm
            // 
            this.AcceptButton = this.bt_Ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bt_Cancel;
            this.ClientSize = new System.Drawing.Size(843, 534);
            this.Controls.Add(this.cb_QCFilter);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.grp_QCList);
            this.Controls.Add(this.bt_Ok);
            this.Controls.Add(this.bt_Cancel);
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "QCIssuesForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ALM Issues";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.QCIssuesForm_FormClosing);
            this.Load += new System.EventHandler(this.QCIssuesForm_Load);
            this.grp_QCList.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grp_QCList;
        private System.Windows.Forms.ListView lv_QCIssues;
        private System.Windows.Forms.Button bt_Cancel;
        private System.Windows.Forms.Button bt_Ok;
        private System.Windows.Forms.ComboBox cb_QCURL;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cb_Project;
        private System.Windows.Forms.ComboBox cb_Domain;
        private System.Windows.Forms.Button bt_Login;
        private System.Windows.Forms.Button bt_Authenticate;
        private System.Windows.Forms.TextBox tb_Password;
        private System.Windows.Forms.TextBox tb_LoginName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox cb_SaveCredentials;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox ckb_AuthAndLogin;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cb_QCFilter;
    }
}