using System;
using System.Net.Http;

namespace PredatorDev.Spudz
{
    public class FogBugzManager
    {
        #region private_properties

        private string password { get; set; }
        private string token { get; set; }
        private HttpClient client { get; set; }

        #endregion

        #region public_properties

        public Uri URL { get; private set; }
        public string UserName { get; private set; }
        public object Tickets { get; private set; }

        #endregion

        #region constructors_destructors

        public FogBugzManager(string URL, string UserName, string Password)
        {
            this.URL = new Uri(URL);
            this.client = new HttpClient();
            this.UserName = UserName;
            this.password = Password;
            this.Tickets = null;
        }

        #endregion

        #region public_member_methods

        public string signon()
        {
            // CREDIT: http://www.asp.net/web-api/overview/advanced/calling-a-web-api-from-a-net-client
            HttpResponseMessage response = (this.client.GetAsync("https://predatordev.fogbugz.com/api.asp?token=mt9lqft81o08r4la3ogcod3ovdjl7f&cmd=logon")).Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsStringAsync().Result;
            }
            else
            {
                return "error";
            }
        }

        #endregion
    }
}
