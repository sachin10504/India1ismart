using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using ReflectionIT.Common.Data.Configuration;
using ReflectionIT.Common.Data.SqlClient;


namespace Reports
{
    public partial class cardOps : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable dtUploadData = null;
            String sWaste = "";
            Int32 iWaste = 0;
            int count = 0;
            int total = 0;

            if (Convert.ToString(Session["Active"]) != "1") { Response.Redirect("/Login"); }

            if (!Generix.utilityAccessAllowed("COPS")) { Response.Redirect("/"); }

            if (IsPostBack)
            {
                try
                {
                    String sUploadFileName = System.IO.Path.GetRandomFileName();

                    if (fileUploadData.HasFile)
                    {
                        fileUploadData.SaveAs(Server.MapPath("tempOutputs") + "\\" + sUploadFileName);

                        String[] aCustomerData = System.IO.File.ReadAllLines(Server.MapPath("tempOutputs") + "\\" + sUploadFileName);
                        
                        foreach (String sCustomerData in aCustomerData)
                        {
                            if (sCustomerData.Split('|').Length == 22)
                            {
                                if (!Generix.setCardDetails(Convert.ToInt32(Session["Bank"]), sCustomerData.Split('|')[0], sCustomerData.Split('|')[1],
                                    sCustomerData.Split('|')[2], sCustomerData.Split('|')[3], sCustomerData.Split('|')[4], sCustomerData.Split('|')[5],
                                    sCustomerData.Split('|')[6], sCustomerData.Split('|')[7], sCustomerData.Split('|')[8], sCustomerData.Split('|')[9],
                                    sCustomerData.Split('|')[10], sCustomerData.Split('|')[11], sCustomerData.Split('|')[12], sCustomerData.Split('|')[13],
                                    sCustomerData.Split('|')[14], sCustomerData.Split('|')[15], sCustomerData.Split('|')[16], sCustomerData.Split('|')[17],
                                    sCustomerData.Split('|')[18], sCustomerData.Split('|')[19], sCustomerData.Split('|')[20], sCustomerData.Split('|')[21], Convert.ToInt32(Session["ActiveUserCode"]),
                                    fileUploadData.FileName))
                                {
                                    iWaste++;
                                }
                            }
                            else
                            {
                                iWaste++;
                            }
                        }

                        Session["CustomerUploadDataStat"] = (iWaste == 0 ? "" : (aCustomerData.Length - iWaste).ToString() + " of ") + aCustomerData.Length.ToString() + " Records Uploaded!";
                        Response.Redirect("cardOps.aspx");
                    }
                    if (fileUploadReissue.HasFile)
                    {
                        fileUploadReissue.SaveAs(Server.MapPath("tempOutputs") + "\\" + sUploadFileName);

                        String[] aReissueData = System.IO.File.ReadAllLines(Server.MapPath("tempOutputs") + "\\" + sUploadFileName);

                        total = aReissueData.Length;

                        foreach (String sReissueData in aReissueData)
                        {
                            if (sReissueData.Split('|').Length == 4)
                            {
                                if (Generix.insertBulkReissueRequest(Convert.ToInt32(Session["Bank"]), sReissueData.Split('|')[0], sReissueData.Split('|')[1],
                                     sReissueData.Split('|')[2], sReissueData.Split('|')[3], Convert.ToInt32(Session["ActiveUserCode"])))
                                {
                                    count++;
                                }

                                
                            }
                            else
                            {
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "ReissueErrorInsert", "<script>$(document).ready(function(){$('#divDialog').html('Please Check Your File Format!'<br/>).dialog({modal: true});});</script>");
                            }
                        }
                        
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "cardRequesRowInserted", "<script>$(document).ready(function(){$('#divDialog').dialog('close').html('Total " + count + " Cards Send For Re-Issue Out Of "+total+"').dialog({title: 'Successfull',show: 'slide',hide: 'blind',modal: true,buttons: {'Ok': function () {$(this).dialog('close'); window.location = 'cardOps.aspx';}}});});</script>");
                        
                    }
                    if (fileUploadHotlist.HasFile)
                    {
                        fileUploadHotlist.SaveAs(Server.MapPath("tempOutputs") + "\\" + sUploadFileName);

                        String[] aHotlistData = System.IO.File.ReadAllLines(Server.MapPath("tempOutputs") + "\\" + sUploadFileName);

                        total = aHotlistData.Length;

                        foreach (String sHotlistData in aHotlistData)
                        {
                            if (sHotlistData.Split('|').Length == 3)
                            {
                                string SourceID = string.Empty;
                                string SwitchRsp = string.Empty;
                                string requesttype = string.Empty;
                                string sHRC = Convert.ToString(sHotlistData.Split('|')[1]).Trim();
                                string sCards =Convert.ToString(sHotlistData.Split('|')[0].Replace("'", "")).Trim();
                                ClscardBlockUnblock ObjBlockUnblock = new ClscardBlockUnblock();
                                APIMessage ObjAPImsg = ObjBlockUnblock.generateBlockUnblockRequets(Convert.ToString(Session["Bank"]), sHRC, sCards.Replace("'", ""), Convert.ToString(Session["AcquirerId"]), Convert.ToString(Session["SwitchType"]),"0"); ///*Added for international usage RBL-ATPCM-862* /
                                SourceID = ObjBlockUnblock.getsourceId(Convert.ToString(Session["Bank"]), Convert.ToString(Session["SwitchType"]));
                                requesttype = ObjBlockUnblock.getRequestType(Convert.ToString(Session["SwitchType"]), sHRC);
                                SwitchRsp = new CallCardAPI().Process(requesttype, ObjAPImsg, SourceID);
                                if (SwitchRsp.Equals("000"))
                                {
                                    Generix.InsertHostlistRecords(Convert.ToInt32(Session["Bank"]), sHotlistData.Split('|')[0], sHotlistData.Split('|')[1], sHotlistData.Split('|')[2], Convert.ToInt32(Session["ActiveUserCode"]));
                                    count++;

                                }

                                #region old code with  query base
                                /*
                                { 
                                    if (Generix.setHotlistCard(Convert.ToInt32(Session["Bank"]), sHotlistData.Split('|')[0], sHotlistData.Split('|')[1],
                                     sHotlistData.Split('|')[2], Convert.ToInt32(Session["ActiveUserCode"])))
                                    {
                                        count++;
                                    }
                                }
                                 */
                                #endregion


                            }
                            else
                            {
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "HotErrorInsert", "<script>$(document).ready(function(){$('#divDialog').html('Please Check Your File Format!'<br/>).dialog({modal: true});});</script>");
                            }
                        }

                        ClientScript.RegisterClientScriptBlock(this.GetType(), "hotlistcardRequesRowInserted", "<script>$(document).ready(function(){$('#divDialog').dialog('close').html('Total " + count + " Cards Hotlisted Out Of " + total + ".').dialog({title: 'Successfull',show: 'slide',hide: 'blind',modal: true,buttons: {'Ok': function () {$(this).dialog('close'); window.location = 'cardOps.aspx';}}});});</script>");

                    }

                }
                catch (Exception xObj)
                {
                }
            }
            else
            {
                dtUploadData = Generix.getData("dbo.LoginUser", "UtilitiesView", "Code=" + Convert.ToString(Session["ActiveUserCode"]), "", "", 3);
                if (dtUploadData.Rows.Count > 0)
                {
                    sWaste = (dtUploadData.Rows[0][0].ToString().IndexOf("19.01") > 0) ? "1" : "0";
                    sWaste += (dtUploadData.Rows[0][0].ToString().IndexOf("19.02") > 0) ? "1" : "0";
                    sWaste += (dtUploadData.Rows[0][0].ToString().IndexOf("19.03") > 0) ? "1" : "0";
                    sWaste += (dtUploadData.Rows[0][0].ToString().IndexOf("19.04") > 0) ? "1" : "0";
                    sWaste += (dtUploadData.Rows[0][0].ToString().IndexOf("19.05") > 0) ? "1" : "0";
                    sWaste += (dtUploadData.Rows[0][0].ToString().IndexOf("19.06") > 0) ? "1" : "0";
                    sWaste += (dtUploadData.Rows[0][0].ToString().IndexOf("19.07") > 0) ? "1" : "0";
                    sWaste += (dtUploadData.Rows[0][0].ToString().IndexOf("19.08") > 0) ? "1" : "0";
                    sWaste += (dtUploadData.Rows[0][0].ToString().IndexOf("19.09") > 0) ? "1" : "0";
                    sWaste += (dtUploadData.Rows[0][0].ToString().IndexOf("19.10") > 0) ? "1" : "0";

                    if (dtUploadData.Rows[0][0].ToString().IndexOf("19.00") > 0)
                    {
                        sWaste = "1111111111";
                    }
                }
                else
                {
                    sWaste = "0000000000";
                }

                //ClientScript.RegisterClientScriptBlock(this.GetType(), "applyCardOperationsAccessRights", "<script>$(document).ready(function(){$('#btnApplyCardOperationsAccessRights').click(function(){applyCardOperationsAccessRights('" + sWaste + "');});$('#btnApplyCardOperationsAccessRights').click();});</script>");
                ClientScript.RegisterClientScriptBlock(this.GetType(), "applyCardOperationsAccessRights", "<script>$(document).ready(function(){$('#btnApplyCardOperationsAccessRights').click(function(){applyCardOperationsAccessRights('" + sWaste + "');});applyCardOperationsAccessRights('" + sWaste + "');});</script>");

                if (Convert.ToString(Session["CustomerUploadDataStat"]) != "")
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "showUploadStatsMessage", "<script>$(document).ready(function(){$('#divDialog').dialog('close').html('" + Convert.ToString(Session["CustomerUploadDataStat"]) + "').dialog({title: 'Notice...',show: 'slide',hide: 'blind',modal: true});});</script>");

                    Session["CustomerUploadDataStat"] = "";
                }

                /*
                dtUploadData = Generix.getData("Declare @SQL Varchar(Max); Declare @SQL2 VarChar(Max); Select @SQL = Replace(BINS,'''',''), @SQL2 = Replace(BINPrefixs,'''','') From dbo.Banks Where Code=" + Session["BINPrefixs"].ToString() + "; Select A.item+B.item From dbo.fnSplit(@SQL,',') A, dbo.fnSplit(@SQL2,',') B Order By A.item+B.item;", 3);
                if (dtUploadData.Rows.Count > 0)
                {
                    foreach (DataRow drUploadData in dtUploadData.Rows)
                    {
                        litBINS.Text += "<li><a href='#' onclick='javascript:reissueCard($(" + Generix.Chr(34) + "#txtCardNum" + Generix.Chr(34) + ").val(), " + drUploadData[0].ToString() + ");'>" + drUploadData[0].ToString() + "</a></li>";
                    }
                }
                */

                /*
                var array1 = Session["BINS"].ToString().Split(',');
                var array2 = Session["BINPrefixs"].ToString().Split(',');

                var array3 =
                    from array1element in array1
                    from array2element in array2
                    select new[] { array1element, array2element };

                foreach (var element in array3)
                {
                    litBINS.Text += "<li><a href='#' onclick='javascript:reissueCard($(" + Generix.Chr(34) + "#txtCardNum" + Generix.Chr(34) + ").val(), " + element[0].ToString().Replace("'", "") + element[1].ToString().Replace("'", "") + ");'>" + element[0].ToString().Replace("'", "") + element[1].ToString().Replace("'", "") + "</a></li>";
                }
                */

                /*
                litBINS.Text = "";

                dtUploadData = Generix.getData("Select Distinct Left(dbo.ufn_DecryptPAN(DecPAN),8) BINS From dbo.CardRPAN Where Left(dbo.ufn_DecryptPAN(DecPAN),8) Not Like '502964%' And IssuerNo=" + Session["IssuerNo"].ToString() + " Order By BINS", 3);
                if (dtUploadData.Rows.Count > 0)
                {
                    foreach (DataRow drUploadData in dtUploadData.Rows)
                    {
                        litBINS.Text += "<li><a href='#' onclick='javascript:reissueCard($(" + Generix.Chr(34) + "#txtCardNum" + Generix.Chr(34) + ").val(), " + drUploadData[0].ToString() + ");'>" + drUploadData[0].ToString() + "</a></li>";
                    }
                }
                */

                litBINS.Text = "";

                dtUploadData = Generix.getData("Select BINPrefix+' : '+Description [Bins] From dbo.AllowedBINPrefixes Where Bank=" + Session["Bank"].ToString() + " Order By BINPrefix", 3);
                if (dtUploadData.Rows.Count > 0)
                {
                    foreach (DataRow drUploadData in dtUploadData.Rows)
                    {
                        litBINS.Text += "<li><a href='#' onclick='javascript:reissueCard($(" + Generix.Chr(34) + "#txtCardNum" + Generix.Chr(34) + ").val(), " + drUploadData[0].ToString().Split(':')[0] + ");'>" + drUploadData[0].ToString() + "</a></li>";
                    }
                }
            }
        }
    }
}
