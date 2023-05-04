using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Reports.Utilities.Loggers;

namespace Reports
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (Convert.ToString(Session["Active"]) != "1") Response.Redirect("/Login");

            if (Convert.ToInt32(Generix.getData("dbo.LoginUser", "DateDiff(d, LastPassChangedOn, GetDate())", "UserName='" + Convert.ToString(Session["ActiveUser"]) + "'", "", "", 3).Rows[0][0]) > 30)
            {
                Response.Redirect("/Cp");
            }

            
            if (Convert.ToString(Session["MenuView"]).IndexOf("1") < 0)
            {
                if (Convert.ToString(Session["MenuView"]).IndexOf("2") < 0) 
                    Response.Redirect("/Logout.aspx");
                else
                    Response.Redirect("/Utl");
            }
            else
            {
                
                Generix.fillDropDown(ref litReportGroup, Generix.getReportGroup(Convert.ToString(Session["ActiveUser"])));
                Generix.fillDropDown(ref litReports, Generix.getReports(Convert.ToString(Session["ActiveUser"])));
                //Generix.fillDropDown(ref litTerminals, Generix.getTerminals(Convert.ToInt32(Session["Bank"])));
                //Generix.fillDropDown(ref litMerchants, Generix.getMerchants());

                              
            }
        }
    }
}
