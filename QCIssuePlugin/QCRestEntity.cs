// ALM Tools: QCBugTraq: a plugin for TortoiseSVN/GIT to write quickly meaningful
//                       commit messages
// Copyright (C) 2012 Hewlett Packard Company
// Authors: 
//      Richard Barrett
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
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

namespace HP.AlmRestClient
{
	public class Entity
	{
		// Fields.

		private readonly XElement _entityXml;

		// Constructors.

		public Entity(XElement entityXml)
		{
			_entityXml = entityXml;
		}

		public Entity(string entityType, IDictionary<string, string> fields)
		{
			_entityXml =
				new XElement("Entity",
					new XAttribute("Type", entityType),
					new XElement("Fields")
				);

			foreach (KeyValuePair<string, string> kvp in fields)
			{
				_entityXml.Element("Fields").Add(
					new XElement("Field",
						new XAttribute("Name", kvp.Key),
						new XElement("Value", kvp.Value)
					)
				);
			}
		}

		// Properties.

		public string Type
		{
			get { return _entityXml.Attribute("Type").Value; }
		}

		// Instance methods.

		public string GetFieldValue(string fieldName)
		{
			foreach (XElement field in _entityXml.Element("Fields").Elements("Field"))
			{
				if (field.Attribute("Name").Value == fieldName)
				{
					return field.Element("Value").Value;
				}
			}
			throw new ArgumentException("Field '" + fieldName + "' not found");
		}

		public XElement GetXml()
		{
			return new XElement(_entityXml);
		}
	}
}
