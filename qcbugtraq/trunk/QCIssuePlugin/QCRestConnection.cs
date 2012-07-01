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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace HP.AlmRestClient
{
	public class AlmRestConnection
	{
		private string _domain;
		private string _project;
        private bool _sessionOpened = false;
        private bool _Authenticated = false;
        private readonly string _serverUrl;
		private  string _queryBase;
		private readonly int _queryPageSize;
		private int _totalQueries = 0;
        private const string CLIENT_NAME = "REST client: TurtleALM";

		private CookieCollection _cookies;

        public AlmRestConnection(string serverUrl)
            : this(serverUrl, 0)
        { }
        
		public AlmRestConnection(string serverUrl, int queryPageSize)
		{
			_domain = "";
			_project = "";

			if (!serverUrl.EndsWith("/"))
			{
				serverUrl += "/";
			}
			_serverUrl = serverUrl;

			if (queryPageSize < 0)
			{
				throw new ArgumentException("queryPageSize must be 0 (max) or greater");
			}
			_queryPageSize = queryPageSize;
		}

        public string Domain
        {
            get { return _domain; }
            set
            {
                _domain = value; _queryBase = "rest/domains/" + _domain + "/projects/" + _project + "/";

            }
        }

        public string Project
        {
            get { return _project; }
            set
            {
                _project = value; _queryBase = "rest/domains/" + _domain + "/projects/" + _project + "/";

            }
        }

        public bool SessionOpened
        {
            get { return _sessionOpened; }
        }

        public bool Authenticated
        {
            get { return _Authenticated; }
        }

        private string PageSizeString
		{
			get { return "page-size=" + ((_queryPageSize == 0) ? "max" : _queryPageSize.ToString()); }
		}

		public int TotalQueries
		{
			get { return _totalQueries; }
		}

		public bool Authenticate(string username, string password)
		{
			_cookies = new CookieCollection(); // New collection every time we login.

			// ALM expects an Authorization header that looks like "Basic username:password" where "username:password" is Base64 encoded.
		    byte[] credBytes = Encoding.UTF8.GetBytes(username + ":" + password);
		    string credEncodedString = "Basic " + Convert.ToBase64String(credBytes);

			Uri secureBaseUri = new Uri(_serverUrl);
			Uri authenticationUri;
            
			if (!Uri.TryCreate(secureBaseUri, "authentication-point/authenticate", out authenticationUri))
			{
				throw new ArgumentException("Invalid URI: " + _serverUrl);
			}

			HttpWebRequest req = (HttpWebRequest)WebRequest.Create(authenticationUri);
            //WebProxy proxy = new WebProxy("http://127.0.0.1:8888/");
            //req.Proxy = proxy;
			req.Method = "GET";
			req.PreAuthenticate = true;
			req.Headers["Authorization"] = credEncodedString;
			CookieContainer cookies = new CookieContainer();
			req.CookieContainer = cookies;

			HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

            if (resp.StatusCode == HttpStatusCode.OK)
            {
                _cookies.Add(cookies.GetCookies(new Uri(_serverUrl))["LWSSO_COOKIE_KEY"]);
                _Authenticated = true;
            }

			resp.Close();

            return _Authenticated;
		}

        public List<string> GetDomainOrProject(bool isDomain)
        {
            List<string> domainsOrProjects = new List<string>();
            Uri domainsProjectsUri;
            if (isDomain)
            {
                if (!Uri.TryCreate(new Uri(_serverUrl), "rest/domains", out domainsProjectsUri))
                {
                    throw new ArgumentException("Invalid URI: " + _serverUrl);
                }
            }
            else
            {
                if (!Uri.TryCreate(new Uri(_serverUrl), "rest/domains/" + _domain + "/projects", out domainsProjectsUri))
                {
                    throw new ArgumentException("Invalid URI: " + _serverUrl);
                }
            }

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(domainsProjectsUri);
            req.Method = "GET";
            req.CookieContainer = new CookieContainer();
            req.CookieContainer.Add(_cookies);

            HttpWebResponse resp = null;
            try
            {
                resp = (HttpWebResponse)req.GetResponse();
                if (resp.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception("ALM failed to get domains or projects: " + resp.ToString());
                }

                using (Stream s = resp.GetResponseStream())
                {
                    XElement responseXml = null; 
                    responseXml = XElement.Load(s);
                    List<string> lDomainsOrProjects = new List<string>();
                    foreach (XElement domainOrProject in responseXml.Nodes())
                    {
                        lDomainsOrProjects.Add(domainOrProject.Attribute("Name").Value);
                    }
                    return lDomainsOrProjects;
                }
            }
            finally
            {
                try
                {
                    if (resp != null) resp.Close();
                }
                catch { }
            }
        }

        public List<string> GetProjects()
        {
            List<string> projects = new List<string>();
            Uri projectsUri;
            if (!Uri.TryCreate(new Uri(_serverUrl), "rest/domains/" + _domain + "/projects", out projectsUri))
            {
                throw new ArgumentException("Invalid URI: " + _serverUrl);
            }

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(projectsUri);
            req.Method = "GET";
            req.CookieContainer = new CookieContainer();
            req.CookieContainer.Add(_cookies);

            HttpWebResponse resp = null;
            try
            {
                resp = (HttpWebResponse)req.GetResponse();
                if (resp.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception("ALM failed to create the requested resource");
                }

                using (Stream s = resp.GetResponseStream())
                {
                    XElement responseXml = null;
                    responseXml = XElement.Load(s);
                    List<string> lDomains = new List<string>();
                    foreach (XElement domain in responseXml.Nodes())
                    {
                        lDomains.Add(domain.Attribute("Name").Value);
                    }
                    return lDomains;
                }
            }
            finally
            {
                try
                {
                    if (resp != null) resp.Close();
                }
                catch { }
            }
        }

		public void CloseSession()
		{
            try
            {
                Uri closeSessionUri;
                if (!Uri.TryCreate(new Uri(_serverUrl), "rest/site-session", out closeSessionUri))
                {
                    throw new ArgumentException("Invalid URI: " + _serverUrl);
                }

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(closeSessionUri);
                req.Method = "DELETE";
                req.CookieContainer = new CookieContainer();
                req.CookieContainer.Add(_cookies);

                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                _sessionOpened = false;
                resp.Close();
            }
            catch {
                // Just ignore, we are just trying to disconnect
                // If session is not opened anymore, we may get a 403
            }
		}

		public bool OpenSession()
		{
			// /qcbin/rest/site-session POST
			Uri openSessionUri;
			if (!Uri.TryCreate(new Uri(_serverUrl), "rest/site-session", out openSessionUri))
			{
				throw new ArgumentException("Invalid URI: " + _serverUrl);
			}

			HttpWebRequest req = (HttpWebRequest)WebRequest.Create(openSessionUri);
			req.Method = "POST";
			CookieContainer cookies = new CookieContainer();
			req.CookieContainer = cookies;
			req.CookieContainer.Add(_cookies);
            req.ContentType = "application/xml"; 

            // Custom Client Type
            string sessionParams = "<session-parameters><client-type>" + CLIENT_NAME + "</client-type></session-parameters>";
            ASCIIEncoding enc = new ASCIIEncoding();
            byte[] bSessionParams = enc.GetBytes(sessionParams);
            req.ContentLength = bSessionParams.Length;
            using (Stream reqStream = req.GetRequestStream())
                reqStream.Write(bSessionParams, 0, bSessionParams.Length);
           
			HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

            if (resp.StatusCode == HttpStatusCode.OK)
            {
                _cookies.Add(cookies.GetCookies(new Uri(_serverUrl))["QCSession"]);
                _sessionOpened = true;
            }
			resp.Close();
            return _sessionOpened;
        }

		public void Unauthenticate()
		{
            try
            {
                Uri logoutUri;
                if (!Uri.TryCreate(new Uri(_serverUrl), "authentication-point/logout", out logoutUri))
                {
                    throw new ArgumentException("Invalid URI: " + _serverUrl);
                }

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(logoutUri);
                req.Method = "GET";
                req.CookieContainer = new CookieContainer();
                req.CookieContainer.Add(_cookies);

                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                resp.Close();
                _Authenticated = false;
            }
            catch
            {
                // Just ignore, we are just trying to disconnect
                // If session is not opened anymore, we may get a 403
            }
		}

		public XElement Query(string queryString)
		{
			string query = _queryBase + Uri.EscapeUriString(queryString);
			if (queryString.Contains('?'))
			{
				query += "&" + PageSizeString;
			}
			else
			{
				query += "?" + PageSizeString;
			}

			int numPages = 0;
			int pageSize = 0;
			int currentPage = 1;
			List<XElement> xmlResults = new List<XElement>();
			do
			{
				_totalQueries++;

				// Get next page of result.
				Uri queryUri;
				int startIndex = ((currentPage - 1) * pageSize) + 1;
				string startIndexString = "&start-index=" + startIndex.ToString();

				if (!Uri.TryCreate(new Uri(_serverUrl), query + startIndexString, out queryUri))
				{
					throw new ArgumentException("Invalid URI: " + _serverUrl);
				}

				HttpWebRequest req = (HttpWebRequest)WebRequest.Create(queryUri);
				req.Method = "GET";
				req.CookieContainer = new CookieContainer();
				req.CookieContainer.Add(_cookies);

				HttpWebResponse resp = null;
				XElement responseXml = null;
				try
				{
					resp = (HttpWebResponse)req.GetResponse();
					using (Stream s = resp.GetResponseStream())
					{
						responseXml = XElement.Load(s);
						xmlResults.Add(responseXml);
					}
				}
				finally
				{
					try
					{
						if (resp != null) resp.Close();
					}
					catch { }
				}

				if (numPages == 0) // Calculate numPages and pageSize.
				{
                    int totalResults = 0;
                    if (responseXml.Attribute("TotalResults") != null)
                        Int32.TryParse(responseXml.Attribute("TotalResults").Value, out totalResults);
                    else
                        totalResults = 0;
					int entityCount = responseXml.Elements("Entity").Count();
					if (entityCount < totalResults)
					{
						pageSize = entityCount;
						numPages = (totalResults + pageSize - 1) / pageSize;
					}
					else
					{
						numPages = 1;
						// pageSize unimportant.
					}
				}
				currentPage++;
			} while (currentPage <= numPages);

			// Concat result sets.
			XElement fullXml = new XElement(xmlResults[0]);
			for (int i = 1; i < xmlResults.Count; i++)
			{
				foreach (XElement xEntity in xmlResults[i].Elements("Entity"))
				{
					fullXml.Add(xEntity);
				}
			}
			return fullXml;
		}

		public XElement Create(string queryString, XElement entityXml)
		{
			string query = _queryBase + Uri.EscapeUriString(queryString);

			Uri queryUri;
			if (!Uri.TryCreate(new Uri(_serverUrl), query, out queryUri))
			{
				throw new ArgumentException("Invalid URI: " + _serverUrl);
			}

			HttpWebRequest req = (HttpWebRequest)WebRequest.Create(queryUri);
			req.Method = "POST";
			req.Accept = "application/xml";
			req.PreAuthenticate = true;
			CookieContainer cookies = new CookieContainer();
			req.CookieContainer = cookies;
			req.CookieContainer.Add(_cookies);
			req.ContentType = "application/xml";
			byte[] requestData = Encoding.UTF8.GetBytes(entityXml.ToString());
			using (Stream reqStream = req.GetRequestStream())
			{
				reqStream.Write(requestData, 0, requestData.Length);
			}

			HttpWebResponse resp = null;
			try
			{
				resp = (HttpWebResponse)req.GetResponse();
				if (resp.StatusCode != HttpStatusCode.Created)
				{
					throw new Exception("ALM failed to create the requested resource");
				}

				using (Stream s = resp.GetResponseStream())
				{
					return XElement.Load(s);
				}
			}
			finally
			{
				try
				{
					if (resp != null) resp.Close();
				}
				catch { }
			}
		}

		public XElement Update(string queryString, XElement entityXml)
		{
			string query = _queryBase + Uri.EscapeUriString(queryString);

			Uri queryUri;
			if (!Uri.TryCreate(new Uri(_serverUrl), query, out queryUri))
			{
				throw new ArgumentException("Invalid URI: " + _serverUrl);
			}

			HttpWebRequest req = (HttpWebRequest)WebRequest.Create(queryUri);
			req.Method = "PUT";
			req.Accept = "application/xml";
			req.PreAuthenticate = true;
			CookieContainer cookies = new CookieContainer();
			req.CookieContainer = cookies;
			req.CookieContainer.Add(_cookies);
			req.ContentType = "application/xml";
			byte[] requestData = Encoding.UTF8.GetBytes(entityXml.ToString());
			using (Stream reqStream = req.GetRequestStream())
			{
				reqStream.Write(requestData, 0, requestData.Length);
			}

			HttpWebResponse resp = null;
			try
			{
				resp = (HttpWebResponse)req.GetResponse();
				if (resp.StatusCode != HttpStatusCode.Created)
				{
					throw new Exception("ALM failed to create the requested resource");
				}

				using (Stream s = resp.GetResponseStream())
				{
					return XElement.Load(s);
				}
			}
			finally
			{
				try
				{
					if (resp != null) resp.Close();
				}
				catch { }
			}
		}

		public XElement CreateAttachment(string entityType, int entityId, byte[] attachment, string filename)
		{
			string query = String.Format("{0}{1}/{2}/attachments", _queryBase, entityType, entityId);

			Uri queryUri;
			if (!Uri.TryCreate(new Uri(_serverUrl), query, out queryUri))
			{
				throw new ArgumentException("Invalid URI: " + _serverUrl);
			}

			//Headers: 
			//Content-Type: application/octet-stream
			//Slug: filename.txt
			//Data: Binary content of file.

			HttpWebRequest req = (HttpWebRequest)WebRequest.Create(queryUri);
			req.Method = "POST";
			req.PreAuthenticate = true;
			req.CookieContainer = new CookieContainer();
			req.CookieContainer.Add(_cookies);
			req.ContentType = "application/octet-stream";
			req.Headers["Slug"] = filename;

			using (Stream reqStream = req.GetRequestStream())
			{
				reqStream.Write(attachment, 0, attachment.Length);
			}

			HttpWebResponse resp = null;
			try
			{
				resp = (HttpWebResponse)req.GetResponse();
				if (resp.StatusCode != HttpStatusCode.Created)
				{
					throw new Exception("ALM failed to create the requested resource");
				}

                using (Stream s = resp.GetResponseStream())
				{
					return XElement.Load(s);
				}
			}
			finally
			{
				try
				{
					if (resp != null) resp.Close();
				}
				catch { }
			}
		}
	}
}
