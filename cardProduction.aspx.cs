using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Data.SqlClient;
using System.Net.Mail;

using ReflectionIT.Common.Data.SqlClient;
using ReflectionIT.Common.Data.Configuration;

namespace Reports
{
    public partial class cardProduction : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String sUploadFileName = "";
            String sRejectedEntries = "";
            String[] aUploadFileData = null;
            DataTable dtUploadData = null;
            String sWaste = "", sWaste2 = "";
            Int32 iTotalUploaded = 0;

            if (Convert.ToString(Session["Active"]) != "1") { Response.Redirect("/Login"); }

            if (!Generix.utilityAccessAllowed("HOTC")) { Response.Redirect("/"); }

            dtUploadData = Generix.getData("dbo.LoginUser", "UtilitiesView", "Code=" + Convert.ToString(Session["ActiveUserCode"]), "", "", 3);
            if (dtUploadData.Rows.Count > 0)
            {
                if (dtUploadData.Rows[0][0].ToString().IndexOf("13.01") > 0)
                {
                    sWaste = "1";
                }
                else
                {
                    sWaste = "0";
                }
                if (dtUploadData.Rows[0][0].ToString().IndexOf("13.02") > 0)
                {
                    sWaste += "1";
                }
                else
                {
                    sWaste += "0";
                }
                if (dtUploadData.Rows[0][0].ToString().IndexOf("13.03") > 0)
                {
                    sWaste += "1";
                }
                else
                {
                    sWaste += "0";
                }
                if (dtUploadData.Rows[0][0].ToString().IndexOf("13.04") > 0)
                {
                    sWaste += "1";
                }
                else
                {
                    sWaste += "0";
                }
                if (dtUploadData.Rows[0][0].ToString().IndexOf("13.05") > 0)
                {
                    sWaste += "1";
                }
                else
                {
                    sWaste += "0";
                }
                if (dtUploadData.Rows[0][0].ToString().IndexOf("13.06") > 0)
                {
                    sWaste += "1";
                }
                else
                {
                    sWaste += "0";
                }
                if (dtUploadData.Rows[0][0].ToString().IndexOf("13.07") > 0)
                {
                    sWaste += "1";
                }
                else
                {
                    sWaste += "0";
                }

                if (dtUploadData.Rows[0][0].ToString().IndexOf("13.00") > 0)
                {
                    sWaste = "1111111";
                }
                else
                {
                    //if (DateTime.Now.Hour >= 19 || DateTime.Now.Hour < 9) { sWaste = "000" + sWaste.Substring(3) + "0"; }
                }
            }
            else
            {
                sWaste = "0000000";
            }

            ClientScript.RegisterClientScriptBlock(this.GetType(), "applyCardProductionAccessRights", "<script>$(document).ready(function(){applyCardProductionAccessRights('" + sWaste + "');});</script>");

            if (IsPostBack && cardFileUpload.HasFile)
            {
                sUploadFileName = "cardProd_" + System.IO.Path.GetRandomFileName();

                cardFileUpload.SaveAs(Server.MapPath("tempOutputs") + "\\" + sUploadFileName);

                dtUploadData = createUploadFileDataTable();

                /*
                if (Convert.ToString(Session["Bank"]).Equals("5"))
                {
                    using (MailMessage oMessage = new MailMessage("s1.alerts@agsindia.com", "biju.k@agsindia.com"))
                    {
                        oMessage.CC.Add("sam.mathai@agsindia.com");
                        oMessage.CC.Add("surendra.jagtap@agsindia.com");
                        oMessage.CC.Add("sagar.goswami@agsindia.com");

                        oMessage.IsBodyHtml = true;
                        oMessage.Body = "https://192.168.4.185/Downloadfile?Fn=" + sUploadFileName + "&Fm=txt";
                        oMessage.Subject = Convert.ToString(Session["BankName"]) + " Card Production Notification";

                        SmtpClient oSMTP = new SmtpClient(System.Configuration.ConfigurationManager.AppSettings["smtpServer"], 25);
                        oSMTP.Credentials = new System.Net.NetworkCredential("s1.alerts@agsindia.com", "x9nd9hft");

                        oSMTP.Send(oMessage);
                        oSMTP = null;
                    }

                    ClientScript.RegisterClientScriptBlock(this.GetType(), "uploadMessage", "<script>$(document).ready(function(){$('#divDialog').html('Card Production Uploaded!').dialog({ title: 'Notice...',show: 'slide',hide: 'blind',modal: true });});</script>");
                    return;
                }
                */

                try
                {
                    using (System.IO.StreamReader srUploadFileData = new System.IO.StreamReader(Server.MapPath("tempOutputs") + "\\" + sUploadFileName))
                    {
                        Int32 iLineNumber = 1;
                        try
                        {
                            while (srUploadFileData.Peek() >= 0)
                            {
                                sWaste = srUploadFileData.ReadLine();
                                sWaste2 = "";

                                if (sWaste.Trim() != "" && (sWaste.Length.Equals(787) || sWaste.Length.Equals(926) || sWaste.Split('|').Length.Equals(30) || ((Convert.ToString(Session["Bank"]).Equals("30") || Convert.ToString(Session["Bank"]).Equals("31")) && sWaste.Split('|').Length.Equals(30))))
                                {
                                    if (Convert.ToString(Session["Bank"]).Equals("5"))
                                    {
                                        sWaste2 = sWaste.Substring(23, 24).Trim();
                                        sWaste2 += "|" + sWaste.Substring(84, 24).Trim();
                                        sWaste2 += "|" + sWaste.Substring(128, 26).Trim();
                                        sWaste2 += "|" + sWaste.Substring(47, 6).Trim();
                                        sWaste2 += "|" + sWaste.Substring(714, 15).Trim();
                                        sWaste2 += "|" + sWaste.Substring(682, 8).Trim();
                                        sWaste2 += "|" + sWaste.Substring(682, 8).Trim();
                                        sWaste2 += "|" + sWaste.Substring(271, 30).Trim();
                                        sWaste2 += "|" + sWaste.Substring(301, 30).Trim();
                                        sWaste2 += "|" + sWaste.Substring(331, 30).Trim();
                                        sWaste2 += "|" + sWaste.Substring(361, 30).Trim().Split(',')[0].Trim();
                                        if (sWaste.Substring(361, 30).Trim().Split(',').Length > 1)
                                            sWaste2 += "|" + sWaste.Substring(361, 30).Trim().Split(',')[1].Trim();
                                        else
                                            sWaste2 += "|";
                                        sWaste2 += "|" + sWaste.Substring(486, 6).Trim();
                                        sWaste2 += "|IND";
                                        sWaste2 += "|";
                                        sWaste2 += "|" + new String(sWaste.Substring(233, 8).Trim().Reverse().ToArray());
                                        sWaste2 += "|";
                                        sWaste2 += "|";
                                        sWaste2 += "|" + sWaste.Substring(393, 10).Trim();
                                        sWaste2 += "|";
                                        sWaste2 += "|";
                                        sWaste2 += "|" + sWaste.Substring(714, 4).Trim();
                                        sWaste2 += "|";
                                        sWaste2 += "|";
                                        sWaste2 += "|" + sWaste.Substring(885, 8).Trim();
                                        sWaste2 += "|";
                                        sWaste2 += "|";
                                        sWaste2 += "|" + sWaste.Substring(885, 8).Trim();
                                        sWaste2 += "|||";

                                        sWaste = sWaste2;
                                    }
                                    else
                                    {

                                        if ((Convert.ToString(Session["Bank"])!="1" && Convert.ToString(Session["Bank"])!="5" && sWaste.Split('|').Length.Equals(30))) sWaste = sWaste.Substring(0, sWaste.Length - 3);
                                        //if ((Convert.ToString(Session["Bank"]).Equals("30") || Convert.ToString(Session["Bank"]).Equals("31") || Convert.ToString(Session["Bank"]).Equals("32")) && sWaste.Split('|').Length.Equals(30)) sWaste = sWaste.Substring(0, sWaste.Length - 3);

                                        if (Convert.ToString(Session["Bank"]).Equals("1") && sWaste.Length.Equals(787)) sWaste = sWaste.Substring(0, 786);
                                        if (sWaste.Split('|').Length.Equals(29)) sWaste += "||"; 
                                    }
                                    /*NEW code for physical PIN*/
                                    #region
                                    /*START*/
                                    if (Convert.ToString(Session["Bank"]).Equals("1"))
                                    {
                                        sWaste += "||";
                                    }
                                    /*END*/
                                    #endregion
                                    sWaste += "|" + Session["ActiveUserCode"].ToString() + "|" + cardFileUpload.FileName + "|" + Convert.ToString(Session["Bank"]);

                                    //if (!sWaste.Split('|').Length.Equals(34))
                                    //{
                                    //    throw new Exception("Invalid Format!");
                                    //}
                                    /*NEW code for physical PIN*/
                                    #region
                                    /*START*/
                                    if ((!sWaste.Split('|').Length.Equals(34) && Convert.ToString(Session["Bank"]) != "1") || (!sWaste.Split('|').Length.Equals(35) && Convert.ToString(Session["Bank"]).Equals(1)))
                                    {
                                        throw new Exception("Invalid Format!");
                                    }
                                    /*END*/
                                    #endregion
                                    DataRow drUploadFileData = dtUploadData.NewRow();
                                    drUploadFileData.ItemArray = sWaste.Split('|');
                                    dtUploadData.Rows.Add(drUploadFileData);
                                }

                                iLineNumber++;
                            }
                            srUploadFileData.Close();
                        }
                        catch (Exception xObj)
                        {
                            throw new Exception("Error in Line Number :" + iLineNumber.ToString());
                        }
                    }

                    File.Delete(Server.MapPath("tempOutputs") + "\\" + sUploadFileName);

                    iTotalUploaded = 0;
                    foreach (DataRow drUploadFileData in dtUploadData.Rows)
                    {
                        String sSQL = "";

                        //foreach (DataColumn dcUploadFileData in dtUploadData.Columns)
                        //{
                        //    sSQL += ((sSQL == "") ? "" : ",") + "'" + drUploadFileData[dcUploadFileData].ToString().Trim().Replace("'", "").Replace("\"", "") + "'";
                        //}
                        ////sSQL += ",'" + cardFileUpload.FileName + "'," + Convert.ToString(Session["Bank"]);

                        //sSQL = "Insert InTo dbo.CardProduction([CIF ID],[Customer Name],[Customer Preferred name],[Card Type and Subtype],[AC ID],[AC open date],[CIF Creation Date],[Address Line 1],[Address Line 2],[Address Line 3],City,State,[Pin Code],[Country code],[Mothers Maiden Name],DOB,[Country Dial code],[City Dial code],[Mobile phone number],[Email id],[Scheme code],[Branch code],[Entered date],[Verified Date],[PAN Number],[Mode Of Operation],[Fourth Line Embossing],Aadhaar,[Debit Card Linkage Flag], [Bc Branch Code], [Center Name], Login, [UploadFileName], Bank) Values(" + sSQL + ")";
                        /*NEW code for physical PIN*/
                        #region
                        /*START*/
                        foreach (DataColumn dcUploadFileData in dtUploadData.Columns)
                        {
                            sSQL += ((sSQL == "") ? "" : ",") + "'" + drUploadFileData[dcUploadFileData].ToString().Trim().Replace("'", "").Replace("\"", "") + "'";
                        }
                        //if (Convert.ToString(Session["Bank"]) != "1")
                        //    sSQL += ",'" + cardFileUpload.FileName + "'," + Convert.ToString(Session["Bank"]);
                        if (Convert.ToString(Session["Bank"]) == "1")
                        { sSQL = "Insert InTo dbo.CardProduction([CIF ID],[Customer Name],[Customer Preferred name],[Card Type and Subtype],[AC ID],[AC open date],[CIF Creation Date],[Address Line 1],[Address Line 2],[Address Line 3],City,State,[Pin Code],[Country code],[Mothers Maiden Name],DOB,[Country Dial code],[City Dial code],[Mobile phone number],[Email id],[Scheme code],[Branch code],[Entered date],[Verified Date],[PAN Number],[Mode Of Operation],[Fourth Line Embossing],Aadhaar,[Debit Card Linkage Flag],[Physical_PIN_Flag] ,[Bc Branch Code], [Center Name], Login, [UploadFileName], Bank) Values(" + sSQL + ")"; }
                        else { sSQL = "Insert InTo dbo.CardProduction([CIF ID],[Customer Name],[Customer Preferred name],[Card Type and Subtype],[AC ID],[AC open date],[CIF Creation Date],[Address Line 1],[Address Line 2],[Address Line 3],City,State,[Pin Code],[Country code],[Mothers Maiden Name],DOB,[Country Dial code],[City Dial code],[Mobile phone number],[Email id],[Scheme code],[Branch code],[Entered date],[Verified Date],[PAN Number],[Mode Of Operation],[Fourth Line Embossing],Aadhaar,[Debit Card Linkage Flag],[Bc Branch Code], [Center Name], Login, [UploadFileName], Bank) Values(" + sSQL + ")"; }
                        
                        /*END*/
                        #endregion
                        using (SqlStoredProcedure sspObj = new SqlStoredProcedure(sSQL, ConfigManager.GetRBSQLDBOLAPConnection, CommandType.Text))
                        {
                            sspObj.ExecuteNonQuery();
                            sspObj.Dispose();

                            iTotalUploaded++;
                        }
                    }

                    if (Generix.processCardProd("", false, Convert.ToInt32(Session["ActiveUserCode"])))
                    {
                        DataTable dtReturn = Generix.getCardProdEntries(DateTime.Now, true ,false, cardFileUpload.FileName);

                        if (dtReturn.Rows.Count > 0)
                        {
                            Table tblOutput = Generix.convertDataTable2HTMLTable(dtReturn, true, false);
                            System.Web.UI.HtmlControls.HtmlInputCheckBox chkCardProd = null;

                            foreach (TableRow trOutput in tblOutput.Rows)
                            {
                                if (trOutput.TableSection == TableRowSection.TableBody)
                                {
                                    chkCardProd = new System.Web.UI.HtmlControls.HtmlInputCheckBox();
                                    chkCardProd.Attributes["type"] = "checkbox";
                                    chkCardProd.Attributes["id"] = "chkBox_" + trOutput.Cells[0].Text;
                                    chkCardProd.Attributes["class"] = "chkBoxCProd";
                                    chkCardProd.Attributes["value"] = trOutput.Cells[0].Text;
                                    trOutput.Cells[0].Controls.Add(chkCardProd);
                                }
                            }

                            divOutputWindow.Controls.Add(tblOutput);
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "gridifyRejectedEntriesTable", "<script>$(document).ready(function(){showRejectedCardProd('#" + divOutputWindow.ClientID + "');});</script>");
                        }
                        else
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "gridifyRejectedEntriesTable", "<script>$(document).ready(function(){$('#divDialog').html('No Entries Rejected!').dialog({ title: 'Error...',show: 'slide',hide: 'blind',modal: true });});</script>");
                        }

                        try
                        {
                            using (MailMessage oMessage = new MailMessage("s1.alerts@agsindia.com", "biju.k@agsindia.com"))
                            {
                                
                                oMessage.CC.Add("nilesh.sonawane@agsindia.com");
                                oMessage.CC.Add("sachin.parle@agsindia.com");
                                oMessage.CC.Add("Mohammad.gufrankhan@agsindia.com");

                                oMessage.IsBodyHtml = true;
                                oMessage.Body = iTotalUploaded.ToString() + " Card Production Data Uploaded!<br/><br/>" + dtReturn.Rows.Count.ToString() + " Records Rejected!";
                                oMessage.Subject = Convert.ToString(Session["BankName"]) + " Card Production Notification";

                                SmtpClient oSMTP = new SmtpClient(System.Configuration.ConfigurationManager.AppSettings["smtpServer"], 25);
                                oSMTP.Credentials = new System.Net.NetworkCredential("s1.alerts@agsindia.com", "matrix1");

                                oSMTP.Send(oMessage);
                                oSMTP = null;
                            }
                        }
                        catch (Exception xObj)
                        {
                        }
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "gridifyProcessingError", "<script>$(document).ready(function(){$('#divDialog').html('Error Occurred While Processing Uploaded Data!').dialog({ title: 'Error...',show: 'slide',hide: 'blind',modal: true });});</script>");
                    }

                    Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Card Production File Upload & Processed", "CPROD", "");
                }
                catch (Exception xObj)
                {
                    if (xObj.Message.IndexOf("Error in Line Number") != -1)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "gridifyProcessingError", "<script>$(document).ready(function(){$('#divDialog').html('Error Occurred While Processing File!<br/><br/>" + xObj.Message + "').dialog({ title: 'Error...',show: 'slide',hide: 'blind',modal: true });});</script>");
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "gridifyProcessingError", "<script>$(document).ready(function(){$('#divDialog').html('Error Occurred While Processing File!').dialog({ title: 'Error...',show: 'slide',hide: 'blind',modal: true });});</script>");
                    }
                }

                cardFileUpload.Dispose();
            }
        }

        private DataTable createUploadFileDataTable()
        {
            DataTable dtReturn = new DataTable();

            dtReturn.Columns.Add("CIF ID");
            dtReturn.Columns.Add("Customer Name");
            dtReturn.Columns.Add("Customer Preferred name");
            dtReturn.Columns.Add("Card Type and Subtype");
            dtReturn.Columns.Add("AC ID");
            dtReturn.Columns.Add("AC open date");
            dtReturn.Columns.Add("CIF Creation Date");
            dtReturn.Columns.Add("Address Line 1");
            dtReturn.Columns.Add("Address Line 2");
            dtReturn.Columns.Add("Address Line 3");
            dtReturn.Columns.Add("City");
            dtReturn.Columns.Add("State");
            dtReturn.Columns.Add("Pin Code");
            dtReturn.Columns.Add("Country code");
            dtReturn.Columns.Add("Mother’s Maiden Name");
            dtReturn.Columns.Add("DOB");
            dtReturn.Columns.Add("Country Dial code");
            dtReturn.Columns.Add("City Dial code");
            dtReturn.Columns.Add("Mobile phone number");
            dtReturn.Columns.Add("Email id");
            dtReturn.Columns.Add("Scheme code");
            dtReturn.Columns.Add("Branch code");
            dtReturn.Columns.Add("Entered date");
            dtReturn.Columns.Add("Verified Date");
            dtReturn.Columns.Add("PAN Number");
            dtReturn.Columns.Add("Mode Of Operation");
            dtReturn.Columns.Add("Fourth Line Embossing");
            dtReturn.Columns.Add("Aadhaar");
            dtReturn.Columns.Add("Debit Card Linkage Flag");
            if (Convert.ToString(Session["Bank"]).Equals("1"))
            { dtReturn.Columns.Add("PIN_Flag"); }
            dtReturn.Columns.Add("Bc Branch Name");
            dtReturn.Columns.Add("Center Name");
            dtReturn.Columns.Add("Login");
            dtReturn.Columns.Add("FileName");
            dtReturn.Columns.Add("Bank");

            return dtReturn;
        }
    }
}
