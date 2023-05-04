using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

using ReflectionIT.Common.Data.SqlClient;
using ReflectionIT.Common.Data.Configuration;

namespace Reports
{
    public partial class APBS : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String sUploadFileName = "";
            DataTable dtUploadData;
            DataTable dtData;
            DataTable dtOutput;
            String sWaste = "";
            Boolean bHasErrorOccurred = false;
            string accName = "", aadharNo = "", accNo = "", cardPrefix = "";
            String query = "";
            int original = 0;
            int duplicate = 0;

            if (Convert.ToString(Session["Active"]) != "1") { Response.Redirect("/Login"); }

            if (!Generix.utilityAccessAllowed("APBS")) { Response.Redirect("/"); }

            if (IsPostBack)
            {
                sUploadFileName = System.IO.Path.GetRandomFileName();


              if(apbsFileUpload.HasFile){
                  
                String apbsData = "";
                
                String queryHeader = "";

                int count = 0;
                apbsFileUpload.SaveAs(Server.MapPath("tempOutputs") + "\\" + sUploadFileName);
                DateTime UploadDate = DateTime.Now;
                String sTxnCode, sDestBankIIN, sDestAccType, sFollioNo, sAadhaarNo, sAccHolderName, sSponsorBankIIN, sUserNo, sUserName, sUserCreditRef, sAmount, sReserved1, sReserved2, sReserved3, sDestBankAccNo, sRetReasonCode, sHeader;

                System.IO.StreamReader abpsFileUpload = new System.IO.StreamReader(Server.MapPath("tempOutputs") + "\\" + sUploadFileName);

                while (abpsFileUpload.Peek() >= 0)
                {
                    apbsData = abpsFileUpload.ReadLine();
                    sHeader = apbsData.Substring(0, 177).Trim();

                    if ((!Generix.isAvailable("dbo.apbsHeader", " Header = '" + sHeader.Replace(" ", "") + "'", 3)))
                    {
                        if ((apbsData.Trim() != "") && (sHeader.Substring(0, 2).Trim() == "33"))
                        {
                            queryHeader = "insert into apbsHeader (Header) values('" + sHeader.Replace(" ", "") + "')";
                            using (SqlStoredProcedure sspObj = new SqlStoredProcedure(queryHeader, ConfigManager.GetRBSQLDBOLAPConnection, CommandType.Text))
                            {
                                sspObj.ExecuteNonQuery();
                            }
                        }

                        sTxnCode = apbsData.Substring(0, 2).Trim();
                        if ((apbsData.Trim() != "") && (sTxnCode != "33"))
                        {
                            sTxnCode = apbsData.Substring(0, 2).Trim();
                            sDestBankIIN = apbsData.Substring(2, 9).Trim();
                            sDestAccType = apbsData.Substring(11, 2).Trim();
                            sFollioNo = apbsData.Substring(13, 3).Trim();
                            sAadhaarNo = apbsData.Substring(16, 15).Trim();
                            sAccHolderName = apbsData.Substring(31, 40).Trim();
                            sSponsorBankIIN = apbsData.Substring(71, 9).Trim();
                            sUserNo = apbsData.Substring(80, 7).Trim();
                            sUserName = apbsData.Substring(87, 20).Trim();
                            sUserCreditRef = apbsData.Substring(107, 13).Trim();
                            sAmount = Int64.Parse(apbsData.Substring(120, 13).Trim()).ToString();
                            sReserved1 = apbsData.Substring(133, 10).Trim();
                            sReserved2 = apbsData.Substring(143, 10).Trim();
                            sReserved3 = apbsData.Substring(153, 2).Trim();
                            sDestBankAccNo = apbsData.Substring(155, 20).Trim();
                            sRetReasonCode = apbsData.Substring(175, 2).Trim();

                            try
                            {
                                if ((sAadhaarNo != ""))
                                {
                                    query = "insert into APbsRequest (TxnCode,DestBankIIN,DestAccType,FollioNo,AadhaarNo,AccHolderName,SponsorBankIIN,UserNo,UserName,UserCreditRef,Amount,Reserved1,Reserved2,Reserved3,DestBankAccNo,RetReasonCode,Reject,UploadDate,Reason,process,fileName) values('" + sTxnCode + "','" + sDestBankIIN + "','" + sDestAccType + "','" + sFollioNo + "',dbo.ufn_EncryptPAN('" + sAadhaarNo + "'),'" + sAccHolderName + "','" + sSponsorBankIIN + "','" + sUserNo + "','" + sUserName + "','" + sUserCreditRef +
                                            "','" + sAmount + "','" + sReserved1 + "','" + sReserved2 + "','" + sReserved3 + "','" + sDestBankAccNo + "','" + sRetReasonCode + "',0,getDate(),null,0,'" + apbsFileUpload.FileName + "')";
                                    using (SqlStoredProcedure sspObj = new SqlStoredProcedure(query, ConfigManager.GetRBSQLDBOLAPConnection, CommandType.Text))
                                    {
                                        sspObj.ExecuteNonQuery();
                                        count++;
                                    }
                                }
                                else
                                {
                                    throw new Exception("Couldn't read Data!");
                                }
                            }
                            catch (Exception ex)
                            {
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "formatError", "<script>$(document).ready(function(){$('#divDialog').html('Please Check Your File Format!<br/>').dialog({modal: true});});</script>");
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                if (count > 0)
                {
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "TotatalInsert", "<script>$(document).ready(function(){$('#divDialog').html(' Total " + count + " Transaction inserted successfully!').dialog();});</script>");
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "TotalInsert", "<script>$(document).ready(function(){$('#divDialog').html(' Total " + count + " Transactions inserted successfully!').dialog({title: 'Error...',show: 'slide',hide: 'blind',modal: true,buttons: {'Ok': function () {$(this).dialog('close');$(this).dialog('destroy'); location.reload();}}});});</script>");
                }
                else
                {
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "DuplicateFile", "<script>$(document).ready(function(){$('#divDialog').html(' File Already Uploaded! <br> Please check. ').dialog({title: 'Error...',show: 'slide',hide: 'blind',modal: true,buttons: {'Ok': function () {$(this).dialog('close');$(this).dialog('destroy');}}});});</script>");
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "TotalInsert", "<script>$(document).ready(function(){$('#divDialog').html(' File Already Uploaded! <br> Please check!').dialog({title: 'Error...',show: 'slide',hide: 'blind',modal: true,buttons: {'Ok': function () {$(this).dialog('close');$(this).dialog('destroy'); location.reload();}}});});</script>");
                }
            }
              
              if (fileUploadAadhar.HasFile)
              {
                  sUploadFileName = System.IO.Path.GetRandomFileName();
                  fileUploadAadhar.SaveAs(Server.MapPath("tempOutputs") + "\\" + sUploadFileName);

                  try
                  {
                      dtData = Generix.getExcelWorksheetData(true, Server.MapPath("tempOutputs") + "\\" + sUploadFileName, "Sheet1");
                      if (dtData.Rows.Count < 1) { bHasErrorOccurred = true; }

                      foreach (DataRow drData in dtData.Rows)
                      {
                          try
                          {
                              accNo = Convert.ToString(drData["ACCOUNT_NUMBER"]);
                          }
                          catch (Exception custIdEx)
                          {
                              accNo = Convert.ToString(drData["ACCOUNT_NUMBER"]);
                          }

                          try
                          {
                              accName = Convert.ToString(drData["ACCT_NAME"]);
                          }
                          catch (Exception cardNoEx)
                          {
                              accName = Convert.ToString(drData["ACCT_NAME"]);
                          }
                          try
                          {
                              aadharNo = Convert.ToString(drData["ADDHAR_NUMBER"]);
                          }
                          catch (Exception cardNoEx)
                          {
                              aadharNo = Convert.ToString(drData["ADDHAR_NUMBER"]);
                          }
                          try
                          {
                              cardPrefix = Convert.ToString(drData["Card_Prefix"]);
                          }
                          catch (Exception cardNoEx)
                          {
                              cardPrefix = Convert.ToString(drData["Card_Prefix"]);
                          }

                          try
                          {
                              if ((accNo != "") && (accName != "") && (aadharNo != "")&&(cardPrefix != ""))
                              {
                                  if (Generix.updateApbsAadhaarDatabase(Convert.ToInt32(Session["Bank"]), accNo, accName, aadharNo, cardPrefix, Convert.ToInt32(Session["ActiveUserCode"])))
                                  {
                                      original++;

                                  }
                                  else {
                                        duplicate++;
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

                  }
                  catch (Exception ex)
                  {
                      divOutputWindow.InnerText = ex.Message;
                  }
                  finally
                  {

                  }
                 //ClientScript.RegisterClientScriptBlock(this.GetType(), "TotalInsert", "<script>$(document).ready(function(){$('#divDialog').html(' Total " + original + " Rows inserted successfully!').dialog();});</script>");

                  ClientScript.RegisterClientScriptBlock(this.GetType(), "TotalInsert", "<script>$(document).ready(function(){$('#divDialog').html('Toal " + original + " Rows inserted successfully! <br> Total " + duplicate + " Rows Duplicate!').dialog({title: 'Error...',show: 'slide',hide: 'blind',modal: true,buttons: {'Ok': function () {$(this).dialog('close');$(this).dialog('destroy'); location.reload();}}});});</script>");

                //ClientScript.RegisterClientScriptBlock(this.GetType(), "TotalInsert", "<script>$(document).ready(function(){$('#divDialog').html(' Total " + original + " Rows inserted successfully!<br> Total'"+duplicate+"' Rows Duplicate!).dialog({title: 'Error...',show: 'slide',hide: 'blind',modal: true,buttons: {'Ok': function () {$(this).dialog('close');$(this).dialog('destroy'); location.reload();}}});});</script>");
                  //if (duplicate > 0) 
                  //{
                  //    ClientScript.RegisterClientScriptBlock(this.GetType(), "TotalInsert", "<script>$(document).ready(function(){$('#divDialog').html(' Total " + duplicate + " Rows Duplicate!').dialog({title: 'Error...',show: 'slide',hide: 'blind',modal: true,buttons: {'Ok': function () {$(this).dialog('close');$(this).dialog('destroy'); location.reload();}}});});</script>");
                  //}
                  
                  Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "APBS Aadhar Database Update", "APBS", "");
              }

            }
            else {

                dtUploadData = Generix.getData("dbo.LoginUser", "UtilitiesView", "Code=" + Convert.ToString(Session["ActiveUserCode"]), "", "", 3);
                if (dtUploadData.Rows.Count > 0)
                {
                    if (dtUploadData.Rows[0][0].ToString().IndexOf("25.01") > 0)
                    {
                        sWaste = "1";
                    }
                    else
                    {
                        sWaste = "0";
                    }
                    if (dtUploadData.Rows[0][0].ToString().IndexOf("25.02") > 0)
                    {
                        sWaste += "1";
                    }
                    else
                    {
                        sWaste += "0";
                    }
                    if (dtUploadData.Rows[0][0].ToString().IndexOf("25.03") > 0)
                    {
                        sWaste += "1";
                    }
                    else
                    {
                        sWaste += "0";
                    }

                    if (dtUploadData.Rows[0][0].ToString().IndexOf("25.00") > 0)
                    {
                        sWaste = "111";
                    }

                }
                else
                {
                    sWaste = "000";
                }

                ClientScript.RegisterClientScriptBlock(this.GetType(), "applyAPBSAccessRights", "<script>$(document).ready(function(){$('#btnApplyAPBSAccessRights').click(function(){applyApbsAccessRights('" + sWaste + "');});$('#btnApplyAPBSAccessRights').click();});</script>");
            }
        }
    }
}