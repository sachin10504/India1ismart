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
    public partial class frmUploadCardFees : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            DivGrid.Visible = false;
            DivButton.Visible = false;
            DivMsg.Visible = false;
            string StrUploadFileName = "";
            DataTable DTCardFee = new DataTable();

            //string[] ColumnName = { "BIN", "SchemeCode", "FeeAmount" };
            //string[] ColumnName = { "BIN", "SchemeCode", "FeeAmount" };
            string[] ColumnName = ConfigurationManager.AppSettings["CardFeeColumnNames"].Split(',');

            if ((IsPostBack) && (FileUploadCardFee.HasFile))
            {
                StrUploadFileName = "CardFees_" + DateTime.Now.ToString("ddMMyyyy_hhmmssffff") + ".txt";
                FileUploadCardFee.SaveAs(Server.MapPath("tempOutputs") + "\\" + StrUploadFileName);
                try
                {
                    using (System.IO.TextReader _TextReader = File.OpenText(Server.MapPath("tempOutputs") + "\\" + StrUploadFileName))
                    {
                        string StrLine;
                        while ((StrLine = _TextReader.ReadLine()) != null)
                        {
                            string[] items = StrLine.Trim().Split('|');
                            if (DTCardFee.Columns.Count == 0)
                            {
                                // Create the data columns for the data table based on the number of items
                                // on the first line of the file
                                for (int i = 0; i < items.Length; i++)
                                {
                                    //Check Duplicate Column
                                    if (DTCardFee.Columns.Contains(items[i]) == false)
                                    {
                                        DTCardFee.Columns.Add(new DataColumn(items[i], typeof(string)));
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
                                    DTCardFee.Rows.Add(items);
                                }
                                catch (Exception ex)
                                {
                                    ErrorLogger.TextLog(ex, "frmUploadCardFees >>Rows Add on Datatable");
                                    ErrorLogger.DBLog(ex, "frmUploadCardFees >>Rows Add on Datatable", ConfigManager.GetRBSQLDBOLAPConnection);

                                    ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), "<script>$(document).ready(function(){$('#divDialog').append(' " + ex.Message.ToString() + " ').dialog({title: 'Note...',show: 'slide',hide: 'blind',modal: true,buttons: {'Ok': function () {$(this).dialog('close');$(this).dialog('destroy');}}});});</script>");
                                }
                            }
                        }

                        //show it on gridview 
                        if (DTCardFee.Rows.Count > 0)
                        {
                            DivGrid.Visible = true;
                            DivButton.Visible = true;

                            // Convert DataTable into HTML to show on Grid(DataTable)
                            Table TblCardFee = Generix.convertDataTable2HTMLTable(DTCardFee, true, true);
                            DivGridView.InnerHtml = GetHTMLTable2String(TblCardFee);

                            ViewState["DTCardFeeDViewState"] = null;
                            ViewState["DTCardFeeDViewState"] = DTCardFee;

                            //GridCardFees.DataSource = null;
                            //GridCardFees.DataBind();
                            //GridCardFees.DataSource = DTCardFee;
                            //GridCardFees.DataBind();
                        }
                        else
                        {
                            ViewState["DTCardFeeDViewState"] = null;
                            //GridCardFees.DataSource = null;
                            DivGrid.Visible = false;
                            DivButton.Visible = false;
                            ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), "<script>$(document).ready(function(){  $('#divDialog').append('Blank file cannot be uploaded.').dialog({title: 'Note...',show: 'slide',hide: 'blind',modal: true,buttons: {'Ok': function () {$(this).dialog('close');$(this).dialog('destroy');}}});});</script>");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorLogger.TextLog(ex, "frmUploadCardFees >>On Page Load");
                    ErrorLogger.DBLog(ex, "frmUploadCardFees >>On Page Load", ConfigManager.GetRBSQLDBOLAPConnection);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), "<script>$(document).ready(function(){$('#divDialog').append(' " + ex.Message.ToString() + " ').dialog({title: 'Note...',show: 'slide',hide: 'blind',modal: true,buttons: {'Ok': function () {$(this).dialog('close');$(this).dialog('destroy');}}});});</script>");
                }
            }
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {

            DataTable DTCardFee = new DataTable();
            DataTable DTOutput = new DataTable();

            //// add the columns to the datatable            
            //if (GridCardFees.HeaderRow != null)
            //{

            //    for (int i = 0; i < GridCardFees.HeaderRow.Cells.Count; i++)
            //    {
            //        DTCardFee.Columns.Add(GridCardFees.HeaderRow.Cells[i].Text);
            //    }
            //}

            ////  add each of the data rows to the table
            //foreach (GridViewRow row in GridCardFees.Rows)
            //{
            //    DataRow dr;
            //    dr = DTCardFee.NewRow();
            //    for (int i = 0; i < row.Cells.Count; i++)
            //    {
            //        dr[i] = row.Cells[i].Text.Replace(" ", "");
            //    }
            //    DTCardFee.Rows.Add(dr);
            //}

            if (ViewState["DTCardFeeDViewState"] != null)
            {
                DTCardFee = (DataTable)ViewState["DTCardFeeDViewState"];
            }

            using (SqlStoredProcedure sspObj = new SqlStoredProcedure("dbo.USP_CardFee", ConfigManager.GetRBSQLDBOLAPConnection, CommandType.StoredProcedure))
            {

                sspObj.AddParameterWithValue("@InstituteId", SqlDbType.VarChar, 0, ParameterDirection.Input, 1);
                sspObj.AddParameterWithValue("@Mode", SqlDbType.VarChar, 0, ParameterDirection.Input, "I");
                sspObj.AddParameterWithValue("@Type_CardFeeMaster", SqlDbType.Structured, 0, ParameterDirection.Input, DTCardFee);
                sspObj.AddParameterWithValue("@LoginUser", SqlDbType.BigInt, 0, ParameterDirection.Input, Convert.ToInt32(Session["ActiveUserCode"]));
                try
                {
                    DTOutput = sspObj.ExecuteDataTable();
                }
                catch (SqlException ex)
                {
                    ErrorLogger.TextLog(ex, "frmUploadCardFees >>On Save of Card Fee Details");
                    ErrorLogger.DBLog(ex, "frmUploadCardFees >>On Save of Card Fee Details", ConfigManager.GetRBSQLDBOLAPConnection);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), "<script>$(document).ready(function(){$('#divDialog').append('Unexpected error occured...').dialog({title: 'Note...',show: 'slide',hide: 'blind',modal: true,buttons: {'Ok': function () {$(this).dialog('close');$(this).dialog('destroy');}}});});</script>");
                    return;
                }


                if (DTOutput.Rows.Count > 0)
                {
                    DivGrid.Visible = true;
                    DivButton.Visible = false;
                    DivMsg.Visible = true;
                    DivMsg.InnerText = "Below Details are Invalid,Please update and re-upload the file.";

                    // Convert DataTable into HTML to show on Grid(DataTable)
                    Table TblCardFee = Generix.convertDataTable2HTMLTable(DTOutput, true, true);
                    DivGridView.InnerHtml = GetHTMLTable2String(TblCardFee);
                    //GridCardFees.DataSource = null;
                    //GridCardFees.DataBind();
                    //GridCardFees.DataSource = DTSchemeCodeDetails;
                    //GridCardFees.DataBind();
                }
                else
                {
                    DivButton.Visible = false;
                    DivMsg.Visible = false;
                    //GridCardFees.DataSource = null;
                    DivGrid.Visible = false;
                    ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), "<script>$(document).ready(function(){$('#divDialog').append(' File has been saved. ').dialog({title: 'Note...',show: 'slide',hide: 'blind',modal: true,buttons: {'Ok': function () {$(this).dialog('close');$(this).dialog('destroy');}}});});</script>");
                }
                //ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), "<script>$(document).ready(function(){$('#divDialog').append('<Div style=\"height: 300px;\"  > " + HTMLContent.ToString() + " </Div >').dialog({title: 'Note...',show: 'slide',hide: 'blind',modal: true,buttons: {'Ok': function () {$(this).dialog('close');$(this).dialog('destroy');}}});});</script>");
            }
        }

        protected void BtnViewBinScheme_Click(object sender, EventArgs e)
        {
            BindGridView();
        }
        private void BindGridView()
        {
            DataTable DTSchemeCodeDetails = new DataTable();
            DataTable DTBlank = new DataTable();
            DTBlank.Columns.Add("Bin", typeof(string));
            DTBlank.Columns.Add("SchemeCode", typeof(string));
            DTBlank.Columns.Add("FeeAmount", typeof(string));

            using (SqlStoredProcedure sspObj = new SqlStoredProcedure("dbo.USP_CardFee", ConfigManager.GetRBSQLDBOLAPConnection, CommandType.StoredProcedure))
            {
                sspObj.AddParameterWithValue("@InstituteId", SqlDbType.VarChar, 0, ParameterDirection.Input, 1);
                sspObj.AddParameterWithValue("@Mode", SqlDbType.VarChar, 0, ParameterDirection.Input, "S");
                sspObj.AddParameterWithValue("@Type_CardFeeMaster", SqlDbType.Structured, 0, ParameterDirection.Input, DTBlank);
                sspObj.AddParameterWithValue("@LoginUser", SqlDbType.BigInt, 0, ParameterDirection.Input, Convert.ToInt32(Session["ActiveUserCode"]));
                try
                {
                    DTSchemeCodeDetails = sspObj.ExecuteDataTable();
                }
                catch (Exception ex)
                {
                    ErrorLogger.TextLog(ex, "frmUploadCardFees >>On Save Button of Card Fee Details");
                    ErrorLogger.DBLog(ex, "frmUploadCardFees >>On Save Button Card Fee Details", ConfigManager.GetRBSQLDBOLAPConnection);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), "<script>$(document).ready(function(){$('#divDialog').append('Unexpected error occured...').dialog({title: 'Note...',show: 'slide',hide: 'blind',modal: true,buttons: {'Ok': function () {$(this).dialog('close');$(this).dialog('destroy');}}});});</script>");
                    return;
                }
                //System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, typeof(System.Web.UI.Page), "Script", "FunBindGrid();", true); 



                if (DTSchemeCodeDetails.Rows.Count > 0)
                {
                    DivGrid.Visible = true;
                    DivButton.Visible = false;
                    DivMsg.Visible = false;

                    // Convert DataTable into HTML to show on Grid(DataTable)
                    Table TblCardFee = Generix.convertDataTable2HTMLTable(DTSchemeCodeDetails, true, true);
                    DivGridView.InnerHtml = GetHTMLTable2String(TblCardFee);

                    //GridCardFees.DataSource = null;
                    //GridCardFees.DataBind();
                    //GridCardFees.DataSource = DTSchemeCodeDetails;
                    //GridCardFees.DataBind();
                }
                else
                {
                    DivButton.Visible = false;
                    DivMsg.Visible = true;
                    DivMsg.InnerText = "No Configuration Found.";
                    //GridCardFees.DataSource = null;
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