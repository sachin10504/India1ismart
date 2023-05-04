using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Reports
{
    public partial class errorPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string sMessage = "";

            if (Request.QueryString["Err"] != null)
            {
                sMessage = Request.QueryString["Err"].ToString();
            }
            else if (Convert.ToString(Session["Active"]) != "1")
            {
                sMessage = "Session Timeout....!";
            }
            else
            {
                sMessage = "Unknown Error Occurred!";
            }

            litErrorMessage.Text = sMessage;
        }
    }
}
