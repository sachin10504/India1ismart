using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace Reports
{
    public partial class InsertUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {

                FunFillDropDown();
                //selectchkboxforRoleRights();
                string url = HttpContext.Current.Request.Url.AbsoluteUri;
                // http://localhost:1302/TESTERS/Default6.aspx

                string path = HttpContext.Current.Request.Url.AbsolutePath;
                // /TESTERS/Default6.aspx

                string host = HttpContext.Current.Request.Url.Host;
                // localhost
                if (Convert.ToString(Session["Active"]) != "1") Response.Redirect("/Login");

                if (!Generix.utilityAccessAllowed("USRMNG") && !Generix.utilityAccessAllowed("AL")) 
                {
                    Response.Redirect("/"); 
                }

                if (Convert.ToString(Request.QueryString["id"]) != null)
                {
                    String Username = Convert.ToString(Request.QueryString["id"]);
                    DataTable _DT_Rights = Generix.getUserRightsDetail(Username);
                    if (_DT_Rights.Rows.Count > 0)
                    {
                        CreateUser.Visible = false;
                        EditUser.Visible = true;
                        foreach (DataRow dt in _DT_Rights.Rows)
                        {
                            Session["Username"] = dt["username"].ToString();
                            txtUserName.Text = dt["username"].ToString();
                            txtUserName.Enabled = false;
                            txtFirstName.Text = dt["FirstName"].ToString();
                            txtLastName.Text = dt["LastName"].ToString();
                            emailId.Text = dt["EMAILID"].ToString();
                            mobile.Text = dt["MobileNo"].ToString();

                            //if (_DT_Rights.Rows[0][9].columnname = "RoleId")
                            //{
                            //    ddlRoleMaper.Items.FindByValue(dt["RoleId"].ToString()).Selected = true;
                            //}
                            string RoleId= dt["RoleId"].ToString();

                            if (RoleId != null && RoleId.Trim() != "")
                            {
                                ddlRoleMaperReadOnly.ClearSelection();
                                ddlRoleMaperReadOnly.Items.FindByValue(dt["RoleId"].ToString()).Selected = true;
                            }
                                                  
                            //start sheetal to hide showsensitive data field
                            //if (dt["ShowSensitiveData"].ToString() == "True") { showSensitive.Checked = true; }
                            //else { showSensitive.Checked = false; }
                            //if (dt["Checker"].ToString() == "True") { checkerChk.Checked = true; }
                            //else { checkerChk.Checked = false; }
                            txtUserConfirmPass.Text = "******";
                            txtUserConfirmPass.Enabled = false;
                            txtUserPass.Text = "******";
                            txtUserPass.Enabled = false;

                        }

                    }
                }
                else
                {
                    DataTable _DT_Rights = Generix.FunfillRoleMastergridView();
                    if (_DT_Rights.Rows.Count > 0)
                    {
                        Session["RollId"] = _DT_Rights.Rows[0]["Id"].ToString();
                    }
                }
            }
        }

        protected void FunFillDropDown()
        {
            DataTable DtOutput = Generix.Funfilldropdown();
            ddlRoleMaperReadOnly.DataSource = DtOutput;
            ddlRoleMaperReadOnly.DataTextField = "RollMaster";
            ddlRoleMaperReadOnly.DataValueField = "Id";
            ddlRoleMaperReadOnly.DataBind();
        }

        protected void btnback_Click(object sender, EventArgs e)
        {
            Session["Username"] = " ";
            Response.Redirect("~/utl");
        }

        

        //[System.Web.Services.WebMethod]
        //protected DataTable selectchkboxforRoleRights()
        //{

        //    SqlConnection oConn = null;
        //    DataTable dtReturn = new DataTable();
        //    try
        //    {
        //        Generix.getConnection(ref oConn, 3);
        //        string commandText = "SELECT PageId,UtilityID FROM RollMaper where RollId = '" + Convert.ToInt32(ddlRoleMaper.SelectedItem.Value) + "'";
        //        SqlCommand cmd = new SqlCommand(commandText, oConn);
        //        oConn.Open();
        //        cmd.ExecuteNonQuery();
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da.Fill(dtReturn);
        //        oConn.Close();
        //        if (dtReturn != null)
        //        {
        //            if (dtReturn.Rows.Count > 0)
        //            {
        //                string Pageid = dtReturn.Rows[0]["PageId"].ToString();
        //                string UtilityID = dtReturn.Rows[0]["UtilityID"].ToString();

        //                string[] reporttype = Pageid.Split(',');
        //                for (int i = 0; i < reporttype.Length; i++)
        //                {
        //                    reporttype[i] = reporttype[i];
        //                }

        //                string[] UtilitiesName = UtilityID.Split(',');
        //                for (int i = 0; i < UtilitiesName.Length; i++)
        //                {
        //                    UtilitiesName[i] = UtilitiesName[i];
        //                }
        //            }


        //            return dtReturn;
        //        }
        //        return dtReturn;

        //        //1.class = chkBoxMenuGroup 
        //        //2.class = chkBoxUtilities
        //    }

        //    catch (Exception xObj)
        //    {
        //        dtReturn = null;
        //        return dtReturn;
        //    }

        //}
    }
}
