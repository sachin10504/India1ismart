using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ReflectionIT.Common.Data.Configuration;
using ReflectionIT.Common.Data.SqlClient;
using Reports.Utilities.Loggers;

namespace Reports
{
    public partial class HardwareConfiguration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Convert.ToString(Session["Active"]) != "1") { Response.Redirect("/Login"); }


        }
        protected void btnConfigure_Click(object sender, EventArgs e)
        {
            if (txtTerminalId.Text.Trim() != "")
            {
                DataTable DTOutput = new DataTable();
                try
                {
                    using (SqlStoredProcedure sspObj = new SqlStoredProcedure("dbo.USP_HardwareConfigurationAGS", ConfigManager.GetRBSQLDBOLAPConnection, CommandType.StoredProcedure))
                    {
                        sspObj.AddParameterWithValue("@BankId", SqlDbType.Int, 0, ParameterDirection.Input, Convert.ToInt32(Session["Bank"]));
                        sspObj.AddParameterWithValue("@TerminalId", SqlDbType.VarChar, 0, ParameterDirection.Input, txtTerminalId.Text);
                        sspObj.AddParameterWithValue("@BATUserId", SqlDbType.BigInt, 0, ParameterDirection.Input, Convert.ToString(Session["ActiveUserCode"]));

                        DTOutput = sspObj.ExecuteDataTable();

                        if (DTOutput.Rows.Count > 0)
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), "<script>$(document).ready(function(){$('#divDialog').append('" + Convert.ToString(DTOutput.Rows[0][0]) + "').dialog({title: 'Information...',show: 'slide',hide: 'blind',modal: true,buttons: {'Ok': function () {$(this).dialog('close');$(this).dialog('destroy');}}});});</script>");
                        }
                        else
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), "<script>$(document).ready(function(){$('#divDialog').append('Unexpected error occured.').dialog({title: 'Information...',show: 'slide',hide: 'blind',modal: true,buttons: {'Ok': function () {$(this).dialog('close');$(this).dialog('destroy');}}});});</script>");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorLogger.DBLog(ex, "HardwareConfiguration|btnConfigure_Click", ConfigManager.GetRBSQLDBOLAPConnection);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), "<script>$(document).ready(function(){$('#divDialog').append('Unexpected error occured.').dialog({title: 'Information...',show: 'slide',hide: 'blind',modal: true,buttons: {'Ok': function () {$(this).dialog('close');$(this).dialog('destroy');}}});});</script>");
                    return;
                }
            }
            else
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), "<script>$(document).ready(function(){$('#divDialog').append('Please enter Terminal ID').dialog({title: 'Information...',show: 'slide',hide: 'blind',modal: true,buttons: {'Ok': function () {$(this).dialog('close');$(this).dialog('destroy');}}});});</script>");
            }
        }
    }
}