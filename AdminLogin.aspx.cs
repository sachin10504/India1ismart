using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Reports
{
    public partial class AdminLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        
        {
            if (Convert.ToString(Session["Active"]) != "1") Response.Redirect("/Login");

            if (IsPostBack)
            {
               
            }


        }
        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            //start sheetal to clear checkboxes and redirect to respective page
            //atpcm 1632 change by abhishek
            (Session["Username"]) = "";
            Response.Redirect("InsertUser.aspx");
            
            //or
            //Server.Transfer("~/AL");
        }
    }
}