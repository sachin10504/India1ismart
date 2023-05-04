using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text.RegularExpressions;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Reports.Utilities.Loggers;
using System.Configuration;
using ReflectionIT.Common.Data.Configuration;
using ReflectionIT.Common.Data.SqlClient;
using System.Collections;

namespace Reports
{
    public partial class FrmUploadBTISurprise : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            DivGrid.Visible = false;
            DivButton.Visible = false;
            DivMsg.Visible = false;
            string sUploadFileName = "";
            DataTable DTStoreData = new DataTable();
            // Boolean bHasErrorOccurred = false;;
            if (Convert.ToString(Session["Active"]) != "1") { Response.Redirect("/Login"); }

            //if (!Generix.utilityAccessAllowed("HOTC")) { Response.Redirect("/"); }
            string[] ColumnName = ConfigurationManager.AppSettings["SurpriseGiftColumnNames"].Split(',');
            if ((IsPostBack) && (BTIfileUpload.HasFile))
            {
                string extension = System.IO.Path.GetExtension(this.BTIfileUpload.PostedFile.FileName);
                if (extension == ".xls")
                {
                    string name = Path.GetFileName(BTIfileUpload.FileName);
                    sUploadFileName = System.IO.Path.GetRandomFileName();
                    BTIfileUpload.SaveAs(Server.MapPath("tempOutputs") + "\\" + sUploadFileName);
                    try
                    {

                        DTStoreData = Generix.getExcelWorksheetData(true, Server.MapPath("tempOutputs") + "\\" + sUploadFileName, "Sheet1");
                        //DTStoreData = DTStoreData.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(field => field is DBNull || string.IsNullOrEmpty(field as string))).CopyToDataTable();
                        DTStoreData = RemoveEmptyRows(DTStoreData);
                        if (DTStoreData.Rows.Count >= 0)
                        {

                            //DataColumnCollection columns = DTStoreData.Columns;
                            //for (int i = 0; i < ColumnName.Length; i++)
                            //{
                            //    if (!columns.Contains(ColumnName[i]))
                            //    {
                            //        ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), "<script>$(document).ready(function(){$('#divDialog').append(' Invalid File Format..').dialog({title: 'Note...',show: 'slide',hide: 'blind',modal: true,buttons: {'Ok': function () {$(this).dialog('close');$(this).dialog('destroy');}}});});</script>");
                            //        return;
                            //    }
                            //}
                            for (int i = 0; i < DTStoreData.Columns.Count; i++)
                            {
                                if (!ColumnName.Contains(DTStoreData.Columns[i].ToString()))
                                {
                                    ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), "<script>$(document).ready(function(){$('#divDialog').append('Invalid Columns.').dialog({title: 'Note...',show: 'slide',hide: 'blind',modal: true,buttons: {'Ok': function () {$(this).dialog('close');$(this).dialog('destroy');}}});});</script>");
                                    return;
                                }
                            }


                            //show it on gridview 
                            if (DTStoreData.Rows.Count >= 0)
                            {
                                DivGrid.Visible = true;
                                DivButton.Visible = true;

                                // Convert DataTable into HTML to show on Grid(DataTable)
                                Table TblSurpriseDetail = Generix.convertDataTable2HTMLTable(DTStoreData, true, true);
                                DivGridView.InnerHtml = GetHTMLTable2String(TblSurpriseDetail);

                                ViewState["FileName"] = null;
                                ViewState["FileName"] = name;

                                ViewState["DTSurpriseDetailViewState"] = null;
                                ViewState["DTSurpriseDetailViewState"] = DTStoreData;

                            }
                            else
                            {
                                ViewState["DTSurpriseDetailViewState"] = null;
                                ViewState["FileName"] = null;

                                //GridCardFees.DataSource = null;
                                DivGrid.Visible = false;
                                DivButton.Visible = false;
                                ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), "<script>$(document).ready(function(){  $('#divDialog').append('Blank file cannot be uploaded.').dialog({title: 'Note...',show: 'slide',hide: 'blind',modal: true,buttons: {'Ok': function () {$(this).dialog('close');$(this).dialog('destroy');}}});});</script>");
                            }
                        }
                        else
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), " ", "<script>$(document).ready(function(){$('#divDialog').html('Given File is Blank! ' ).dialog({title: 'Note...',show: 'slide',hide: 'blind',modal: true,buttons: {'Ok': function () {$(this).dialog('close');$(this).dialog('destroy');}}});});</script>");
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorLogger.TextLog(ex, "FrmUploadBTISurprise");
                        ErrorLogger.DBLog(ex, "FrmUploadBTISurprise ", ConfigManager.GetRBSQLDBOLAPConnection);
                        //string msg = "The 'Microsoft.ACE.OLEDB.12.0' provider is not registered on the local machine.";
                        //" + ex.Message.ToString().Replace("'", " ").Replace(".", " ").ToString() + "
                        //ClientScript.RegisterClientScriptBlock(this.GetType(), "Error", "<script>$(document).ready(function(){$('#divDialog').html('Error ').dialog({title: 'Note...',show: 'slide',hide: 'blind',modal: true,buttons: {'Ok': function () {$(this).dialog('close');$(this).dialog('destroy');}}});});</script>");
                        ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), "<script>$(document).ready(function(){$('#divDialog').append(' " + ex.Message.Replace("'", "\"") + " ').dialog({title: 'Note...',show: 'slide',hide: 'blind',modal: true,buttons: {'Ok': function () {$(this).dialog('close');$(this).dialog('destroy');}}});});</script>");
                    }
                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "", "<script>$(document).ready(function(){$('#divDialog').html('Only xls file format will accepted.').dialog({title: 'Note...',show: 'slide',hide: 'blind',modal: true,buttons: {'Ok': function () {$(this).dialog('close');$(this).dialog('destroy');}}});});</script>");
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
        protected void BtnSave_Click(object sender, EventArgs e)
        {

            DataTable DTAtMDetails = new DataTable();
            DataTable DTOutput = new DataTable();
            string FileName = "";

            if (ViewState["DTSurpriseDetailViewState"] != null)
            {
                DTAtMDetails = (DataTable)ViewState["DTSurpriseDetailViewState"];
            }
            if (ViewState["FileName"] != null)
            {
                FileName = Convert.ToString(ViewState["FileName"]);
            }


            //SqlConnection sqlcon = null;
            //SqlCommand sqlcmd = new SqlCommand();
            //Generix.getConnection(ref sqlcon, 3);

            //sqlcmd.Connection = sqlcon;
            //sqlcmd.CommandText = "[usp_UploadBTISurprise]";
            //sqlcmd.CommandType = CommandType.StoredProcedure;
            //sqlcmd.Parameters.AddWithValue("@UploadBTISurprise", dtData);
            //sqlcmd.Parameters.Add("@ATMID", SqlDbType.Int);
            //sqlcmd.Parameters.AddWithValue("@upload_filename", name);
            //sqlcmd.Parameters["@ATMID"].Direction = ParameterDirection.Output;
            //sqlcon.Open();
            //sqlcmd.ExecuteNonQuery();

            //retVal = sqlcmd.Parameters["@ATMID"].Value.ToString();
            //retupdate = sqlcmd.Parameters["@ATMID1"].Value.ToString();
            //sqlcon.Close();

            using (SqlStoredProcedure sspObj = new SqlStoredProcedure("dbo.usp_UploadBTISurprise", ConfigManager.GetRBSQLDBOLAPConnection, CommandType.StoredProcedure))
            {
                sspObj.AddParameterWithValue("@UploadBTISurprise", SqlDbType.Structured, 0, ParameterDirection.Input, DTAtMDetails);
                //sspObj.AddParameterWithValue("@ATMID", SqlDbType.Int, 0, ParameterDirection.Input, "I");
                sspObj.AddParameterWithValue("@upload_filename", SqlDbType.VarChar, 0, ParameterDirection.Input, FileName);
                sspObj.AddParameterWithValue("@LoginUser", SqlDbType.BigInt, 0, ParameterDirection.Input, Convert.ToInt32(Session["ActiveUserCode"]));
                try
                {
                    DTOutput = sspObj.ExecuteDataTable();
                }
                catch (Exception ex)
                {
                    ErrorLogger.TextLog(ex, "FrmUploadBTISurprise >>On Save of ATM Details");
                    ErrorLogger.DBLog(ex, "FrmUploadBTISurprise >>On Save of ATM Details", ConfigManager.GetRBSQLDBOLAPConnection);
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
                    Table TblSurpriseGift = Generix.convertDataTable2HTMLTable(DTOutput, true, true);
                    DivGridView.InnerHtml = GetHTMLTable2String(TblSurpriseGift);
                }
                else
                {
                    DivButton.Visible = false;
                    DivMsg.Visible = false;
                    //GridCardFees.DataSource = null;
                    DivGrid.Visible = false;
                    ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), "<script>$(document).ready(function(){$('#divDialog').append(' File has been saved. ').dialog({title: 'Note...',show: 'slide',hide: 'blind',modal: true,buttons: {'Ok': function () {$(this).dialog('close');$(this).dialog('destroy');}}});});</script>");
                }
            }
        }

        public string atmnumber { get; set; }

        public static DataTable RemoveEmptyRows(DataTable source)
        {
            try
            {

                for (int i = source.Rows.Count - 10; i >= 0; i += -1)
                {
                    DataRow row = source.Rows[i];
                    if (row[0] == null)
                    {
                        source.Rows.Remove(row);
                    }
                    else if (string.IsNullOrEmpty(row[0].ToString()))
                    {
                        source.Rows.Remove(row);
                    }
                }
                return source;
            }
            catch (Exception ex)
            {
                ErrorLogger.TextLog(ex, "FrmUploadBTISurprise");
                ErrorLogger.DBLog(ex, "FrmUploadBTISurprise ", ConfigManager.GetRBSQLDBOLAPConnection);
                DataTable DTNull = new DataTable();
                return DTNull;
            }
        }
    }
}
