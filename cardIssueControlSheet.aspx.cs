using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Reports
{
    public partial class cardIssueControlSheet : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Convert.ToString(Session["Active"]) != "1") { Response.Redirect("/Login"); }

            if (!Generix.utilityAccessAllowed("CICS")) { Response.Redirect("/"); }

            Generix.fillDropDown(ref bcCodeLit, Generix.getBcCodeList(), true);
            Generix.fillDropDown(ref cardsubTypeLit, Generix.getcardTypeSubType(), true);
        }
    }
}