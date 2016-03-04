using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Linq;
using System.Xml.Linq;
using System.Text;

namespace PredatorDev.Spudz
{
    public class FogBugzManager
    {
        #region private_properties

        private string password { get; set; }
        private string token { get; set; }
        private HttpClient client { get; set; }

        private string _tickets;

        #endregion

        #region public_properties

        public Uri URL { get; private set; }
        public string UserName { get; private set; }
        public string Tickets
        {
            get
            {
                List<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>();
                values.Add(new KeyValuePair<string, string>("token", this.token));
                values.Add(new KeyValuePair<string, string>("cmd", "search"));
                values.Add(new KeyValuePair<string, string>("q", "*"));
                values.Add(new KeyValuePair<string, string>("cols", "sTitle,sStatus,sPersonAssignedTo,sPriority"));

                string response = this.submitRequest(values);
                XElement xml = parseXmlResponse(response);
                IEnumerable<XElement> cases =
                    from fbCase in xml.Descendants("case")
                    select fbCase;

                StringBuilder casesString = new StringBuilder();
                casesString.AppendLine("  bug  |     Priority    |          Status           |        Assigned To        |   Title");
                foreach (XElement fbCase in cases)
                {
                    casesString.AppendFormat("{0, -6} | {1, -15} | {2, -25} | {3, -25} | {4}\n",
                        fbCase.Attribute("ixBug").Value,
                        fbCase.Element("sPriority").Value,
                        fbCase.Element("sStatus").Value,
                    fbCase.Element("sPersonAssignedTo").Value,
                    fbCase.Element("sTitle").Value);
                }

                return casesString.ToString();
            }

            private set
            {
                this._tickets = value;
            }
        }

        #endregion

        #region constructors_destructors

        //public FogBugzManager(string URL, string UserName, string Password)
        //{
        //    this.URL = new Uri(URL);
        //    this.client = new HttpClient();
        //    this.UserName = UserName;
        //    this.password = Password;
        //    this.token = null;
        //    this.Tickets = null;
        //    this.signon();
        //}

        public FogBugzManager(string URL, string Token)
        {
            this.URL = new Uri(URL);
            this.client = new HttpClient();
            this.UserName = null;
            this.password = null;
            this.token = Token;
            this.Tickets = null;
            this.signon();
        }

        #endregion

        #region public_member_methods

        #endregion

        #region private_member_methods

        private void signon()
        {
            List<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>();
            if (this.token == null)
            {
                if (this.UserName == null || this.password == null)
                {
                    throw new ArgumentException("No username or password!");
                }

                values.Add(new KeyValuePair<string, string>("email", this.UserName));
                values.Add(new KeyValuePair<string, string>("password", this.password));
            }
            else
            {
                values.Add(new KeyValuePair<string, string>("token", this.token));
            }

            values.Add(new KeyValuePair<string, string>("cmd", "logon"));

            string xmlResponse = this.submitRequest(values);
            XElement responseElement = parseXmlResponse(xmlResponse);
            if (this.token == null)
            {
                this.token = responseElement.Descendants("token").FirstOrDefault()?.Value;
            }
        }

        private void signoff()
        {
            List<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>();
            values.Add(new KeyValuePair<string, string>("token", this.token));
            values.Add(new KeyValuePair<string, string>("cmd", "logoff"));
            parseXmlResponse(this.submitRequest(values));
        }

        private string submitRequest(List<KeyValuePair<string, string>> Values)
        {
            // CREDIT: http://www.asp.net/web-api/overview/advanced/calling-a-web-api-from-a-net-client
            // CREDIT: jeffrey richtor - wintellectnow.com - advanced threading
            // CREDIT: http://www.asp.net/web-api/overview/formats-and-model-binding/media-formatters
            // CREDIT: http://ronaldrosiernet.azurewebsites.net/Blog/2013/12/07/posting_urlencoded_key_values_with_httpclient

            HttpContent content = new FormUrlEncodedContent(Values);

            FormUrlEncodedMediaTypeFormatter formatter = new FormUrlEncodedMediaTypeFormatter();
            HttpResponseMessage response = this.client.PostAsync(this.URL, content).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }
            else
            {
                return "ERROR";
            }
        }

        private XElement parseXmlResponse(string XML)
        {
            XElement responseXml = XElement.Parse(XML);

            IEnumerable<XElement> errors =
                from error in responseXml.Descendants("error")
                select error;

            if (errors.Count() > 0)
            {
                throw ParseResponseErrors(errors);
            }

            return responseXml;
        }

        private Exception ParseResponseErrors(IEnumerable<XElement> Errors)
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine("ERROR(s): ");

            for (int i = 0; i < Errors.Count(); i++)
            {
                message.AppendFormat("   [ {0} ] {1} \n", Errors.ElementAt(i).Attribute("code").Value, Errors.ElementAt(i).Value);
            }

            return new Exception(message.ToString());
        }

        #endregion
    }
}
