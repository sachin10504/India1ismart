using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using ReflectionIT.Common.Data.Configuration;
using ReflectionIT.Common.Data.SqlClient;
using System.Text;
using System.Web.Script.Serialization;
using System.Configuration;
using Reports.Utilities.Loggers;

namespace Reports
{
    public partial class frmUploadthreshold : System.Web.UI.Page
    {
        public static string StrUploadFileName = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            DivGrid.Visible = false;
            DivButton.Visible = false;
            DivMsg.Visible = false;
            
            DataTable DTThreshold = new DataTable();

            //string[] ColumnName = { "ATMID", "CumulativeBalance", "DispenseAmount", "Mode" };
            string[] ColumnName = ConfigurationManager.AppSettings["ThresholdColumnNames"].Split(',');
                       
            
            if ((IsPostBack) && (FileUploadThreshold.HasFile))
            {
                StrUploadFileName = "threshold_" + DateTime.Now.ToString("ddMMyyyy_hhmmssffff") + ".txt";
                FileUploadThreshold.SaveAs(Server.MapPath("tempOutputs") + "\\" + StrUploadFileName);
                try
                {
                    using (System.IO.TextReader _TextReader = File.OpenText(Server.MapPath("tempOutputs") + "\\" + StrUploadFileName))
                    {
                        string StrLine;
                        while ((StrLine = _TextReader.ReadLine()) != null)
                        {
                            string[] items = StrLine.Trim().Split('|');
                            if (DTThreshold.Columns.Count == 0)
                            {
                                // Create the data columns for the data table based on the number of items
                                // on the first line of the file
                                for (int i = 0; i < items.Length; i++)
                                {
                                    //Check Duplicate Column
                                    if (DTThreshold.Columns.Contains(items[i]) == false)
                                    {
                                        DTThreshold.Columns.Add(new DataColumn(items[i], typeof(string)));
                                        //Check All Column
                                        if (!ColumnName.Contains(items[i].Trim()))
                                        {
                                            ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), "<script>$(document).ready(function(){$('#divDialog').append(' Invalid File Format..').dialog({title: 'Note...',show: 'slide',hide: 'blind',modal: true,buttons: {'Ok': function () {$(this).dialog('close');$(this).dialog('destroy');}}});});</script>");
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), "<script>$(document).ready(function(){$('#divDialog').append('Invalid Column Name..').dialog({title: 'Note...',show: 'slide',hide: 'blind',modal: true,buttons: {'Ok': function () {$(this).dialog('close');$(this).dialog('destroy');}}});});</script>");
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                try
                                {
                                    DTThreshold.Rows.Add(items);
                                }
                                catch (Exception ex)
                                {
                                    ErrorLogger.TextLog(ex, "frmUploadthreshold >>Rows Add on Datatable");
                                    ErrorLogger.DBLog(ex, "frmUploadthreshold >>Rows Add on Datatable", ConfigManager.GetRBSQLDBOLAPConnection);

                                    ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), "<script>$(document).ready(function(){$('#divDialog').append(' " + ex.Message.ToString() + " ').dialog({title: 'Note...',show: 'slide',hide: 'blind',modal: true,buttons: {'Ok': function () {$(this).dialog('close');$(this).dialog('destroy');}}});});</script>");
                                }
                            }
                        }

                        //show it on gridview 
                        if (DTThreshold.Rows.Count > 0)
                        {
                            DivGrid.Visible = true;
                            DivButton.Visible = true;

                            // Convert DataTable into HTML to show on Grid(DataTable)
                            Table TblThresholdDetails = Generix.convertDataTable2HTMLTable(DTThreshold, true, true);
                            DivGridView.InnerHtml = GetHTMLTable2String(TblThresholdDetails);

                            ViewState["DTThresholdViewState"] = null;
                            ViewState["DTThresholdViewState"] = DTThreshold;
                            ViewState["FileName"] = StrUploadFileName;
                        }
                        else
                        {
                            ViewState["DTThresholdViewState"] = null;
                            ViewState["FileName"] = null;
                            //GridCardFees.DataSource = null;
                            DivGrid.Visible = false;
                            DivButton.Visible = false;
                            ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), "<script>$(document).ready(function(){  $('#divDialog').append('Blank file cannot be uploaded.').dialog({title: 'Note...',show: 'slide',hide: 'blind',modal: true,buttons: {'Ok': function () {$(this).dialog('close');$(this).dialog('destroy');}}});});</script>");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorLogger.TextLog(ex, "frmUploadthreshold >>On Page Load");
                    ErrorLogger.DBLog(ex, "frmUploadthreshold >>On Page Load", ConfigManager.GetRBSQLDBOLAPConnection);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), "<script>$(document).ready(function(){$('#divDialog').append(' " + ex.Message.ToString() + " ').dialog({title: 'Note...',show: 'slide',hide: 'blind',modal: true,buttons: {'Ok': function () {$(this).dialog('close');$(this).dialog('destroy');}}});});</script>");
                }
            }
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {

            DataTable DTThresholddata = new DataTable();
            DataTable DTOutput = new DataTable();
            string filenm = string.Empty;

            if (ViewState["DTThresholdViewState"] != null)
            {
                DTThresholddata = (DataTable)ViewState["DTThresholdViewState"];
            }

            if (ViewState["FileName"] != null)
            {
                filenm = (string)ViewState["FileName"];
            }
            

            using (SqlStoredProcedure sspObj = new SqlStoredProcedure("dbo.USP_AddThresholdDetails", ConfigManager.GetRBSQLDBOLAPConnection, CommandType.StoredProcedure))
            {

                sspObj.AddParameterWithValue("@filenm", SqlDbType.VarChar, 0, ParameterDirection.Input, filenm);
                sspObj.AddParameterWithValue("@Type_ThresholdMaster", SqlDbType.Structured, 0, ParameterDirection.Input, DTThresholddata);
                sspObj.AddParameterWithValue("@LoginUser", SqlDbType.BigInt, 0, ParameterDirection.Input, Convert.ToInt32(Session["ActiveUserCode"]));
                try
                {
                    DTOutput = sspObj.ExecuteDataTable();
                }
                catch (SqlException ex)
                {
                    ErrorLogger.TextLog(ex, "frmUploadthreshold >>On Save of Card Fee Details");
                    ErrorLogger.DBLog(ex, "frmUploadthreshold >>On Save of Card Fee Details", ConfigManager.GetRBSQLDBOLAPConnection);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), "<script>$(document).ready(function(){$('#divDialog').append('Unexpected error occured...').dialog({title: 'Note...',show: 'slide',hide: 'blind',modal: true,buttons: {'Ok': function () {$(this).dialog('close');$(this).dialog('destroy');}}});});</script>");
                    return;
                }


                if (DTOutput.Rows.Count > 0)
                {
                    DivGrid.Visible = true;
                    DivButton.Visible = false;
                    DivMsg.Visible = true;
                    DivMsg.InnerText = "Dispense Amount is not allowed greater the 10,000,Please update and re-upload the file.";

                    // Convert DataTable into HTML to show on Grid(DataTable)
                    Table TblThresholdDetails = Generix.convertDataTable2HTMLTable(DTOutput, true, true);
                    DivGridView.InnerHtml = GetHTMLTable2String(TblThresholdDetails);
                    
                }
                else
                {
                    DivButton.Visible = false;
                    DivMsg.Visible = false;                    
                    DivGrid.Visible = false;
                    ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), "<script>$(document).ready(function(){$('#divDialog').append(' File has been saved. ').dialog({title: 'Note...',show: 'slide',hide: 'blind',modal: true,buttons: {'Ok': function () {$(this).dialog('close');$(this).dialog('destroy');}}});});</script>");
                }
                //ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), "<script>$(document).ready(function(){$('#divDialog').append('<Div style=\"height: 300px;\"  > " + HTMLContent.ToString() + " </Div >').dialog({title: 'Note...',show: 'slide',hide: 'blind',modal: true,buttons: {'Ok': function () {$(this).dialog('close');$(this).dialog('destroy');}}});});</script>");
            }
        }

        //protected void BtnViewThreshold_Click(object sender, EventArgs e)
        //{
        //    BindGridView();
        //}
       
        
        private void BindGridView()
        {
            DataTable DTThresholdDetails = new DataTable();
            //DataTable DTBlank = new DataTable();
            //DTBlank.Columns.Add("ATMID", typeof(string));
            //DTBlank.Columns.Add("CumulativeBalance", typeof(string));
            //DTBlank.Columns.Add("DispenseAmount", typeof(string));
            //DTBlank.Columns.Add("Mode", typeof(string));

            DataTable DTdata = new DataTable();
            
            if (ViewState["DTThresholdViewState"] != null)
            {
                DTdata = (DataTable)ViewState["DTThresholdViewState"];
            }


            using (SqlStoredProcedure sspObj = new SqlStoredProcedure("dbo.[USP_AddThresholdDetails]", ConfigManager.GetRBSQLDBOLAPConnection, CommandType.StoredProcedure))
            {                
                sspObj.AddParameterWithValue("@filenm", SqlDbType.VarChar, 0, ParameterDirection.Input, StrUploadFileName);
                sspObj.AddParameterWithValue("@Type_ThresholdMaster", SqlDbType.Structured, 0, ParameterDirection.Input, DTdata);
                sspObj.AddParameterWithValue("@LoginUser", SqlDbType.BigInt, 0, ParameterDirection.Input, Convert.ToInt32(Session["ActiveUserCode"]));
                try
                {
                    DTThresholdDetails = sspObj.ExecuteDataTable();
                }
                catch (Exception ex)
                {
                    ErrorLogger.TextLog(ex, "frmUploadthreshold >>On Save Button of Card Fee Details");
                    ErrorLogger.DBLog(ex, "frmUploadthreshold >>On Save Button Card Fee Details", ConfigManager.GetRBSQLDBOLAPConnection);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), "<script>$(document).ready(function(){$('#divDialog').append('Unexpected error occured...').dialog({title: 'Note...',show: 'slide',hide: 'blind',modal: true,buttons: {'Ok': function () {$(this).dialog('close');$(this).dialog('destroy');}}});});</script>");
                    return;
                }
                //System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(System.Web.UI.Page), "Script", "FunBindGrid();", true); 



                if (DTThresholdDetails.Rows.Count > 0)
                {
                    DivGrid.Visible = true;
                    DivButton.Visible = false;
                    DivMsg.Visible = false;

                    // Convert DataTable into HTML to show on Grid(DataTable)
                    Table TblCardFee = Generix.convertDataTable2HTMLTable(DTThresholdDetails, true, true);
                    DivGridView.InnerHtml = GetHTMLTable2String(TblCardFee);

                }
                else
                {
                    DivButton.Visible = false;
                    DivMsg.Visible = true;
                    DivMsg.InnerText = "No Configuration Found.";                    
                    DivGrid.Visible = false;
                }

            }
        }

        private static string GetHTMLTable2String(Table HTMLTable2String)
        {
            //HTMLTable2String.Attributes.Add("style", "border:1px thin #ccc; padding: 0px 12px; ");
            StringBuilder ObjStringBuilder = new StringBuilder();
            StringWriter ObjStringWriter = new StringWriter(ObjStringBuilder);
            HtmlTextWriter ObjHtmlTextWriter = new HtmlTextWriter(ObjStringWriter);
            HTMLTable2String.RenderControl(ObjHtmlTextWriter);
            String HTMLContent = ObjStringBuilder.ToString();
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@">\s*<");
            HTMLContent = regex.Replace(HTMLContent, "><");
            //DivGridView.InnerHtml = HTMLContent;
            return HTMLContent;
        }
    }
}