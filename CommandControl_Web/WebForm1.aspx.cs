using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CommandControl_Web
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string userName;
            userName = User.Identity.Name;
            greetingLabel.Text = "Welcome " + userName;
            HttpClientCertificate cert = Request.ClientCertificate;
            if (cert.IsPresent)
                certLabel.Text = cert.Get("SUBJECT O");
            else
                certLabel.Text = "No certificate was found.";
        }
    }
}