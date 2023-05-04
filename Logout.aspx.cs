using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Reports
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Convert.ToString(Session["Active"]) == "1")
            {
                Session["Active"] = "";
                Session["ActiveUser"] = "";
                Session["MenuView"] = "";
                Session["ActiveUserCode"] = "";
                Session["ShowSensitive"] = "";
                Session["Bank"] = "";
                Session["BankName"] = "";
                Session["TerminalPrefix"] = "";
                Session["Transactions"] = "";
                Session["BINS"] = "";
                Session["BINPrefixs"] = "";
                Session["AdminBINS"] = "";
                Session["LogoFileName"] = "";
                Session["VISAReconDB"] = "";
                Session["ParticipantID"] = "";
                Session["IssuerNo"] = "";
                Session["ExtractsFolder"] = "";
                Session["ApbsIPport"] = "";
                Session["gharPayIdDetail"] = "";
                Session["UserEmail"] = "";
                Session["UserMobile"] = "";

            }

            Session.Abandon();
            Session.Clear();

            Response.Redirect("/Login");
        }
    }
}
