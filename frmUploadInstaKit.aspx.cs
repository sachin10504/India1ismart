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
using System.Configuration;
using Reports.Utilities.Loggers;


namespace Reports
{
    public partial class frmUploadInstaKit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DivGrid.Visible = false;
            DivButton.Visible = false;
            DivMsg.Visible = false;
            string StrUploadFileName = "";
            DataTable DTCardFeeInstaBin = new DataTable();


            if ((IsPostBack) && (FileUploaderInstaKit.HasFile))
            {
                StrUploadFileName = "InstaKit_" + Convert.ToInt32(Session["ActiveUserCode"]) + "_" + DateTime.Now.ToString("ddMMyyyy_hhmmssffff") + ".txt";
                lblPath.Text = Server.MapPath("tempOutputs") + "\\" + StrUploadFileName;
                FileUploaderInstaKit.SaveAs(Server.MapPath("tempOutputs") + "\\" + StrUploadFileName);
                try
                {
                    //Get Column Names from Web Confige.
                    string[] ArrInstaColumnName = ConfigurationManager.AppSettings["CardFeeColumnNames"].Split(',');

                    using (System.IO.TextReader _TextReader = File.OpenText(Server.MapPath("tempOutputs") + "\\" + StrUploadFileName))
                    {
                        string StrLine;
                        while ((StrLine = _TextReader.ReadLine()) != null)
                        {

                            string[] items = StrLine.Trim().Split('|');
                            if (DTCardFeeInstaBin.Columns.Count == 0)
                            {
                                // Create the data columns for the data table based on the number of items
                                // on the first line of the file
                                for (int i = 0; i < items.Length; i++)
                                {
                                    //Check Duplicate Column
                                    if (DTCardFeeInstaBin.Columns.Contains(items[i]) == false)
                                    {
                                        DTCardFeeInstaBin.Columns.Add(new DataColumn(items[i], typeof(string)));
                                        //Check All Columns
                                        if (!ArrInstaColumnName.Contains(items[i].Trim()))
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
                                    DTCardFeeInstaBin.Rows.Add(items);
                                    System.Type type = DTCardFeeInstaBin.Columns[0].GetType();
                                }
                                catch (Exception ex)
                                {
                                    ErrorLogger.TextLog(ex, "frmUploadInstaKit >>Rows Add on Datatable");
                                    ErrorLogger.DBLog(ex, "frmUploadInstaKit >>Rows Add on Datatable", ConfigManager.GetRBSQLDBOLAPConnection);
                                    ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), "<script>$(document).ready(function(){  $('#divDialog').append(' " + ex.Message.Replace("'", "\"") + " ').dialog({title: 'Note...',show: 'slide',hide: 'blind',modal: true,buttons: {'Ok': function () {$(this).dialog('close');$(this).dialog('destroy');}}});});</script>");
                                }
                            }


                        }

                        //Show it On gridview 
                        if (DTCardFeeInstaBin.Rows.Count > 0)
                        {
                            DivGrid.Visible = true;
                            DivButton.Visible = true;

                            // Convert DataTable into HTML to show on Grid(DataTable)
                            Table TblCardFee = Generix.convertDataTable2HTMLTable(DTCardFeeInstaBin, true, true);
                            DivGridView.InnerHtml = GetHTMLTable2String(TblCardFee);

                            ViewState["DTInstaKitViewState"] = null;
                            ViewState["DTInstaKitViewState"] = DTCardFeeInstaBin;
                            //GridCardFees.DataSource = null;
                            //GridCardFees.DataBind();
                            //GridCardFees.DataSource = DTCardFeeInstaBin;
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
                    ErrorLogger.TextLog(ex, "frmUploadInstaKit >>On Page Load ");
                    ErrorLogger.DBLog(ex, "frmUploadInstaKit >>On Page Load", ConfigManager.GetRBSQLDBOLAPConnection);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), "<script>$(document).ready(function(){  $('#divDialog').append(' " + ex.Message.Replace("'", "\"") + " ').dialog({title: 'Note...',show: 'slide',hide: 'blind',modal: true,buttons: {'Ok': function () {$(this).dialog('close');$(this).dialog('destroy');}}});});</script>");
                }
            }

        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {

            DataTable DTInstaBinDetails = new DataTable();
            DataTable DTOutput = new DataTable();

            // add the columns to the datatable            
            //if (GridCardFees.HeaderRow != null)
            //{

            //    for (int i = 0; i < GridCardFees.HeaderRow.Cells.Count; i++)
            //    {
            //        DTInstaBinDetails.Columns.Add(GridCardFees.HeaderRow.Cells[i].Text);
            //    }
            //}

            //  add each of the data rows to the table
            //foreach (GridViewRow row in GridCardFees.Rows)
            //{
            //    DataRow dr;
            //    dr = DTInstaBinDetails.NewRow();
            //    for (int i = 0; i < row.Cells.Count; i++)
            //    {
            //        dr[i] = row.Cells[i].Text.Replace(" ", "");
            //    }
            //    DTInstaBinDetails.Rows.Add(dr);
            //}

            if (ViewState["DTCardFeeDViewState"] != null)
            {
                DTInstaBinDetails = (DataTable)ViewState["DTCardFeeDViewState"];
            }

            using (SqlStoredProcedure sspObj = new SqlStoredProcedure("dbo.USP_uploadInstaKit", ConfigManager.GetRBSQLDBOLAPConnection, CommandType.StoredProcedure))
            {

                sspObj.AddParameterWithValue("@InstituteId", SqlDbType.VarChar, 0, ParameterDirection.Input, 1);
                sspObj.AddParameterWithValue("@Mode", SqlDbType.VarChar, 0, ParameterDirection.Input, "I");
                sspObj.AddParameterWithValue("@Type_CardFeeInstaBinDetails", SqlDbType.Structured, 0, ParameterDirection.Input, DTInstaBinDetails);
                sspObj.AddParameterWithValue("@LoginUser", SqlDbType.BigInt, 0, ParameterDirection.Input, Convert.ToInt32(Session["ActiveUserCode"]));
                sspObj.AddParameterWithValue("@FilePath", SqlDbType.VarChar, 0, ParameterDirection.Input, lblPath.Text);
                try
                {
                    DTOutput = sspObj.ExecuteDataTable();
                }
                catch (Exception ex)
                {
                    ErrorLogger.TextLog(ex, "frmUploadInstaKit >>On Save of Insta Bin File ");
                    ErrorLogger.DBLog(ex, "frmUploadInstaKit >>On Save of Insta Bin File ", ConfigManager.GetRBSQLDBOLAPConnection);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), "<script>$(document).ready(function(){$('#divDialog').append('Unexpected error occured...').dialog({title: 'Note...',show: 'slide',hide: 'blind',modal: true,buttons: {'Ok': function () {$(this).dialog('close');$(this).dialog('destroy');}}});});</script>");
                }

                //show it on gridview 
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
                    ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), "<script>$(document).ready(function(){$('#divDialog').append(' Given File is successfully uploaded. ').dialog({title: 'Note...',show: 'slide',hide: 'blind',modal: true,buttons: {'Ok': function () {$(this).dialog('close');$(this).dialog('destroy');}}});});</script>");
                }
                //ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), "<script>$(document).ready(function(){$('#divDialog').append('<Div style=\"height: 300px;\"  > " + HTMLContent.ToString() + " </Div >').dialog({title: 'Note...',show: 'slide',hide: 'blind',modal: true,buttons: {'Ok': function () {$(this).dialog('close');$(this).dialog('destroy');}}});});</script>");
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