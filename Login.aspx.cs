using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Data;
using Reports.Utilities.Loggers;

namespace Reports
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            bool hasException = false;
            string sException = "";

            if (IsPostBack)
            {
                try
                {
                    //RSA algo use for encryption password 20-07-18 $
                    //start
                    RSABuilder ObjRSA = new RSABuilder();
                    string username = ObjRSA.Decrypt(Convert.ToString(Request.Form["txtUserName"]));
                    string password = ObjRSA.Decrypt(Convert.ToString(Request.Form["txtpass"]));
                    if (!Regex.IsMatch(username, "^[a-zA-Z0-9]*$", RegexOptions.Compiled))
                    {
                        //Response.Redirect("/Logout");
                        throw new Exception("Unable to identify User!");
                    }
                    else
                    {                                                                        
                        DataTable drLogin = Generix.GetUserDeatilswithBank(username, password);
                        //end
                        if (drLogin.Rows.Count <= 0)
                        {
                            throw new Exception("Unable to identify User!");
                        }
                        else
                        {
                            Session["Active"] = "1";
                            Session["ActiveUser"] = username;                            
                            Session["UserStatus"] = Convert.ToString(drLogin.Rows[0]["status"]);//To check if user is active or not 
                            Session["MenuView"] = Convert.ToString(drLogin.Rows[0]["MenuView"]); //Generix.getMenuAccessRights(Convert.ToString(Request.Form["txtUserName"]));
                            Session["UtilityAccess"] = Convert.ToString(drLogin.Rows[0]["UtilitiesView"]);
                            Session["ReportAccess"] = Convert.ToString(drLogin.Rows[0]["ReportView"]);                            
                            Session["ActiveUserCode"] = Convert.ToString(drLogin.Rows[0]["Code"]);
                            Session["ShowSensitive"] = Convert.ToString(drLogin.Rows[0]["ShowSensitive"]);
                            Session["Bank"] = Convert.ToString(drLogin.Rows[0]["Bank"]);
                            Session["BankName"] = Convert.ToString(drLogin.Rows[0]["BankName"]);
                            Session["TerminalPrefix"] = Convert.ToString(drLogin.Rows[0]["TerminalPrefixs"]);
                            Session["Transactions"] = Convert.ToString(drLogin.Rows[0]["Transactions"]);
                            Session["BINS"] = Convert.ToString(drLogin.Rows[0]["BINS"]);
                            Session["BINPrefixs"] = Convert.ToString(drLogin.Rows[0]["BINPrefixs"]);
                            Session["AdminBINS"] = Convert.ToString(drLogin.Rows[0]["AdminBINS"]);
                            Session["LogoFileName"] = Convert.ToString(drLogin.Rows[0]["LogoFileName"]);
                            Session["VISAReconDB"] = Convert.ToString(drLogin.Rows[0]["VISAReconDB"]);
                            Session["ParticipantID"] = Convert.ToString(drLogin.Rows[0]["ParticipantID"]);
                            Session["IssuerNo"] = Convert.ToString(drLogin.Rows[0]["IssuerNo"]);
                            Session["ExtractsFolder"] = Convert.ToString(drLogin.Rows[0]["ExtractFolder"]);
                            Session["ApbsIPport"] = Convert.ToString(drLogin.Rows[0]["ApbsIpPort"]);
                            Session["gharPayIdDetail"] = Convert.ToString(drLogin.Rows[0]["gharPayIdDetail"]);
                            Session["forgotPassIdDetail"] = Convert.ToString(drLogin.Rows[0]["ForgotPassIdDetail"]);
                            Session["ApbsDefaultAcc"] = Convert.ToString(drLogin.Rows[0]["apbsDefaultAcc"]);
                            Session["Branch"] = Convert.ToString(drLogin.Rows[0]["BranchName"]);
                            Session["BranchCode"] = Convert.ToString(drLogin.Rows[0]["BranchCode"]);
                            Session["RegularExpression_Terminal"] = Convert.ToString(drLogin.Rows[0]["RegularExpression_Terminal"]);
                            Session["SwitchType"] = Convert.ToString(drLogin.Rows[0]["SwitchType"]);
                            Session["AcquirerId"] = Convert.ToString(drLogin.Rows[0]["AcquirerId"]);
                            Session["FileName"] = "";
                            Session["isTMKSerialKey"] = Convert.ToString(drLogin.Rows[0]["isTMKSerialKey"]); 

                            //Session.Timeout = 900;
                            Session.Timeout = 15;

                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Log In", "", "");
                        }
                    }
                }
                catch (Exception xObj)
                {
                    Session.Abandon();
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
                    Session["ExtractsFolder"] = "";
                    Session["ApbsIPport"] = "";
                    Session["gharPayIdDetail"] = "";
                    Session["forgotPassIdDetail"] = "";
                    Session["ApbsDefaultAcc"] = "";
                    Session["Branch"] = "";
                    Session["BranchCode"] = "";
                    Session["UtilityAccess"] = "";
                    Session["ReportAccess"] = "";

                    hasException = true;
                    sException = xObj.Message;                    
                }

                if (hasException)
                    Response.Redirect("/Error?Err=" + sException);
                else
                    routeToDefaultPage();
            }
            else
            {
                //RSA algo use for encryption password 20-07-18 $
                if (Convert.ToString(Session["Active"]) == "1")
                {
                    routeToDefaultPage();
                }
                
                HdnPublic.Value = new RSABuilder().InitiateRSA();

            }
        }
        /*new change for admin forntend*/

        private void routeToDefaultPage()
        {
            if (Convert.ToString(Session["UserStatus"]) == "1")
            {
                if (Convert.ToString(Session["MenuView"]) == "")
                {
                    Response.Redirect("/Error?Err=No Access Rights Assigned!");
                }
                else
                {
                    string[] aMenuAccessRights = Convert.ToString(Session["MenuView"]).Split(',');

                    string sMenuAccessRights = (Array.Find(aMenuAccessRights, element => element.Equals("1")));
                    #region
                    if (sMenuAccessRights == null)
                    {
                        sMenuAccessRights = (Array.Find(aMenuAccessRights, element => element.Equals("2")));

                        if (sMenuAccessRights == null)
                        {
                            Response.Redirect("/Error?Err=No Access Rights Assigned!");
                        }
                        else
                        {
                            Response.Redirect("/utl");
                        }
                    }
                    #endregion
                    else
                    {                        
                        //Response.Redirect("~");
                        Response.Redirect("/main");                        
                    }
                }
            }
            else
            {
                if (Convert.ToString(Session["UserStatus"]) == "0")
                {
                    string script = "<script>$(document).ready(function(){$('#divDialog').html('UserID Is Deactivated! Please Contact Your Admin!!<br/>').dialog({title: 'Notice',show: 'slide',hide: 'blind',modal: true,buttons: {'Ok': function () {$(this).dialog('close');";
                    script += "window.open('/Logout.aspx');";
                    script += "location.replace(window.location.href );}}});});</script>";
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "", script);
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "User Is Deactivated!", "<script>$(document).ready(function(){$('#divDialog').append('UserID Is Deactivated! Please Contact Your Admin!').dialog({title: 'Note...',show: 'slide',hide: 'blind',modal: true,buttons: {'Ok': function () {$(this).dialog('close');$(this).dialog('destroy');}}});});</script>");
                }
                else
                {
                    string script = "<script>$(document).ready(function(){$('#divDialog').html('UserID Had Been Deleted! Please Contact Your Admin!!<br/>').dialog({title: 'Notice',show: 'slide',hide: 'blind',modal: true,buttons: {'Ok': function () {$(this).dialog('close');";
                    script += "window.open('/Logout.aspx');";
                    script += "location.replace(window.location.href );}}});});</script>";
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "", script);
                    //  Response.Redirect("~/Login");
                    ////    ClientScript.RegisterClientScriptBlock(this.GetType(), "UserID Had Been Deleted! Please Contact Your Admin!", "<script>$(document).ready(function(){$('#divDialog').append('UserID Had Been Deleted! Please Contact Your Admin!').dialog({title: 'Note...',show: 'slide',hide: 'blind',modal: true,buttons: {'Ok': function () {$(this).dialog('close');$(this).dialog('destroy');}}});});</script>");
                }
            }
        }
        #region old code before admin forntend
        /* old code 
        private void routeToDefaultPage()
        {
            if (Convert.ToString(Session["MenuView"]) == "")
            {
                Response.Redirect("/Error?Err=No Access Rights Assigned!");
            }
            else
            {
                string[] aMenuAccessRights = Convert.ToString(Session["MenuView"]).Split(',');

                string sMenuAccessRights = (Array.Find(aMenuAccessRights, element => element.Equals("1")));

                if (sMenuAccessRights == null)
                {
                    sMenuAccessRights = (Array.Find(aMenuAccessRights, element => element.Equals("2")));

                    if (sMenuAccessRights == null)
                    {
                        Response.Redirect("/Error?Err=No Access Rights Assigned!");
                    }
                    else
                    {
                        Response.Redirect("/utl");
                    }
                }
                else
                {
                    Response.Redirect("~");
                }
            }
        }
        */
        #endregion 
    }
}
