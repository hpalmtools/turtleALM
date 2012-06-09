// ALM Tools: QCBugTraq: a plugin for TortoiseSVN/GIT/HG to write quickly meaningful
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
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace QCIssuePlugin
{
    // GUID for the QC Plugin
    
    [ComVisible(true),
      Guid("3B2D3087-EFC5-4556-88A4-644863B5CA05"),
        ClassInterface(ClassInterfaceType.None)]

    public class QCPlugin : Interop.BugTraqProvider.IBugTraqProvider
    {
        public static List<TicketItem> tickets = new List<TicketItem>();
        // Name of the QC entity: Item, Bug, Defect, ...
        public const string QCITEMNAME = "item";
        public const string QCITEMNAMEPLURAL = "items";

        public bool ValidateParameters(IntPtr hParentWnd, string parameters)
        {
            return true;
        }

        public string GetLinkText(IntPtr hParentWnd, string parameters)
        {
            return "Choose QC/ALM " + QCITEMNAMEPLURAL;
        }

        public string GetCommitMessage(IntPtr hParentWnd, string parameters, string commonRoot, string[] pathList,
                                       string originalMessage)
        {
            try
            {
                QCIssuesForm form = new QCIssuesForm(tickets);
                
                if (form.ShowDialog() != DialogResult.OK)
                    return originalMessage;

                StringBuilder result = new StringBuilder(originalMessage);
                if (originalMessage.Length != 0 && !originalMessage.EndsWith("\n"))
                    result.AppendLine();

                foreach (TicketItem ticket in form.TicketsFixed)
                {
                    if (Properties.Settings.Default.useGUID)
                    {
                        result.AppendFormat("{0} {1} - {2}",
                            Properties.Settings.Default.Verb,
                            ticket.GUID,
                            ticket.Summary);
                    }
                    else
                    {
                        result.AppendFormat("{0} {1}:{2}:{3} - {4}",
                            Properties.Settings.Default.Verb,
                            Properties.Settings.Default.DefectPrefix,
                            ticket.Context,
                            ticket.Number,
                            ticket.Summary);
                    }
                    result.AppendLine();
                }

                return result.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());                
                throw;
            }
        }
    }
}
