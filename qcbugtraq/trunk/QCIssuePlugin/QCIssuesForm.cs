// ALM Tools: QCBugTraq: a plugin for TortoiseSVN/GIT to write quickly meaningful
//                       commit messages
// Copyright (C) 2012 Hewlett Packard Company
// Authors: 
//      Olivier Jacques
//        from Hewlett Packard Company
//      
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License along
// with this program; if not, write to the Free Software Foundation, Inc.,
// 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Net;
using System.Xml.Linq;

namespace QCIssuePlugin
{
    public partial class QCIssuesForm : Form
    {
        private readonly IEnumerable<TicketItem> _tickets;
        private readonly List<TicketItem> _ticketsAffected = new List<TicketItem>();
        private ListViewColumnSorter lvwColumnSorter;

        const long VERSION = 20120705;
        const string DOWNLOAD_LINK = "https://sourceforge.net/projects/almtools/files/TurtleALM/";
        const string VERSION_URI = "http://almtools.sourceforge.net/turtlealm-latest.txt";

        HP.AlmRestClient.AlmRestConnection _almc;

        private string _query = "";

        // All default settings
        public string _SettingDefectPrefix = "defect";
        public string _SettingReqPrefix = "requirement";
        public string _SettingVerb = "fixing";
        public bool _SettingUseGUID = false;
        public string _SettingGUIDDefectField = "user-template-04";
        public string _SettingGUIDReqField = "user-template-05";
        public bool _SettingCheckForUpdate = true;
        
        public QCIssuesForm(IEnumerable<TicketItem> tickets)
        {
            InitializeComponent();
            _tickets = tickets;
            lvwColumnSorter = new ListViewColumnSorter();
            this.lv_QCIssues.ListViewItemSorter = lvwColumnSorter;
        }

        public IEnumerable<TicketItem> TicketsFixed
        {
            get { return _ticketsAffected; }
        }

        private void AddTicket(TicketItem ticket)
        {
            bool bGroupFound = false;
            foreach (ListViewGroup group in lv_QCIssues.Groups)
            {
                if (group.Name == ticket.DomainProject)
                    bGroupFound = true;
            }
            if (!bGroupFound)
            {
                lv_QCIssues.Groups.Add(ticket.DomainProject, ticket.DomainProject);
            }
            // Add a ticket with all its attributes
            ListViewItem lvi = new ListViewItem();
            lvi.Text = "";
            if (_SettingUseGUID)
                lvi.SubItems.Add(ticket.GUID);
            else
                lvi.SubItems.Add(ticket.Number.ToString());
            lvi.SubItems.Add(ticket.Status);
            lvi.SubItems.Add(ticket.Summary);
            lvi.SubItems.Add(ticket.LastModified);
            lvi.SubItems.Add(ticket.TargetRel);
            lvi.SubItems.Add(ticket.Owner);
            lvi.Group = lv_QCIssues.Groups[ticket.DomainProject];
            lvi.Tag = ticket;

            lv_QCIssues.Items.Add(lvi);

            QCIssuePlugin.QCPlugin.tickets.Add(ticket);
        }

        private void QCIssuesForm_Load(object sender, EventArgs e)
        {
            // Form title
            this.Text = "Select ALM " + QCPlugin.QCITEMNAMEPLURAL;
            this.grp_QCList.Text = "List of ALM " + QCPlugin.QCITEMNAMEPLURAL;

            if (_SettingCheckForUpdate)
            {
                string strLastCheck = RegistryGet("LastVersionCheck");
                if (String.Compare(strLastCheck, DateTime.Now.AddDays(-7).ToString("yyyyMMdd")) < 0)
                {
                    // We checked more than 7 days ago: check again
                    long webVersion;
                    string strVersionMessage = getLatestVersion(VERSION_URI, out webVersion);
                    RegistrySet("LastVersionCheck", DateTime.Now.ToString("yyyyMMdd"));

                    if ((!strVersionMessage.Contains("ERROR")) && (webVersion > VERSION))
                    {
                        if ((MessageBox.Show("There is a new version of TurtleALM.\r\nCurrent version: " + VERSION.ToString() +
                            ". Latest version: " + webVersion.ToString() +
                            ".\r\nDo you want to download the latest version?",
                            "Version outdated", MessageBoxButtons.YesNo) == DialogResult.Yes))
                        {
                            System.Diagnostics.Process.Start(DOWNLOAD_LINK);
                        }
                    }
                    else
                    {
                        // Silently ignore
                    }
                }
            }
            // Restore Form size as saved in registry
            string strWinSize = RegistryGet("WindowSize"); 
            if (strWinSize != "")
            {
                string[] strSize = Regex.Split(strWinSize, ":");
                this.Size = new Size(int.Parse(strSize[0]), int.Parse(strSize[1]));
            }
            // Construct the item list
            lv_QCIssues.Columns.Add("X", 15);
            //lv_QCIssues.Columns.Add("Context", 130);
            lv_QCIssues.Columns.Add(QCPlugin.QCITEMNAME + "#", 30);
            lv_QCIssues.Columns.Add("Status", 50);
            lv_QCIssues.Columns.Add("Summary", 200);
            lv_QCIssues.Columns.Add("Last Modified", 50);
            lv_QCIssues.Columns.Add("Target Release", 80);
            lv_QCIssues.Columns.Add("Owner", 50);

            bt_Login.Enabled = false;

            // Get last Values saved in the registry
            tb_LoginName.Text = RegistryGet("QCUser");  // QC User
            // Get encrypted password using Microsoft DPAPI (can only be decrypted by the same Windows user)
            if (RegistryGet("QCPass") != "")            // QC Password
            {
                byte[] bQCPassword, bEncryptedQCPassword, bEntropyBytes;
                bEntropyBytes = Encoding.Unicode.GetBytes("QCRulezzz");
                bEncryptedQCPassword = Convert.FromBase64String(RegistryGet("QCPass"));
                bQCPassword = ProtectedData.Unprotect(bEncryptedQCPassword, bEntropyBytes, DataProtectionScope.CurrentUser);
                tb_Password.Text = Encoding.Unicode.GetString(bQCPassword);
            }

            // Add the QC URLs from the registry
            cb_QCURL.Items.Clear();
            if (RegistryGet("QCURLs") != "")
            {
                string[] strQCURLs = Regex.Split(RegistryGet("QCURLs"), ",");
                foreach (string strQCURL in strQCURLs)
                {
                    if (strQCURL != "")
                    {
                        cb_QCURL.Items.Add((string)strQCURL);
                    }
                }
                cb_QCURL.Text = cb_QCURL.Items[0].ToString();
            }

            // Restore last QC URL
            if (RegistryGet("lastQCURL") != "")
            {
                cb_QCURL.Text = RegistryGet("lastQCURL");
            }
            
            // SaveCredentials checked last time?
            if (RegistryGet("SaveCredentials") != "")
            {
                cb_SaveCredentials.Checked = bool.Parse(RegistryGet("SaveCredentials"));
            }
            else 
            {
                cb_SaveCredentials.Checked = false;
            }

            // AuthenticateAndLogin checked last time?
            if (RegistryGet("AuthenticateAndLogin") != "")
            {
                ckb_AuthAndLogin.Checked = bool.Parse(RegistryGet("AuthenticateAndLogin"));
            }
            else
            {
                ckb_AuthAndLogin.Checked = false;
            }

            // Settings - if anything in the registry, get it - otherwise write in registry
            if (RegistryGet("DefectPrefix") != "")
                _SettingDefectPrefix = RegistryGet("DefectPrefix");
            else
                RegistrySet("DefectPrefix", _SettingDefectPrefix);

            if (RegistryGet("ReqPrefix") != "")
                _SettingReqPrefix = RegistryGet("ReqPrefix");
            else
                RegistrySet("ReqPrefix", _SettingReqPrefix);

            if (RegistryGet("Verb") != "")
                _SettingVerb = RegistryGet("Verb");
            else
                RegistrySet("Verb", _SettingVerb);

            if (RegistryGet("UseGUID") != "")
                _SettingUseGUID = bool.Parse(RegistryGet("UseGUID"));
            else
                RegistrySet("UseGUID", _SettingUseGUID.ToString());

            if (RegistryGet("GUIDDefectField") != "")
                _SettingGUIDDefectField = RegistryGet("GUIDDefectField");
            else
                RegistrySet("GUIDDefectField", _SettingGUIDDefectField);

            if (RegistryGet("GUIDReqField") != "")
                _SettingGUIDReqField = RegistryGet("GUIDReqField");
            else
                RegistrySet("GUIDReqField", _SettingGUIDReqField);

            if (RegistryGet("CheckForUpdate") != "")
                _SettingCheckForUpdate = bool.Parse(RegistryGet("CheckForUpdate"));
            else
                RegistrySet("CheckForUpdate", _SettingCheckForUpdate.ToString());

            _query = QueryFromDropDown(cb_QCFilter.Text);

        }

        private string getLatestVersion(string strUrl, out long result)
        {
            // Check if an update of the tool is available
            Cursor.Current = Cursors.WaitCursor;
            System.Net.WebClient wc = new System.Net.WebClient();
            string myVersion;
            result = 0;
            try
            {
                wc.Credentials = System.Net.CredentialCache.DefaultCredentials;
                System.IO.Stream str;
                str = wc.OpenRead(strUrl);
                System.IO.StreamReader sr = new System.IO.StreamReader(str);
                myVersion = sr.ReadToEnd();
                sr.Close();
                long.TryParse(myVersion, out result);
                return "";
            }
            catch (Exception ex)
            {
                return "ERROR: " + ex.Message.ToString();
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void bt_Ok_Click(object sender, EventArgs e)
        {
            // Add items to the list
            foreach (ListViewItem lvi in lv_QCIssues.Items)
            {
                TicketItem ticketItem = lvi.Tag as TicketItem;
                if (ticketItem != null && lvi.Checked)
                    _ticketsAffected.Add(ticketItem);
            }
        }

        /// <summary>
        /// Retrieve ALM items using REST API
        /// </summary>
        private void FetchItems()
        {
            try
            {
                // Save misc info in registry
                RegistrySet("QCDomain", cb_Domain.Text);
                RegistrySet("QCProject", cb_Project.Text);

                _almc.Domain = cb_Domain.Text;
                _almc.Project = cb_Project.Text;

                Cursor.Current = Cursors.WaitCursor;
                // Clear all the tickets already fetched for the project
                var selectedTi = new List<TicketItem>(); 
                foreach (TicketItem ti in QCIssuePlugin.QCPlugin.tickets)
                {
                    if (ti.DomainProject == cb_Domain.Text + ":" + cb_Project.Text)
                        selectedTi.Add(ti);
                }
                foreach (TicketItem ti in selectedTi)
                    QCIssuePlugin.QCPlugin.tickets.Remove(ti);

                // Clear all list view items already fetched for the project
                var selectedLvi = new List<ListViewItem>();
                foreach (ListViewItem lvi in lv_QCIssues.Items)
                {
                    if (lvi.Group.Name == cb_Domain.Text + ":" + cb_Project.Text)
                        selectedLvi.Add(lvi);
                }
                foreach (ListViewItem lvi in selectedLvi)
                    lv_QCIssues.Items.Remove(lvi);
            
                if (!_almc.SessionOpened)
                    _almc.OpenSession();

                XElement xQuery;
                if (_SettingUseGUID) {
                    xQuery = _almc.Query(@"defects?fields=id,name,status,summary,severity,owner,target-rel,last-modified," +
                        _SettingGUIDDefectField + "&" + _query);
                }
                else
                {
                    xQuery = _almc.Query(@"defects?fields=id,name,status,summary,severity,owner,target-rel,last-modified&" + _query);
                }
                

                // Release dictionary: key: release ID, value: release name
                Dictionary<string, string> relDic = new Dictionary<string, string>();
                relDic.Add("", "");
                foreach (XElement xbug in xQuery.Nodes())
                {
                    HP.AlmRestClient.Entity bug = new HP.AlmRestClient.Entity(xbug);
                    string target_rel = bug.GetFieldValue("target-rel");
                    if ((target_rel != "") && (!relDic.ContainsKey(target_rel)))
                    {
                        XElement xQueryRel;
                        xQueryRel = _almc.Query(@"releases/" + target_rel);
                        HP.AlmRestClient.Entity rel = new HP.AlmRestClient.Entity(xQueryRel);
                        relDic.Add(target_rel, rel.GetFieldValue("name"));
                    }
                }

                foreach (XElement xbug in xQuery.Nodes())
                {
                    HP.AlmRestClient.Entity bug = new HP.AlmRestClient.Entity(xbug);
                    AddTicket(new TicketItem(cb_Domain.Text + ":" + cb_Project.Text,
                        int.Parse(bug.GetFieldValue("id")),
                        bug.GetFieldValue("status"),
                        bug.GetFieldValue("name"),
                        bug.GetFieldValue("owner"),
                        relDic[bug.GetFieldValue("target-rel")],
                        bug.GetFieldValue("last-modified"),
                        _SettingUseGUID ? bug.GetFieldValue(_SettingGUIDDefectField) : ""
                        ));
                }

                Cursor.Current = Cursors.Default;

                lv_QCIssues.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error while fetching items");
                bt_Login.Enabled = true;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

        }


        private void QCIssuesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if ( _almc != null)
                {
                    if (_almc.SessionOpened)
                        _almc.CloseSession();
                    if (_almc.Authenticated)
                        _almc.Unauthenticate();
                }
                // Save window position
                RegistrySet("WindowSize", this.Size.Width.ToString() + ":" + this.Size.Height.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error while closing");
                MessageBox.Show(ex.InnerException.ToString(), "Error while closing");
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

        }

        private void cb_Domain_TextChanged(object sender, EventArgs e)
        {
            if (_almc == null)
                return;
            if (!_almc.SessionOpened)
                return;

            cb_Project.Items.Clear();
            cb_Project.Text = "";
            Application.DoEvents();

            // Domain changed: get the list of projects
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                _almc.Domain = cb_Domain.Text;

                foreach (string project in _almc.GetDomainOrProject(false))
                {
                    cb_Project.Items.Add(project);
                }
                if (cb_Project.Items.Count > 0)
                {
                    // Get last project used from registry
                    if ((RegistryGet("QCProject") != "") && (cb_Project.Items.Contains(RegistryGet("QCProject"))))
                    {
                        cb_Project.Text = RegistryGet("QCProject");
                    }
                    else
                    {
                        cb_Project.Text = cb_Project.Items[0].ToString();
                    }

                    bt_Login.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.ToString(), "Error getting the list of QC projects");
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

        }

        private void bt_Authenticate_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                
                bt_Authenticate.Enabled = false;

                // Save misc info in registry
                // If current QC URL is not in the list of URLs saved, add it now
                if (!(RegistryGet("QCURLs").Contains(cb_QCURL.Text)))
                {
                    RegistrySet("QCURLs", RegistryGet("QCURLs") + "," + cb_QCURL.Text);
                }
                RegistrySet("lastQCURL", cb_QCURL.Text);

                // User credentials (user and password) are stored in user's registry 
                // using Microsoft's Data Protection API. They can only be decrypted by the user.
                // (Reference: http://msdn.microsoft.com/en-us/library/system.security.cryptography.protecteddata.aspx)
                if (cb_SaveCredentials.Checked)
                {
                    byte[] bQCPassword, bEncryptedQCPassword, bEntropyBytes;
                    bEntropyBytes = Encoding.Unicode.GetBytes("QCRulezzz"); 
                    bQCPassword = Encoding.Unicode.GetBytes(tb_Password.Text);
                    bEncryptedQCPassword = ProtectedData.Protect(bQCPassword, bEntropyBytes, DataProtectionScope.CurrentUser);
                    string strEncryptedQCPassword = Convert.ToBase64String(bEncryptedQCPassword);
                    RegistrySet("QCUser", tb_LoginName.Text);
                    RegistrySet("QCPass", strEncryptedQCPassword);
                    RegistrySet("SaveCredentials", cb_SaveCredentials.Checked.ToString());
                    RegistrySet("AuthenticateAndLogin", ckb_AuthAndLogin.Checked.ToString());
                }
                else
                { 
                    // Save credentials is unchecked - make sure we remove the Registry entry for password
                    RegistryDelete("QCPass");
                }


                _almc = new HP.AlmRestClient.AlmRestConnection(cb_QCURL.Text);
                if (!_almc.Authenticate(tb_LoginName.Text, tb_Password.Text)) {
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (!_almc.SessionOpened)
                    _almc.OpenSession();

                foreach (string domain in _almc.GetDomainOrProject(true))
                    cb_Domain.Items.Add(domain);

                // Get last domain used from registry
                if (RegistryGet("QCDomain") != "")
                    cb_Domain.Text = RegistryGet("QCDomain");
                else 
                    cb_Domain.Text = cb_Domain.Items[0].ToString();
                _almc.Domain = cb_Domain.Text;

                
                // Also login if asked and Domain/project available in the registry
                if (ckb_AuthAndLogin.Checked) 
                {
                    FetchItems();
                    Cursor.Current = Cursors.Default;
                    return;
                }

                bt_Login.Enabled = true;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                bt_Authenticate.Enabled = true;
                if (ex.Message.Contains("500"))
                    MessageBox.Show(ex.Message.ToString() + " - incorrect user or password?", "Error: authenticate with ALM");
                else
                    MessageBox.Show(ex.Message.ToString(), "Error: authenticate with ALM");
            }

        }

        private void cb_Project_TextChanged(object sender, EventArgs e)
        {
            bt_Login.Enabled = true;
        }

        private string RegistryGet(string strKeyName)
        {
            RegistryKey regkey;
            string strValue = "";
            regkey = Registry.CurrentUser.OpenSubKey(@"Software\TurtleALM");

            if (!(regkey == null)) 
                strValue = (string)regkey.GetValue(strKeyName, "");
            return strValue;
        }

        private void RegistrySet(string strKeyName, string strKeyValue)
        {
            RegistryKey regkey;
            regkey = Registry.CurrentUser.CreateSubKey(@"Software\TurtleALM");
            regkey.SetValue(strKeyName, strKeyValue);
        }

        private void RegistryDelete(string strKeyName)
        {
            RegistryKey regkey;
            regkey = Registry.CurrentUser.OpenSubKey(@"Software\TurtleALM\" + strKeyName);
            if (!(regkey == null))
                Registry.CurrentUser.DeleteSubKey(@"Software\TurtleALM\" + strKeyName);
        }

        private void bt_RetrieveItems_Click(object sender, EventArgs e)
        {
            bt_Login.Enabled = false;
            FetchItems();
        }

        private void lv_QCIssues_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            this.lv_QCIssues.Sort();

        }

        private void ckb_AuthAndLogin_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void cb_QCFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            _query = QueryFromDropDown(cb_QCFilter.Text);
            FetchItems();
        }

        private string QueryFromDropDown(string sText)
        {
            string sQuery = "";
            switch (sText.Substring(0, 2))
            {
                case "1.":
                    sQuery = "query={owner['" + tb_LoginName.Text + "']; status[NOT Closed]}";
                    break;
                case "2.":
                    sQuery = "query={status[Open]}";
                    break;
            }
            return sQuery;
        }

        private void cb_QCURL_TextChanged(object sender, EventArgs e)
        {
            // Change background color of the URL
            // Green: OK, Yellow: no https, Red: bad format
            if (Regex.Matches(cb_QCURL.Text, @"https://(.+?)(:\d+)?/qcbin$").Count == 1)
            {
                cb_QCURL.BackColor = Color.LightGreen;
                toolTip1.SetToolTip(cb_QCURL, "QC/ALM URL (ends with qcbin)");
            }
            else if (Regex.Matches(cb_QCURL.Text, @"http://(.+?)(:\d+)?/qcbin$").Count == 1)
            {
                cb_QCURL.BackColor = Color.Orange;
                toolTip1.SetToolTip(cb_QCURL, "Prefer https if configured");
            }
            else
            {
                cb_QCURL.BackColor = Color.Tomato;
                toolTip1.SetToolTip(cb_QCURL, "QC/ALM URL (ends with qcbin)");
            }
        }

    }
}