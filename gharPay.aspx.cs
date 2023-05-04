using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using ReflectionIT.Common.Data.Configuration;
using ReflectionIT.Common.Data.SqlClient;
using System.Data;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using System.Text;


namespace Reports
{
    public partial class gharPay : System.Web.UI.Page
    {
        DataTable dtOutput;
        DataTable dtData;
        string custId = "", cardNo = "", accNo = "", smsText = "", email_Text_Url = "", rrn = "", trace="";
        string cashBackAmt = "0", bankComm = "0", clinkComm = "0";
        
        Int32 count = 0;
        Int32 dailyRespData = 0;

        ClinkEncryption objEnc = new ClinkEncryption();

        protected void Page_Load(object sender, EventArgs e)
        {
            String sUploadFileName;
            Boolean bHasErrorOccurred = false;
            string query = "";

            if (Convert.ToString(Session["Active"]) != "1") { Response.Redirect("/Login"); }

            if (!Generix.utilityAccessAllowed("GPAY")) { Response.Redirect("/"); }

            if (IsPostBack)
            {
                if (gpFileUpload.HasFile)
                {
                    sUploadFileName = System.IO.Path.GetRandomFileName();
                    gpFileUpload.SaveAs(Server.MapPath("tempOutputs") + "\\" + sUploadFileName);

                    try
                    {

                        dtData = Generix.getExcelWorksheetData(true, Server.MapPath("tempOutputs") + "\\" + sUploadFileName, "Sheet1");
                        if (dtData.Rows.Count < 1) { bHasErrorOccurred = true; }

                        foreach (DataRow drData in dtData.Rows)
                        {
                            try
                            {
                                custId = objEnc.GetDecryptedPlainText(Convert.ToString(drData["customer_id"]));
                            }
                            catch (Exception custIdEx)
                            {
                                custId = Convert.ToString(drData["customer_id"]);
                            }

                            try
                            {
                                cardNo = objEnc.GetDecryptedPlainText(Convert.ToString(drData["card_number_hash"]));
                            }
                            catch (Exception cardNoEx)
                            {
                                cardNo = Convert.ToString(drData["card_number_hash"]);
                            }

                            try
                            {
                                accNo = objEnc.GetDecryptedPlainText(Convert.ToString(drData["account_number_hash"]));
                            }
                            catch (Exception accNoEx)
                            {
                                accNo = Convert.ToString(drData["account_number_hash"]);
                            }

                            try
                            {
                                if ((Convert.ToString(drData["sms_text"]) != null))
                                {
                                    smsText = Convert.ToString(drData["sms_text"]);
                                }
                            }
                            catch (Exception smsEx)
                            {
                                email_Text_Url = Convert.ToString(drData["email_text_url"]);
                            }
                            try
                            {
                                if (Convert.ToString(drData["email_text_url"]) != "")
                                {
                                    email_Text_Url = Convert.ToString(drData["email_text_url"]);
                                }
                            }
                            catch (Exception emailEx)
                            {
                                smsText = Convert.ToString(drData["sms_text"]);
                            }

                            try
                            {
                                if (Convert.ToString(drData["datetime_of_linking_offer"]) != "")
                                {
                                    dailyRespData = 1;
                                }
                            }
                            catch (Exception)
                            {
                                try
                                {
                                    if (Convert.ToString(drData["linked_campaigns"]) != "")
                                    {
                                        dailyRespData = 2;
                                    }
                                }
                                catch (Exception)
                                {
                                    dailyRespData = 0;
                                }

                            }

                            try
                            {
                                if (Convert.ToString(drData["customer_cashback_amount"]) != "")
                                {
                                    cashBackAmt = Convert.ToString(drData["customer_cashback_amount"]);
                                }
                                
                            }
                            catch (Exception Ex)
                            {
                                
                            }
                            try
                            {
                                if (Convert.ToString(drData["bank_commission"]) != "")
                                {
                                    bankComm = Convert.ToString(drData["bank_commission"]);
                                }
                                
                            }
                            catch (Exception Ex)
                            {
                                
                            }
                            try
                            {
                                if (Convert.ToString(drData["clink_commission"]) != "")
                                {
                                    clinkComm = Convert.ToString(drData["clink_commission"]);
                                }
                            }
                            catch (Exception Ex)
                            {
                                
                            }
                            try
                            {
                                if (Convert.ToString(drData["rrn"]) != "")
                                {
                                    rrn = Convert.ToString(drData["rrn"]);
                                }
                            }
                            catch (Exception Ex)
                            {
                                
                            }

                            try
                            {
                                if (Convert.ToString(drData["trace_number"]) != "")
                                {
                                    trace = Convert.ToString(drData["trace_number"]);
                                }
                            }
                            catch (Exception Ex)
                            {

                            }


                            try
                            {
                                if ((custId != "") && (cardNo != "") && (accNo != ""))
                                {
                                    query = "insert into dbo.GharPay(CustomerID,CardNoHash,AccountNoHash,SMSText,EmailURL,CashBackAmount,BankCommisssion,ClinkCommission,RRN,Trace,DailyRespData,Reject,SMSSent,EmailSent,SMSGUID,UploadDate,SentDate) values(dbo.ufn_EncryptPAN('" + custId + "'),dbo.ufn_EncryptPAN('" + cardNo + "'),dbo.ufn_EncryptPAN('" + accNo + "'),'" + smsText + "','" + email_Text_Url + "','" + cashBackAmt + "','" + bankComm + "','" + clinkComm + "','" + rrn + "','"+ trace +"','" + dailyRespData + "',0,0,0,null,getDate(),null)";
                                    using (SqlStoredProcedure sspObj = new SqlStoredProcedure(query, ConfigManager.GetRBSQLDBOLAPConnection, CommandType.Text))
                                    {
                                        sspObj.ExecuteNonQuery();
                                        //divOutputWindow.InnerText = query;
                                        count++;
                                        custId = "";
                                        cardNo = "";
                                        accNo = "";
                                        smsText = "";
                                        email_Text_Url = "";
                                        rrn = "";
                                        cashBackAmt = "0"; bankComm = "0"; clinkComm = "0"; 
                                        dailyRespData = 0;
                                    }
                                }
                                else
                                {
                                    bHasErrorOccurred = true;

                                    throw new Exception("Couldn't read Data!");
                                }
                            }
                            catch (Exception ex)
                            {
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
                    }
                    finally
                    {

                    }


                    // try
                    // {
                    //     if (!bHasErrorOccurred)
                    //     {
                    //         dtOutput = Generix.getGharPay(DateTime.Now, 0, 0, 1);
                    //         if (dtOutput.Rows.Count > 0)
                    //         {
                    //             Table tblOutput = Generix.convertDataTable2HTMLTable(dtOutput, true, false);
                    //             System.Web.UI.HtmlControls.HtmlInputCheckBox chkGharpay = null;

                    //             foreach (TableRow trOutput in tblOutput.Rows)
                    //             {
                    //                 if (trOutput.TableSection == TableRowSection.TableBody)
                    //                 {
                    //                     chkGharpay = new System.Web.UI.HtmlControls.HtmlInputCheckBox();
                    //                     chkGharpay.Attributes["type"] = "checkbox";
                    //                     chkGharpay.Attributes["id"] = "chkBox_" + trOutput.Cells[0].Text;
                    //                     chkGharpay.Attributes["class"] = "chkgharPay";
                    //                     chkGharpay.Attributes["value"] = trOutput.Cells[0].Text;
                    //                     trOutput.Cells[0].Controls.Add(chkGharpay);
                    //                 }
                    //             }
                    //             divOutputWindow.Controls.Add(tblOutput);

                    //             ClientScript.RegisterClientScriptBlock(this.GetType(), "gridifygharPayTable", "<script>$(document).ready(function(){showGharPayDataTable('#" + divOutputWindow.ClientID + "');});</script>");
                    //         }
                    //         else
                    //         {
                    //             // Response.Write("ERROR: No Entries Found!");
                    //             ClientScript.RegisterClientScriptBlock(this.GetType(), "ErroMsg", "<script>$(document).ready(function(){$('#divDialog').dialog('close').html('Please Check Your Input File!').dialog({title: 'Error',show: 'slide',hide: 'blind',modal: true,buttons: {'Ok': function () {$(this).dialog('close');}}});});</script>");
                    //         }
                    //     }
                    //}
                    //catch(Exception ex)
                    // {
                    //    ClientScript.RegisterClientScriptBlock(this.GetType(), "ErroMsg1", "<script>$(document).ready(function(){$('#divDialog').dialog('close').html('"+ex.Message+"').dialog({title: 'Error',show: 'slide',hide: 'blind',modal: true,buttons: {'Ok': function () {$(this).dialog('close');}}});});</script>");
                    //}
                    Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Gharpay Processed ", "GharPay", "");
                }
                else
                {
                }
            }
        }
    }
}