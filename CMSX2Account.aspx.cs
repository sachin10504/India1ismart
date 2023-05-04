using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ReflectionIT.Common.Data.SqlClient;
using ReflectionIT.Common.Data.Configuration;
using System.IO;

namespace Reports
{
    public partial class CMSX2account : System.Web.UI.Page
    {
        
        DataTable dtUploadData = null;
        Int32 iTotalUploaded = 0;
        String sWaste = "";
        Int32 iLineNumber = 1;
        Boolean Fileformat;

        //        ClinkEncryption objEnc = new ClinkEncryption();
        protected void Page_Load(object sender, EventArgs e)
        {
            String sUploadFileName;
            //Boolean bHasErrorOccurred = false;
            string query = "";
            
            if (Convert.ToString(Session["Active"]) != "1")
            {
                Response.Redirect("/Login");
            }

            if (IsPostBack)
            {
                if (CMSX2AccountFileUpload.HasFile)
                {
                    sUploadFileName = System.IO.Path.GetRandomFileName();
                    CMSX2AccountFileUpload.SaveAs(Server.MapPath("tempOutputs") + "\\" + sUploadFileName);
                    dtUploadData = createUploadFileDataTable();
                    foreach (String sWaste in File.ReadAllLines(Server.MapPath("tempOutputs") + "\\" + sUploadFileName))
                    {
                        if (!System.Text.RegularExpressions.Regex.Matches(sWaste, @"\|").Count.ToString().Equals("14"))
                        {
                            Fileformat = false;
                            break;
                        }
                        else { Fileformat = true; }
                        
                        iLineNumber++;
                    }

                    ClientScript.RegisterClientScriptBlock(this.GetType(), "", "<script>$(document).ready(function(){$('#divDialog').html(' Invalid File Format Occurred At Line No:" + iLineNumber + "Correct File & Upload It again<br/>').dialog({title: 'Notice',show: 'slide',hide: 'blind',modal: true,buttons: {'Ok': function () {$(this).dialog('close');}}});});</script>");
                    if (Fileformat == true)
                    {
                        try
                        {
                            using (System.IO.StreamReader srUploadFileData = new System.IO.StreamReader(Server.MapPath("tempOutputs") + "\\" + sUploadFileName))
                            {

                                try
                                {
                                    while (srUploadFileData.Peek() >= 0)
                                    {
                                        sWaste = srUploadFileData.ReadLine();
                                        DataRow drUploadFileData = dtUploadData.NewRow();
                                        drUploadFileData.ItemArray = sWaste.Split('|');
                                        dtUploadData.Rows.Add(drUploadFileData);

                                    }
                                    iLineNumber++;
                                }
                                catch (Exception xObj)
                                {
                                    throw new Exception("Error in Line Number :" + iLineNumber.ToString());
                                }
                                srUploadFileData.Close();

                            }
                            File.Delete(Server.MapPath("tempOutputs") + "\\" + sUploadFileName);
                            iTotalUploaded = 0;
                            foreach (DataRow drUploadFileData in dtUploadData.Rows)
                            {
                                String sSQL = "";
                                sSQL += "'" + Convert.ToInt32(Session["Bank"]) + "'";
                                foreach (DataColumn dcUploadFileData in dtUploadData.Columns)
                                {
                                    sSQL += ((sSQL == "") ? "" : ",") + "'" + drUploadFileData[dcUploadFileData].ToString().Trim().Replace("'", "").Replace("\"", "") + "'";
                                }
                                //sSQL += ",'" + cardFileUpload.FileName + "'," + Convert.ToString(Session["Bank"]);
                                //sSQL += ",'" + cardFileUpload.FileName + "'," + Convert.ToString(Session["Bank"]);
                                sSQL += ",'" + Session["ActiveUserCode"].ToString() + "','" + CMSX2AccountFileUpload.FileName + "'";

                                sSQL = "Insert InTo dbo.CMSX2Account([Bank],[CIF ID],[AC ID],[AC Close date],[Customer Name],[Customer preferred Name],[Email id],[Country Dial code],[City Dial code],[Mobile phone number],[Entered date],[Verified Date],[PAN Number],[Mode Of Operation],[Scheme code],[Debit Card Linkage Flag],Login,[UploadFilename])  Values(" + sSQL + " )";

                                using (SqlStoredProcedure sspObj = new SqlStoredProcedure(sSQL, ConfigManager.GetRBSQLDBOLAPConnection, CommandType.Text))
                                {
                                    sspObj.ExecuteNonQuery();
                                    sspObj.Dispose();

                                    iTotalUploaded++;
                                }
                            }

                        }
                        catch
                        {

                        }
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "TotatalInsert", "<script>$(document).ready(function(){$('#divDialog').html('Total " + iTotalUploaded + " Transactions Inserted Successfully!<br/>').dialog({title: 'Notice',show: 'slide',hide: 'blind',modal: true,buttons: {'Ok': function () {$(this).dialog('close');}}});});</script>");
                    }
                    /*try
                    {
                        //dtData = Generix.getExcelWorksheetData(true, Server.MapPath("tempOutputs") + "\\" + sUploadFileName, "Sheet1");
                        
                        if (dtData.Rows.Count < 1) { bHasErrorOccurred = true; }
                        foreach (DataRow drData in dtData.Rows)
                        {
                            cardNo=Convert.ToString(drData["Card No"]);
                            accNo = Convert.ToString(drData["Account"]);
                            CProd = Convert.ToString(drData["Product"]);
                            try
                            {
                                if (cardNo != "" && accNo != "" && CProd != "")
                                {
                                    query = "insert into dbo.CMSX2(CardNo,AccountNo,CardProduct)  values('" + cardNo + "','" + accNo + "','" + CProd + "')";

                                    using (SqlStoredProcedure sspObj = new SqlStoredProcedure(query, ConfigManager.GetRBSQLDBOLAPConnection, CommandType.Text))
                                    {
                                        sspObj.ExecuteNonQuery();
                                        count++;
                                        cardNo = "";
                                        accNo = "";
                                        CProd = "";

                                    }

                                }
                                else
                                {
                                    bHasErrorOccurred = true;

                                    throw new Exception("Couldn't read Data!");

                                }
                            }
                            catch {
                                bHasErrorOccurred = true;

                                ClientScript.RegisterClientScriptBlock(this.GetType(), "formatError", "<script>$(document).ready(function(){$('#divDialog').html('Please Check Your File Format!<br/>').dialog({modal: true});});</script>");
                            }
                        }
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "TotatalInsert", "<script>$(document).ready(function(){$('#divDialog').html('Total " + count + " Transactions Inserted Successfully!<br/>').dialog({title: 'Notice',show: 'slide',hide: 'blind',modal: true,buttons: {'Ok': function () {$(this).dialog('close');}}});});</script>");


                    }
                    catch (Exception ex)
                    {
                        //bHasErrorOccurred = true;
                        divOutputWindow.InnerText = ex.Message;
                        // ClientScript.RegisterClientScriptBlock(this.GetType(), "TotatalInsert", "<script>$(document).ready(function(){$('#divDialog').html('Please Change Your Sheet Name to Sheet1<br/>').dialog({title: 'Error',show: 'slide',hide: 'blind',modal: true,buttons: {'Ok': function () {$(this).dialog('close');}}});});</script>");
                    }*/
                    //Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "CMSX2Account Processed ", "CMSX2Account", "");
                    

                }
                else
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "", "<script>$(document).ready(function(){$('#divDialog').html('File Not Selected <br/>').dialog({title: 'Notice',show: 'slide',hide: 'blind',modal: true,buttons: {'Ok': function () {$(this).dialog('close');}}});});</script>");
                }


            }


        }

        private DataTable createUploadFileDataTable()
        {
            DataTable dtReturn = new DataTable();

            dtReturn.Columns.Add("CIF ID");
            dtReturn.Columns.Add("AC ID");
            dtReturn.Columns.Add("AC Close date");
            dtReturn.Columns.Add("Customer Name");
            dtReturn.Columns.Add("Customer preferred Name");
            dtReturn.Columns.Add("Email id");
            dtReturn.Columns.Add("Country Dial code");
            dtReturn.Columns.Add("City Dial code");
            dtReturn.Columns.Add("Mobile phone number");
            dtReturn.Columns.Add("Entered date");
            dtReturn.Columns.Add("Verified Date");
            dtReturn.Columns.Add("PAN Number");
            dtReturn.Columns.Add("Mode Of Operation");
            dtReturn.Columns.Add("Scheme code");
            dtReturn.Columns.Add("Flag for Account Closure/Account card delinking");
            return dtReturn;
        }
    }
}