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
using System.Text;

namespace QCIssuePlugin
{
    public class TicketItem
    {
        private readonly string _ticketContext; 
        private readonly int _ticketNumber;
        private readonly string _ticketStatus;
        private readonly string _ticketSummary;
        private readonly string _ticketLastModified;
        private readonly string _ticketTargetRel;
        private readonly string _ticketOwner;
        private readonly string _ticketGUID;


        public TicketItem(string ticketContext, int ticketNumber, string ticketStatus, string ticketSummary, string ticketOwner, string ticketTargetRel, string ticketLastModified, string ticketGUID)
        {
            _ticketContext = ticketContext;
            _ticketNumber = ticketNumber;
            _ticketStatus = ticketStatus;
            _ticketSummary = ticketSummary;
            _ticketTargetRel = ticketTargetRel;
            _ticketLastModified = ticketLastModified;
            _ticketOwner = ticketOwner;
            _ticketGUID = ticketGUID;
        }

        public string Context
        {
            get { return _ticketContext; }
        }

        public int Number
        {
            get { return _ticketNumber; }
        }

        public string Status
        {
            get { return _ticketStatus; }
        }

        public string Summary
        {
            get { return _ticketSummary; }
        }

        public string LastModified
        {
            get { return _ticketLastModified; }
        }

        public string TargetRel
        {
            get { return _ticketTargetRel; }
        }

        public string Owner
        {
            get { return _ticketOwner; }
        }

        public string GUID
        {
            get { return _ticketGUID; }
        }
    }
}
