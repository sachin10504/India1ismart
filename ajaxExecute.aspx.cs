using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Text;
using System.Data.SqlClient;
using System.Threading;
using Reports.App_Code;
using ExportToExcel;
using Newtonsoft.Json.Linq;
using ReflectionIT.Common.Data.Configuration;
using Reports.Utilities.Loggers;
using System.Net;

namespace Reports
{
    public partial class ajaxExecute : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            string[] aQueryString;
            DataTable dtOutput;
            string sOutput;

            string sATM = "";
            string sATM2 = "";
            DateTime dFrom = new DateTime();
            DateTime dTo = new DateTime();
            string sSource = "";
            string sMerchants = "";
            string sCards = "";
            string sHr = "";
            string sTranSource = "";
            string sNode = "";
            string sOrigin = "";
            string sHRC = "";
            string sRemarks = "";
            string sCustName = "";
            string sMobile = "";
            string sEmail = "";
            string sCardNumber = "";
            string sID = "";
            string sField = "";
            string sWaste = "";
            string sData = "";
            string sTop = "";
            string sSkipDownloadFile = "";
            string sAll = "";
            string sFile = "";
            string sLims = "";
            string sBranch = "";
            string sBranchName = "";
            string sIvrID = "";
            string sIvrData = "";
            string sIvrMobile = "";
            string sIvrRemark = "";
            string sRollId = "";

            System.Web.UI.HtmlControls.HtmlInputCheckBox chkCardno;
            ClinkEncryption objEnc = new ClinkEncryption();

            ISO8583Lib iso8583 = new ISO8583Lib();
            HyperComp hypercom = new HyperComp();
            byte[] recbytes = null;

            HtmlTableParser htmlTable = new HtmlTableParser();

            Boolean success = false;
            Boolean success1 = false;
            Boolean success2 = false;
            Boolean success3 = false;


            try
            {
                Response.Cache.SetAllowResponseInBrowserHistory(false);
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetNoStore();

                aQueryString = Convert.ToString(Request.QueryString["Fn"]).Split(new char[] { '|' });

                if (Convert.ToString(HttpContext.Current.Session["Active"]) != "1" && Convert.ToString(aQueryString[0]) != "VUSR" && Convert.ToString(aQueryString[0]) != "FORGOTPASS" && Convert.ToString(aQueryString[0]) != "IVRUCM" && Convert.ToString(aQueryString[0]) != "CMDTRML")
                {
                    if (!Convert.ToString(Request.QueryString["Src"]).Equals("AGSME"))
                    {
                        Response.Write("ERROR: Invalid Session!");

                        return;
                    }
                }

                if (aQueryString.Length > 0)
                {
                    if (Request.Form["ATM"] != "" && Request.Form["ATM"] != null)
                        sATM = Request.Form["ATM"];
                    else
                        if (Request.QueryString["ATM"] != null && Request.QueryString["ATM"] != "") sATM = Request.QueryString["ATM"];

                    switch (Convert.ToString(aQueryString[0]))
                    {
                        //case "RTATMBAL":
                        //    if (sATM == null || sATM == "")
                        //        sATM = "Left(term_id,2) In (" + Convert.ToString(Session["TerminalPrefix"]) + ") ";
                        //    else
                        //        sATM = "term_id In ('" + sATM.Replace(",", "','") + "') ";

                        //    break;

                        case "NOPT":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            sHr = Convert.ToString(Request.Form["Hr"]);

                            break;

                        default:
                            if (Request.Form["Of"] != "" && Request.Form["Of"] != null) dFrom = Convert.ToDateTime(Request.Form["Of"]);
                            if (Request.Form["To"] != "" && Request.Form["To"] != null) dTo = Convert.ToDateTime(Request.Form["To"]);
                            if (Request.Form["Source"] != "" && Request.Form["Source"] != null)
                            {
                                sSource = Convert.ToString(Request.Form["Source"]);

                                if (!Regex.IsMatch(sSource, "^[0-1]*$", RegexOptions.Compiled) || sSource.Length != 3)
                                {
                                    throw new Exception("Invalid Argument!");
                                }
                            }
                            //if (Request.Form["Cards"] != "" && Request.Form["Cards"] != null)
                            //{
                            //    sCards = Convert.ToString(Request.Form["Cards"]);

                            //    if (!Regex.IsMatch(sCards, "^[0-9,%]*$", RegexOptions.Compiled))
                            //    {
                            //        throw new Exception("Invalid Argument!");
                            //    }
                            //}
                            if (Request.Form["Merchants"] != "" && Request.Form["Merchants"] != null)
                            {
                                sMerchants = Convert.ToString(Request.Form["Merchants"]);

                                if (!Regex.IsMatch(sMerchants, "^[0-9,]*$", RegexOptions.Compiled))
                                {
                                    throw new Exception("Invalid Argument!");
                                }
                            }
                            if (Request.Form["TranSource"] != "" && Request.Form["TranSource"] != null)
                            {
                                sTranSource = Convert.ToString(Request.Form["TranSource"]);

                                if (!Regex.IsMatch(sTranSource, "^[0-1,]*$", RegexOptions.Compiled) || sTranSource.Length != 2)
                                {
                                    throw new Exception("Invalid Argument!");
                                }
                            }
                            if (Request.Form["Node"] != "" && Request.Form["Node"] != null)
                            {
                                sNode = Convert.ToString(Request.Form["Node"]);

                                if (!Regex.IsMatch(sNode, "^[0-1,]*$", RegexOptions.Compiled) || sNode.Length != 4)
                                {
                                    throw new Exception("Invalid Argument!");
                                }
                            }
                            if (Request.Form["Origin"] != "" && Request.Form["Origin"] != null)
                            {
                                sOrigin = Convert.ToString(Request.Form["Origin"]);

                                if (!Regex.IsMatch(sOrigin, "^[0-1,]*$", RegexOptions.Compiled) || sOrigin.Length != 2)
                                {
                                    throw new Exception("Invalid Argument!");
                                }
                            }
                            if (Request.Form["HRC"] != "" && Request.Form["HRC"] != null)
                            {
                                sHRC = Convert.ToString(Request.Form["HRC"]);

                                if (!Regex.IsMatch(sHRC, "^[0-9]*$", RegexOptions.Compiled) || sHRC.Length != 2)
                                {
                                    throw new Exception("Invalid Argument!");
                                }
                            }
                            if (Request.Form["Rem"] != "" && Request.Form["Rem"] != null)
                            {
                                sRemarks = Convert.ToString(Request.Form["Rem"]);

                                if (!Regex.IsMatch(sRemarks, "^[a-zA-Z0-9,. ]*$", RegexOptions.Compiled))
                                {
                                    throw new Exception("Invalid Argument!");
                                }
                            }
                            if (Request.Form["Remark"] != "" && Request.Form["Remark"] != null)
                            {
                                sRemarks = Convert.ToString(Request.Form["Remark"]);

                                if (!Regex.IsMatch(sRemarks, "^[a-zA-Z0-9,. ]*$", RegexOptions.Compiled))
                                {
                                    throw new Exception("Invalid Argument!");
                                }
                            }
                            if (Request.Form["CName"] != "" && Request.Form["CName"] != null)
                            {
                                sCustName = Convert.ToString(Request.Form["CName"]);

                                if (!Regex.IsMatch(sCustName, "^[a-zA-Z. ]*$", RegexOptions.Compiled))
                                {
                                    throw new Exception("Invalid Argument!");
                                }
                            }
                            if (Request.Form["Mobile"] != "" && Request.Form["Mobile"] != null)
                            {
                                sMobile = Convert.ToString(Request.Form["Mobile"]);

                                if (!Regex.IsMatch(sMobile, "^[0-9]*$", RegexOptions.Compiled))
                                {
                                    throw new Exception("Invalid Argument!");
                                }
                            }
                            if (Request.Form["Email"] != "" && Request.Form["Email"] != null)
                            {
                                sEmail = Convert.ToString(Request.Form["Email"]);

                                if (!Regex.IsMatch(sEmail, "^[a-zA-Z0-9@._]*$", RegexOptions.Compiled))
                                {
                                    throw new Exception("Invalid Argument!");
                                }
                            }
                            if (Request.Form["Lims"] != "" && Request.Form["Lims"] != null)
                            {
                                sLims = Encoding.ASCII.GetString(Convert.FromBase64String(Request.Form["Lims"]));
                            }
                            if (Request.Form["cardnoh"] != "" && Request.Form["cardnoh"] != null)
                            {
                                sCardNumber = Convert.ToString(Request.Form["cardnoh"]);
                                string showdata = Convert.ToString(Session["ShowSensitive"]);


                                if ((Convert.ToString(Session["ShowSensitive"]) == "1"))
                                {
                                    if (!Regex.IsMatch(sCardNumber, "^[0-9]*$", RegexOptions.Compiled))
                                    {
                                        throw new Exception("Invalid Argument!");
                                    }
                                }
                                else
                                {
                                    if (!Regex.IsMatch(sCardNumber, "^[0-9,*]*$", RegexOptions.Compiled))
                                    {
                                        throw new Exception("Invalid Argument!");
                                    }
                                }


                            }
                            if (Request.Form["ID"] != "" && Request.Form["ID"] != null)
                            {
                                sID = Convert.ToString(Request.Form["ID"]);

                                if (!Regex.IsMatch(sID, "^[0-9,]*$", RegexOptions.Compiled))
                                {
                                    throw new Exception("Invalid Argument!");
                                }
                            }
                            if (Request.Form["Field"] != "" && Request.Form["Field"] != null) sField = Convert.ToString(Request.Form["Field"]);
                            if (Request.Form["RollId"] != "" && Request.Form["RollId"] != null) sRollId = Convert.ToString(Request.Form["RollId"]);
                            if (Request.Form["Data"] != "" && Request.Form["Data"] != null) sData = Convert.ToString(Request.Form["Data"]);
                            if (Request.Form["ShowTop"] != "" && Request.Form["ShowTop"] != null) sTop = Convert.ToString(Request.Form["ShowTop"]);
                            if (Request.Form["SDF"] != "" && Request.Form["SDF"] != null) sSkipDownloadFile = Convert.ToString(Request.Form["SDF"]);
                            if (Request.Form["All"] != "" && Request.Form["All"] != null) sAll = Convert.ToString(Request.Form["All"]);
                            if (Request.Form["File"] != "" && Request.Form["File"] != null) sFile = Convert.ToString(Request.Form["File"]);
                            if (Request.Form["Branch"] != "" && Request.Form["Branch"] != null) sBranch = Convert.ToString(Request.Form["Branch"]);

                            if (sATM == null || sATM == "")
                                sATM = "";
                            else
                                sATM = "'" + sATM.Replace(",", "','") + "'";

                            if (sCards == null || sCards == "")
                                sCards = "";
                            else
                                sCards = "'" + sCards.Replace(",", "','") + "'";

                            if (sMerchants == null || sMerchants == "")
                                sMerchants = "";
                            else
                                sMerchants = "'" + sMerchants.Replace(",", "','") + "'";

                            if (Request.QueryString["ID"] != "" && Request.QueryString["ID"] != null)
                            {
                                sIvrID = Convert.ToString(Request.QueryString["ID"]);

                                if (!Regex.IsMatch(sIvrID, "^[0-9,]*$", RegexOptions.Compiled))
                                {
                                    throw new Exception("Invalid Argument!");
                                }
                            }

                            if (Request.QueryString["Mobile"] != "" && Request.QueryString["Mobile"] != null)
                            {
                                sIvrMobile = Convert.ToString(Request.QueryString["Mobile"]);

                                if (!Regex.IsMatch(sIvrMobile, "^[0-9]*$", RegexOptions.Compiled))
                                {
                                    throw new Exception("Invalid Argument!");
                                }
                            }

                            if (Request.QueryString["Data"] != "" && Request.QueryString["Data"] != null)
                            {
                                if (!Convert.ToString(Request.QueryString["Src"]).Equals("AGSME"))
                                {
                                    sIvrData = Convert.ToString(Request.QueryString["Data"]);

                                    if (!Regex.IsMatch(sIvrData, "^[0-9]*$", RegexOptions.Compiled))
                                    {
                                        throw new Exception("Invalid Argument!");
                                    }
                                }
                                else
                                {
                                    sData = Convert.ToString(Request.QueryString["Data"]);

                                    if (!sData.Equals("CLOSE")) throw new Exception("Invalid Argument!");
                                }
                            }



                            //if (Request.QueryString["Data"] != "" && Request.QueryString["Data"] != null) sIvrData = Convert.ToString(Request.QueryString["Data"]);

                            break;
                    }

                    switch (Convert.ToString(aQueryString[0]))
                    {
                        
                        case "VUSR":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            //added new to encrypt password 20-07-18 sheetal
                            //start
                            RSABuilder ObjRSA = new RSABuilder();
                            string UserPass = ObjRSA.Decrypt(Request.Form["Pass"]);
                            string UserName = ObjRSA.Decrypt(Request.Form["User"]);
                            if (!Regex.IsMatch(UserName, "^[a-zA-Z0-9]*$", RegexOptions.Compiled)) throw new Exception("Invalid User Name!");

                            dtOutput= Generix.validateUser(UserName, UserPass);                            
                            if (dtOutput.Rows[0]["Code"].ToString() == "0")
                            {
                                Response.Write("1");
                            }
                            else
                            {
                                Response.Write(dtOutput.Rows[0]["OutputDescription"].ToString());
                                
                            }

                                //if (Generix.validateUser(UserName, UserPass))
                                //{
                                //    Response.Write("1");
                                //}
                                //end
                                break;

                        case "FORGOTPASS":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            if (!Regex.IsMatch(Convert.ToString(Request.Form["User"]), "^[a-zA-Z0-9]*$", RegexOptions.Compiled)) throw new Exception("Invalid User Name!");
                            String[] sForgotPassDetail = new String[4];

                            String sUserEmail = "", sUserMobile = "", sUserPass = "", sUserName = "", sEmailPass = "";

                            dtOutput = Generix.forgotPassWord(Convert.ToString(Request.Form["User"]));

                            if (dtOutput.Rows.Count > 0)
                            {
                                foreach (DataRow drRow in dtOutput.Rows)
                                {
                                    sUserEmail = Convert.ToString(drRow[0]);
                                    sUserMobile = Convert.ToString(drRow[1]);
                                    sUserPass = Convert.ToString(drRow[2]);
                                    sForgotPassDetail = Convert.ToString(drRow[3]).Split(',');
                                    sUserName = Convert.ToString(drRow[4]);
                                    sEmailPass = Convert.ToString(drRow[5]);

                                    if (sUserEmail != "" && sUserPass != "")
                                    {
                                        using (MailMessage oMessage = new MailMessage(Convert.ToString(sForgotPassDetail[1]), sUserEmail))
                                        {
                                            //oMessage.CC.Add("kishor.ghadi@agsindia.com");
                                            oMessage.IsBodyHtml = true;
                                            oMessage.Body = " Dear " + sUserName + ", <br/><br/> Your New Password is '" + sUserPass + "' <br/><br/>";
                                            oMessage.Subject = "New PassWord";

                                            SmtpClient oSMTP = new SmtpClient(Convert.ToString(sForgotPassDetail[2]), Convert.ToInt32(sForgotPassDetail[3]));
                                            oSMTP.EnableSsl = true;                                     
                                            oSMTP.Credentials = new System.Net.NetworkCredential(Convert.ToString(sForgotPassDetail[0]), sEmailPass);
                                           
                                            try
                                            {
                                                oSMTP.Send(oMessage);

                                            }
                                            catch (Exception smtpEx)
                                            {
                                                ErrorLogger.Log(smtpEx.ToString(), sForgotPassDetail[0].ToString() + "|" + sForgotPassDetail[1].ToString() + "|" + sForgotPassDetail[2].ToString() + "|" + sForgotPassDetail[3].ToString() + "|" + sEmailPass);
                                                ErrorLogger.TextLog(smtpEx, "FORGOTPASS" + "|" + System.Reflection.MethodBase.GetCurrentMethod().Name);
                                                Response.Write("Invalid Email ID !");
                                            }
                                            oSMTP = null;
                                        }
                                    }
                                    else
                                    {
                                        Response.Write("No EmailID Found! Please Update Your Email.");

                                        return;
                                    }
                                    Response.Write("1");
                                }
                            }
                            else
                            {
                                Response.Write("Invalid UserName!");
                            }

                            break;

                        case "CUSW":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            if (Generix.changePassword(Convert.ToString(HttpContext.Current.Session["ActiveUser"]), Convert.ToString(Request.Form["OldPass"]), Convert.ToString(Request.Form["NewPass"])))
                            {
                                Response.Write("1");
                            }

                            break;

                        /*
                            Title           : RealTime Terminal Balance Report
                            Author          : Sagar Goswami
                            Creation Date   : 11 April 2012
                            
                            Modifications   : 1) 10 September 2012 - Modified the data fetch methodology from RT Server from direct connection to linked server model.
                        */
                        case "RTATMBAL":
                            /*
                                                        dtOutput = Generix.getData("realtime.dbo.ssf_media_cassette a ",
                                                        "term_id [Terminal ID], Convert(Numeric(18,0),(Select IsNull(item_value,0)/100 From realtime.dbo.ssf_media_cassette Where term_id=a.term_id And cassette_id In ('F','2'))) [100 Deno], Convert(Numeric(18,0),(Select IsNull(item_value,0)/100 From realtime.dbo.ssf_media_cassette Where term_id=a.term_id And cassette_id In ('G','1'))) [500 Deno], Convert(Numeric(18,0),Sum(IsNull(item_value,0)/100)) [Current Bal] ",
                                                        sATM + " And cassette_id In ('F','G','1','2') ",
                                                        "term_id ",
                                                        "term_id ",
                                                        1);
                            */
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getATMBal(Convert.ToInt32(Session["Bank"]), sATM, false);

                            //if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateDownloadFile(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, true));
                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Report Generated", Convert.ToString(aQueryString[0]), "RealTime");

                            break;

                        case "RTBALBTI":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getATMBal(Convert.ToInt32(Session["Bank"]), sATM, true);

                            if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, true));
                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Report Generated", Convert.ToString(aQueryString[0]), "RealTime");

                            break;


                        case "ATMBAL":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getATMBal(Convert.ToInt32(Session["Bank"]), sATM, dFrom, dTo);

                            //if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateDownloadFile(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, false, true));
                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Report Generated", Convert.ToString(aQueryString[0]), "From:" + Convert.ToString(dFrom) + "|To:" + Convert.ToString(dTo));

                            break;

                        case "BTIATMBAL":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getBTIATMBal(Convert.ToInt32(Session["Bank"]), sATM, dFrom, dTo);

                            if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, false, true));
                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Report Generated", Convert.ToString(aQueryString[0]), "From:" + Convert.ToString(dFrom) + "|To:" + Convert.ToString(dTo));

                            break;

                        case "SWDL":
                        case "SDEP":
                        case "SFTR":
                        case "SREV":
                        case "REVR":
                        case "DECL":
                        case "CSRQ":
                        case "DUMP":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            if (sTop == "") sTop = "-1";
                            dtOutput = Generix.getTransaction(Convert.ToInt32(Session["Bank"]), Convert.ToString(aQueryString[0]), sATM, dFrom, dTo, sSource, sCards, sMerchants, sTranSource, sNode, sOrigin, (Convert.ToString(Session["ShowSensitive"]) == "1"), Convert.ToInt32(sTop));
                            //if (sSkipDownloadFile != "Y") if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateDownloadFile(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            if (sSkipDownloadFile != "Y") if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, true));
                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Report Generated", Convert.ToString(aQueryString[0]), "Terminals:" + sATM + "|From:" + Convert.ToString(dFrom) + "|To:" + Convert.ToString(dTo) + "|Source:" + sSource + "|Cards:" + sCards + "|Merchants:" + sMerchants + "|TranSource:" + sTranSource + "|Node:" + sNode + "|Origin:" + sOrigin);

                            break;

                        case "AEPS":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            if (sTop == "") sTop = "-1";
                            dtOutput = Generix.getAEPSTransaction(Convert.ToInt32(Session["Bank"]), Convert.ToString(aQueryString[0]), sATM, dFrom, dTo, sSource, sCards, sMerchants, sTranSource, sNode, sOrigin, (Convert.ToString(Session["ShowSensitive"]) == "1"), Convert.ToInt32(sTop));
                            //if (sSkipDownloadFile != "Y") if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateDownloadFile(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            if (sSkipDownloadFile != "Y") if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, true));
                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Report Generated", Convert.ToString(aQueryString[0]), "Terminals:" + sATM + "|From:" + Convert.ToString(dFrom) + "|To:" + Convert.ToString(dTo) + "|Source:" + sSource + "|Cards:" + sCards + "|Merchants:" + sMerchants + "|TranSource:" + sTranSource + "|Node:" + sNode + "|Origin:" + sOrigin);

                            break;
                        case "DLBLOG":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            if (sTop == "") sTop = "-1";
                            dtOutput = Generix.getDLBLOGReport(Convert.ToInt32(Session["Bank"]), Convert.ToString(aQueryString[0]), sATM, dFrom, dTo, sSource, sCards, sMerchants, sTranSource, sNode, sOrigin, (Convert.ToString(Session["ShowSensitive"]) == "1"), Convert.ToInt32(sTop));
                            //if (sSkipDownloadFile != "Y") if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateDownloadFile(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            if (sSkipDownloadFile != "Y") if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, true));
                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Report Generated", Convert.ToString(aQueryString[0]), "Terminals:" + sATM + "|From:" + Convert.ToString(dFrom) + "|To:" + Convert.ToString(dTo) + "|Source:" + sSource + "|Cards:" + sCards + "|Merchants:" + sMerchants + "|TranSource:" + sTranSource + "|Node:" + sNode + "|Origin:" + sOrigin);
                            break;

                        case "SSODUMP":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            if (sTop == "") sTop = "-1";
                            dtOutput = Generix.getSSOTransaction(Convert.ToInt32(Session["Bank"]), Convert.ToString(aQueryString[0]), sATM, dFrom, dTo, sSource, sCards, sMerchants, sTranSource, sNode, sOrigin, (Convert.ToString(Session["ShowSensitive"]) == "1"), Convert.ToInt32(sTop));
                            //if (sSkipDownloadFile != "Y") if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateDownloadFile(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            if (sSkipDownloadFile != "Y") if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, true));
                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Report Generated", Convert.ToString(aQueryString[0]), "Terminals:" + sATM + "|From:" + Convert.ToString(dFrom) + "|To:" + Convert.ToString(dTo) + "|Source:" + sSource + "|Cards:" + sCards + "|Merchants:" + sMerchants + "|TranSource:" + sTranSource + "|Node:" + sNode + "|Origin:" + sOrigin);

                            break;

                        case "IVRDUMP":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            if (sTop == "") sTop = "-1";
                            dtOutput = Generix.getSSOIVRTransaction(Convert.ToInt32(Session["Bank"]), Convert.ToString(aQueryString[0]), sATM, dFrom, dTo, sSource, sCards, sMerchants, sTranSource, sNode, sOrigin, (Convert.ToString(Session["ShowSensitive"]) == "1"), Convert.ToInt32(sTop));
                            //if (sSkipDownloadFile != "Y") if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateDownloadFile(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            if (sSkipDownloadFile != "Y") if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, true));
                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Report Generated", Convert.ToString(aQueryString[0]), "Terminals:" + sATM + "|From:" + Convert.ToString(dFrom) + "|To:" + Convert.ToString(dTo) + "|Source:" + sSource + "|Cards:" + sCards + "|Merchants:" + sMerchants + "|TranSource:" + sTranSource + "|Node:" + sNode + "|Origin:" + sOrigin);

                            break;

                        case "APBSDUMP":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            if (sTop == "") sTop = "-1";
                            dtOutput = Generix.getAPBSTransaction(Convert.ToInt32(Session["Bank"]), Convert.ToString(aQueryString[0]), sATM, dFrom, dTo, sSource, sCards, sMerchants, sTranSource, sNode, sOrigin, (Convert.ToString(Session["ShowSensitive"]) == "1"), Convert.ToInt32(sTop));
                            //if (sSkipDownloadFile != "Y") if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateDownloadFile(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            if (sSkipDownloadFile != "Y") if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, true));
                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Report Generated", Convert.ToString(aQueryString[0]), "Terminals:" + sATM + "|From:" + Convert.ToString(dFrom) + "|To:" + Convert.ToString(dTo) + "|Source:" + sSource + "|Cards:" + sCards + "|Merchants:" + sMerchants + "|TranSource:" + sTranSource + "|Node:" + sNode + "|Origin:" + sOrigin);

                            break;

                        case "GPCBR":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getGharPayCashBackTransaction(Convert.ToDateTime(Request.Form["Of"]), Convert.ToDateTime(Request.Form["To"]));
                            if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, false));
                            break;

                        case "PAPDECL":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            if (sTop == "") sTop = "-1";
                            dtOutput = Generix.getPinAtPOSTransaction(Convert.ToInt32(Session["Bank"]), Convert.ToString(aQueryString[0]), sATM, dFrom, dTo, sSource, sCards, sMerchants, sTranSource, sNode, sOrigin, (Convert.ToString(Session["ShowSensitive"]) == "1"), Convert.ToInt32(sTop));
                            //if (sSkipDownloadFile != "Y") if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateDownloadFile(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            if (sSkipDownloadFile != "Y") if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, true));
                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Report Generated", Convert.ToString(aQueryString[0]), "Terminals:" + sATM + "|From:" + Convert.ToString(dFrom) + "|To:" + Convert.ToString(dTo) + "|Source:" + sSource + "|Cards:" + sCards + "|Merchants:" + sMerchants + "|TranSource:" + sTranSource + "|Node:" + sNode + "|Origin:" + sOrigin);

                            break;

                        case "CMR":
                        case "LC":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getTransactionDetail(Convert.ToInt32(Session["Bank"]), Convert.ToString(aQueryString[0]), dFrom, dTo);
                            if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, true));
                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Report Generated", Convert.ToString(aQueryString[0]), "Terminals:" + sATM + "|From:" + Convert.ToString(dFrom) + "|To:" + Convert.ToString(dTo) + "|Source:" + sSource + "|Cards:" + sCards + "|Merchants:" + sMerchants + "|TranSource:" + sTranSource + "|Node:" + sNode + "|Origin:" + sOrigin);

                            break;

                        case "MTXNBTI":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getTransactionDetail_BTI(Convert.ToInt32(Session["Bank"]), Convert.ToString(aQueryString[0]), dFrom, dTo);
                            if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, true));
                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Report Generated", Convert.ToString(aQueryString[0]), "Terminals:" + sATM + "|From:" + Convert.ToString(dFrom) + "|To:" + Convert.ToString(dTo) + "|Source:" + sSource + "|Cards:" + sCards + "|Merchants:" + sMerchants + "|TranSource:" + sTranSource + "|Node:" + sNode + "|Origin:" + sOrigin);

                            break;

                        case "ADMD":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getAdminTranSummary(Convert.ToInt32(Session["Bank"]), false, false, sATM, dFrom, dTo);

                            //if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateDownloadFile(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, true));
                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Report Generated", "ADMD", "Terminals:" + sATM + "|From:" + Convert.ToString(dFrom) + "|To:" + Convert.ToString(dTo) + "|Terminals:" + sATM);

                            break;

                        case "ADAM":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getAdminTranSummary(Convert.ToInt32(Session["Bank"]), false, true, sATM, dFrom, dTo);

                            //if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateDownloadFile(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, true));
                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Report Generated", "ADAM", "Terminals:" + sATM + "|From:" + Convert.ToString(dFrom) + "|To:" + Convert.ToString(dTo) + "|Terminals:" + sATM);

                            break;

                        case "ADAR":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getAdminTranSummary(Convert.ToInt32(Session["Bank"]), true, false, sATM, dFrom, dTo);

                            //if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateDownloadFile(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, true));
                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Report Generated", "ADAR", "Terminals:" + sATM + "|From:" + Convert.ToString(dFrom) + "|To:" + Convert.ToString(dTo) + "|Terminals:" + sATM);

                            break;

                        case "NOPT":
                            /*
                                                        dtOutput = Generix.getData("realtime.dbo.term a With (NoLock) Left Join realtime.dbo.tm_card_acceptor b With (NoLock) On a.card_acceptor=b.card_acceptor ",
                                                        "id [Terminal ID], a.card_acceptor [Terminal Name], name_location [Terminal Loc], LEFT(miscellaneous,CHARINDEX('|',miscellaneous)-1) [Mode], (Select Max(in_req) From realtime.dbo.tm_trans C With (NoLock) Where card_acceptor_term_id=a.id And tran_type='01' And rsp_code_req_rsp='00') [Last Tran On] ",
                                                        "term_active=1 And id Not In (Select Distinct card_acceptor_term_id From realtime.dbo.tm_trans A With (NoLock) Where card_acceptor_term_id Like 'R%' And tran_type='01' And in_req Between DATEADD(HH,-" + sHr + ",GETDATE()) And GETDATE() And Not Exists (Select * From realtime.dbo.tm_trans With (NoLock) Where ret_ref_no=A.ret_ref_no And rsp_code_req_rsp<>'00')) ",
                                                        "", "id", 1);
                            */
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getRTNOPTerminals(Convert.ToInt32(Session["Bank"]), Convert.ToInt32(sHr));

                            //if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateDownloadFile(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, false));
                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Report Generated", Convert.ToString(aQueryString[0]), "RealTime");

                            break;

                        case "DF1C":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            Table oTable;
                            TableRow oTableRow;
                            TableCell oTableCell;

                            var oFiles = Directory.GetFiles(Server.MapPath("/Downloads/CMS")).OrderByDescending(f => new FileInfo(f).CreationTime);

                            oTable = new Table();

                            foreach (var oFiles2 in oFiles)
                            {
                                if (Convert.ToString(Request.Form["Of"]) == "")
                                {
                                    oTableRow = new TableRow();

                                    oTableCell = new TableCell();
                                    oTableCell.Text = "<a href='/Downloads/CMS/" + Path.GetFileName(oFiles2) + "'>" + Path.GetFileName(oFiles2) + "</a>";

                                    oTableRow.Cells.Add(oTableCell);

                                    oTable.Rows.Add(oTableRow);
                                }
                                else
                                {
                                    if (Convert.ToDateTime(Request.Form["Of"]) == Convert.ToDateTime(new FileInfo(oFiles2).CreationTime.ToLongDateString()))
                                    {
                                        oTableRow = new TableRow();

                                        oTableCell = new TableCell();
                                        oTableCell.Text = "<a href='/Downloads/CMS/" + Path.GetFileName(oFiles2) + "'>" + Path.GetFileName(oFiles2) + "</a>";

                                        oTableRow.Cells.Add(oTableCell);

                                        oTable.Rows.Add(oTableRow);
                                    }
                                }
                            }

                            Page.Controls.Add(oTable);

                            break;

                        case "DF2C":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            Table oTable2;
                            TableRow oTableRow2;
                            TableCell oTableCell2;

                            var oFiles3 = Directory.GetFiles(Server.MapPath("/Downloads/" + Convert.ToString(Session["ExtractsFolder"]))).OrderByDescending(f => new FileInfo(f).CreationTime);

                            oTable2 = new Table();

                            foreach (var oFiles4 in oFiles3)
                            {
                                if (Convert.ToString(Request.Form["Of"]) == "")
                                {
                                    oTableRow2 = new TableRow();

                                    oTableCell2 = new TableCell();
                                    oTableCell2.Text = "<a href='/Downloads/" + Convert.ToString(Session["ExtractsFolder"]) + "/" + Path.GetFileName(oFiles4) + "'>" + Path.GetFileName(oFiles4) + "</a>";

                                    oTableRow2.Cells.Add(oTableCell2);

                                    oTable2.Rows.Add(oTableRow2);
                                }
                                else
                                {
                                    if (Convert.ToDateTime(Request.Form["Of"]) == Convert.ToDateTime(new FileInfo(oFiles4).CreationTime.ToLongDateString()))
                                    {
                                        oTableRow2 = new TableRow();

                                        oTableCell2 = new TableCell();
                                        oTableCell2.Text = "<a href='/Downloads/" + Convert.ToString(Session["ExtractsFolder"]) + "/" + Path.GetFileName(oFiles4) + "'>" + Path.GetFileName(oFiles4) + "</a>";

                                        oTableRow2.Cells.Add(oTableCell2);

                                        oTable2.Rows.Add(oTableRow2);
                                    }
                                }
                            }

                            Page.Controls.Add(oTable2);

                            break;

                        case "DF3C":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            Table oTable3;
                            TableRow oTableRow3;
                            TableCell oTableCell3;

                            var oFiles5 = Directory.GetDirectories(Server.MapPath("/Downloads/EJ")).OrderByDescending(f => new FileInfo(f).CreationTime);

                            //var oFiles5 = Directory.GetFiles(Server.MapPath("/Downloads/EJ")).OrderByDescending(f => new FileInfo(f).CreationTime);

                            oTable3 = new Table();

                            foreach (var oFiles6 in oFiles5)
                            {
                                if (Convert.ToString(Request.Form["Of"]) == "")
                                {
                                    oTableRow3 = new TableRow();

                                    oTableCell3 = new TableCell();
                                    oTableCell3.Text = "<a target='_blank' href='/Downloads/EJ/" + Path.GetFileName(oFiles6) + "'>" + Path.GetFileName(oFiles6) + "</a>";

                                    oTableRow3.Cells.Add(oTableCell3);

                                    oTable3.Rows.Add(oTableRow3);
                                }
                                else
                                {
                                    if (Convert.ToDateTime(Request.Form["Of"]) == Convert.ToDateTime(new FileInfo(oFiles6).CreationTime.ToLongDateString()))
                                    {
                                        oTableRow3 = new TableRow();

                                        oTableCell3 = new TableCell();
                                        oTableCell3.Text = "<a target='_blank' href='/Downloads/EJ/" + Path.GetFileName(oFiles6) + "'>" + Path.GetFileName(oFiles6) + "</a>";

                                        oTableRow3.Cells.Add(oTableCell3);

                                        oTable3.Rows.Add(oTableRow3);
                                    }
                                }
                            }

                            Page.Controls.Add(oTable3);

                            break;

                        case "NCRD":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getCardGenerationDetails(Convert.ToString(Session["BINS"]), Convert.ToString(aQueryString[0]), dFrom, dTo, (Convert.ToString(Session["ShowSensitive"]) == "1"));
                            //if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateDownloadFile(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, false));
                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Report Generated", Convert.ToString(aQueryString[0]), "NCRD");

                            break;

                        case "RCRD":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getCardGenerationDetails(Convert.ToString(Session["BINS"]), Convert.ToString(aQueryString[0]), dFrom, dTo, (Convert.ToString(Session["ShowSensitive"]) == "1"));
                            //if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateDownloadFile(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, false));
                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Report Generated", Convert.ToString(aQueryString[0]), "RCRD");

                            break;

                        case "RPIN":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getCardGenerationDetails(Convert.ToString(Session["BINS"]), Convert.ToString(aQueryString[0]), dFrom, dTo, (Convert.ToString(Session["ShowSensitive"]) == "1"));
                            //if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateDownloadFile(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, false));
                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Report Generated", Convert.ToString(aQueryString[0]), "RPIN");

                            break;

                        case "CARD":
                        case "CARDC":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            //sOutput = Generix.executeURL("http://192.168.4.74:8080/portal/getDecRealTimePAN.jsp");
                            //dtOutput = Generix.getData("[192.168.4.70].[postcard].[dbo].[pc_cards_1_B] A with (NoLock) Left Join CardRPAN E On A.pan_encrypted COLLATE Latin1_General_CI_AI  =E.EncPAN COLLATE Latin1_General_CI_AI Left Join [192.168.4.70].[postcard].[dbo].[pc_card_accounts_1_B] B With (NoLock) On A.pan  COLLATE Latin1_General_CI_AI =B.pan COLLATE Latin1_General_CI_AI Left Join [192.168.4.70].[postcard].[dbo].[pc_accounts_1_B] C With (NoLock) On B.account_id COLLATE Latin1_General_CI_AI =C.account_id COLLATE Latin1_General_CI_AI Left Join [192.168.4.70].[postcard].[dbo].[pc_customers_1_B] D With (NoLock) On A.customer_id COLLATE Latin1_General_CI_AI  =D.customer_id COLLATE Latin1_General_CI_AI", "distinct ''''+dbo.ufn_DecryptPAN(DecPAN) [PAN],''''+A.branch_code[Branch_Code],''''+A.customer_id[Cust_ID],date_issued[Issue_Date],date_activated[Activate_Date],card_status[Card_Status],IsNull(A.hold_rsp_code,'') [hold_rsp_code],account_type_qualifier[Acc_Typ_Qualifier],expiry_date[Exp_Date],D.c1_name_on_card[c1_name_on_card],IsNull(D.mobile_nr,'') [Mobile_No],IsNull(D.email_address,'') [Email_ID],date_of_birth[DOB]",
                            //        "account_id_encrypted Is Not Null And card_status<>'6' And account_type_qualifier=1 And date_issued between '" + Convert.ToString(dFrom) + "' And '" + Convert.ToString(dTo)+"'", "", "date_issued",3);
                            dtOutput = Generix.getCardDetails(Convert.ToString(aQueryString[0]), Convert.ToInt32(Session["Bank"]), dFrom, dTo, Convert.ToString(Request.Form["Status"]), sCards);
                            //if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateDownloadFile(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            if (dtOutput.Rows.Count != 0)
                            {
                                Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            }
                            else
                            {
                                Response.Write("No Entries Found !");
                            }
                            //Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, false));
                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Report Generated", Convert.ToString(aQueryString[0]), Convert.ToString(aQueryString[0]));

                            break;

                        case "CFEE":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getCardFees(Convert.ToInt32(Session["Bank"]), dFrom, dTo, (Convert.ToString(Session["ShowSensitive"]) == "1"));
                            if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, false));
                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Report Generated", Convert.ToString(aQueryString[0]), "CFEE");

                            break;

                        /*
                                                case "GERP":
                                                    if (aQueryString[1].ToString() != "")
                                                    {
                                                        DataTable dtCardHolderDetails = Generix.getData("CardRPAN", "EncPAN", "dbo.ufn_DecryptPAN(DecPAN)='" + aQueryString[1].ToString() + "'", "", "", 3);
                                                        if (dtCardHolderDetails != null)
                                                        {
                                                            if (dtCardHolderDetails.Rows.Count > 0)
                                                            {
                                                                sOutput = dtCardHolderDetails.Rows[0][0].ToString();
                                                            }
                                                            else
                                                            {
                                                                throw new Exception("No Such PAN Found!");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            throw new Exception("PAN Retreival Failed!");
                                                        }

                                                        if (sOutput != "")
                                                        {
                                                            if (sOutput.Length > 0)
                                                            {
                                                                if (sOutput.Substring(0, 3) != "01H")
                                                                {
                                                                    throw new Exception("PAN Retreival Failed!");
                                                                }
                                                                else
                                                                {
                                                                    dtCardHolderDetails = Generix.getData("postcard.dbo.pc_cards_1_B A Left Join postcard.dbo.pc_customers_1_B B On A.customer_id=B.customer_id", "c1_name_on_card, Case When IsNull(hold_rsp_code,'00') Not In ('00','55','75') Then 'InActive' Else 'Active' End, mobile_nr, email_address, IsNull(hold_rsp_code,'00') hold_rsp_code", "A.pan_encrypted='" + sOutput + "'", "", "", 1);

                                                                    if (dtCardHolderDetails != null)
                                                                    {
                                                                        if (dtCardHolderDetails.Rows.Count > 0)
                                                                        {
                                                                            string sHoldRspCode = dtCardHolderDetails.Rows[0][4].ToString();
                                                                            sOutput += "|" + dtCardHolderDetails.Rows[0][0].ToString() + "|" + dtCardHolderDetails.Rows[0][1].ToString() + "|" + dtCardHolderDetails.Rows[0][2].ToString() + "|" + dtCardHolderDetails.Rows[0][3].ToString();

                                                                            dtCardHolderDetails = Generix.getData("HotlistCard A Left Join LoginUser B On A.LoginUser=B.Code", "Top 1 B.UserName,A.LoginDate,A.Remarks", "A.PAN='" + sOutput.Split('|')[0].ToString() + "'", "", "LoginDate Desc", 3);

                                                                            if (dtCardHolderDetails != null)
                                                                            {
                                                                                if (dtCardHolderDetails.Rows.Count > 0)
                                                                                {
                                                                                    sOutput += "|" + dtCardHolderDetails.Rows[0][0].ToString() + "|" + dtCardHolderDetails.Rows[0][1].ToString() + "|" + dtCardHolderDetails.Rows[0][2].ToString();
                                                                                }
                                                                                else
                                                                                {
                                                                                    sOutput += "|||";
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                sOutput += "|||";
                                                                            }

                                                                            sOutput += "|" + sHoldRspCode;

                                                                            Response.Write(sOutput);
                                                                        }
                                                                        else
                                                                        {
                                                                            throw new Exception("No Such PAN Found!");
                                                                        }

                                                                        dtCardHolderDetails.Dispose();
                                                                        dtCardHolderDetails = null;
                                                                    }
                                                                    else
                                                                    {
                                                                        throw new Exception("PAN Retreival Failed!");
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                throw new Exception("PAN Retreival Failed!");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            throw new Exception("PAN Retreival Failed!");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        throw new Exception("PAN Missing!");
                                                    }

                                                    break;
                        */

                        case "GERP":
                        case "GERPOA":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            if (aQueryString[1].ToString() != "")
                            {
                                //using (DataTable dtCardDetails = Generix.getCardDetails2(Convert.ToInt32(Session["Bank"]), aQueryString[1].ToString()))
                                //{
                                //    if (dtCardDetails != null)
                                //    {
                                //        if (dtCardDetails.Rows.Count == 1)
                                //        {
                                //            sOutput = "";
                                //            foreach (DataRow drCardDetail in dtCardDetails.Rows)
                                //            {
                                //                foreach (DataColumn dcCardDetail in dtCardDetails.Columns)
                                //                {
                                //                    sOutput += (sOutput == "" ? "" : "|") + drCardDetail[dcCardDetail].ToString();
                                //                }
                                //            }

                                //            Response.Write(sOutput);
                                //        }
                                //        else
                                //        {
                                //            throw new Exception("PAN Retrieval Corrupted!");
                                //        }
                                //    }
                                //    else
                                //    {
                                //        throw new Exception("PAN Retrieval Failed!");
                                //    }
                                //}
                                // new CallCardAPI().Process("GetCardDetailsByCustomerId");
                                using (SqlDataReader sdrCardDetails = Generix.getCardDetails2(Convert.ToInt32(Session["Bank"]), aQueryString[1].ToString(), aQueryString[1].ToString().Equals("GERPOA")))
                                {
                                    if (sdrCardDetails != null)
                                    {
                                        if (!sdrCardDetails.IsClosed)
                                        {
                                            if (sdrCardDetails.HasRows)
                                            {
                                                sOutput = "";

                                                if (aQueryString[0].ToString().Equals("GERP"))
                                                {
                                                    dtOutput = new DataTable();
                                                    dtOutput.Load(sdrCardDetails);

                                                    foreach (DataRow drCardDetail in dtOutput.Rows)
                                                    {
                                                        foreach (DataColumn dcCardDetail in dtOutput.Columns)
                                                        {
                                                            sOutput += (sOutput == "" ? "" : "|") + drCardDetail[dcCardDetail].ToString();
                                                        }
                                                    }

                                                    sOutput += "|";
                                                }

                                                if (sdrCardDetails.HasRows)
                                                {
                                                    dtOutput = new DataTable();
                                                    dtOutput.Load(sdrCardDetails);

                                                    dtOutput.Columns[3].ReadOnly = false;
                                                    dtOutput.Columns[3].MaxLength = 500;

                                                    for (int row = 0; row < dtOutput.Rows.Count; row++)
                                                    {
                                                        dtOutput.Rows[row][3] = "<a href='#' onclick=" + Generix.Chr(34) + "javascript:linkAccount(this,'" + dtOutput.Rows[row][1] + "','" + dtOutput.Rows[row][3] + "');" + Generix.Chr(34) + ">link</a>";
                                                    }

                                                    dtOutput.Columns.RemoveAt(1);

                                                    Table dtTable = Generix.convertDataTable2HTMLTable(dtOutput, false, false);
                                                    using (StringWriter swTable = new StringWriter())
                                                    {
                                                        dtTable.RenderControl(new HtmlTextWriter(swTable));
                                                        sOutput += swTable.ToString();
                                                    }
                                                }
                                                else
                                                    sdrCardDetails.NextResult();

                                                sOutput += "|";
                                                if (sdrCardDetails.HasRows)
                                                {
                                                    dtOutput = new DataTable();
                                                    dtOutput.Load(sdrCardDetails);

                                                    dtOutput.Columns[3].ReadOnly = false;
                                                    dtOutput.Columns[3].MaxLength = 500;
                                                    dtOutput.Columns[4].ReadOnly = false;
                                                    dtOutput.Columns[4].MaxLength = 500;

                                                    for (int row = 0; row < dtOutput.Rows.Count; row++)
                                                    {
                                                        if (dtOutput.Rows[row][3].Equals("Primary"))
                                                            dtOutput.Rows[row][3] = "<a href='#' onclick=" + Generix.Chr(34) + "javascript:makeAccountSecondary(this,'" + dtOutput.Rows[row][1] + "','" + dtOutput.Rows[row][4] + "');" + Generix.Chr(34) + ">" + dtOutput.Rows[row][3] + "</a>";
                                                        else
                                                            dtOutput.Rows[row][3] = "<a href='#' onclick=" + Generix.Chr(34) + "javascript:makeAccountPrimary(this,'" + dtOutput.Rows[row][1] + "','" + dtOutput.Rows[row][4] + "');" + Generix.Chr(34) + ">" + dtOutput.Rows[row][3] + "</a>";

                                                        dtOutput.Rows[row][4] = "<a href='#' onclick=" + Generix.Chr(34) + "javascript:delinkAccount(this,'" + dtOutput.Rows[row][1] + "','" + dtOutput.Rows[row][4] + "');" + Generix.Chr(34) + ">delink</a>";
                                                    }

                                                    dtOutput.Columns.RemoveAt(1);

                                                    Table dtTable = Generix.convertDataTable2HTMLTable(dtOutput, false, false);
                                                    using (StringWriter swTable = new StringWriter())
                                                    {
                                                        dtTable.RenderControl(new HtmlTextWriter(swTable));
                                                        sOutput += swTable.ToString();
                                                    }
                                                }

                                                Response.Write(sOutput);
                                            }
                                            else
                                                throw new Exception("No Such PAN Found!");
                                        }
                                        else
                                            throw new Exception("PAN Retrieval Failed!");
                                    }
                                    else
                                        throw new Exception("PAN Retrieval Failed!");
                                }
                            }
                            else
                            {
                                throw new Exception("PAN Missing!");
                            }

                            break;

                        case "GERPA":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            if (aQueryString[1].ToString() != "")
                            {
                                sOutput = "";

                                using (DataTable dtCardDetails = Generix.getCardsByAccountNo(aQueryString[1].ToString()))
                                {
                                    if (dtCardDetails != null)
                                    {
                                        if (dtCardDetails.Rows.Count > 0)
                                        {
                                            foreach (DataRow drCardDetail in dtCardDetails.Rows)
                                            {
                                                sOutput += (sOutput == "" ? "" : ",") + drCardDetail[0].ToString();
                                            }

                                            Response.Write(sOutput);
                                        }
                                        else
                                        {
                                            throw new Exception("Account No Not Found or is not linked to any Cards!");
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception("PAN Retrieval Failed!");
                                    }
                                }
                            }
                            else
                            {
                                throw new Exception("Account No Missing!");
                            }

                            break;
                        case "AEPSIS":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            String _result = Generix.insertIncomeShareDetails(Convert.ToString(Request.Form["_BCNAME"]), Convert.ToString(Request.Form["_BCAcc"]), Convert.ToString(Request.Form["_Rblper"]), Convert.ToString(Request.Form["_BCper"]), Convert.ToString(Request.Form["_RBLmax"]), Convert.ToString(Request.Form["_RBLMin"]), Convert.ToString(Request.Form["_BI"]), Convert.ToString(Request.Form["_CW"]), Convert.ToString(Request.Form["_CD"]), Convert.ToString(Request.Form["_NpciMax"]), Convert.ToString(Request.Form["_npciMin"]));
                            Response.Write(_result);

                            break;

                        case "GERPC":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            if (aQueryString[1].ToString() != "")
                            {
                                sOutput = "";

                                using (DataTable dtCardDetails = Generix.getCardsByCustomerId(aQueryString[1].ToString()))
                                {
                                    if (dtCardDetails != null)
                                    {
                                        if (dtCardDetails.Rows.Count > 0)
                                        {
                                            foreach (DataRow drCardDetail in dtCardDetails.Rows)
                                            {
                                                sOutput += (sOutput == "" ? "" : ",") + drCardDetail[0].ToString();
                                            }

                                            Response.Write(sOutput);
                                        }
                                        else
                                        {
                                            throw new Exception("Customer Id Not Found or is not linked to any Cards!");
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception("PAN Retrieval Failed!");
                                    }
                                }
                                /*
                                DataTable dtCardDetails = Generix.getData("postcard.dbo.pc_cards_1_B", "pan_encrypted", "customer_id='" + aQueryString[1].ToString() + "'", "", "", 1);
                                sOutput = "";

                                if (dtCardDetails != null)
                                {
                                    if (dtCardDetails.Rows.Count > 0)
                                    {
                                        int iRow = 0;
                                        foreach (DataRow drCardDetail in dtCardDetails.Rows)
                                        {
                                            sOutput += "'" + drCardDetail[0].ToString() + "'";
                                            iRow++;

                                            if (dtCardDetails.Rows.Count != iRow) sOutput += ",";
                                        }

                                        dtCardDetails = Generix.getData("CardRPAN", "dbo.ufn_DecryptPAN(DecPAN)", "EncPAN In (" + sOutput + ")", "", "", 3);
                                        sOutput = "";

                                        if (dtCardDetails != null)
                                        {
                                            iRow = 0;
                                            foreach (DataRow drCardDetail in dtCardDetails.Rows)
                                            {
                                                sOutput += drCardDetail[0].ToString();
                                                iRow++;

                                                if (dtCardDetails.Rows.Count != iRow) sOutput += ",";
                                            }

                                            Response.Write(sOutput);

                                            dtCardDetails.Dispose();
                                            dtCardDetails = null;
                                        }
                                        else
                                        {
                                            throw new Exception("PAN Retrieval Failed!");
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception("No Such Customer ID Found!");
                                    }
                                }
                                else
                                {
                                    throw new Exception("PAN Retrieval Failed!");
                                }
                                */
                            }
                            else
                            {
                                throw new Exception("Customer ID Missing!");
                            }

                            break;

                        case "MACP":
                        case "MACS":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            try
                            {
                                Generix.setAccountStateInRealtimeFormat(Convert.ToInt32(Session["Bank"]), sData.Split('.')[0], sData.Split('.')[1], Convert.ToString(aQueryString[0]).Equals("MACP"));

                                Response.Write("1");
                            }
                            catch (Exception xObj)
                            {
                                throw new Exception("Unexpected Error Occurred!");
                            }

                            break;

                        case "LINKAC":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            break;

                        case "DELINKAC":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }

                            break;

                        case "IVRUCM":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            if (sIvrData.Equals("") || sIvrID.Equals("") || sIvrMobile.Equals(""))
                            //if ((Request.QueryString["Data"] == "" && Request.QueryString["Data"] == null) || (Request.QueryString["ID"] == "" && Request.QueryString["ID"] == null) || (Request.QueryString["Mobile"] == "" && Request.QueryString["Mobile"] == null))
                            {
                                Response.Write("Failed");
                            }
                            else
                            {
                                if (sIvrRemark.Equals("")) sIvrRemark = "IVR: IVR Initiated Modification";

                                if (Generix.setCardDetails(Convert.ToInt32(sIvrData), sIvrID, sIvrMobile, "IVR:" + sIvrRemark, 129))
                                {
                                    Response.Write("Success");
                                }
                                else
                                {
                                    Response.Write("Failed");
                                }
                            }

                            break;


                        case "SAVECD":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            if (sCards != "" && sCustName != "")
                            {
                                using (DataTable dtCardDetails = Generix.getCardDetails(Convert.ToInt32(Session["Bank"]), sCards.Replace("'", "")))
                                {
                                    Boolean bModified = false;
                                    Boolean bCustModified = false;
                                    Boolean bLimModified = false;

                                    if (dtCardDetails.Rows.Count > 0)
                                    {
                                        if (!bModified) bModified = !dtCardDetails.Rows[0]["c1_name_on_card"].ToString().Trim().ToUpper().Equals(sCustName.Trim().ToUpper());
                                        if (!bModified) bModified = !dtCardDetails.Rows[0]["mobile_nr"].ToString().Trim().ToUpper().Equals(sMobile.Trim().ToUpper());
                                        if (!bModified) bModified = !dtCardDetails.Rows[0]["email_address"].ToString().Trim().ToUpper().Equals(sEmail.Trim().ToUpper());

                                        if (bModified)
                                            bCustModified = Generix.setCardDetails(Convert.ToInt32(Session["Bank"]), sCards.Replace("'", ""), sCustName, sEmail, sMobile, Convert.ToInt32(Session["ActiveUserCode"]), Convert.ToString(Request.Form["Remark"]));
                                        else
                                            bCustModified = true;

                                        bModified = false;

                                        String[] aLimits = sLims.Split('|');

                                        if (!bModified) bModified = !Convert.ToInt32(dtCardDetails.Rows[0]["goods_nr_trans_lim"]).Equals(Convert.ToInt32(aLimits[0]));
                                        if (!bModified) bModified = !Convert.ToDouble(dtCardDetails.Rows[0]["goods_lim"]).Equals(Convert.ToDouble(aLimits[1]));
                                        if (!bModified) bModified = !Convert.ToDouble(dtCardDetails.Rows[0]["tran_goods_lim"]).Equals(Convert.ToDouble(aLimits[2]));
                                        if (!bModified) bModified = !Convert.ToDouble(dtCardDetails.Rows[0]["cash_nr_trans_lim"]).Equals(Convert.ToInt32(aLimits[3]));
                                        if (!bModified) bModified = !Convert.ToDouble(dtCardDetails.Rows[0]["cash_lim"]).Equals(Convert.ToDouble(aLimits[4]));
                                        if (!bModified) bModified = !Convert.ToDouble(dtCardDetails.Rows[0]["tran_cash_lim"]).Equals(Convert.ToDouble(aLimits[5]));
                                        if (!bModified) bModified = !Convert.ToDouble(dtCardDetails.Rows[0]["paymnt_nr_trans_lim"]).Equals(Convert.ToInt32(aLimits[6]));
                                        if (!bModified) bModified = !Convert.ToDouble(dtCardDetails.Rows[0]["paymnt_lim"]).Equals(Convert.ToDouble(aLimits[7]));
                                        if (!bModified) bModified = !Convert.ToDouble(dtCardDetails.Rows[0]["tran_paymnt_lim"]).Equals(Convert.ToDouble(aLimits[8]));
                                        if (!bModified) bModified = !Convert.ToDouble(dtCardDetails.Rows[0]["cnp_lim"]).Equals(Convert.ToDouble(aLimits[9]));
                                        if (!bModified) bModified = !Convert.ToDouble(dtCardDetails.Rows[0]["tran_cnp_lim"]).Equals(Convert.ToDouble(aLimits[10]));

                                        if (Convert.ToDouble(aLimits[1]) < Convert.ToDouble(aLimits[2]) || Convert.ToDouble(aLimits[4]) < Convert.ToDouble(aLimits[5]) || Convert.ToDouble(aLimits[7]) < Convert.ToDouble(aLimits[8]) || Convert.ToDouble(aLimits[9]) < Convert.ToDouble(aLimits[10]))
                                        {
                                            Response.Write("Per Transaction cannot be greater than Daily limit!");

                                            break;
                                        }

                                        if (bModified)
                                        {
                                            if (Session["SwitchType"].Equals("EPS"))
                                            {
                                                /*NEW LOGIC FOR API*/
                                                APIMessage ObjAPImsg = new APIMessage();
                                                ObjAPImsg.CardNo = sCards.Replace("'", "");
                                                ObjAPImsg.POSLimitCount = Convert.ToInt32(aLimits[0]);
                                                ObjAPImsg.POSLimit = Convert.ToDouble(aLimits[1]) * 100;
                                                ObjAPImsg.PTPOSLimit = Convert.ToDouble(aLimits[2]) * 100;
                                                ObjAPImsg.ATMLimitCount = Convert.ToInt32(aLimits[3]);
                                                ObjAPImsg.ATMLimit = Convert.ToDouble(aLimits[4]) * 100;
                                                ObjAPImsg.PTATMLimit = Convert.ToDouble(aLimits[5]) * 100;
                                                ObjAPImsg.PaymentsCount = Convert.ToInt32(aLimits[6]);
                                                ObjAPImsg.PaymentsLimit = Convert.ToDouble(aLimits[7]) * 100;
                                                ObjAPImsg.PTPaymentsLimit = Convert.ToDouble(aLimits[8]) * 100;
                                                ObjAPImsg.EComLimit = Convert.ToDouble(aLimits[9]) * 100;
                                                ObjAPImsg.PTEComLimit = Convert.ToDouble(aLimits[10]) * 100;
                                                ObjAPImsg.IssuerNo = Convert.ToString(Session["AcquirerId"]);
                                                string SwitchRsp = new CallCardAPI().Process("SetCardLimit", ObjAPImsg, "");
                                                if (SwitchRsp == "000")
                                                {
                                                    bLimModified = true;
                                                    Generix.InsertCardLimits(Convert.ToInt32(Session["Bank"]), sCards.Replace("'", ""), Convert.ToInt32(aLimits[0]), Convert.ToDouble(aLimits[1]), Convert.ToDouble(aLimits[2]), Convert.ToInt32(aLimits[3]), Convert.ToDouble(aLimits[4]), Convert.ToDouble(aLimits[5]), Convert.ToInt32(aLimits[6]), Convert.ToDouble(aLimits[7]), Convert.ToDouble(aLimits[8]), Convert.ToDouble(aLimits[9]), Convert.ToDouble(aLimits[10]), Convert.ToInt32(Session["ActiveUserCode"]), Convert.ToString(Request.Form["Remark"]));
                                                }
                                                else
                                                {
                                                    bLimModified = false;
                                                    bCustModified = false;

                                                }

                                            }
                                            else
                                            {
                                                Generix.setCardLimits(Convert.ToInt32(Session["Bank"]), sCards.Replace("'", ""), Convert.ToInt32(aLimits[0]), Convert.ToDouble(aLimits[1]), Convert.ToDouble(aLimits[2]), Convert.ToInt32(aLimits[3]), Convert.ToDouble(aLimits[4]), Convert.ToDouble(aLimits[5]), Convert.ToInt32(aLimits[6]), Convert.ToDouble(aLimits[7]), Convert.ToDouble(aLimits[8]), Convert.ToDouble(aLimits[9]), Convert.ToDouble(aLimits[10]), Convert.ToInt32(Session["ActiveUserCode"]), Convert.ToString(Request.Form["Remark"]));
                                            }

                                        }


                                        else
                                            bLimModified = true;

                                        if (bCustModified || bLimModified)
                                        {
                                            Response.Write("1");
                                        }
                                        else
                                        {
                                            Response.Write("Unable to Save Card Details. Error Occurred!");
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception("Unable to retrieve Card Details. Error Occurred!");
                                    }
                                }

                                /*
                                if (Generix.setCardDetails(Convert.ToInt32(Session["Bank"]), sCards.Replace("'", ""), sCustName, sEmail, sMobile, Convert.ToInt32(Session["ActiveUserCode"])))
                                {
                                    Response.Write("1");
                                }
                                else
                                {
                                    throw new Exception("Unable to Save Card Details. Error Occurred!");
                                }
                                */
                            }
                            else
                            {
                                throw new Exception("Parameters Missing!");
                            }

                            break;

                        case "HOTC":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            if (sCards != "" && sHRC != "" && sRemarks != "")
                            {
                                string SourceID = string.Empty;
                                string SwitchRsp = string.Empty;
                                string requesttype = string.Empty;
                                ClscardBlockUnblock ObjBlockUnblock = new ClscardBlockUnblock();
                                APIMessage ObjAPImsg = ObjBlockUnblock.generateBlockUnblockRequets(Convert.ToString(Session["Bank"]), sHRC, sCards.Replace("'", ""), Convert.ToString(Session["AcquirerId"]), Convert.ToString(Session["SwitchType"]), "0");///*Added for international usage RBL-ATPCM-862*/
                                SourceID = ObjBlockUnblock.getsourceId(Convert.ToString(Session["Bank"]), Convert.ToString(Session["SwitchType"]));
                                requesttype = ObjBlockUnblock.getRequestType(Convert.ToString(Session["SwitchType"]), sHRC);
                                SwitchRsp = new CallCardAPI().Process(requesttype, ObjAPImsg, SourceID);
                                if (SwitchRsp.Equals("000"))
                                {
                                    Generix.InsertHostlistRecords(Convert.ToInt32(Session["Bank"]), sCards.Replace("'", ""), sHRC, sRemarks, Convert.ToInt32(Session["ActiveUserCode"]));
                                    Response.Write("1");
                                }
                                else
                                {
                                    string msg = "Unable to Hotlist Card. Error Occurred!";
                                    if (sHRC.Equals("00", StringComparison.OrdinalIgnoreCase))
                                    {
                                        msg = "Unable to Unblock Card. Error Occurred!";
                                    }
                                    throw new Exception(msg);
                                }
                                /*OLD CODE query based*/
                                #region old code with query based
                                /*
                                else
                                {
                                    if (Generix.setHotlistCard(Convert.ToInt32(Session["Bank"]), sCards.Replace("'", ""), sHRC, sRemarks, Convert.ToInt32(Session["ActiveUserCode"])))
                                    {
                                        Response.Write("1");
                                    }
                                    else
                                    {
                                        throw new Exception("Unable to Hotlist Card. Error Occurred!");
                                    }

                                }
                                  */
                                #endregion

                            }
                            else
                            {
                                throw new Exception("Parameters Missing!");
                            }

                            break;

                        case "HOTCR":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getHotlistCard(Convert.ToInt32(Session["Bank"]), dFrom, dTo, (Convert.ToString(Session["ShowSensitive"]) == "1"));

                            //if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateDownloadFile(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, false));
                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Report Generated", "HOTCR", "From:" + Convert.ToString(dFrom) + "|To:" + Convert.ToString(dTo));

                            break;

                        case "RQPIN":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            if (sCards != "")
                            {
                                /*
                                if(Convert.ToString(Session["SwitchType"]).Equals("EPS",StringComparison.OrdinalIgnoreCase))
                                {
                                APIMessage ObjAPImsg = new APIMessage();
                                ObjAPImsg.CardNo = sCards.Replace("'", "");
                                ObjAPImsg.IssuerNo = "1" + Convert.ToString(Session["IssuerNo"]).PadLeft(5, '0');
                                ObjAPImsg.CustomerId = "";
                                ObjAPImsg.AccountNo = "";
                                ObjAPImsg.IssuerNo = Convert.ToString(Session["AcquirerId"]);
                                string SwitchRsp = new CallCardAPI().Process("CardRepin", ObjAPImsg);
                                if (SwitchRsp == "000")
                                    {
 try
                                    {
                                        using (MailMessage oMessage = new MailMessage("s1.alerts@agsindia.com", "biju.k@agsindia.com"))
                                        {                                            
                                            oMessage.CC.Add("Mohammad.gufrankhan@agsindia.com");
                                            oMessage.CC.Add("nilesh.sonawane@agsindia.com");
                                            oMessage.IsBodyHtml = true;
                                            string CardNo = sCards.Substring(0, 8);
                                            oMessage.Body = "Card RePIN Request made for " + CardNo + "!<br/>";
                                            //oMessage.Body = "Card RePIN Request made for " + sCards + "!<br/>";
                                            oMessage.Subject = Session["BankName"].ToString() + "-Card RePIN Notification";

                                            SmtpClient oSMTP = new SmtpClient(System.Configuration.ConfigurationManager.AppSettings["smtpServer"], 25);
                                            oSMTP.Credentials = new System.Net.NetworkCredential("s1.alerts@agsindia.com", "matrix1");

                                            oSMTP.Send(oMessage);
                                            oSMTP = null;
                                        }
                                    }
                                    catch (Exception xObj)
                                    {
                                    }

                                    Response.Write("1");
                                    }
                                    else
                                {
                                    throw new Exception("Unable to process Re-PIN Request. Error Occurred!");
                                }
                                }
                                 * else
                                {
                                 */

                                if (Generix.setRepinRequest(Convert.ToInt32(Session["Bank"]), sCards.Replace("'", ""), sRemarks, Convert.ToInt32(Session["ActiveUserCode"])))
                                {
                                    try
                                    {
                                        using (MailMessage oMessage = new MailMessage("s1.alerts@agsindia.com", "biju.k@agsindia.com"))
                                        {
                                            
                                            oMessage.CC.Add("Mohammad.gufrankhan@agsindia.com");
                                            oMessage.CC.Add("nilesh.sonawane@agsindia.com");
                                            oMessage.IsBodyHtml = true;
                                            string CardNo = sCards.Substring(0, 8);
                                            oMessage.Body = "Card RePIN Request made for " + CardNo + "!<br/>";
                                            //oMessage.Body = "Card RePIN Request made for " + sCards + "!<br/>";
                                            oMessage.Subject = Session["BankName"].ToString() + "-Card RePIN Notification";

                                            SmtpClient oSMTP = new SmtpClient(System.Configuration.ConfigurationManager.AppSettings["smtpServer"], 25);
                                            oSMTP.Credentials = new System.Net.NetworkCredential("s1.alerts@agsindia.com", "matrix1");

                                            oSMTP.Send(oMessage);
                                            oSMTP = null;
                                        }
                                    }
                                    catch (Exception xObj)
                                    {
                                    }

                                    Response.Write("1");
                                }
                                else
                                {
                                    throw new Exception("Unable to process Re-PIN Request. Error Occurred!");
                                }


                            }
                            else
                            {
                                throw new Exception("Parameters Missing!");
                            }

                            break;

                        case "REISU":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            if (sCards != "" && sData != "")
                            {
                                if (Generix.setReissueRequest(Convert.ToInt32(Session["Bank"]), sCards.Replace("'", ""), sData, sRemarks, Convert.ToInt32(Session["ActiveUserCode"])))
                                {
                                    try
                                    {
                                        using (MailMessage oMessage = new MailMessage("s1.alerts@agsindia.com", "biju.k@agsindia.com"))
                                        {
                                                                                        
                                            oMessage.CC.Add("mohammad.gufrankhan@agsindia.com");
                                            oMessage.CC.Add("sachin.parle@agsindia.com");
                                            oMessage.IsBodyHtml = true;
                                            string CardNo = sCards.Substring(0, 8);
                                            oMessage.Body = "Card ReIssue Request made for " + CardNo + "!<br/>";
                                            //oMessage.Body = "Card ReIssue Request made for " + sCards + "!<br/>";
                                            oMessage.Subject = Session["BankName"].ToString() + "-Card ReIssue Notification";

                                            SmtpClient oSMTP = new SmtpClient(System.Configuration.ConfigurationManager.AppSettings["smtpServer"], 25);
                                            oSMTP.Credentials = new System.Net.NetworkCredential("s1.alerts@agsindia.com", "matrix1");

                                            oSMTP.Send(oMessage);
                                            oSMTP = null;
                                        }
                                    }
                                    catch (Exception xObj)
                                    {

                                    }

                                    Response.Write("1");
                                }
                                else
                                {
                                    throw new Exception("Unable to process Re-Issue Request. Error Occurred!");
                                }
                            }
                            else
                            {
                                throw new Exception("Parameters Missing!");
                            }

                            break;

                        case "ADDONCARDREQ":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.insertAddOnRequest(Convert.ToInt32(Session["Bank"]), Convert.ToString(Request.Form["CustID"]), Convert.ToString(Request.Form["PrefName"]), Convert.ToString(Request.Form["accNo"]), Convert.ToInt32(Session["ActiveUserCode"]));
                            Response.Write("1");
                            break;

                        case "IPIN":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            try
                            {
                                if (Generix.processInstaPIN(Convert.ToInt32(Session["Bank"]), sCards.Replace("'", ""), sData, Convert.ToInt32(Session["ActiveUserCode"])))
                                    Response.Write("1");
                                else
                                    Response.Write("0");
                            }
                            catch (Exception xObj)
                            {
                                Response.Write(xObj.Message);
                            }

                            break;

                        case "DBCS":
                        case "DBCSMW":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getDebitCardPOSSpends(Convert.ToInt32(Session["Bank"]), dFrom, dTo, sCards, sOrigin, (Convert.ToString(Session["ShowSensitive"]) == "1"), Convert.ToString(aQueryString[0]));

                            //if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateDownloadFile(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, false));
                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Report Generated", "DBCS", "From:" + Convert.ToString(dFrom) + "|To:" + Convert.ToString(dTo) + "|Cards:" + sCards + "|Origin:" + sOrigin);

                            break;

                        case "TCWB":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getTransactionCount(Convert.ToInt32(Session["Bank"]), dFrom, dTo, (Convert.ToString(Session["ShowSensitive"]) == "1"));

                            //if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateDownloadFile(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, false));
                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Report Generated", "TCWB", "From:" + Convert.ToString(dFrom) + "|To:" + Convert.ToString(dTo));

                            break;

                        case "TCTW":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getTerminalWiseTransactionCount(Convert.ToInt32(Session["Bank"]), sATM, dFrom, dTo, (Convert.ToString(Session["ShowSensitive"]) == "1"));

                            //if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateDownloadFile(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, false));
                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Report Generated", "TCTW", "From:" + Convert.ToString(dFrom) + "|To:" + Convert.ToString(dTo));

                            break;

                        case "TTV": //Terminal Transaction Volume
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getTerminalWiseTransactionSummary(Convert.ToInt32(Session["Bank"]), sATM, dFrom, dTo, (Convert.ToString(Session["ShowSensitive"]) == "1"));

                            //if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateDownloadFile(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, false));
                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Report Generated", "TTV", "From:" + Convert.ToString(dFrom) + "|To:" + Convert.ToString(dTo));

                            break;


                        case "CPRODPRC":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            if (dFrom.Equals(Convert.ToDateTime("01-01-0001"))) dFrom = DateTime.Now;
                            dtOutput = Generix.getCardProdEntries(dFrom, false, (sAll == "Y"), sFile);

                            //if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateDownloadFile(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, false));

                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Card Production Processed Entries View", "CPRODPRC", "");

                            break;

                        case "CPRODREJ":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            if (dFrom.Equals(Convert.ToDateTime("01-01-0001"))) dFrom = DateTime.Now;
                            dtOutput = Generix.getCardProdEntries(dFrom, true, (sAll == "Y"), sFile);

                            if (dtOutput.Rows.Count > 0)
                            {
                                //if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateDownloadFile(dtOutput, Server.MapPath("tempOutputs")) + "|");
                                if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                                Table tblOutput = Generix.convertDataTable2HTMLTable(dtOutput, true, false);
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

                                Page.Controls.Add(tblOutput);
                            }
                            else
                            {
                                Response.Write("ERROR: No Entries Found!");
                            }

                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Card Production Rejected Entries View", "CPRODREJ", "");

                            break;

                        case "CPRODREISU":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            if (dFrom.Equals(Convert.ToDateTime("01-01-0001"))) dFrom = DateTime.Now;
                            dtOutput = Generix.getCardProdReissueEntries(Convert.ToInt32(Session["Bank"]), dFrom, (Convert.ToString(Session["ShowSensitive"]) == "1"), (sAll == "Y"));

                            if (dtOutput.Rows.Count > 0)
                            {
                                //if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateDownloadFile(dtOutput, Server.MapPath("tempOutputs")) + "|");
                                if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                                Table tblOutput = Generix.convertDataTable2HTMLTable(dtOutput, true, false);
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
                                        if (trOutput.Cells[8].Text != "Unprocessed") chkCardProd.Attributes["disabled"] = "disabled";
                                        trOutput.Cells[0].Controls.Add(chkCardProd);

                                        trOutput.Attributes["title"] = "Remarks: " + trOutput.Cells[5].Text; //+ ((trOutput.Cells[5].Text == "Rejected") ? " >> Rejected Reason:" + trOutput.Cells[7].Text : "");
                                    }

                                    trOutput.Cells[5].CssClass = "hideMe";
                                    trOutput.Cells[10].CssClass = "hideMe";
                                }

                                Page.Controls.Add(tblOutput);
                            }
                            else
                            {
                                Response.Write("ERROR: No Entries Found!");
                            }

                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Card Production Reissue Entries View", "CPRODREISU", "");

                            break;

                        case "CPRODPRCSUC":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            if (sData == "N")
                            {
                                if (Generix.processCardProd(sID, false, Convert.ToInt32(Session["ActiveUserCode"])))
                                {
                                    Response.Write("1");
                                }
                                else
                                {
                                    Response.Write("0");
                                }
                            }
                            else
                            {
                                if (Generix.processCardReissue(sID, false, Convert.ToInt32(Session["ActiveUserCode"])))
                                {
                                    Response.Write("1");


                                }
                                else
                                {
                                    Response.Write("0");
                                }
                            }

                            break;

                        case "CPRODPRCREJ":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            if (sData == "N")
                            {
                                if (Generix.processCardProd(sID, true, Convert.ToInt32(Session["ActiveUserCode"])))
                                {
                                    Response.Write("1");
                                }
                                else
                                {
                                    Response.Write("0");
                                }
                            }
                            else
                            {
                                if (Generix.processCardReissue(sID, true, Convert.ToInt32(Session["ActiveUserCode"])))
                                {
                                    Response.Write("1");
                                }
                                else
                                {
                                    Response.Write("0");
                                }
                            }

                            break;

                        case "CPRODUPD":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            if (Generix.updateCardProdField(sID, sField, sData, Session["ActiveUserCode"].ToString()))
                            {
                                Response.Write("1");
                            }
                            else
                            {
                                Response.Write("0");
                            }

                            break;

                        case "CPRODPRCDWNLD":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            if (Session["Bank"].ToString() == "1")
                            {
                                sWaste = Generix.generateCardProdProcessedDownloadFileRBL(Convert.ToInt32(Session["Bank"]), dFrom, false, sFile);
                            }
                            else
                            {
                                sWaste = Generix.generateCardProdProcessedDownloadFile(Convert.ToInt32(Session["Bank"]), dFrom, false, sFile);
                            }


                            Response.Write(sWaste);

                            break;

                        case "CPRODPRCRDWNLD":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            if (Session["Bank"].ToString() == "1")
                            {
                                sWaste = Generix.generateCardProdProcessedDownloadFileRBL(Convert.ToInt32(Session["Bank"]), dFrom, true, sFile);
                            }
                            else
                            {
                                sWaste = Generix.generateCardProdProcessedDownloadFile(Convert.ToInt32(Session["Bank"]), dFrom, true, sFile);
                            }

                            Response.Write(sWaste);

                            break;

                        case "GUFN":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            Literal litObj = new Literal();
                            Generix.fillDropDown(ref litObj, Generix.getData("dbo.CardProduction", "UploadFileName", "Bank=" + Convert.ToString(Session["Bank"]) + " And [Uploaded On]>'" + dFrom.ToString("dd-MMM-yyyy HH:mm") + "' And [Uploaded On]<'" + dFrom.AddDays(1).ToString("dd-MMM-yyyy") + "'", "UploadFileName", "UploadFileName", 3));
                            if (litObj.Text == "")
                                Response.Write("Error: No Files!");
                            else
                                Response.Write(litObj.Text);

                            break;

                        case "TGLM":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            String sTerminalGLMappingReSyncApp = "";

                            try
                            {
                                sTerminalGLMappingReSyncApp = Convert.ToString(Generix.getData("dbo.Banks", "TerminalGLMappingReSyncApp", "Code=" + Convert.ToString(Session["Bank"]), "", "", 3).Rows[0][0]);
                            }
                            catch (Exception xObj)
                            {
                                sTerminalGLMappingReSyncApp = "";
                            }

                            if (sTerminalGLMappingReSyncApp != "")
                            {
                                try
                                {
                                    if (Generix.sendResyncCommand(sTerminalGLMappingReSyncApp.Split(':')[0], Convert.ToInt32(sTerminalGLMappingReSyncApp.Split(':')[1])))
                                    {
                                        //Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "terminalGLMappingSFNotif", "$('divDialog').dialog('close');alert('Terminal-GL Mapping ReSync Done.');", true);
                                        Response.Write("Terminal-GL Mapping ReSync Done.|");
                                    }
                                    else
                                    {
                                        //Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "terminalGLMappingSFNotif", "$('divDialog').dialog('close');alert('Terminal-GL Mapping ReSync Failed.');", true);
                                        Response.Write("Terminal-GL Mapping ReSync Failed.|");
                                    }
                                }
                                catch (Exception xObj)
                                {
                                    Response.Write(xObj.Message + ".|");
                                }
                            }
                            else
                            {
                                //Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "terminalGLMappingSFNotif", "$('divDialog').dialog('close');alert('Terminal-GL Mapping ReSync Failed as App not defined.');", true);
                                Response.Write("Terminal-GL Mapping ReSync Failed as App not defined.|");
                            }

                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(Generix.getTerminalGLMapping(Convert.ToInt32(Session["Bank"])), true, false));

                            break;

                        case "MACC":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getMultipleActiveCardsCustomers(Convert.ToInt32(Session["Bank"]), (Convert.ToString(Session["ShowSensitive"]) == "1"));

                            if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, false));
                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Report Generated", "MACC", "");

                            break;

                        case "ENDF":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            Response.Write("#" + Generix.CreateDownloadCSVFile(Generix.getEnstageData(Convert.ToInt32(Session["Bank"]), dFrom), Server.MapPath("tempOutputs"), false, "", "", ""));
                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Report Generated", "ENDF", "");

                            break;

                        case "CDUU":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getCustomerDataUploadedUpdates(Convert.ToInt32(Session["Bank"]), dFrom, dTo);

                            if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, false));
                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Report Generated", "CDUU", "");

                            break;

                        case "ALSS":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getAccountLinkageSMSSentDetails(Convert.ToInt32(Session["Bank"]), dFrom, dTo);

                            if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, false));
                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Report Generated", "ALSS", "");

                            break;

                        case "SALS":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            Int32 iWaste = 0, iWaste2 = 0;
                            dtOutput = Generix.getData("dbo.CardProduction", "Code, Left([AC ID],2) + 'XXXX' + Right([AC ID],4) [AC ID2], [Mobile phone number], [CIF ID], [Customer Name], [AC ID], AccountLinkageSMSSent, SubString(AccountLinkageSMSGUID, PATINDEX('%GUID=\"%', AccountLinkageSMSGUID)+6, 36)", "Processed=1 And Downloaded=1 And AccountLinkage=1 And AccountLinkageSMSSent=0 And LTrim(RTrim(IsNull([Mobile phone number],'')))!=''", "", "", 3);

                            if (dtOutput.Rows.Count > 0)
                            {
                                foreach (DataRow drReturn in dtOutput.Rows)
                                {
                                    sWaste = "Dear Customer, your new account " + Convert.ToString(drReturn[1]) + " has been linked to your existing Debit Card/s.";

                                    sWaste = Generix.sendSMS(sWaste, Convert.ToString(drReturn[2]));

                                    if (!sWaste.Contains("ERROR"))
                                    {
                                        iWaste++;
                                        drReturn[6] = true;
                                        drReturn[7] = sWaste.Substring(sWaste.IndexOf("GUID=") + 6, 36);
                                    }
                                    else
                                    {
                                        iWaste2++;
                                        drReturn[6] = false;
                                        drReturn[7] = sWaste;
                                    }
                                    Generix.UpdateData("dbo.CardProduction", "AccountLinkageSMSSent=" + (sWaste.Contains("ERROR") ? "0" : "1") + ", AccountLinkageSMSGUID='" + sWaste + "'", "Code=" + Convert.ToString(drReturn[0]), 3);
                                }

                                Response.Write(iWaste.ToString() + " Account Linkage SMS Sent!<br/><br/>While " + iWaste2.ToString() + " SMS failed!|");
                                Response.Write(Generix.CreateDownloadFile(dtOutput, Server.MapPath("tempOutputs")) + "|");

                                dtOutput.Columns.Remove("Code");
                                dtOutput.Columns.Remove("AC ID2");
                                dtOutput.Columns.Remove("Mobile phone number");

                                Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, false));
                            }
                            else
                            {
                                Response.Write("No Pending Account Linkage SMS to be sent!");
                            }
                            break;

                        case "IPINRQ":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            if (!Convert.ToString(Session["Branch"]).Equals(""))
                            {
                                sBranch = Convert.ToString(Session["Branch"]);
                            }

                            if (sBranch.Equals("")) throw new Exception("Branch not specified!");

                            try
                            {
                                using (DataTable dtObj = Generix.getData("dbo.Branch", "Code,Name", "Bank=" + Convert.ToString(Session["Bank"]) + " And BranchCode='" + sBranch + "'", "", "", 3))
                                {
                                    sBranch = Convert.ToString(dtObj.Rows[0][0]);
                                    sBranchName = Convert.ToString(dtObj.Rows[0][1]);
                                }
                            }
                            catch (Exception xObj)
                            {
                                throw new Exception("Unable to retreive Branch details!");
                            }

                            if (Generix.putInstaPINRequest(Convert.ToInt32(Session["Bank"]), Convert.ToInt32(sBranch), Convert.ToInt32(sData), Convert.ToInt32(Session["ActiveUserCode"])))
                            {
                                try
                                {
                                    using (MailMessage oMessage = new MailMessage("s1.alerts@agsindia.com", "biju.k@agsindia.com"))
                                    {
                                        oMessage.CC.Add("mohammad.gufrankhan@agsindia.com");
                                        oMessage.CC.Add("sachin.parle@agsindia.com");

                                        oMessage.IsBodyHtml = true;
                                        oMessage.Body = "Branch: " + sBranchName + "<br/>Nos: " + sData + "<br/>";
                                        oMessage.Subject = Session["BankName"].ToString() + "- InstaPIN Request Notification";

                                        SmtpClient oSMTP = new SmtpClient(System.Configuration.ConfigurationManager.AppSettings["smtpServer"], 25);
                                        oSMTP.Credentials = new System.Net.NetworkCredential("s1.alerts@agsindia.com", "matrix1");

                                        oSMTP.Send(oMessage);
                                        oSMTP = null;
                                    }
                                }
                                catch (Exception xObj)
                                {
                                }

                                Response.Write("1");
                            }
                            else
                                Response.Write("Error while placing InstaPIN Request");

                            break;

                        // ******************************************************************************************************************************************************
                        // GharPay SECTION BEGINS HERE
                        // ******************************************************************************************************************************************************

                        case "GPAYPROCESS":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            DataTable custDetail;
                            //DataTable dtUniqueOutput;
                            //String[] sUrl = null;
                            string smsData = "", executeURL = "", sEmailId = "", previousURL = "", sURLValue = "";
                            string validEmailPattern = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                                                        + "@"
                                                        + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";

                            String[] gharPayIdDetail = Session["gharPayIdDetail"].ToString().Split(',');

                            //dtUniqueOutput = Generix.getData("dbo.GharPay", "EmailURL[EmailURL]", "Reject=0 and Code in (" + Convert.ToString(Request.Form["ID"]) + ")", "", "", 3);

                            //List<string[]> MyStringArrays = new List<string[]>();
                            //foreach (var row in dtUniqueOutput.Rows)//or similar
                            // {
                            //     MyStringArrays.Add(new string[] { row.ToString() });
                            // }

                            //string[] UniqueURL = dtOutput.AsEnumerable().Select(s => s.Field<string>("SMSText")).ToArray<string>();

                            //var uniqueUrl = (from DataColumn x in dtUniqueOutput.Columns select x.ColumnName).ToArray();

                            //var uniqueUrl = (from x in dtUniqueOutput.AsEnumerable() select x).Distinct().ToArray(); //("SMSText").ToString())).ToArray();

                            // var uniqueUrl = (from IDataRecord  r in dtUniqueOutput select Convert.ToString(r["SMSText"])).Distinct().ToArray(); //("SMSText").ToString())).ToArray();

                            dtOutput = Generix.getData("dbo.GharPay", "Code,dbo.ufn_DecryptPAN(CustomerID)[CustID], SMSText[SMSText], EmailURL[EmailURL],Htmlcontent[HtmlContent],DailyRespData[RespData]", "Reject=0 and Code in (" + Convert.ToString(Request.Form["ID"]) + ")", "", "", 3);

                            if (dtOutput.Rows.Count > 0)
                            {
                                foreach (DataRow drReturn in dtOutput.Rows)
                                {

                                    custDetail = Generix.getData("AGSS1RT.postcard.dbo.pc_customers_1_B", "ISNULL(mobile_nr,''),ISNULL(email_address,''),c1_name_on_card", "customer_id='" + Convert.ToString(drReturn[1]) + "'", "", "", 3);

                                    smsData = Convert.ToString(drReturn[2]);

                                    if (Convert.ToString(drReturn[5]) != "2")
                                    {
                                        executeURL = Convert.ToString(drReturn[3]);
                                        sURLValue = Generix.executeURL(executeURL, "", "");
                                    }
                                    else
                                    {
                                        sURLValue = Convert.ToString(drReturn[4]);
                                    }
                                    //previousURL = Convert.ToString(drReturn[3]);

                                    if (custDetail.Rows.Count > 0)
                                    {
                                        foreach (DataRow drcustReturn in custDetail.Rows)
                                        {

                                            if ((Convert.ToString(drcustReturn[0]) != "") && (smsData != ""))
                                            {
                                                smsData = Generix.sendSMS(smsData, Convert.ToString(drcustReturn[0]));

                                                if (!smsData.Contains("ERROR"))
                                                {
                                                    Generix.UpdateData("dbo.GharPay", " process=1,SMSSent=1, SentDate=getDate(),SMSGUID='" + smsData + "'", "Code=" + Convert.ToString(drReturn[0]), 3);
                                                }
                                            }
                                            else
                                            {
                                                Generix.UpdateData("dbo.GharPay", "process=1,SMSSent=0", "Code=" + Convert.ToString(drReturn[0]), 3);
                                            }

                                            sEmailId = Convert.ToString(drcustReturn[1]).Trim();

                                            if (!Regex.IsMatch(sEmailId, validEmailPattern, RegexOptions.Compiled))
                                            {
                                                sEmailId = "";
                                            }

                                            if ((sURLValue != "") && (sEmailId != "") && (!sURLValue.Contains("ERROR")))
                                            {
                                                using (MailMessage oMessage = new MailMessage(gharPayIdDetail[2], Convert.ToString(drcustReturn[1])))
                                                {

                                                    oMessage.IsBodyHtml = true;
                                                    oMessage.Body = " Dear " + Convert.ToString(drcustReturn[2]) + ", <br/><br/>" + sURLValue + " <br/><br/>";
                                                    oMessage.Subject = "RBL Bank Debit CashBack Offer";

                                                    SmtpClient oSMTP = new SmtpClient(gharPayIdDetail[3], Convert.ToInt32(gharPayIdDetail[4]));
                                                    oSMTP.Credentials = new System.Net.NetworkCredential(gharPayIdDetail[0], gharPayIdDetail[1]);
                                                    try
                                                    {
                                                        oSMTP.Send(oMessage);
                                                        Generix.UpdateData("dbo.GharPay", " process=1,EmailSent=1,SentDate=getDate()", "Code=" + Convert.ToString(drReturn[0]), 3);
                                                        sURLValue = "";
                                                    }
                                                    catch (Exception smtpEx)
                                                    {

                                                        Generix.UpdateData("dbo.GharPay", "process=1, Reason='" + smtpEx.InnerException + "'", " Code=" + Convert.ToString(drReturn[0]), 3);
                                                        sURLValue = "";
                                                    }
                                                    oSMTP = null;
                                                }
                                            }
                                            else
                                            {
                                                Generix.UpdateData("dbo.GharPay", " process=1,EmailSent=0,Reason='Invalid Email Address'", "Code=" + Convert.ToString(drReturn[0]), 3);
                                                sURLValue = "";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Generix.UpdateData("dbo.GharPay", "process=1", "Code=" + Convert.ToString(drReturn[0]), 3);
                                    }
                                }
                                Response.Write("1");
                            }
                            else
                            {
                                Response.Write("Process Failed!");
                            }
                            break;
                        case "GPAYREJECT":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            Generix.UpdateData("dbo.GharPay", "process=1,Reject=1,RejectDate=getDate() ", "Code in (" + Convert.ToString(Request.Form["ID"]) + ")", 3);
                            Response.Write("1");
                            break;
                        case "GPAYSHWPRC":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getGharPay(Convert.ToDateTime(Request.Form["Date"]), 0, 1, 0, (Convert.ToString(Session["ShowSensitive"]) == "1"));
                            dtOutput.Columns.RemoveAt(0);
                            if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, false));
                            break;
                        case "GPAYSHWFAIL":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getGharPay(Convert.ToDateTime(Request.Form["Date"]), 1, 0, 0, (Convert.ToString(Session["ShowSensitive"]) == "1"));
                            if (dtOutput.Rows.Count > 0)
                            {
                                Table tblOutput = Generix.convertDataTable2HTMLTable(dtOutput, true, false);
                                System.Web.UI.HtmlControls.HtmlInputCheckBox chkGharpay = null;

                                foreach (TableRow trOutput in tblOutput.Rows)
                                {
                                    if (trOutput.TableSection == TableRowSection.TableBody)
                                    {
                                        chkGharpay = new System.Web.UI.HtmlControls.HtmlInputCheckBox();
                                        chkGharpay.Attributes["type"] = "checkbox";
                                        chkGharpay.Attributes["id"] = "chkBox_" + trOutput.Cells[0].Text;
                                        chkGharpay.Attributes["class"] = "chkGharpay";
                                        chkGharpay.Attributes["value"] = trOutput.Cells[0].Text;
                                        trOutput.Cells[0].Controls.Add(chkGharpay);
                                    }
                                }
                                Page.Controls.Add(tblOutput);
                            }
                            else
                            {
                                Response.Write("ERROR: No Entries Found!");
                            }
                            if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");

                            break;
                        case "GPAYSHOWREJECT":

                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getGharPay(Convert.ToDateTime(Request.Form["Date"]), 1, 0, 0, (Convert.ToString(Session["ShowSensitive"]) == "1"));
                            dtOutput.Columns.RemoveAt(0);
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, false));
                            if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Gharpay Reject ", "GharPay", "");
                            break;
                        case "GPAYSHOW":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getGharPay(Convert.ToDateTime(Request.Form["Date"]), 0, 0, 1, (Convert.ToString(Session["ShowSensitive"]) == "1"));
                            if (dtOutput.Rows.Count > 0)
                            {
                                Table tblOutput = Generix.convertDataTable2HTMLTable(dtOutput, true, false);
                                System.Web.UI.HtmlControls.HtmlInputCheckBox chkGharpay = null;
                                foreach (TableRow trOutput in tblOutput.Rows)
                                {
                                    if (trOutput.TableSection == TableRowSection.TableBody)
                                    {
                                        chkGharpay = new System.Web.UI.HtmlControls.HtmlInputCheckBox();
                                        chkGharpay.Attributes["type"] = "checkbox";
                                        chkGharpay.Attributes["id"] = "chkBox_" + trOutput.Cells[0].Text;
                                        chkGharpay.Attributes["class"] = "chkgharPay";
                                        chkGharpay.Attributes["value"] = trOutput.Cells[0].Text;
                                        trOutput.Cells[0].Controls.Add(chkGharpay);
                                    }
                                }
                                Page.Controls.Add(tblOutput);
                            }
                            else
                            {
                                Response.Write("ERROR: No Entries Found!");
                            }
                            if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Gharpay Processed ", "GharPay", "");
                            break;
                        case "GPAYDUMP":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            if (sTop == "") sTop = "-1";

                            dtOutput = Generix.getTransaction(Convert.ToInt32(Session["Bank"]), Convert.ToString(aQueryString[0]), sATM, dFrom, dTo, sSource, sCards, sMerchants, sTranSource, sNode, sOrigin, (Convert.ToString(Session["ShowSensitive"]) == "1"), Convert.ToInt32(sTop));
                            if (dtOutput.Rows.Count > 0)
                            {
                                foreach (DataRow dtRow in dtOutput.Rows)
                                {
                                    try
                                    {
                                        dtRow["Card No"] = objEnc.GetEncryptAndHash(Convert.ToString(dtRow[5]).Replace("'", ""));
                                        dtRow["From Account"] = objEnc.GetEncryptAndHash(Convert.ToString(dtRow[6]).Replace("'", ""));
                                    }
                                    catch (Exception ex)
                                    {
                                        Response.Write("Error : " + ex.InnerException);
                                    }
                                }
                                //DataTable UniqueRecords = Generix.RemoveDuplicateRows(dtOutput, "RRN");
                                Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                                //string sGHeader = null;
                                //string sGFooter = null;
                                //if (dtOutput.Rows.Count != 0) Response.Write("#" + Generix.CreateDownloadCSVFile(dtOutput, Server.MapPath("tempOutputs"), false, "", sGHeader, sGFooter) + "|");

                                //Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, true));
                            }
                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Report Generated", Convert.ToString(aQueryString[0]), "Terminals:" + sATM + "|From:" + Convert.ToString(dFrom) + "|To:" + Convert.ToString(dTo) + "|Source:" + sSource + "|Cards:" + sCards + "|Merchants:" + sMerchants + "|TranSource:" + sTranSource + "|Node:" + sNode + "|Origin:" + sOrigin);
                            break;

                        case "GPAYCARD":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getCardDetails(Convert.ToString(aQueryString[0]), Convert.ToInt32(Session["Bank"]), dFrom, dTo, Convert.ToString(Request.Form["Status"]), sCards);

                            foreach (DataRow dtRow in dtOutput.Rows)
                            {
                                try
                                {
                                    dtRow["Card No"] = objEnc.GetEncryptAndHash(Convert.ToString(dtRow["Card No"]).Replace("'", ""));
                                    dtRow["Cust ID"] = objEnc.GetEncryptAndHash(Convert.ToString(dtRow["Cust ID"]).Replace("'", ""));
                                    dtRow["Account"] = objEnc.GetEncryptAndHash(Convert.ToString(dtRow["Account"]).Replace("'", ""));
                                }
                                catch (Exception ex)
                                {
                                    Response.Write("Erro : " + ex.InnerException);
                                }
                            }

                            if (dtOutput.Rows.Count != 0)
                            {
                                Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                                //string sGHeader1 = null;
                                //string sGFooter1 = null;
                                //Response.Write("#" + Generix.CreateDownloadCSVFile(dtOutput, Server.MapPath("tempOutputs"), false, "", sGHeader1, sGFooter1) + "|");

                            }
                            else
                            {
                                Response.Write("No Entries Found !");
                            }
                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Report Generated", Convert.ToString(aQueryString[0]), "GPAYCARD");
                            break;

                        case "GPAYTTUM": //GharPay TTUM Upload File
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getGharPayTTUM(Convert.ToString(Session["VISAReconDB"]), Convert.ToString(aQueryString[0]), Convert.ToDateTime(Request.Form["Date"]));
                            string sGpayHeader = null;
                            string sGpayFooter = null;
                            if (dtOutput.Rows.Count != 0) Response.Write("#" + Generix.CreateDownloadCSVFile(dtOutput, Server.MapPath("tempOutputs"), false, "", sGpayHeader, sGpayFooter) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, false));
                            break;

                        // ******************************************************************************************************************************************************
                        // APBS SECTION BEGINS HERE
                        // ******************************************************************************************************************************************************

                        case "APBSSHOW":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getAPBS(Convert.ToDateTime(Request.Form["Date"]), 0, 0, 1);
                            if (dtOutput.Rows.Count > 0)
                            {
                                Table tblOutput = Generix.convertDataTable2HTMLTable(dtOutput, true, false);
                                System.Web.UI.HtmlControls.HtmlInputCheckBox chkApbs = null;
                                foreach (TableRow trOutput in tblOutput.Rows)
                                {
                                    if (trOutput.TableSection == TableRowSection.TableBody)
                                    {
                                        chkApbs = new System.Web.UI.HtmlControls.HtmlInputCheckBox();
                                        chkApbs.Attributes["type"] = "checkbox";
                                        chkApbs.Attributes["id"] = "chkBox_" + trOutput.Cells[0].Text;
                                        chkApbs.Attributes["class"] = "chkApbs";
                                        chkApbs.Attributes["value"] = trOutput.Cells[0].Text;
                                        trOutput.Cells[0].Controls.Add(chkApbs);
                                    }
                                }
                                Page.Controls.Add(tblOutput);
                            }
                            else
                            {
                                Response.Write("ERROR: No Entries Found!");
                            }
                            if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Apbs Processed ", "Apbs", "");
                            break;
                        case "APBSREJECT":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            Generix.UpdateData("dbo.APBSRequest", "Reject=1,RejectDate=getDate() ", "Code in (" + Convert.ToString(Request.Form["ID"]) + ")", 3);
                            Response.Write("1");
                            break;
                        case "APBSPROCESS":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            String sSwitchMesgSpec = "";
                            String response = "", responseCode = "";
                            String[] responseFields = new string[130];
                            String[] sBin = Convert.ToString(HttpContext.Current.Session["BINS"]).Split(',');
                            String[] sPrefix = Convert.ToString(HttpContext.Current.Session["BINPrefixs"]).Split(',');
                            DataTable adhardtOutput = new DataTable();
                            int rowCount = 0;

                            dtOutput = Generix.getData("dbo.APBSRequest", "DestBankIIN[DestBankIIN],DestAccType[DestAccType],dbo.ufn_DecryptPAN(AadhaarNo)[AadhaarNo],Amount[Amount],SponsorBankIIN[SponsorBankIIN],UserNo[UserNo],UserName[UserName],UserCreditRef[UserCreditRef],Code,FollioNo,AccHolderName,Reserved1,Reserved2,Reserved3,fileName", "process=0 and Reject=0 and Code in (" + Convert.ToString(Request.Form["ID"]) + ")", "", "", 3);

                            if (dtOutput.Rows.Count > 0)
                            {
                                adhardtOutput = Generix.getData("dbo.Aadhaar", "dbo.ufn_DecryptPAN(AccountNO),AadhaarNo2,AccountName,cardPrefix", "'000'+AadhaarNo2 In (Select Distinct dbo.ufn_DecryptPAN(AadhaarNo) From APBSRequest Where process=0 and Reject=0 and Code in (" + Convert.ToString(Request.Form["ID"]) + "))", "", "", 3);

                                foreach (DataRow drReturn in dtOutput.Rows)
                                {

                                    //adhardtOutput = Generix.getData("dbo.Aadhaar", "dbo.ufn_DecryptPAN(AccountNO),dbo.ufn_DecryptPAN(AadhaarNo),Bin,Prefix", "'000'+dbo.ufn_DecryptPAN(AadhaarNo) ='" + Convert.ToString(drReturn[2]) + "'", "", "", 3);
                                    //adhardtOutput = Generix.getData("dbo.Aadhaar", "dbo.ufn_DecryptPAN(AccountNO),AadhaarNo2,AccountName,cardPrefix", "'000'+AadhaarNo2 ='" + Convert.ToString(drReturn[2]) + "'", "", "", 3);

                                    DataRow[] adhardrRow = adhardtOutput.Select("'000'+AadhaarNo2='" + Convert.ToString(drReturn[2]) + "'");

                                    if (adhardrRow.Count() > 0)
                                    {
                                        foreach (DataRow accReturn in adhardrRow)
                                        {
                                            //string[] responseFields = new string[130];
                                            String[] ISOFormat = new String[130];
                                            ISOFormat[2] = Convert.ToString(drReturn[0]).Trim() + "0" + Convert.ToString(drReturn[2]).Substring(3).Trim();
                                            ISOFormat[3] = "2100" + Convert.ToString(drReturn[1]).Trim();
                                            ISOFormat[4] = Convert.ToString(drReturn[3]).PadLeft(12, '0').Trim();
                                            ISOFormat[7] = DateTime.Now.ToString("MMddHHmmss");
                                            ISOFormat[11] = DateTime.Now.ToString("HHmmss").Substring(3) + DateTime.Now.Millisecond.ToString();
                                            ISOFormat[12] = DateTime.Now.ToString("HHmmss");
                                            ISOFormat[13] = DateTime.Now.ToString("MMdd");
                                            //ISOFormat[18] = "6012";
                                            //ISOFormat[22] = "011";
                                            ISOFormat[32] = Convert.ToString(drReturn[4]).Trim();
                                            ISOFormat[37] = Convert.ToString(drReturn[7]).Substring(1).Trim();
                                            ISOFormat[41] = Convert.ToString(drReturn[5]).PadRight(8, '0').Trim();
                                            ISOFormat[42] = Convert.ToString(drReturn[5]).PadRight(15, '0').Trim();
                                            ISOFormat[43] = (Convert.ToString(drReturn[6])).Trim();
                                            ISOFormat[59] = "APBS";
                                            //ISOFormat[100] = Convert.ToString(sBin[Convert.ToInt32(accReturn[2])] + sPrefix[Convert.ToInt32(accReturn[3])]).Replace("'", "");
                                            ISOFormat[100] = Convert.ToString(accReturn[3]).Replace("'", "").Trim();
                                            ISOFormat[102] = (Convert.ToString(accReturn[0])).Trim();
                                            ISOFormat[103] = Convert.ToString(Session["ApbsDefaultAcc"]).Trim();
                                            //ISOFormat[103] = "1010010030000018";
                                            //ISOFormat[59] = "RBAPBSSrc";
                                            //ISOFormat[49] = "356";

                                            sSwitchMesgSpec = iso8583.Build(ISOFormat, "0200");
                                            sSwitchMesgSpec = "30323030" + sSwitchMesgSpec.Substring(4, 32) + Generix.ConvertToHex(sSwitchMesgSpec.Substring(36));
                                            sSwitchMesgSpec = sSwitchMesgSpec.Length.ToString("X").PadLeft(4, '0') + sSwitchMesgSpec;
                                            try
                                            {
                                                recbytes = hypercom.SendDataToSwitch(Encoding.ASCII.GetBytes(sSwitchMesgSpec));

                                            }
                                            catch (Exception ex)
                                            {
                                                recbytes = null;
                                            }

                                            if (recbytes != null)
                                            {
                                                response = Generix.TextToHex(recbytes);
                                            }
                                            else
                                            {
                                                Generix.UpdateData("dbo.APBSRequest", "Reject=1,RejectDate=getDate() ", "Code= " + (Convert.ToString(drReturn[8])) + "", 3);
                                            }

                                            //string[] responseFields = new string[128];
                                            if (response != "")
                                            {
                                                responseFields = iso8583.Parse(response.Substring(4), true);
                                            }

                                            Generix.setApbsData(responseFields, Convert.ToString(drReturn[0]), Convert.ToString(drReturn[2]), Convert.ToString(drReturn[3]), Convert.ToString(drReturn[5]), Convert.ToString(drReturn[6]), Convert.ToString(drReturn[10]), Convert.ToString(drReturn[1]), Convert.ToString(drReturn[4]), Convert.ToString(drReturn[7]), Convert.ToString(drReturn[9]), Convert.ToString(drReturn[11]), Convert.ToString(drReturn[12]), Convert.ToString(drReturn[13]), Convert.ToString(accReturn[0]), Convert.ToString(drReturn[14]));


                                            Generix.UpdateData("dbo.APBSRequest", "process=1,SentDate=getDate(),respCode='" + responseFields[39] + "'", "Code= " + (Convert.ToString(drReturn[8])) + "", 3);

                                            ISOFormat = null;
                                            responseFields = null;
                                            response = "";
                                            responseCode = "";
                                        }
                                    }
                                    else
                                    {
                                        Generix.UpdateData("dbo.APBSRequest", "process=1,SentDate=getDate(),respCode='56'", "Code= " + (Convert.ToString(drReturn[8])) + "", 3);

                                        Generix.setApbsData(responseFields, Convert.ToString(drReturn[0]), Convert.ToString(drReturn[2]), Convert.ToString(drReturn[3]), Convert.ToString(drReturn[5]), Convert.ToString(drReturn[6]), Convert.ToString(drReturn[10]), Convert.ToString(drReturn[1]), Convert.ToString(drReturn[4]), Convert.ToString(drReturn[7]), Convert.ToString(drReturn[9]), Convert.ToString(drReturn[11]), Convert.ToString(drReturn[12]), Convert.ToString(drReturn[13]), "", Convert.ToString(drReturn[14]));
                                    }
                                }

                                Response.Write("1");

                            }
                            else
                            {
                                Response.Write("Process Failed!");
                            }
                            break;

                        case "APBSFILELIST":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            Literal apbslitObj = new Literal();
                            Generix.fillDropDown(ref apbslitObj, Generix.getData("dbo.APBSResponse", "fileName", " UploadDate > '" + dFrom.ToString("dd-MMM-yyyy HH:mm") + "' And UploadDate <'" + dFrom.AddDays(1).ToString("dd-MMM-yyyy") + "'", "fileName", "fileName", 3));
                            if (apbslitObj.Text == "")
                                Response.Write("Error: No Files!");
                            else
                                Response.Write(apbslitObj.Text);

                            break;
                        case "APBSRESPFILE":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getApbsRespFile(Convert.ToString(Request.Form["File"]), Convert.ToDateTime(Request.Form["Date"]));
                            string sApbsHeader = null;
                            string sApbsFooter = null;
                            //string sApbsHeader = "33100990SWITCH" + dFrom.ToString("ddMMyyyy") + "SWITCH UPLOAD FILE            M1356";
                            //string sApbsFooter = "9" + dtOutput.Rows.Count.ToString().PadLeft(6, '0');
                            if (dtOutput.Rows.Count != 0) Response.Write("#" + Generix.CreateDownloadCSVFile(dtOutput, Server.MapPath("tempOutputs"), false, "", sApbsHeader, sApbsFooter) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, false));
                            break;
                        case "APBSSHWPRC":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getAPBS(Convert.ToDateTime(Request.Form["Date"]), 0, 1, 0);
                            if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, false));
                            break;

                        case "AADHAAR":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getAadhaarDump(Convert.ToDateTime(Request.Form["Of"]), Convert.ToDateTime(Request.Form["To"]));
                            if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, false));
                            break;

                        case "APBSSHOWREJECT":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getAPBS(Convert.ToDateTime(Request.Form["Date"]), 1, 0, 0);
                            dtOutput.Columns.RemoveAt(0);
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, false));
                            if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Apbs Reject ", "Apbs", "");
                            break;

                        case "UITM":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            if (Generix.putMarkup(Convert.ToInt32(Session["Bank"]), Convert.ToString(Request.Form["Network"]), Convert.ToString(Request.Form["Currency"]).Split('|')[0], Convert.ToString(Request.Form["Currency"]).Split('|')[1], Convert.ToDouble(Request.Form["Base"]), Convert.ToInt32(Session["ActiveUserCode"])))
                                Response.Write("1");
                            else
                                Response.Write("0");

                            break;

                        case "ITMR":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getMarkup(Convert.ToInt32(Session["Bank"]), dFrom, dTo);
                            //if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateDownloadFile(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, true));
                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Report Generated", Convert.ToString(aQueryString[0]), "From:" + Convert.ToString(dFrom) + "|To:" + Convert.ToString(dTo));

                            break;

                        // ******************************************************************************************************************************************************
                        // RECON SECTION BEGINS HERE
                        // ******************************************************************************************************************************************************
                        case "H2HM":
                        case "H2HHUM":
                        case "H2HSUM":
                        case "H2HAUM":
                        case "H2HAREV":
                        case "H2HHREV":
                        case "H2HSREV":
                        case "H2HSD":
                        case "NPCIUM":
                        case "NPCIM":
                        case "NPCISUM":
                        case "VUM":
                        case "VM":
                        case "VSUM":
                        case "CFP":
                        case "LPR":
                        case "MCR":
                        case "EXR":
                        case "H2HFSM":
                        case "H2HFHM":
                        case "H2NFNM":
                        case "H2NFSM":
                        case "H2VFVM":
                        case "H2VFSM":
                        case "RM":
                        case "RUM":
                        case "RSUM":
                        case "LRT":
                        case "VAR":
                        case "VAC":
                        case "NPCIAGN":
                        case "GPAYMCR":
                        case "GPAYAGN":
                        case "VISREV":
                        case "H2AEPSM":
                        case "H2AEPSUM":
                        case "H2GM":
                        case "H2GUM":
                        case "HS2GM":
                        case "HS2GUM":
                        case "H2HMAOF":
                        case "H2HUMAOF":
                        case "NFSSMAEPS":
                        case "NFSSUMAEPS":
                        case "NFSFMAEPS":
                        case "NFSFUMAEPS":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getReconReport(Convert.ToString(Session["VISAReconDB"]), Convert.ToString(aQueryString[0]), dFrom, sTranSource, sOrigin, sSource, (Convert.ToString(Session["ShowSensitive"]) == "1"), Convert.ToString(Session["IssuerNo"]));
                            if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateDownloadFile(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            /*   code is commnet due to excel cretaion fails for record more then 65000  2017-04-24----OLD
                            if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                              */
                            /*Chabge by Gufran khan* 24-04-2017----NEW */
                            //if (dtOutput.Rows.Count != 0) Response.Write( CreateExcelFile.CreateExcelDocument(dtOutput, Server.MapPath("tempOutputs")) + "|");

                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, false));
                            break;

                        case "MASM":
                        case "MUM":
                        case "MASSUM":
                        case "MAGN":
                        case "MLPR":
                        case "MMCR":
                        case "MEXR":
                        case "H2MFMM":
                        case "H2MFSM":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getMasterReconReport(Convert.ToString(Session["VISAReconDB"]), Convert.ToString(aQueryString[0]), dFrom, sTranSource, sOrigin, sSource, (Convert.ToString(Session["ShowSensitive"]) == "1"));
                            if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, false));
                            break;

                        case "MASMISS":
                        case "MUMISS":
                        case "MASSUMISS":
                        case "MAGNISS":
                        case "MLPRISS":
                        case "MMCRISS":
                        case "MEXRISS":
                        case "H2MFMMISS":
                        case "H2MFSMISS":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getMasterIssuingReconReport(Convert.ToString(Session["VISAReconDB"]), Convert.ToString(aQueryString[0]), dFrom, sTranSource, sOrigin, sSource, (Convert.ToString(Session["ShowSensitive"]) == "1"));
                            if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, false));
                            break;



                        case "PACQMTCH":
                        case "PACQUNMTCH":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getPOSACQReconReport(Convert.ToString(Session["VISAReconDB"]), Convert.ToString(aQueryString[0]), dFrom, sTranSource, sOrigin, sSource, sNode, (Convert.ToString(Session["ShowSensitive"]) == "1"));
                            if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, false));
                            break;



                        case "CSHU":
                        case "NPSUM":
                        case "VISUM":
                        case "HUM":
                        case "NUM":
                        case "VUUM":
                        case "MASUM":
                        case "MFUM":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getForceReconReport(Convert.ToString(Session["VISAReconDB"]), Convert.ToString(aQueryString[0]), DateTime.Now, sCardNumber, (Convert.ToString(Session["ShowSensitive"]) == "1"));
                            Table tOutput2 = Generix.convertDataTable2HTMLTable(dtOutput, true, false);
                            System.Web.UI.HtmlControls.HtmlInputCheckBox chkCardno2;
                            foreach (TableRow trOutput in tOutput2.Rows)
                            {
                                if (trOutput.TableSection == TableRowSection.TableBody)
                                {
                                    chkCardno2 = new System.Web.UI.HtmlControls.HtmlInputCheckBox();
                                    chkCardno2.Attributes["type"] = "checkbox";
                                    chkCardno2.Attributes["id"] = "chkbox" + trOutput.Cells[1].Text;
                                    chkCardno2.Attributes["class"] = "Box1";
                                    chkCardno2.Attributes["value"] = trOutput.Cells[0].Text;
                                    trOutput.Cells[0].Controls.Add(chkCardno2);
                                }
                            }

                            Page.Controls.Add(tOutput2);
                            break;

                        case "NPCICHGBK":
                        case "NPCICHGBKR":
                        case "VISACHGBK":
                        case "VISACHGBKR":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            if ((Convert.ToString(aQueryString[0]) == "NPCICHGBKR") || (Convert.ToString(aQueryString[0]) == "VISACHGBKR"))
                            {
                                dtOutput = Generix.getChargeBack(Convert.ToString(Session["VISAReconDB"]), Convert.ToString(aQueryString[0]), DateTime.Now, (Convert.ToString(Session["ShowSensitive"]) == "1"));
                            }
                            else
                            {
                                dtOutput = Generix.getChargeBack(Convert.ToString(Session["VISAReconDB"]), Convert.ToString(aQueryString[0]), Convert.ToDateTime(Request.Form["date"]), (Convert.ToString(Session["ShowSensitive"]) == "1"));
                            }
                            Table tOutput6 = Generix.convertDataTable2HTMLTable(dtOutput, true, false);
                            System.Web.UI.HtmlControls.HtmlInputCheckBox chkCardno3;
                            foreach (TableRow trOutput in tOutput6.Rows)
                            {
                                if (trOutput.TableSection == TableRowSection.TableBody)
                                {
                                    chkCardno3 = new System.Web.UI.HtmlControls.HtmlInputCheckBox();
                                    chkCardno3.Attributes["type"] = "checkbox";
                                    chkCardno3.Attributes["id"] = "chkbox" + trOutput.Cells[1].Text;
                                    chkCardno3.Attributes["class"] = "Box1";
                                    chkCardno3.Attributes["value"] = trOutput.Cells[0].Text;
                                    trOutput.Cells[0].Controls.Add(chkCardno3);
                                }
                            }

                            Page.Controls.Add(tOutput6);
                            break;
                        case "GSHU":
                        case "NSUM":
                        case "SNUM":
                        case "SVUM":
                        case "AGNFORCE":
                        case "NPCIAGNFORCE":
                        case "GPAYAGNFORCE":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getReconReport(Convert.ToString(Session["VISAReconDB"]), Convert.ToString(aQueryString[0]), DateTime.Now, sTranSource, sOrigin, sSource, (Convert.ToString(Session["ShowSensitive"]) == "1"), Convert.ToString(Session["IssuerNo"]));
                            Table tOutput = Generix.convertDataTable2HTMLTable(dtOutput, true, false);
                            // System.Web.UI.HtmlControls.HtmlInputCheckBox chkCardno;
                            foreach (TableRow trOutput in tOutput.Rows)
                            {
                                if (trOutput.TableSection == TableRowSection.TableBody)
                                {
                                    chkCardno = new System.Web.UI.HtmlControls.HtmlInputCheckBox();
                                    chkCardno.Attributes["type"] = "checkbox";
                                    chkCardno.Attributes["id"] = "chkbox" + trOutput.Cells[1].Text;
                                    chkCardno.Attributes["class"] = "Box1";
                                    chkCardno.Attributes["value"] = trOutput.Cells[0].Text;
                                    trOutput.Cells[0].Controls.Add(chkCardno);
                                }
                            }

                            Page.Controls.Add(tOutput);

                            break;
                        case "AGNFORCEATMPOS":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            string SourceTerm = Convert.ToString(Request.Form["atm"]) + Convert.ToString(Request.Form["pos"]);
                            string Origin = Convert.ToString(Request.Form["dom"]) + Convert.ToString(Request.Form["inter"]);
                            dtOutput = Generix.getReconReport(Convert.ToString(Session["VISAReconDB"]), Convert.ToString(aQueryString[0]), DateTime.Now, SourceTerm, Origin, sSource, (Convert.ToString(Session["ShowSensitive"]) == "1"), Convert.ToString(Session["IssuerNo"]));
                            Table ageingtOutput = Generix.convertDataTable2HTMLTable(dtOutput, true, false);

                            foreach (TableRow trOutput in ageingtOutput.Rows)
                            {
                                if (trOutput.TableSection == TableRowSection.TableBody)
                                {
                                    chkCardno = new System.Web.UI.HtmlControls.HtmlInputCheckBox();
                                    chkCardno.Attributes["type"] = "checkbox";
                                    chkCardno.Attributes["id"] = "chkbox" + trOutput.Cells[1].Text;
                                    chkCardno.Attributes["class"] = "Box1";
                                    chkCardno.Attributes["value"] = trOutput.Cells[0].Text;
                                    trOutput.Cells[0].Controls.Add(chkCardno);
                                }
                            }

                            Page.Controls.Add(ageingtOutput);

                            break;
                        case "GPAYMCRFORCEMATCH":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getReconReport(Convert.ToString(Session["VISAReconDB"]), Convert.ToString(aQueryString[0]), DateTime.Now, "00", "00", sSource, (Convert.ToString(Session["ShowSensitive"]) == "1"), Convert.ToString(Session["IssuerNo"]));
                            Table merchandisedtOutput = Generix.convertDataTable2HTMLTable(dtOutput, true, false);

                            foreach (TableRow trOutput in merchandisedtOutput.Rows)
                            {
                                if (trOutput.TableSection == TableRowSection.TableBody)
                                {
                                    chkCardno = new System.Web.UI.HtmlControls.HtmlInputCheckBox();
                                    chkCardno.Attributes["type"] = "checkbox";
                                    chkCardno.Attributes["id"] = "chkbox" + trOutput.Cells[1].Text;
                                    chkCardno.Attributes["class"] = "Box1";
                                    chkCardno.Attributes["value"] = trOutput.Cells[0].Text;
                                    trOutput.Cells[0].Controls.Add(chkCardno);
                                }
                            }

                            Page.Controls.Add(merchandisedtOutput);

                            break;
                        case "GRHU":
                        case "VUMFORCE":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getReconReport(Convert.ToString(Session["VISAReconDB"]), Convert.ToString(aQueryString[0]), DateTime.Now, sTranSource, sOrigin, sSource, (Convert.ToString(Session["ShowSensitive"]) == "1"), Convert.ToString(Session["IssuerNo"]));
                            Table tOutput1 = Generix.convertDataTable2HTMLTable(dtOutput, true, false);
                            System.Web.UI.HtmlControls.HtmlInputCheckBox chkCardno1;
                            foreach (TableRow trOutput in tOutput1.Rows)
                            {
                                if (trOutput.TableSection == TableRowSection.TableBody)
                                {
                                    chkCardno1 = new System.Web.UI.HtmlControls.HtmlInputCheckBox();
                                    chkCardno1.Attributes["type"] = "checkbox";
                                    chkCardno1.Attributes["id"] = "chkbox" + trOutput.Cells[1].Text;
                                    chkCardno1.Attributes["class"] = "Box1";
                                    chkCardno1.Attributes["value"] = trOutput.Cells[0].Text;
                                    trOutput.Cells[0].Controls.Add(chkCardno1);
                                }
                            }
                            Page.Controls.Add(tOutput1);
                            break;

                        case "VUF": //Visa Difference Upload File
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getVisaUploadFile(Convert.ToString(Session["VISAReconDB"]), Convert.ToString(aQueryString[0]), dFrom);
                            string sTempHeader = null;
                            string sTempFooter = null;
                            if (dtOutput.Rows.Count != 0) Response.Write("#" + Generix.CreateDownloadCSVFile(dtOutput, Server.MapPath("tempOutputs"), false, "", sTempHeader, sTempFooter) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, false));
                            break;

                        case "MUF": //Master Difference Upload File
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getMasterUploadFile(Convert.ToString(Session["VISAReconDB"]), Convert.ToString(aQueryString[0]), dFrom);
                            string sTempMasterHeader = null;
                            string sTempMasterFooter = null;
                            if (dtOutput.Rows.Count != 0) Response.Write("#" + Generix.CreateDownloadCSVFile(dtOutput, Server.MapPath("tempOutputs"), false, "", sTempMasterHeader, sTempMasterFooter) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, false));
                            break;

                        case "MCTTUM": // Merchandised Credit Upload File
                            /*
                            dtOutput = Generix.getMerchantCreditUploadFile(Convert.ToString(Session["VISAReconDB"]), Convert.ToString(aQueryString[0]), dFrom);
                            string sMCTHeader = null;
                            string sMCTFooter = null;
                            if (dtOutput.Rows.Count != 0) Response.Write("#" + Generix.CreateDownloadCSVFile(dtOutput, Server.MapPath("tempOutputs"), false, "", sMCTHeader, sMCTFooter) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, false));
                            */
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getMerchantCreditUploadFile(Convert.ToString(Session["VISAReconDB"]), Convert.ToString(aQueryString[0]), dFrom);

                            if (dtOutput != null)
                            {
                                if (dtOutput.Rows.Count != 0)
                                {
                                    DataTable dtCurrencies = dtOutput.DefaultView.ToTable(true, "CurrencyCode");
                                    DataTable dtCurrencyOutput;
                                    String sFiles = "", sFile2 = "";

                                    foreach (DataRow drCurrency in dtCurrencies.Rows)
                                    {
                                        dtCurrencyOutput = dtOutput.Select("CurrencyCode='" + drCurrency[0].ToString() + "'").CopyToDataTable();

                                        sFile2 = Generix.CreateDownloadCSVFile(dtOutput, Server.MapPath("tempOutputs"), false, "", "", "");
                                        File.Move(Server.MapPath("tempOutputs") + "/" + sFile2, Server.MapPath("tempOutputs") + "/mcttum_" + drCurrency[0].ToString() + "_" + sFile2 + ".txt");

                                        sFiles += (sFiles.Length > 0 ? "," : "") + Server.MapPath("tempOutputs") + "/mcttum_" + drCurrency[0].ToString() + "_" + sFile2 + ".txt";
                                    }

                                    if (sFiles.Length != 0)
                                    {
                                        sFile2 = "ttum_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + "_" + System.IO.Path.GetRandomFileName().Replace(".", "") + ".rar";

                                        if (Generix.compress(Server.MapPath("tempOutputs") + "/" + sFile2, Server.MapPath("tempOutputs") + "/mcttum_*.txt", ""))
                                            Response.Write("!" + sFile2);

                                        Response.Write("|");
                                    }

                                    Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, false));
                                }
                            }

                            break;

                        case "BCTTUM": // BC Credit Upload File for AEPSOFFUS
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            DataSet __dsOutput = Generix.getBCUploadFile(Convert.ToString(Session["VISAReconDB"]), Convert.ToString(aQueryString[0]), dFrom);
                            String _BCFiles = "", _BCfile = "";
                            DataTable __mergeResult = new DataTable();
                            for (int i = 0; i < __dsOutput.Tables.Count; i++)
                            {

                                DataTable __dt = __dsOutput.Tables[i];


                                __mergeResult.Merge(__dt);
                                __mergeResult.AcceptChanges();
                                if (__dt != null)
                                {
                                    if (__dt.Rows.Count != 0)
                                    {
                                        DataTable dtCurrencies = __dt.DefaultView.ToTable(true, "CurrencyCode");
                                        DataTable dtCurrencyOutput;


                                        foreach (DataRow drCurrency in dtCurrencies.Rows)
                                        {
                                            dtCurrencyOutput = __dt.Select("CurrencyCode='" + drCurrency[0].ToString() + "'").CopyToDataTable();

                                            _BCfile = Generix.CreateDownloadCSVFile(__dt, Server.MapPath("tempOutputs"), false, "", "", "");
                                            File.Move(Server.MapPath("tempOutputs") + "/" + _BCfile, Server.MapPath("tempOutputs") + "/BCTTUM_" + drCurrency[0].ToString() + "_" + _BCfile + ".txt");

                                            _BCFiles += (_BCFiles.Length > 0 ? "," : "") + Server.MapPath("tempOutputs") + "/BCTTUM_" + drCurrency[0].ToString() + "_" + _BCfile + ".txt";
                                        }
                                    }
                                }

                            }
                            if (_BCFiles.Length != 0)
                            {
                                _BCfile = "ttum_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + "_" + System.IO.Path.GetRandomFileName().Replace(".", "") + ".rar";

                                if (Generix.compress(Server.MapPath("tempOutputs") + "/" + _BCfile, Server.MapPath("tempOutputs") + "/BCTTUM_*.txt", ""))
                                    Response.Write("!" + _BCfile);

                                Response.Write("|");
                            }

                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(__mergeResult, true, false));
                            break;
                        case "IncomeTTUM": // AEPS Income Sharing  Credit Upload File
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            __dsOutput = Generix.getIncomeSharingUploadFile(Convert.ToString(Session["VISAReconDB"]), Convert.ToString(aQueryString[0]), dFrom);
                            // String _BCFiles = "", _BCfile = "";
                            _BCFiles = "";
                            __mergeResult = new DataTable();
                            DataTable __dtoutput = __dsOutput.Tables[0];
                            __dtoutput.Columns.Remove("SrNo");
                            DataTable __DtReport = __dsOutput.Tables[1];
                            //for (int i = 0; i < __dsOutput.Tables.Count; i++)
                            //{

                            //    DataTable __dt = __dsOutput.Tables[i];


                            //    __mergeResult.Merge(__dt);
                            //    __mergeResult.AcceptChanges();
                            if (__dtoutput != null)
                            {
                                if (__dtoutput.Rows.Count != 0)
                                {
                                    DataTable dtCurrencies = __dtoutput.DefaultView.ToTable(true, "CurrencyCode");
                                    DataTable dtCurrencyOutput;


                                    foreach (DataRow drCurrency in dtCurrencies.Rows)
                                    {
                                        dtCurrencyOutput = __dtoutput.Select("CurrencyCode='" + drCurrency[0].ToString() + "'").CopyToDataTable();
                                        _BCfile = Generix.CreateExcel(__DtReport, Server.MapPath("tempOutputs"));
                                        File.Move(Server.MapPath("tempOutputs") + "/" + _BCfile, Server.MapPath("tempOutputs") + "/INCOMESharingReport_" + drCurrency[0].ToString() + "_" + _BCfile + ".xls");
                                        _BCfile = Generix.CreateDownloadCSVFile(__dtoutput, Server.MapPath("tempOutputs"), false, "", "", "");
                                        File.Move(Server.MapPath("tempOutputs") + "/" + _BCfile, Server.MapPath("tempOutputs") + "/INCOMESharingTTUM_" + drCurrency[0].ToString() + "_" + _BCfile + ".txt");

                                        _BCFiles += (_BCFiles.Length > 0 ? "," : "") + Server.MapPath("tempOutputs") + "/INCOMESharingTTUM_" + drCurrency[0].ToString() + "_" + _BCfile + ".txt";
                                    }
                                }
                            }


                            if (_BCFiles.Length != 0)
                            {
                                _BCfile = "ttum_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + "_" + System.IO.Path.GetRandomFileName().Replace(".", "") + ".rar";

                                if (Generix.compress(Server.MapPath("tempOutputs") + "/" + _BCfile, Server.MapPath("tempOutputs") + "/INCOMESharing*", ""))
                                    Response.Write("!" + _BCfile);

                                Response.Write("|");
                            }

                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(__DtReport, true, false));
                            break;

                        case "SMUM":
                        case "MUMFORCE":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = dtOutput = Generix.getMasterReconReport(Convert.ToString(Session["VISAReconDB"]), Convert.ToString(aQueryString[0]), DateTime.Now, sTranSource, sOrigin, sSource, (Convert.ToString(Session["ShowSensitive"]) == "1"));
                            tOutput1 = Generix.convertDataTable2HTMLTable(dtOutput, true, false);

                            foreach (TableRow trOutput in tOutput1.Rows)
                            {
                                if (trOutput.TableSection == TableRowSection.TableBody)
                                {
                                    chkCardno = new System.Web.UI.HtmlControls.HtmlInputCheckBox();
                                    chkCardno.Attributes["type"] = "checkbox";
                                    chkCardno.Attributes["id"] = "chkbox" + trOutput.Cells[1].Text;
                                    chkCardno.Attributes["class"] = "Box1";
                                    chkCardno.Attributes["value"] = trOutput.Cells[0].Text;
                                    trOutput.Cells[0].Controls.Add(chkCardno);
                                }
                            }
                            Page.Controls.Add(tOutput1);
                            break;

                        case "H2HS":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = new DataTable();
                            DataTable dtop = new DataTable();
                            dtOutput.Columns.Add("Host to Host Summary");
                            dtOutput.Columns.Add("ATM Tran \nCount");
                            dtOutput.Columns.Add("ATM Tran \nAmount");
                            dtOutput.Columns.Add("ATM NPCI Tran \nCount");
                            dtOutput.Columns.Add("ATM NPCI Tran \nAmount");
                            dtOutput.Columns.Add("ATM acq Tran \nCount");
                            dtOutput.Columns.Add("ATM acq Tran \nAmount");
                            dtOutput.Columns.Add("POS VISA Iss Tran \nCount");
                            dtOutput.Columns.Add("POS VISA Iss Tran \nAmount");
                            dtOutput.Columns.Add("ATM VISA Iss Tran \nCount");
                            dtOutput.Columns.Add("ATM VISA Iss Tran \nAmount");
                            dtOutput.Columns.Add("POS VISA Acq Tran \nCount");
                            dtOutput.Columns.Add("POS VISA Acq Tran \nAmount");
                            dtOutput.Columns.Add("ATM VISA Acq Tran \nCount");
                            dtOutput.Columns.Add("ATM VISA Acq Tran \nAmount");
                            dtOutput.Columns.Add("ATM Rupay Iss Tran \nCount");
                            dtOutput.Columns.Add("ATM Rupay IssTran \nAmount");
                            dtOutput.Columns.Add("POS Rupay Iss Tran \nCount");
                            dtOutput.Columns.Add("POS Rupay Iss Tran \nAmount");

                            DataTable dtOutput1 = Generix.getReconSummary(Convert.ToString(Session["VISAReconDB"]), Convert.ToString(aQueryString[0]), dFrom, sTranSource, sOrigin, sSource, (Convert.ToString(Session["ShowSensitive"]) == "1"), Convert.ToString(Session["IssuerNo"]));
                            dtOutput.Rows.Add("'Reconciled", dtOutput1.Rows[0][0].ToString(), dtOutput1.Rows[0][1].ToString(), dtOutput1.Rows[1][0].ToString(), dtOutput1.Rows[1][1].ToString(), dtOutput1.Rows[2][0].ToString(), dtOutput1.Rows[2][1].ToString(), dtOutput1.Rows[3][0].ToString(), dtOutput1.Rows[3][1].ToString(), dtOutput1.Rows[4][0].ToString(), dtOutput1.Rows[4][1].ToString(), dtOutput1.Rows[5][0].ToString(), dtOutput1.Rows[5][1].ToString(), dtOutput1.Rows[6][0].ToString(), dtOutput1.Rows[6][1].ToString(), dtOutput1.Rows[7][0].ToString(), dtOutput1.Rows[7][1].ToString(), dtOutput1.Rows[8][0].ToString(), dtOutput1.Rows[8][1].ToString());
                            dtOutput.Rows.Add("'Unreconciled Switch", dtOutput1.Rows[9][0].ToString(), dtOutput1.Rows[9][1].ToString(), dtOutput1.Rows[10][0].ToString(), dtOutput1.Rows[10][1].ToString(), dtOutput1.Rows[11][0].ToString(), dtOutput1.Rows[11][1].ToString(), dtOutput1.Rows[12][0].ToString(), dtOutput1.Rows[12][1].ToString(), dtOutput1.Rows[13][0].ToString(), dtOutput1.Rows[13][1].ToString(), dtOutput1.Rows[14][0].ToString(), dtOutput1.Rows[14][1].ToString(), dtOutput1.Rows[15][0].ToString(), dtOutput1.Rows[15][1].ToString(), dtOutput1.Rows[16][0].ToString(), dtOutput1.Rows[16][1].ToString(), dtOutput1.Rows[17][0].ToString(), dtOutput1.Rows[17][1].ToString());
                            dtOutput.Rows.Add("'Unreconciled Host", dtOutput1.Rows[18][0].ToString(), dtOutput1.Rows[18][1].ToString(), dtOutput1.Rows[19][0].ToString(), dtOutput1.Rows[19][1].ToString(), dtOutput1.Rows[20][0].ToString(), dtOutput1.Rows[20][1].ToString(), dtOutput1.Rows[21][0].ToString(), dtOutput1.Rows[21][1].ToString(), dtOutput1.Rows[22][0].ToString(), dtOutput1.Rows[22][1].ToString(), dtOutput1.Rows[23][0].ToString(), dtOutput1.Rows[23][1].ToString(), dtOutput1.Rows[24][0].ToString(), dtOutput1.Rows[24][1].ToString(), dtOutput1.Rows[25][0].ToString(), dtOutput1.Rows[25][1].ToString(), dtOutput1.Rows[26][0].ToString(), dtOutput1.Rows[26][1].ToString());
                            dtOutput.Rows.Add("Late Reversal", dtOutput1.Rows[36][0].ToString(), dtOutput1.Rows[36][1].ToString(), dtOutput1.Rows[37][0].ToString(), dtOutput1.Rows[37][1].ToString(), dtOutput1.Rows[38][0].ToString(), dtOutput1.Rows[38][1].ToString(), dtOutput1.Rows[39][0].ToString(), dtOutput1.Rows[39][1].ToString(), dtOutput1.Rows[40][0].ToString(), dtOutput1.Rows[40][1].ToString(), dtOutput1.Rows[41][0].ToString(), dtOutput1.Rows[41][1].ToString(), dtOutput1.Rows[42][0].ToString(), dtOutput1.Rows[42][1].ToString(), dtOutput1.Rows[43][0].ToString(), dtOutput1.Rows[43][1].ToString(), dtOutput1.Rows[44][0].ToString(), dtOutput1.Rows[44][1].ToString());
                            //dtOutput.Rows.Add("Late Reversal", dtOutput1.Rows[27][0].ToString(), dtOutput1.Rows[27][1].ToString(), dtOutput1.Rows[28][0].ToString(), dtOutput1.Rows[28][1].ToString(), dtOutput1.Rows[29][0].ToString(), dtOutput1.Rows[29][1].ToString(), dtOutput1.Rows[30][0].ToString(), dtOutput1.Rows[30][1].ToString(), dtOutput1.Rows[31][0].ToString(), dtOutput1.Rows[31][1].ToString(), dtOutput1.Rows[32][0].ToString(), dtOutput1.Rows[32][1].ToString(), dtOutput1.Rows[33][0].ToString(), dtOutput1.Rows[33][1].ToString(), dtOutput1.Rows[34][0].ToString(), dtOutput1.Rows[34][1].ToString(), dtOutput1.Rows[35][0].ToString(), dtOutput1.Rows[35][1].ToString());

                            //dtOutput.Rows.Add("Total", dtOutput1.Rows[27][0].ToString(), dtOutput1.Rows[27][1].ToString(), dtOutput1.Rows[28][0].ToString(), dtOutput1.Rows[28][1].ToString(), dtOutput1.Rows[29][0].ToString(), dtOutput1.Rows[29][1].ToString(), dtOutput1.Rows[30][0].ToString(), dtOutput1.Rows[30][1].ToString(), dtOutput1.Rows[31][0].ToString(), dtOutput1.Rows[31][1].ToString(), dtOutput1.Rows[32][0].ToString(), dtOutput1.Rows[32][1].ToString(), dtOutput1.Rows[33][0].ToString(), dtOutput1.Rows[33][1].ToString(), dtOutput1.Rows[34][0].ToString(), dtOutput1.Rows[34][1].ToString(), dtOutput1.Rows[35][0].ToString(), dtOutput1.Rows[35][1].ToString());
                            dtOutput.Rows.Add("Total", (double.Parse(dtOutput1.Rows[0][0].ToString()) + (dtOutput1.Rows[9][0].ToString()) + (dtOutput1.Rows[18][0].ToString()) + (dtOutput1.Rows[27][0].ToString())), dtOutput1.Rows[27][1].ToString(), dtOutput1.Rows[28][0].ToString(), dtOutput1.Rows[28][1].ToString(), dtOutput1.Rows[29][0].ToString(), dtOutput1.Rows[29][1].ToString(), dtOutput1.Rows[30][0].ToString(), dtOutput1.Rows[30][1].ToString(), dtOutput1.Rows[31][0].ToString(), dtOutput1.Rows[31][1].ToString(), dtOutput1.Rows[32][0].ToString(), dtOutput1.Rows[32][1].ToString(), dtOutput1.Rows[33][0].ToString(), dtOutput1.Rows[33][1].ToString(), dtOutput1.Rows[34][0].ToString(), dtOutput1.Rows[34][1].ToString(), dtOutput1.Rows[35][0].ToString(), dtOutput1.Rows[35][1].ToString());

                            if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs"), true) + "|");
                            dtOutput.Rows.Remove(dtOutput.Rows[dtOutput.Rows.Count - 1]);
                            dtOutput.Rows.Add("<b>Total</b>", "<b>" + dtOutput1.Rows[27][0].ToString() + "</b>", "<b>" + dtOutput1.Rows[27][1].ToString() + "</b>", "<b>" + dtOutput1.Rows[28][0].ToString() + "</b>", "<b>" + dtOutput1.Rows[28][1].ToString() + "</b>", "<b>" + dtOutput1.Rows[29][0].ToString() + "</b>", "<b>" + dtOutput1.Rows[29][1].ToString() + "</b>", "<b>" + dtOutput1.Rows[30][0].ToString() + "</b>", "<b>" + dtOutput1.Rows[30][1].ToString() + "</b>", "<b>" + dtOutput1.Rows[31][0].ToString() + "</b>", "<b>" + dtOutput1.Rows[31][1] + "</b>", "<b>" + dtOutput1.Rows[32][0] + "</b>", "<b>" + dtOutput1.Rows[32][1] + "</b>", "<b>" + dtOutput1.Rows[33][0] + "</b>", "<b>" + dtOutput1.Rows[33][1] + "</b>", "<b>" + dtOutput1.Rows[34][0] + "</b>", "<b>" + dtOutput1.Rows[34][1] + "</b>", "<b>" + dtOutput1.Rows[35][0] + "</b>", "<b>" + dtOutput1.Rows[35][1] + "</b>");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, false));
                            break;

                        case "H2NS":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = new DataTable();
                            dtOutput.Columns.Add("NFS Summary");
                            dtOutput.Columns.Add("Issuing ATM Tran Count");
                            dtOutput.Columns.Add("Issuing ATM Amount");
                            dtOutput.Columns.Add("Acquiring ATM Tran Count");
                            dtOutput.Columns.Add("Acquiring ATM Amount");
                            dtOutput1 = Generix.getReconSummary(Convert.ToString(Session["VISAReconDB"]), Convert.ToString(aQueryString[0]), dFrom, sTranSource, sOrigin, sSource, (Convert.ToString(Session["ShowSensitive"]) == "1"), Convert.ToString(Session["IssuerNo"]));
                            dtOutput.Rows.Add("'Reconciled", dtOutput1.Rows[0][0].ToString(), dtOutput1.Rows[0][1].ToString(), dtOutput1.Rows[1][0].ToString(), dtOutput1.Rows[1][1].ToString());
                            dtOutput.Rows.Add("'Unreconciled_Switch ", dtOutput1.Rows[2][0].ToString(), dtOutput1.Rows[2][1].ToString(), dtOutput1.Rows[3][0].ToString(), dtOutput1.Rows[3][1].ToString());
                            dtOutput.Rows.Add("'Unreconciled_NFS", dtOutput1.Rows[4][0].ToString(), dtOutput1.Rows[4][1].ToString(), dtOutput1.Rows[5][0].ToString(), dtOutput1.Rows[5][1].ToString());
                            dtOutput.Rows.Add("Total", dtOutput1.Rows[6][0].ToString(), dtOutput1.Rows[6][1].ToString(), dtOutput1.Rows[7][0].ToString(), dtOutput1.Rows[7][1].ToString());
                            if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs"), true) + "|");
                            dtOutput.Rows.Remove(dtOutput.Rows[dtOutput.Rows.Count - 1]);
                            dtOutput.Rows.Add("<b>Total</b>", "<b>" + dtOutput1.Rows[6][0].ToString() + "</b>", "<b>" + dtOutput1.Rows[6][1].ToString() + "</b>", "<b>" + dtOutput1.Rows[7][0].ToString() + "</b>", "<b>" + dtOutput1.Rows[7][1].ToString() + "</b>");
                            //if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateDownloadFile(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, false));
                            break;

                        case "H2VS":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = new DataTable();
                            dtOutput.Columns.Add("VISA Summary");
                            dtOutput.Columns.Add("Issuing ATM \nCount");
                            dtOutput.Columns.Add("Issuing ATM \nAmount");
                            dtOutput.Columns.Add("Issuing POS \nCount");
                            dtOutput.Columns.Add("Issuing POS \nAmount");
                            dtOutput.Columns.Add("Acquiring ATM \nCount");
                            dtOutput.Columns.Add("Acquiring ATM \nAmount");
                            dtOutput.Columns.Add("Acquiring POS \nCount");
                            dtOutput.Columns.Add("Acquiring POS \nAmount");
                            dtOutput1 = Generix.getReconSummary(Convert.ToString(Session["VISAReconDB"]), Convert.ToString(aQueryString[0]), dFrom, sTranSource, sOrigin, sSource, (Convert.ToString(Session["ShowSensitive"]) == "1"), Convert.ToString(Session["IssuerNo"]));

                            dtOutput.Rows.Add("'Reconciled", dtOutput1.Rows[0][0].ToString(), dtOutput1.Rows[0][1].ToString(), dtOutput1.Rows[1][0].ToString(), dtOutput1.Rows[1][1].ToString(), dtOutput1.Rows[2][0].ToString(), dtOutput1.Rows[2][1].ToString(), dtOutput1.Rows[3][0].ToString(), dtOutput1.Rows[3][1].ToString());
                            dtOutput.Rows.Add("'Unreconciled_Switch", dtOutput1.Rows[4][0].ToString(), dtOutput1.Rows[4][1].ToString(), dtOutput1.Rows[5][0].ToString(), dtOutput1.Rows[5][1].ToString(), dtOutput1.Rows[6][0].ToString(), dtOutput1.Rows[6][1].ToString(), dtOutput1.Rows[7][0].ToString(), dtOutput1.Rows[7][1].ToString());
                            dtOutput.Rows.Add("'Unreconciled_Visa", dtOutput1.Rows[8][0].ToString(), dtOutput1.Rows[8][1].ToString(), dtOutput1.Rows[9][0].ToString(), dtOutput1.Rows[9][1].ToString(), dtOutput1.Rows[10][0].ToString(), dtOutput1.Rows[10][1].ToString(), dtOutput1.Rows[11][0].ToString(), dtOutput1.Rows[11][1].ToString());
                            dtOutput.Rows.Add("Total", dtOutput1.Rows[12][0].ToString(), dtOutput1.Rows[12][1].ToString(), dtOutput1.Rows[13][0].ToString(), dtOutput1.Rows[13][1].ToString(), dtOutput1.Rows[14][0].ToString(), dtOutput1.Rows[14][1].ToString(), dtOutput1.Rows[15][0].ToString(), dtOutput1.Rows[15][1].ToString());
                            if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs"), true) + "|");
                            dtOutput.Rows.Remove(dtOutput.Rows[dtOutput.Rows.Count - 1]);
                            dtOutput.Rows.Add("<b>Total</b>", "<b>" + dtOutput1.Rows[12][0].ToString() + "</b>", "<b>" + dtOutput1.Rows[12][1].ToString() + "</b>", "<b>" + dtOutput1.Rows[13][0].ToString() + "</b>", "<b>" + dtOutput1.Rows[13][1].ToString() + "</b>", "<b>" + dtOutput1.Rows[14][0].ToString() + "</b>", "<b>" + dtOutput1.Rows[14][1].ToString() + "</b>", "<b>" + dtOutput1.Rows[15][0].ToString() + "</b>", "<b>" + dtOutput1.Rows[15][1].ToString() + "</b>");
                            //if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateDownloadFile(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, false));
                            break;

                        case "MASMR":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = new DataTable();
                            dtOutput.Columns.Add("Master Summary");
                            dtOutput.Columns.Add("Issuing ATM \nCount");
                            dtOutput.Columns.Add("Issuing ATM \nAmount");
                            dtOutput.Columns.Add("Issuing POS \nCount");
                            dtOutput.Columns.Add("Issuing POS \nAmount");
                            dtOutput.Columns.Add("Acquiring ATM \nCount");
                            dtOutput.Columns.Add("Acquiring ATM \nAmount");
                            dtOutput.Columns.Add("Acquiring POS \nCount");
                            dtOutput.Columns.Add("Acquiring POS \nAmount");
                            dtOutput1 = Generix.getMasterReconSummary(Convert.ToString(Session["VISAReconDB"]), Convert.ToString(aQueryString[0]), dFrom, sTranSource, sOrigin, sSource, (Convert.ToString(Session["ShowSensitive"]) == "1"), Convert.ToString(Session["IssuerNo"]));

                            dtOutput.Rows.Add("'Reconciled", dtOutput1.Rows[0][0].ToString(), dtOutput1.Rows[0][1].ToString(), dtOutput1.Rows[1][0].ToString(), dtOutput1.Rows[1][1].ToString(), dtOutput1.Rows[2][0].ToString(), dtOutput1.Rows[2][1].ToString(), dtOutput1.Rows[3][0].ToString(), dtOutput1.Rows[3][1].ToString());
                            dtOutput.Rows.Add("'Unreconciled_Switch", dtOutput1.Rows[4][0].ToString(), dtOutput1.Rows[4][1].ToString(), dtOutput1.Rows[5][0].ToString(), dtOutput1.Rows[5][1].ToString(), dtOutput1.Rows[6][0].ToString(), dtOutput1.Rows[6][1].ToString(), dtOutput1.Rows[7][0].ToString(), dtOutput1.Rows[7][1].ToString());
                            dtOutput.Rows.Add("'Unreconciled_Master", dtOutput1.Rows[8][0].ToString(), dtOutput1.Rows[8][1].ToString(), dtOutput1.Rows[9][0].ToString(), dtOutput1.Rows[9][1].ToString(), dtOutput1.Rows[10][0].ToString(), dtOutput1.Rows[10][1].ToString(), dtOutput1.Rows[11][0].ToString(), dtOutput1.Rows[11][1].ToString());
                            dtOutput.Rows.Add("Total", dtOutput1.Rows[12][0].ToString(), dtOutput1.Rows[12][1].ToString(), dtOutput1.Rows[13][0].ToString(), dtOutput1.Rows[13][1].ToString(), dtOutput1.Rows[14][0].ToString(), dtOutput1.Rows[14][1].ToString(), dtOutput1.Rows[15][0].ToString(), dtOutput1.Rows[15][1].ToString());
                            if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs"), true) + "|");
                            dtOutput.Rows.Remove(dtOutput.Rows[dtOutput.Rows.Count - 1]);
                            dtOutput.Rows.Add("<b>Total</b>", "<b>" + dtOutput1.Rows[12][0].ToString() + "</b>", "<b>" + dtOutput1.Rows[12][1].ToString() + "</b>", "<b>" + dtOutput1.Rows[13][0].ToString() + "</b>", "<b>" + dtOutput1.Rows[13][1].ToString() + "</b>", "<b>" + dtOutput1.Rows[14][0].ToString() + "</b>", "<b>" + dtOutput1.Rows[14][1].ToString() + "</b>", "<b>" + dtOutput1.Rows[15][0].ToString() + "</b>", "<b>" + dtOutput1.Rows[15][1].ToString() + "</b>");
                            //if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateDownloadFile(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, false));
                            break;

                        case "H2RS":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = new DataTable();
                            dtOutput.Columns.Add("Rupay Summary");
                            dtOutput.Columns.Add("Issuing ATM \nCount");
                            dtOutput.Columns.Add("Issuing ATM \nAmount");
                            dtOutput.Columns.Add("Issuing POS \nCount");
                            dtOutput.Columns.Add("Issuing POS \nAmount");
                            dtOutput.Columns.Add("Acquiring ATM \nCount");
                            dtOutput.Columns.Add("Acquiring ATM \nAmount");
                            dtOutput1 = Generix.getReconSummary(Convert.ToString(Session["VISAReconDB"]), Convert.ToString(aQueryString[0]), dFrom, sTranSource, sOrigin, sSource, (Convert.ToString(Session["ShowSensitive"]) == "1"), Convert.ToString(Session["IssuerNo"]));
                            dtOutput.Rows.Add("'Reconciled", dtOutput1.Rows[0][0].ToString(), dtOutput1.Rows[0][1].ToString(), dtOutput1.Rows[1][0].ToString(), dtOutput1.Rows[1][1].ToString(), dtOutput1.Rows[2][0].ToString(), dtOutput1.Rows[2][1].ToString());
                            dtOutput.Rows.Add("'Unreconciled_Switch", dtOutput1.Rows[3][0].ToString(), dtOutput1.Rows[3][1].ToString(), dtOutput1.Rows[4][0].ToString(), dtOutput1.Rows[4][1].ToString(), dtOutput1.Rows[5][0].ToString(), dtOutput1.Rows[5][1].ToString());
                            dtOutput.Rows.Add("'Unreconciled_Rupay", dtOutput1.Rows[6][0].ToString(), dtOutput1.Rows[6][1].ToString(), dtOutput1.Rows[7][0].ToString(), dtOutput1.Rows[7][1].ToString(), dtOutput1.Rows[8][0].ToString(), dtOutput1.Rows[8][1].ToString());
                            dtOutput.Rows.Add("Total", dtOutput1.Rows[9][0].ToString(), dtOutput1.Rows[9][1].ToString(), dtOutput1.Rows[10][0].ToString(), dtOutput1.Rows[10][1].ToString(), dtOutput1.Rows[11][0].ToString(), dtOutput1.Rows[11][1].ToString());
                            if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs"), true) + "|");
                            dtOutput.Rows.Remove(dtOutput.Rows[dtOutput.Rows.Count - 1]);
                            dtOutput.Rows.Add("<b>Total</b>", "<b>" + dtOutput1.Rows[9][0].ToString() + "</b>", "<b>" + dtOutput1.Rows[9][1].ToString() + "</b>", "<b>" + dtOutput1.Rows[10][0].ToString() + "</b>", "<b>" + dtOutput1.Rows[10][1].ToString() + "</b>", "<b>" + dtOutput1.Rows[11][0].ToString() + "</b>", "<b>" + dtOutput1.Rows[11][1].ToString() + "</b>");
                            //if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateDownloadFile(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, false));
                            break;

                        case "HSM": // H2H Host Stand-in Matching
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            string issuer = Convert.ToString(Session["IssuerNo"]);
                            if (issuer == "1")
                            {
                                success = Generix.UpdateData(Convert.ToString(Session["VISAReconDB"]) + ".dbo.Finacle", "host_flag=1,process_date=GetDate(),force_switch_flag=2,forced_user='" + Convert.ToString(Session["ActiveUser"]) + "'", "ID in(" + Convert.ToString(Request.Form["hostidh"]) + ")", 3);
                                Response.Write(success);
                            }
                            else if (issuer == "2")
                            {
                                success = Generix.UpdateData(Convert.ToString(Session["VISAReconDB"]) + ".dbo.cb_host", "host_flag=1,process_date=GetDate(),force_switch_flag=2,forced_user='" + Convert.ToString(Session["ActiveUser"]) + "'", "host_identity in(" + Convert.ToString(Request.Form["hostidh"]) + ")", 3);
                                Response.Write(success);
                            }
                            else
                            {
                                success = Generix.UpdateData(Convert.ToString(Session["VISAReconDB"]) + ".dbo.cb_host_acq", "host_flag=1,process_date=GetDate(),force_switch_flag=2,forced_user='" + Convert.ToString(Session["ActiveUser"]) + "'", "host_identity in(" + Convert.ToString(Request.Form["hostidh"]) + ")", 3);
                                Response.Write(success);
                            }

                            break;

                        case "SSM": // H2H Switch Stand-in Matching
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            success = Generix.UpdateData(Convert.ToString(Session["VISAReconDB"]) + ".dbo.switch", "switch_flag=1,process_date=GetDate(),force_host_flag=2,forced_user='" + Convert.ToString(Session["ActiveUser"]) + "'", "switch_identity in(" + Convert.ToString(Request.Form["switchidh"]) + ")", 3);
                            Response.Write(success);
                            break;

                        case "SUF": // Switch Upload File
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getSwitchUploadFile(Convert.ToString(Session["VISAReconDB"]), Convert.ToString(Request.Form["switchidh"]));
                            string sSwitchHeader = null;
                            string sSwitchFooter = null;
                            if (dtOutput.Rows.Count != 0)
                            {
                                Response.Write("#" + Generix.CreateDownloadCSVFile(dtOutput, Server.MapPath("tempOutputs"), false, "", sSwitchHeader, sSwitchFooter));
                                success = Generix.UpdateData(Convert.ToString(Session["VISAReconDB"]) + ".dbo.switch", "switch_flag=1,process_date=GetDate(),force_host_flag=2,forced_user='" + Convert.ToString(Session["ActiveUser"]) + "'", "switch_identity in(" + Convert.ToString(Request.Form["switchidh"]) + ")", 3);
                            }
                            break;

                        case "SUF1": // Switch Upload File
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getSwitchUploadFile(Convert.ToString(Session["VISAReconDB"]), Convert.ToString(Request.Form["switchidh"]));
                            string sSwitchHeader1 = null;
                            string sSwitchFooter1 = null;
                            if (dtOutput.Rows.Count != 0)
                            {
                                Response.Write("#" + Generix.CreateDownloadCSVFile(dtOutput, Server.MapPath("tempOutputs"), false, "", sSwitchHeader1, sSwitchFooter1));
                                success = Generix.UpdateData(Convert.ToString(Session["VISAReconDB"]) + ".dbo.switch", "npci_flag=1,force_npci_flag=2,npci_process_date=GetDate(),forced_user='" + Convert.ToString(Session["ActiveUser"]) + "'", "switch_identity in (" + Convert.ToString(Request.Form["switchidh"]) + ")", 3);
                            }
                            break;
                        case "SUF2": // Switch Visa Force Match Upload File
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getSwitchUploadFile(Convert.ToString(Session["VISAReconDB"]), Convert.ToString(Request.Form["switchidh"]));
                            string sSwitchHeader2 = null;
                            string sSwitchFooter2 = null;
                            // string sSwitchHeader2 = "00100990SWITCH" + dFrom.ToString("ddMMyyyy") + "SWITCH UPLOAD FILE            M1356";
                            //string sSwitchFooter2 = "9" + dtOutput.Rows.Count.ToString().PadLeft(6, '0');
                            if (dtOutput.Rows.Count != 0)
                            {
                                Response.Write("#" + Generix.CreateDownloadCSVFile(dtOutput, Server.MapPath("tempOutputs"), false, "", sSwitchHeader2, sSwitchFooter2));
                                success = Generix.UpdateData(Convert.ToString(Session["VISAReconDB"]) + ".dbo.switch", "visa_flag=1,force_visa_flag=2,visa_process_date=GetDate(),forced_user='" + Convert.ToString(Session["ActiveUser"]) + "'", "switch_identity in (" + Convert.ToString(Request.Form["switchidh"]) + ")", 3);
                            }
                            break;

                        case "SMUF": // Switch Master Force Match Upload File
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getswitchmasterUploadFile(Convert.ToString(Session["VISAReconDB"]), Convert.ToString(Request.Form["switchidh"]));
                            string sSwitchHeader3 = null;
                            string sSwitchFooter3 = null;

                            if (dtOutput.Rows.Count != 0)
                            {
                                Response.Write("#" + Generix.CreateDownloadCSVFile(dtOutput, Server.MapPath("tempOutputs"), false, "", sSwitchHeader3, sSwitchFooter3));
                                success = Generix.UpdateData(Convert.ToString(Session["VISAReconDB"]) + ".dbo.switch", "master_flag=1,force_master_flag=2,master_process_date=GetDate(),forced_user='" + Convert.ToString(Session["ActiveUser"]) + "'", "switch_identity in (" + Convert.ToString(Request.Form["switchidh"]) + ")", 3);
                            }
                            break;

                        case "AGNUP": // Visa Ageing Upload File
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getAgeingUploadFile(Convert.ToString(Session["VISAReconDB"]), Convert.ToString(Request.Form["agnIDh"]));
                            string sAGNHeader = null;
                            string sAGNFooter = null;
                            if (dtOutput.Rows.Count != 0)
                            {
                                Response.Write("#" + Generix.CreateDownloadCSVFile(dtOutput, Server.MapPath("tempOutputs"), false, "", sAGNHeader, sAGNFooter));
                                success = Generix.UpdateData(Convert.ToString(Session["VISAReconDB"]) + ".dbo.switch", "visa_flag=1,force_visa_flag=2,visa_process_date=GetDate(),forced_user='" + Convert.ToString(Session["ActiveUser"]) + "'", "switch_identity in (" + Convert.ToString(Request.Form["agnIDh"]) + ")", 3);
                            }
                            break;
                        case "NPCIAGNUP": // Npci Ageing Upload File
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getNpciAgeingUploadFile(Convert.ToString(Session["VISAReconDB"]), Convert.ToString(Request.Form["agnIDh"]));
                            string sNPCIAGNHeader = null;
                            string sNPCIAGNFooter = null;
                            if (dtOutput.Rows.Count != 0)
                            {
                                Response.Write("#" + Generix.CreateDownloadCSVFile(dtOutput, Server.MapPath("tempOutputs"), false, "", sNPCIAGNHeader, sNPCIAGNFooter));
                                success3 = Generix.UpdateData(Convert.ToString(Session["VISAReconDB"]) + ".dbo.switch", "npci_flag=1,npci_process_date=GetDate(),force_npci_flag=2,forced_user='" + Convert.ToString(Session["ActiveUser"]) + "'", "switch_identity in( " + Convert.ToString(Request.Form["agnIDh"]) + ")", 3);
                            }
                            break;
                        case "NUF": // NPCI Upload File
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getNPCIUploadFile(Convert.ToString(Session["VISAReconDB"]), Convert.ToString(Request.Form["npciIDh"]));
                            string sNPCIHeader = null;
                            string sNPCIFooter = null;
                            if (dtOutput.Rows.Count != 0)
                            {
                                Response.Write("#" + Generix.CreateDownloadCSVFile(dtOutput, Server.MapPath("tempOutputs"), false, "", sNPCIHeader, sNPCIFooter));
                                success = Generix.UpdateData(Convert.ToString(Session["VISAReconDB"]) + ".dbo.npci", "npci_flag=1,process_date=GetDate(),force_switch_flag=2,forced_user='" + Convert.ToString(Session["ActiveUser"]) + "'", "ID in(" + Convert.ToString(Request.Form["npciIDh"]) + ")", 3);
                            }
                            break;
                        case "HUF": // Host Upload File
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getHostUploadFile(Convert.ToString(Session["VISAReconDB"]), Convert.ToString(Request.Form["hostidh"]));
                            string sHostHeader = null;
                            string sHostFooter = null;
                            if (dtOutput.Rows.Count != 0)
                            {
                                Response.Write("#" + Generix.CreateDownloadCSVFile(dtOutput, Server.MapPath("tempOutputs"), false, "", sHostHeader, sHostFooter));
                            }

                            if (Convert.ToString(Session["IssuerNo"]) == "1")
                            {
                                success = Generix.UpdateData(Convert.ToString(Session["VISAReconDB"]) + ".dbo.Finacle", "host_flag=1,process_date=GetDate(),force_switch_flag=2,forced_user='" + Convert.ToString(Session["ActiveUser"]) + "'", "ID in(" + Convert.ToString(Request.Form["hostidh"]) + ")", 3);
                                Response.Write(success);
                            }
                            else if (Convert.ToString(Session["IssuerNo"]) == "2")
                            {
                                success = Generix.UpdateData(Convert.ToString(Session["VISAReconDB"]) + ".dbo.cb_host", "host_flag=1,process_date=GetDate(),force_switch_flag=2,forced_user='" + Convert.ToString(Session["ActiveUser"]) + "'", "host_identity in(" + Convert.ToString(Request.Form["hostidh"]) + ")", 3);
                                Response.Write(success);
                            }
                            else
                            {
                                success = Generix.UpdateData(Convert.ToString(Session["VISAReconDB"]) + ".dbo.cb_host_acq", "host_flag=1,process_date=GetDate(),force_switch_flag=2,forced_user='" + Convert.ToString(Session["ActiveUser"]) + "'", "host_identity in(" + Convert.ToString(Request.Form["hostidh"]) + ")", 3);
                                Response.Write(success);
                            }
                            break;
                        case "VIUMUF": // Visa Force Matched Upload File
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getVisaUploadFile(Convert.ToString(Session["VISAReconDB"]), Convert.ToString(Request.Form["visaIDh"]));
                            string sVisaHeader = null;
                            string sVisaFooter = null;
                            if (dtOutput.Rows.Count != 0)
                            {
                                Response.Write("#" + Generix.CreateDownloadCSVFile(dtOutput, Server.MapPath("tempOutputs"), false, "", sVisaHeader, sVisaFooter));
                                success = Generix.UpdateData(Convert.ToString(Session["VISAReconDB"]) + ".dbo.Visa", "Visa_flag=1,process_date=GetDate(),force_switch_flag=2,forced_user='" + Convert.ToString(Session["ActiveUser"]) + "'", "ID in(" + Convert.ToString(Request.Form["visaIDh"]) + ")", 3);
                            }
                            break;

                        case "MASUF": // Master Force Matched Upload File
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getMasterUploadFile(Convert.ToString(Session["VISAReconDB"]), Convert.ToString(Request.Form["masterIDh"]));
                            string sMasterHeader = null;
                            string sMasterFooter = null;
                            if (dtOutput.Rows.Count != 0)
                            {
                                Response.Write("#" + Generix.CreateDownloadCSVFile(dtOutput, Server.MapPath("tempOutputs"), false, "", sMasterHeader, sMasterFooter));
                                success = Generix.UpdateData(Convert.ToString(Session["VISAReconDB"]) + ".dbo.Master", "master_flag=1,process_date=GetDate(),force_switch_flag=2,forced_user='" + Convert.ToString(Session["ActiveUser"]) + "'", "ID in(" + Convert.ToString(Request.Form["masterIDh"]) + ")", 3);
                            }
                            break;


                        case "SNPCIM": // H2N NPCI Stand-in Matching
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            success = Generix.UpdateData(Convert.ToString(Session["VISAReconDB"]) + ".dbo.npci", "npci_flag=1,process_date=GetDate(),force_switch_flag=2,forced_user='" + Convert.ToString(Session["ActiveUser"]) + "'", "ID in(" + Convert.ToString(Request.Form["npciIDh"]) + ")", 3);
                            Response.Write(success);
                            break;

                        case "SSNM": // H2N Switch Stand-in Matching
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            success = Generix.UpdateData(Convert.ToString(Session["VISAReconDB"]) + ".dbo.switch", "npci_flag=1,force_npci_flag=2,npci_process_date=GetDate(),forced_user='" + Convert.ToString(Session["ActiveUser"]) + "'", "switch_identity in (" + Convert.ToString(Request.Form["switchID"]) + ")", 3);
                            Response.Write(success);
                            break;
                        case "RHOST": // H2H Force Matching
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            success = false;
                            if (Request.Form["hostidh"] != "" && Request.Form["switchidh"] != "")
                            {
                                if (Convert.ToString(Session["IssuerNo"]) == "1")
                                {
                                    success = Generix.UpdateData(Convert.ToString(Session["VISAReconDB"]) + ".dbo.Finacle", "host_flag=1,process_date=GetDate(),force_switch_flag=2,forced_user='" + Convert.ToString(Session["ActiveUser"]) + "'", "ID in(" + Convert.ToString(Request.Form["hostidh"]) + ")", 3);
                                    success = Generix.UpdateData(Convert.ToString(Session["VISAReconDB"]) + ".dbo.switch", "switch_flag=1,process_date=GetDate(),force_host_flag=2,forced_user='" + Convert.ToString(Session["ActiveUser"]) + "'", "switch_identity in(" + Convert.ToString(Request.Form["switchidh"]) + ")", 3);
                                    Response.Write(success);
                                }
                                else if (Convert.ToString(Session["IssuerNo"]) == "2")
                                {
                                    success = Generix.UpdateData(Convert.ToString(Session["VISAReconDB"]) + ".dbo.cb_host", "host_flag=1,process_date=GetDate(),force_switch_flag=2,forced_user='" + Convert.ToString(Session["ActiveUser"]) + "'", "host_identity in(" + Convert.ToString(Request.Form["hostidh"]) + ")", 3);
                                    success = Generix.UpdateData(Convert.ToString(Session["VISAReconDB"]) + ".dbo.switch", "switch_flag=1,process_date=GetDate(),force_host_flag=2,forced_user='" + Convert.ToString(Session["ActiveUser"]) + "'", "switch_identity in(" + Convert.ToString(Request.Form["switchidh"]) + ")", 3);
                                    Response.Write(success);
                                }
                                else
                                {
                                    success = Generix.UpdateData(Convert.ToString(Session["VISAReconDB"]) + ".dbo.cb_host_acq", "host_flag=1,process_date=GetDate(),force_switch_flag=2,forced_user='" + Convert.ToString(Session["ActiveUser"]) + "'", "host_identity in(" + Convert.ToString(Request.Form["hostidh"]) + ")", 3);
                                    success = Generix.UpdateData(Convert.ToString(Session["VISAReconDB"]) + ".dbo.switch", "switch_flag=1,process_date=GetDate(),force_host_flag=2,forced_user='" + Convert.ToString(Session["ActiveUser"]) + "'", "switch_identity in(" + Convert.ToString(Request.Form["switchidh"]) + ")", 3);
                                    Response.Write(success);
                                }
                            }

                            break;
                        case "NSR": // H2N Force Matching
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            if (Request.Form["npciIDh"] != "" && Request.Form["switchID"] != "")
                            {
                                success1 = Generix.UpdateData(Convert.ToString(Session["VISAReconDB"]) + ".dbo.npci", "npci_flag=1,process_date=GetDate(),force_switch_flag=2,forced_user='" + Convert.ToString(Session["ActiveUser"]) + "'", "ID in(" + Convert.ToString(Request.Form["npciIDh"]) + ")", 3);
                                success1 = Generix.UpdateData(Convert.ToString(Session["VISAReconDB"]) + ".dbo.switch", "npci_flag=1,process_date=GetDate(),force_npci_flag=2,forced_user='" + Convert.ToString(Session["ActiveUser"]) + "'", "switch_identity in (" + Convert.ToString(Request.Form["switchID"]) + ")", 3);
                            }
                            Response.Write(success1);

                            break;

                        case "VFORCEMATCH":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            if (Request.Form["visaIDh"] != "" && Request.Form["switchID"] != "")
                            {
                                if (Generix.isAvailable(Convert.ToString(Session["VISAReconDB"]) + ".dbo.Visa_Duplicate", "ID in (" + Convert.ToString(Request.Form["visaIDh"]) + ")", 3))
                                {
                                    success2 = Generix.UpdateData(Convert.ToString(Session["VISAReconDB"]) + ".dbo.Visa_Duplicate", "visa_flag=1,process_date=GetDate(),force_switch_flag=2,forced_user='" + Convert.ToString(Session["ActiveUser"]) + "'", "ID in(" + Convert.ToString(Request.Form["visaIDh"]) + ")", 3);
                                }
                                else
                                {
                                    success2 = Generix.UpdateData(Convert.ToString(Session["VISAReconDB"]) + ".dbo.Visa", "visa_flag=1,process_date=GetDate(),force_switch_flag=2,forced_user='" + Convert.ToString(Session["ActiveUser"]) + "'", "ID in(" + Convert.ToString(Request.Form["visaIDh"]) + ")", 3);
                                }
                                success2 = Generix.UpdateData(Convert.ToString(Session["VISAReconDB"]) + ".dbo.switch", "visa_flag=1,visa_process_date=GetDate(),force_visa_flag=2,forced_user='" + Convert.ToString(Session["ActiveUser"]) + "'", "switch_identity in( " + Convert.ToString(Request.Form["switchID"]) + ")", 3);
                            }
                            Response.Write(success2);

                            break;
                        case "SVFORCEMATCH":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            if (Generix.isAvailable(Convert.ToString(Session["VISAReconDB"]) + ".dbo.Visa_Duplicate", "ID in (" + Convert.ToString(Request.Form["visaIDh"]) + ")", 3))
                            {
                                success2 = Generix.UpdateData(Convert.ToString(Session["VISAReconDB"]) + ".dbo.Visa_Duplicate", "visa_flag=1,process_date=GetDate(),force_switch_flag=2,forced_user='" + Convert.ToString(Session["ActiveUser"]) + "'", "ID in(" + Convert.ToString(Request.Form["visaIDh"]) + ")", 3);
                            }
                            else
                            {
                                success2 = Generix.UpdateData(Convert.ToString(Session["VISAReconDB"]) + ".dbo.Visa", "visa_flag=1,process_date=GetDate(),force_switch_flag=2,forced_user='" + Convert.ToString(Session["ActiveUser"]) + "'", "ID in(" + Convert.ToString(Request.Form["visaIDh"]) + ")", 3);
                            }
                            Response.Write(success2);

                            break;

                        case "SMFORCEMATCH":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            if (Generix.isAvailable(Convert.ToString(Session["VISAReconDB"]) + ".dbo.master_Duplicate", "ID in (" + Convert.ToString(Request.Form["masterIDh"]) + ")", 3))
                            {
                                success2 = Generix.UpdateData(Convert.ToString(Session["VISAReconDB"]) + ".dbo.master_Duplicate", "master_flag=1,process_date=GetDate(),force_switch_flag=2,forced_user='" + Convert.ToString(Session["ActiveUser"]) + "'", "ID in(" + Convert.ToString(Request.Form["masterIDh"]) + ")", 3);
                            }
                            else
                            {
                                success2 = Generix.UpdateData(Convert.ToString(Session["VISAReconDB"]) + ".dbo.Master", "master_flag=1,process_date=GetDate(),force_switch_flag=2,forced_user='" + Convert.ToString(Session["ActiveUser"]) + "'", "ID in(" + Convert.ToString(Request.Form["masterIDh"]) + ")", 3);
                            }
                            Response.Write(success2);

                            break;


                        case "VVFORCEMATCH":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            if (Request.Form["switchID"] != "")
                            {
                                success2 = Generix.UpdateData(Convert.ToString(Session["VISAReconDB"]) + ".dbo.switch", "visa_flag=1,visa_process_date=GetDate(),force_visa_flag=2,forced_user='" + Convert.ToString(Session["ActiveUser"]) + "'", "switch_identity in( " + Convert.ToString(Request.Form["switchID"]) + ")", 3);
                            }
                            Response.Write(success2);

                            break;

                        case "MASFORCEMATCH":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            if (Request.Form["switchID"] != "")
                            {
                                success2 = Generix.UpdateData(Convert.ToString(Session["VISAReconDB"]) + ".dbo.switch", "master_flag=1,master_process_date=GetDate(),force_master_flag=2,forced_user='" + Convert.ToString(Session["ActiveUser"]) + "'", "switch_identity in( " + Convert.ToString(Request.Form["switchID"]) + ")", 3);
                            }
                            Response.Write(success2);

                            break;

                        case "MFORCEMATCH":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            if (Request.Form["masterIDh"] != "" && Request.Form["switchID"] != "")
                            {
                                if (Generix.isAvailable(Convert.ToString(Session["VISAReconDB"]) + ".dbo.master_duplicate", "ID in (" + Convert.ToString(Request.Form["masterIDh"]) + ")", 3))
                                {
                                    success2 = Generix.UpdateData(Convert.ToString(Session["VISAReconDB"]) + ".dbo.master_duplicate", "master_flag=1,process_date=GetDate(),force_switch_flag=2,forced_user='" + Convert.ToString(Session["ActiveUser"]) + "'", "ID in(" + Convert.ToString(Request.Form["masterIDh"]) + ")", 3);
                                }
                                else
                                {
                                    success2 = Generix.UpdateData(Convert.ToString(Session["VISAReconDB"]) + ".dbo.master", "master_flag=1,process_date=GetDate(),force_switch_flag=2,forced_user='" + Convert.ToString(Session["ActiveUser"]) + "'", "ID in(" + Convert.ToString(Request.Form["masterIDh"]) + ")", 3);
                                }
                                success2 = Generix.UpdateData(Convert.ToString(Session["VISAReconDB"]) + ".dbo.switch", "master_flag=1,master_process_date=GetDate(),force_master_flag=2,forced_user='" + Convert.ToString(Session["ActiveUser"]) + "'", "switch_identity in( " + Convert.ToString(Request.Form["switchID"]) + ")", 3);
                            }
                            Response.Write(success2);

                            break;



                        case "AGEING":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            if (Request.Form["agnIDh"] != "")
                            {
                                success3 = Generix.UpdateData(Convert.ToString(Session["VISAReconDB"]) + ".dbo.switch", "visa_flag=1,visa_process_date=GetDate(),force_visa_flag=2,forced_user='" + Convert.ToString(Session["ActiveUser"]) + "'", "switch_identity in( " + Convert.ToString(Request.Form["agnIDh"]) + ")", 3);
                            }
                            Response.Write(success3);

                            break;
                        case "NPCIAGEING":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            if (Request.Form["agnIDh"] != "")
                            {
                                success3 = Generix.UpdateData(Convert.ToString(Session["VISAReconDB"]) + ".dbo.switch", "npci_flag=1,npci_process_date=GetDate(),force_npci_flag=2,forced_user='" + Convert.ToString(Session["ActiveUser"]) + "'", "switch_identity in( " + Convert.ToString(Request.Form["agnIDh"]) + ")", 3);
                            }
                            Response.Write(success3);

                            break;

                        case "GPAYAGNUP": // GharPay Ageing Upload File
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getGpayAgeingUploadFile(Convert.ToString(Session["VISAReconDB"]), Convert.ToString(Request.Form["agnIDh"]));
                            string sGpayAGNHeader = null;
                            string sGpayAGNFooter = null;
                            if (dtOutput.Rows.Count != 0)
                            {
                                Response.Write("#" + Generix.CreateDownloadCSVFile(dtOutput, Server.MapPath("tempOutputs"), false, "", sGpayAGNHeader, sGpayAGNFooter));
                                success = Generix.UpdateData("Temp_PO.dbo.Gharpay", "AgeingMatch=1, AgeingMatchDate=GetDate(),AgeingMatchUser='" + Convert.ToString(Session["ActiveUser"]) + "'", "Code in (" + Convert.ToString(Request.Form["agnIDh"]) + ")", 3);
                            }
                            break;

                        case "GPAYMCTTUM": // GharPay Ageing Upload File
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getGpayAgeingUploadFile(Convert.ToString(Session["VISAReconDB"]), Convert.ToString(Request.Form["mctIDh"]));
                            string sGpayMCTHeader = null;
                            string sGpayMCTFooter = null;
                            if (dtOutput.Rows.Count != 0)
                            {
                                Response.Write("#" + Generix.CreateDownloadCSVFile(dtOutput, Server.MapPath("tempOutputs"), false, "", sGpayMCTHeader, sGpayMCTFooter));
                                success = Generix.UpdateData("Temp_PO.dbo.Gharpay", "AgeingMatch=1, AgeingMatchDate=GetDate(),AgeingMatchUser='" + Convert.ToString(Session["ActiveUser"]) + "'", "Code in (" + Convert.ToString(Request.Form["mctIDh"]) + ")", 3);
                            }
                            break;

                        case "SEARCHTXN":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.searchTxn(Convert.ToString(Request.Form["cardNo"]), Convert.ToDateTime(Request.Form["frmdate"]), Convert.ToDateTime(Request.Form["todate"]), Convert.ToString(Request.Form["term_id"]), (Convert.ToString(Session["ShowSensitive"]) == "1"), Convert.ToInt32(Session["Bank"]));
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, false));
                            if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            break;


                        // ******************************************************************************************************************************************************
                        // RECON SECTION ENDS HERE AND CHARGE BACK SECTION START HERE
                        // ******************************************************************************************************************************************************

                        case "NPCICHG":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            if (Request.Form["chgIDh"] != "")
                            {
                                success3 = Generix.UpdateData(Convert.ToString(Session["VISAReconDB"]) + ".dbo.npci", "chg_back_flag=2,chg_bck_date=GetDate(),forced_user='" + Convert.ToString(Session["ActiveUser"]) + "'", "ID in(" + Convert.ToString(Request.Form["chgIDh"]) + ")", 3);
                            }
                            Response.Write(success3);

                            break;

                        case "VISACHG":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            if (Request.Form["VisachgIDh"] != "")
                            {
                                success3 = Generix.UpdateData(Convert.ToString(Session["VISAReconDB"]) + ".dbo.Visa", "chg_back_flag=2,chg_bck_date=GetDate(),forced_user='" + Convert.ToString(Session["ActiveUser"]) + "'", "ID in(" + Convert.ToString(Request.Form["VisachgIDh"]) + ")", 3);
                            }
                            Response.Write(success3);

                            break;


                        // ******************************************************************************************************************************************************
                        // CHARGE BACK SECTION ENDS HERE
                        // ******************************************************************************************************************************************************

                        // ******************************************************************************************************************************************************
                        // RECON GL START HERE
                        // ******************************************************************************************************************************************************

                        case "DISPLAYGL":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            string sNetwork = "", sCategory = "", sProcessor = "", sTerm_type = "", sAccNo = "", sAccountName = "", sFullName = "";

                            dtOutput = Generix.displayReconGL(Convert.ToString(Session["VISAReconDB"]), Convert.ToDateTime(Request.Form["date"]), Convert.ToInt32(Request.Form["AccNo"]));

                            try
                            {
                                using (DataTable dtObj = Generix.getData(Convert.ToString(Session["VISAReconDB"]) + ".dbo.ReconGLAccountDetail a ", " network,category,processor,term_type,acc_name,acc_no ", " a.ID=" + Convert.ToInt32(Request.Form["AccNo"]), "", "", 3))
                                {
                                    sNetwork = Convert.ToString(dtObj.Rows[0][0]);
                                    sCategory = Convert.ToString(dtObj.Rows[0][1]);
                                    sProcessor = Convert.ToString(dtObj.Rows[0][2]);
                                    sTerm_type = Convert.ToString(dtObj.Rows[0][3]);
                                    sAccountName = Convert.ToString(dtObj.Rows[0][4]);
                                    sAccNo = Convert.ToString(dtObj.Rows[0][5]);

                                    sFullName = sNetwork + " " + sProcessor + " " + sTerm_type + " " + sCategory + "_" + sAccountName + " " + sAccNo;

                                }
                            }
                            catch (Exception xObj)
                            {
                                throw new Exception("Unable to retreive Account details!");
                            }

                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, false));
                            if (dtOutput.Rows.Count != 0)
                            {
                                dtOutput.Columns.RemoveAt(0);
                                Response.Write(Generix.generateGLReconExcel(dtOutput, Server.MapPath("tempOutputs"), sFullName) + "|");
                            }

                            break;

                        case "RECONGL":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            if (Generix.doReconGL(Convert.ToString(Session["VISAReconDB"]), Convert.ToDateTime(Request.Form["date"]), Convert.ToInt32(Request.Form["AccNo"])))
                            {
                                Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "ReconGL Done Successfully", " ", "");
                                Response.Write("1");
                            }

                            break;

                        case "MANUALENTRY":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.putGLReconManualEntry(Convert.ToString(Session["VISAReconDB"]), Convert.ToString(Request.Form["ACCNO"]), Convert.ToDateTime(Request.Form["Date"]), Convert.ToInt32(Request.Form["ACCENTRY"]), Convert.ToString(Request.Form["Network"]), Convert.ToString(Request.Form["AMOUNT"]), Convert.ToString(Request.Form["Remark"]), Convert.ToString(Session["ActiveUser"]));
                            sOutput = "";

                            if (dtOutput != null)
                            {
                                if (dtOutput.Rows.Count > 0)
                                {
                                    foreach (DataRow drOutput in dtOutput.Rows)
                                    {
                                        sOutput += ((sOutput == "") ? "" : "~");
                                        foreach (DataColumn dcOutput in dtOutput.Columns)
                                        {
                                            sOutput += ((sOutput == "") ? "" : "|") + Convert.ToString(drOutput[dcOutput]);
                                        }
                                    }
                                }
                            }
                            Response.Write(sOutput.Replace("~", "|"));

                            break;

                        // ******************************************************************************************************************************************************
                        // RECON GL END HERE
                        // ******************************************************************************************************************************************************

                        case "CICS":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            string sBCBranchCode = "";
                            string sBCBranchName = "";
                            string sRBLBranchName = "";
                            string html = "";
                            dtOutput = Generix.getcardIssueControlSheet(Convert.ToString(Request.Form["bcCode"]), Convert.ToString(Request.Form["cardType"]), Convert.ToDateTime(Request.Form["date"]));
                            Table displayReport = Generix.convertDataTable2HTMLTable(dtOutput, true, false);

                            try
                            {
                                using (DataTable dtObj = Generix.getData(" dbo.BcBranch a Left Join dbo.Branch b on a.RBLBranchCode=b.branchCode ", " BCBranchCode,BCBranchName,b.Name ", "a.Code=" + Convert.ToString(Request.Form["bcCode"]), "", "", 3))
                                {
                                    sBCBranchCode = Convert.ToString(dtObj.Rows[0][0]);
                                    sBCBranchName = Convert.ToString(dtObj.Rows[0][1]);
                                    sRBLBranchName = Convert.ToString(dtObj.Rows[0][2]);
                                }
                            }
                            catch (Exception xObj)
                            {
                                throw new Exception("Unable to retreive Branch details!");
                            }

                            Page.Controls.Add(displayReport);
                            if (dtOutput.Rows.Count != 0) Response.Write("#" + Generix.generateCICS(dtOutput, sBCBranchCode, sBCBranchName, sRBLBranchName, Convert.ToString(Request.Form["cardType"]), Server.MapPath("tempOutputs")) + "|");
                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "CICS ", "CICS", "");

                            break;

                        //BTI Report Start here
                        case "CSHRL":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getCashManagementReport(Convert.ToString(aQueryString[0]), Convert.ToInt32(Session["Bank"]), sATM, dFrom, dTo);
                            if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, true));
                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Report Generated", Convert.ToString(aQueryString[0]), "RealTime");

                            break;

                        case "AWKA":
                        case "BTITCTW":
                        case "BTILNSTL":
                        case "HRTXN":
                        case "TXNDM":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getTerminalWiseTransactionCount_BTI(Convert.ToString(aQueryString[0]), Convert.ToInt32(Session["Bank"]), sATM, dFrom, dTo, (Convert.ToString(Session["ShowSensitive"]) == "1"));

                            //if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateDownloadFile(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, false));
                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Report Generated", "TCTW", "From:" + Convert.ToString(dFrom) + "|To:" + Convert.ToString(dTo));

                            break;

                        case "BTIDUMP":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            if (sTop == "") sTop = "-1";
                            dtOutput = Generix.getTransaction_BTI(Convert.ToInt32(Session["Bank"]), Convert.ToString(aQueryString[0]), sATM, dFrom, dTo, sSource, sCards, sMerchants, sTranSource, sNode, sOrigin, (Convert.ToString(Session["ShowSensitive"]) == "1"), Convert.ToInt32(sTop));
                            if (sSkipDownloadFile != "Y") if (dtOutput.Rows.Count != 0) Response.Write("#" + Generix.CreateDownloadCSVFile(dtOutput, Server.MapPath("tempOutputs"), false, ",", "", "") + "|");
                            //if (sSkipDownloadFile != "Y") if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, true));
                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Report Generated", Convert.ToString(aQueryString[0]), "Terminals:" + sATM + "|From:" + Convert.ToString(dFrom) + "|To:" + Convert.ToString(dTo) + "|Source:" + sSource + "|Cards:" + sCards + "|Merchants:" + sMerchants + "|TranSource:" + sTranSource + "|Node:" + sNode + "|Origin:" + sOrigin);
                            break;

                        case "DUMPTXT":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            if (sTop == "") sTop = "-1";
                            dtOutput = Generix.getTransaction_BTI(Convert.ToInt32(Session["Bank"]), Convert.ToString(aQueryString[0]), sATM, dFrom, dTo, sSource, sCards, sMerchants, sTranSource, sNode, sOrigin, (Convert.ToString(Session["ShowSensitive"]) == "1"), Convert.ToInt32(sTop));
                            //if (sSkipDownloadFile != "Y") if (dtOutput.Rows.Count != 0) Response.Write("$"+Generix.CreateDownloadFile(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            if (sSkipDownloadFile != "Y") if (dtOutput.Rows.Count != 0) Response.Write("$" + Generix.CreateDownloadCSVFile(dtOutput, Server.MapPath("tempOutputs"), false, ",", "", "") + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, true));
                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Report Generated", Convert.ToString(aQueryString[0]), "Terminals:" + sATM + "|From:" + Convert.ToString(dFrom) + "|To:" + Convert.ToString(dTo) + "|Source:" + sSource + "|Cards:" + sCards + "|Merchants:" + sMerchants + "|TranSource:" + sTranSource + "|Node:" + sNode + "|Origin:" + sOrigin);
                            break;

                        case "DUMPXLS":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            if (sTop == "") sTop = "-1";
                            dtOutput = Generix.getTransaction_BTI(Convert.ToInt32(Session["Bank"]), Convert.ToString(aQueryString[0]), sATM, dFrom, dTo, sSource, sCards, sMerchants, sTranSource, sNode, sOrigin, (Convert.ToString(Session["ShowSensitive"]) == "1"), Convert.ToInt32(sTop));
                            if (sSkipDownloadFile != "Y") if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, true));
                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Report Generated", Convert.ToString(aQueryString[0]), "Terminals:" + sATM + "|From:" + Convert.ToString(dFrom) + "|To:" + Convert.ToString(dTo) + "|Source:" + sSource + "|Cards:" + sCards + "|Merchants:" + sMerchants + "|TranSource:" + sTranSource + "|Node:" + sNode + "|Origin:" + sOrigin);
                            break;

                        case "MCSHLD":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getAdminTranSummary_bti(Convert.ToInt32(Session["Bank"]), true, false, sATM, dFrom, dTo);
                            //if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateDownloadFile(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, true));
                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Report Generated", "MCSHLD", "Terminals:" + sATM + "|From:" + Convert.ToString(dFrom) + "|To:" + Convert.ToString(dTo) + "|Terminals:" + sATM);

                            break;

                        case "MCBILLS":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getAdminTranSummary_bti(Convert.ToInt32(Session["Bank"]), false, true, sATM, dFrom, dTo);
                            //if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateDownloadFile(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, true));
                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Report Generated", "MCSHLD", "Terminals:" + sATM + "|From:" + Convert.ToString(dFrom) + "|To:" + Convert.ToString(dTo) + "|Terminals:" + sATM);

                            break;

                        case "HCBR":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getHourlyCashBal_bti(Convert.ToInt32(Session["Bank"]), sATM);
                            if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, false));
                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Report Generated", "HCBR", "From:" + Convert.ToString(dFrom) + "|To:" + Convert.ToString(dTo));
                            break;

                        case "DISPSTWS":
                        case "DISPTRWS":
                        case "DISP":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getTerminalWiseDispense(Convert.ToString(aQueryString[0]), Convert.ToInt32(Session["Bank"]), sATM, dFrom, dTo, (Convert.ToString(Session["ShowSensitive"]) == "1"));
                            if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, false));
                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Report Generated", "Dispnse Report", "From:" + Convert.ToString(dFrom) + "|To:" + Convert.ToString(dTo));

                            break;

                        case "TKCFG":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.insertTMK(Convert.ToString(Request.Form["key1"]), Convert.ToString(Request.Form["key2"]), Convert.ToString(Request.Form["key3"]), Convert.ToString(Request.Form["key4"]), Convert.ToString(Request.Form["key5"]), Convert.ToString(Request.Form["key6"]), Convert.ToString(Request.Form["keyType"]), Convert.ToInt32(Session["Bank"]));
                            sOutput = "";

                            if (dtOutput != null)
                            {
                                if (dtOutput.Rows.Count > 0)
                                {
                                    foreach (DataRow drOutput in dtOutput.Rows)
                                    {
                                        sOutput += ((sOutput == "") ? "" : "~");
                                        foreach (DataColumn dcOutput in dtOutput.Columns)
                                        {
                                            sOutput += ((sOutput == "") ? "" : "|") + Convert.ToString(drOutput[dcOutput]);
                                        }
                                    }
                                }
                            }
                            Response.Write(sOutput.Replace("~", "|"));

                            break;
                        /*old code */
                        /*
                    case "GETREPORTGROUP":
                        dtOutput = Generix.getReportGroup(Convert.ToInt32(Session["Bank"]));
                        if (dtOutput.Rows.Count > 0)
                        {
                            Table tblOutput = Generix.convertDataTable2HTMLTable(dtOutput, true, false);
                            tblOutput.Attributes["class"] = "imagetable";
                            System.Web.UI.HtmlControls.HtmlInputCheckBox chkMenuGroup = null;

                            foreach (TableRow trOutput in tblOutput.Rows)
                            {
                                if (trOutput.TableSection == TableRowSection.TableBody)
                                {
                                    chkMenuGroup = new System.Web.UI.HtmlControls.HtmlInputCheckBox();
                                    chkMenuGroup.Attributes["style"] = "margin-left:80px;";
                                    chkMenuGroup.Attributes["type"] = "checkbox";
                                    chkMenuGroup.Attributes["id"] = "chkBox_" + trOutput.Cells[0].Text;
                                    chkMenuGroup.Attributes["class"] = "chkBoxMenuGroup";
                                    chkMenuGroup.Attributes["value"] = trOutput.Cells[0].Text;
                                    trOutput.Cells[0].Controls.Add(chkMenuGroup);
                                }
                            }

                            Page.Controls.Add(tblOutput);
                        }
                        else
                        {
                            Response.Write("ERROR: No Entries Found!");
                        }

                        break;


                    case "GETUTILITIES":
                        dtOutput = Generix.getUtilities(Convert.ToInt32(Session["Bank"]));
                        if (dtOutput.Rows.Count > 0)
                        {
                            Table tblOutput = Generix.convertDataTable2HTMLTable(dtOutput, true, false);
                            tblOutput.Attributes["class"] = "imagetable";
                            System.Web.UI.HtmlControls.HtmlInputCheckBox chkMenuGroup = null;

                            foreach (TableRow trOutput in tblOutput.Rows)
                            {
                                if (trOutput.TableSection == TableRowSection.TableBody)
                                {
                                    chkMenuGroup = new System.Web.UI.HtmlControls.HtmlInputCheckBox();
                                    chkMenuGroup.Attributes["style"] = "margin-left:80px;";
                                    chkMenuGroup.Attributes["type"] = "checkbox";
                                    chkMenuGroup.Attributes["id"] = "chkBox_" + trOutput.Cells[0].Text;
                                    chkMenuGroup.Attributes["class"] = "chkBoxUtilities";
                                    chkMenuGroup.Attributes["value"] = trOutput.Cells[0].Text;
                                    trOutput.Cells[0].Controls.Add(chkMenuGroup);
                                }
                            }

                            Page.Controls.Add(tblOutput);
                        }
                        else
                        {
                            Response.Write("ERROR: No Entries Found!");
                        }

                        break;


                    case "INSERTUSER":
                        if (Generix.insertUser(Convert.ToString(Request.Form["userName"]), Convert.ToInt32(Session["Bank"]), Convert.ToString(Request.Form["userPass"]), Convert.ToString(Request.Form["userEmail"]), Convert.ToString(Request.Form["sMobileNo"]), Convert.ToString(Request.Form["sReportRights"]), Convert.ToString(Request.Form["sUtilitiesRights"]), Convert.ToInt32(Request.Form["sShowSensitiveData"]), Convert.ToInt32(Session["ActiveUserCode"]), Convert.ToInt32(Request.Form["Checker"])))
                        {
                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Report Generated", "INSERTUSER", "From:" + Convert.ToString(dFrom) + "|To:" + Convert.ToString(dTo));
                            Response.Write("1");
                        }

                        break;
                        */
                        /*new code for admin forntend*/


                        //change by $ for ATPCM 1632 start
                        case "GETREPORTGROUP":
                            dtOutput = Generix.getReportGroup(Convert.ToInt32(Session["Bank"]));

                            DataSet ds = new DataSet();
                            DataRow dr = dtOutput.NewRow();
                            dr["REPORT TYPE"] = "SELECT / DESELECT ALL";
                            dtOutput.Rows.InsertAt(dr, 0);
                            ds.Tables.Add(dtOutput);

                            String[] Report = null;
                            try
                            {
                                if ((Session["Username"]) != null && (Session["Username"]).ToString() != " " && (Session["Username"]).ToString() != "")
                                {
                                    if ((Session["Username"]).ToString() != "" && (Session["Username"]).ToString() !=" ")
                                    {
                                        DataTable dtuserright = Generix.getUserRightsDetail((Session["Username"]).ToString());
                                        if (dtuserright.Rows.Count > 0)
                                        {
                                            Report = dtuserright.Rows[0][6].ToString().Split(',');
                                            //5000 for default value for newly created role.
                                        }

                                    }
                                }
                                else
                                {
                                    if ((Session["RollId"]) != null)
                                    {
                                        DataTable dtuserright = Generix.getRollMaper(Convert.ToInt32(Session["RollId"]));
                                        if (dtuserright.Rows.Count > 0)
                                        {
                                            Report = dtuserright.Rows[0][1].ToString().Split(',');
                                            //5000 for default value for newly created role.
                                        }
                                    }

                                }
                            }
                            catch (Exception Obx) { }

                            if (dtOutput.Rows.Count > 0)
                            {
                                Table tblOutput = Generix.convertDataTable2HTMLTable(dtOutput, true, false);
                                tblOutput.Attributes["class"] = "imagetable";
                                System.Web.UI.HtmlControls.HtmlInputCheckBox chkMenuGroup = null;

                                foreach (TableRow trOutput in tblOutput.Rows)
                                {
                                    if (trOutput.TableSection == TableRowSection.TableBody)
                                    {
                                        chkMenuGroup = new System.Web.UI.HtmlControls.HtmlInputCheckBox();
                                        chkMenuGroup.Attributes["style"] = "margin-left:80px;";
                                        chkMenuGroup.Attributes["type"] = "checkbox";
                                        chkMenuGroup.Attributes["Id"] = "chkBox1_" + trOutput.Cells[0].Text;
                                        //if ((Session["Username"]) != null)
                                        {
                                            if (Report.Contains(trOutput.Cells[0].Text))
                                            {
                                                chkMenuGroup.Checked = true;
                                            }

                                        }
                                        //chkMenuGroup.Attributes["autopostback"] = "true";
                                        chkMenuGroup.Attributes["Class"] = "chkBoxMenuGroup";
                                        chkMenuGroup.Attributes["Group"] = "chkBoxMenuGroup";
                                        chkMenuGroup.Attributes["value"] = trOutput.Cells[0].Text;
                                        trOutput.Cells[0].Controls.Add(chkMenuGroup);
                                    }
                                }

                                Page.Controls.Add(tblOutput);
                            }
                            else
                            {
                                Response.Write("ERROR: No Entries Found!");
                            }

                            break;

                        case "GETREPORTGROUPREADONLY":
                            dtOutput = Generix.getReportGroup(Convert.ToInt32(Session["Bank"]));

                            DataSet dts = new DataSet();
                            DataRow dtr = dtOutput.NewRow();
                            dtr["REPORT TYPE"] = "SELECT / DESELECT ALL";
                            dtOutput.Rows.InsertAt(dtr, 0);
                            dts.Tables.Add(dtOutput);

                            String[] Reports = null;
                            try
                            {
                                if ((Session["Username"]) != null && (Session["Username"]).ToString() != " " && (Session["Username"]).ToString() != "")
                                {
                                    if ((Session["Username"]).ToString() != "" && (Session["Username"]).ToString() != " ")
                                    {
                                        DataTable dtuserright = Generix.getUserRightsDetail((Session["Username"]).ToString());
                                        if (dtuserright.Rows.Count > 0)
                                        {
                                            Reports = dtuserright.Rows[0][7].ToString().Split(',');
                                            //5000 for default value for newly created role.
                                        }

                                    }
                                }
                                else
                                {
                                    if ((Session["RollId"]) != null)
                                    {
                                        DataTable dtuserright = Generix.getRollMaper(Convert.ToInt32(Session["RollId"]));
                                        if (dtuserright.Rows.Count > 0)
                                        {
                                            Reports = dtuserright.Rows[0][1].ToString().Split(',');
                                            //5000 for default value for newly created role.
                                        }
                                    }

                                }
                            }
                            catch (Exception Obx) { }

                            if (dtOutput.Rows.Count > 0)
                            {
                                Table tblOutput = Generix.convertDataTable2HTMLTable(dtOutput, true, false);
                                tblOutput.Attributes["class"] = "imagetable";
                                System.Web.UI.HtmlControls.HtmlInputCheckBox chkMenuGroup = null;

                                foreach (TableRow trOutput in tblOutput.Rows)
                                {
                                    if (trOutput.TableSection == TableRowSection.TableBody)
                                    {
                                        chkMenuGroup = new System.Web.UI.HtmlControls.HtmlInputCheckBox();
                                        chkMenuGroup.Attributes["style"] = "margin-left:80px;";
                                        chkMenuGroup.Attributes["type"] = "checkbox";
                                        chkMenuGroup.Attributes["Id"] = "chkBox1_" + trOutput.Cells[0].Text;
                                        //if ((Session["Username"]) != null)
                                        {
                                            if (Reports.Contains(trOutput.Cells[0].Text))
                                            {
                                                chkMenuGroup.Checked = true;
                                            }

                                        }
                                        //chkMenuGroup.Attributes["autopostback"] = "true";
                                        chkMenuGroup.Attributes["Class"] = "chkBoxMenuGroup";
                                        chkMenuGroup.Attributes["Group"] = "chkBoxMenuGroup";
                                        chkMenuGroup.Attributes["Disabled"] = "true";
                                        chkMenuGroup.Attributes["value"] = trOutput.Cells[0].Text;
                                        trOutput.Cells[0].Controls.Add(chkMenuGroup);
                                    }
                                }

                                Page.Controls.Add(tblOutput);
                            }
                            else
                            {
                                Response.Write("ERROR: No Entries Found!");
                            }

                            break;

                        case "GETREPORTGROUPBYROLEMASTER":
                            dtOutput = Generix.getReportGroup(Convert.ToInt32(Session["Bank"]));

                            DataSet dtset = new DataSet();
                            DataRow dtrow = dtOutput.NewRow();
                            dtrow["REPORT TYPE"] = "SELECT / DESELECT ALL";
                            dtOutput.Rows.InsertAt(dtrow, 0);
                            dtset.Tables.Add(dtOutput);

                            String[] ReportRights = null;
                            try
                            {
                                //if ((Session["Username"]) != null)
                                //{
                                //    if ((Session["Username"]).ToString() != "")
                                //    {
                                //if (Convert.ToString(Request.QueryString["RoleMaster"]) != null)
                                //{

                                //String RoleMaster = Convert.ToString(Request.QueryString["RoleMaster"]);
                                //DataTable dtuserright = Generix.getRollMaper(Convert.ToInt32(Session["RollId"]));

                                DataTable dtuserright = Generix.getRollMaper(Convert.ToInt32(Request.Form["RollId"]));
                                ReportRights = dtuserright.Rows[0][1].ToString().Split(',');
                                //}
                                //}
                            }
                            catch (Exception Obx) { }

                            if (dtOutput.Rows.Count > 0)
                            {
                                Table tblOutput = Generix.convertDataTable2HTMLTable(dtOutput, true, false);
                                tblOutput.Attributes["class"] = "imagetable";
                                System.Web.UI.HtmlControls.HtmlInputCheckBox chkMenuGroup = null;

                                foreach (TableRow trOutput in tblOutput.Rows)
                                {
                                    if (trOutput.TableSection == TableRowSection.TableBody)
                                    {
                                        chkMenuGroup = new System.Web.UI.HtmlControls.HtmlInputCheckBox();
                                        chkMenuGroup.Attributes["style"] = "margin-left:80px;";
                                        chkMenuGroup.Attributes["type"] = "checkbox";
                                        chkMenuGroup.Attributes["Id"] = "chkBox1_" + trOutput.Cells[0].Text;
                                        //if ((Session["Username"]) != null)
                                        {
                                            if (ReportRights.Contains(trOutput.Cells[0].Text))
                                            {
                                                chkMenuGroup.Checked = true;
                                            }

                                        }

                                        chkMenuGroup.Attributes["Class"] = "chkBoxMenuGroup";
                                        chkMenuGroup.Attributes["Group"] = "chkBoxMenuGroup";
                                        chkMenuGroup.Attributes["value"] = trOutput.Cells[0].Text;
                                        trOutput.Cells[0].Controls.Add(chkMenuGroup);
                                    }
                                }

                                Page.Controls.Add(tblOutput);
                            }
                            else
                            {
                                Response.Write("ERROR: No Entries Found!");
                            }

                            break;

                        case "GETUTILITIES":
                            dtOutput = Generix.getUtilities(Convert.ToInt32(Session["Bank"]));
                            DataSet dset = new DataSet();
                            DataRow drow = dtOutput.NewRow();
                            drow["UTILITIES NAME"] = "SELECT / DESELECT ALL";
                            //drow["UtilitiesName"] = "SELECT / DESELECT ALL";//04-05-2022 $@hin
                            //drow["Code"] = 0;
                            dtOutput.Rows.InsertAt(drow, 0);
                            dset.Tables.Add(dtOutput);


                            String[] Utility = null;
                            try
                            {
                                if ((Session["Username"]) != null && (Session["Username"]).ToString() != " " && (Session["Username"]).ToString() != "")
                                {

                                    if ((Session["Username"]).ToString() != "" && (Session["Username"]).ToString() != " ")
                                    {
                                        DataTable dtuserright = Generix.getUserRightsDetail((Session["Username"]).ToString());
                                        if (dtuserright.Rows.Count > 0)
                                        {
                                            Utility = dtuserright.Rows[0][7].ToString().Split(',');
                                            //Utility = dtuserright.Rows[0][8].ToString().Split(',');
                                        }
                                    }
                                }
                                else
                                {
                                    if ((Session["RollId"]) != null)
                                    {
                                        DataTable dtuserright = Generix.getRollMaper(Convert.ToInt32(Session["RollId"]));
                                        if (dtuserright.Rows.Count > 0)
                                        {
                                            Utility = dtuserright.Rows[0][2].ToString().Split(',');
                                        }
                                    }
                                }

                            }
                            catch (Exception Obx) { }
                            if (dtOutput.Rows.Count > 0)
                            {
                                Table tblOutput = Generix.convertDataTable2HTMLTable(dtOutput, true, false);
                                tblOutput.Attributes["class"] = "imagetable";
                                System.Web.UI.HtmlControls.HtmlInputCheckBox chkMenuGroup = null;

                                foreach (TableRow trOutput in tblOutput.Rows)
                                {
                                    if (trOutput.TableSection == TableRowSection.TableBody)
                                    {
                                        chkMenuGroup = new System.Web.UI.HtmlControls.HtmlInputCheckBox();
                                        chkMenuGroup.Attributes["style"] = "margin-left:80px;";
                                        chkMenuGroup.Attributes["type"] = "checkbox";
                                        chkMenuGroup.Attributes["Id"] = "chkBox2_" + trOutput.Cells[0].Text;
                                        //if ((Session["Username"]) != null)
                                        {
                                            if (Utility.Contains(trOutput.Cells[0].Text))
                                            {
                                                chkMenuGroup.Checked = true;
                                            }
                                        }
                                        chkMenuGroup.Attributes["Class"] = "chkBoxUtilities";
                                        chkMenuGroup.Attributes["Group"] = "chkBoxUtilities";
                                       //chkMenuGroup.Attributes["Disabled"] = "true";
                                        chkMenuGroup.Attributes["value"] = trOutput.Cells[0].Text;
                                        trOutput.Cells[0].Controls.Add(chkMenuGroup);
                                    }
                                }

                                Page.Controls.Add(tblOutput);
                            }
                            else
                            {
                                Response.Write("ERROR: No Entries Found!");
                            }

                            break;

                        case "GETUTILITIESREADONLY":
                            dtOutput = Generix.getUtilities(Convert.ToInt32(Session["Bank"]));
                            DataSet dtsetro = new DataSet();
                            DataRow dtrowro = dtOutput.NewRow();
                            dtrowro["UTILITIES NAME"] = "SELECT / DESELECT ALL";
                            //drow["Code"] = 0;
                            dtOutput.Rows.InsertAt(dtrowro, 0);
                            dtsetro.Tables.Add(dtOutput);


                            String[] Utilityro = null;
                            try
                            {
                                if ((Session["Username"]) != null && (Session["Username"]).ToString() != " " && (Session["Username"]).ToString() != "")
                                {

                                    if ((Session["Username"]).ToString() != "" && (Session["Username"]).ToString() != " ")
                                    {
                                        DataTable dtuserright = Generix.getUserRightsDetail((Session["Username"]).ToString());
                                        if (dtuserright.Rows.Count > 0)
                                        {
                                            Utilityro = dtuserright.Rows[0][8].ToString().Split(',');
                                        }
                                    }
                                }
                                else
                                {
                                    if ((Session["RollId"]) != null)
                                    {
                                        DataTable dtuserright = Generix.getRollMaper(Convert.ToInt32(Session["RollId"]));
                                        if (dtuserright.Rows.Count > 0)
                                        {
                                            Utilityro = dtuserright.Rows[0][2].ToString().Split(',');
                                        }
                                    }
                                }

                            }
                            catch (Exception Obx) { }
                            if (dtOutput.Rows.Count > 0)
                            {
                                Table tblOutput = Generix.convertDataTable2HTMLTable(dtOutput, true, false);
                                tblOutput.Attributes["class"] = "imagetable";
                                System.Web.UI.HtmlControls.HtmlInputCheckBox chkMenuGroup = null;

                                foreach (TableRow trOutput in tblOutput.Rows)
                                {
                                    if (trOutput.TableSection == TableRowSection.TableBody)
                                    {
                                        chkMenuGroup = new System.Web.UI.HtmlControls.HtmlInputCheckBox();
                                        chkMenuGroup.Attributes["style"] = "margin-left:80px;";
                                        chkMenuGroup.Attributes["type"] = "checkbox";
                                        chkMenuGroup.Attributes["Id"] = "chkBox2_" + trOutput.Cells[0].Text;
                                        //if ((Session["Username"]) != null)
                                        {
                                            if (Utilityro.Contains(trOutput.Cells[0].Text))
                                            {
                                                chkMenuGroup.Checked = true;
                                            }
                                        }
                                        chkMenuGroup.Attributes["Class"] = "chkBoxUtilities";
                                        chkMenuGroup.Attributes["Group"] = "chkBoxUtilities";
                                        chkMenuGroup.Attributes["Disabled"] = "true";
                                        chkMenuGroup.Attributes["value"] = trOutput.Cells[0].Text;
                                        trOutput.Cells[0].Controls.Add(chkMenuGroup);
                                    }
                                }

                                Page.Controls.Add(tblOutput);
                            }
                            else
                            {
                                Response.Write("ERROR: No Entries Found!");
                            }

                            break;

                        case "GETUTILITIESBYROLEMASTER":
                            dtOutput = Generix.getUtilities(Convert.ToInt32(Session["Bank"]));
                            DataSet dt_set = new DataSet();
                            DataRow dt_row = dtOutput.NewRow();
                            dt_row["UTILITIES NAME"] = "SELECT / DESELECT ALL";
                            dtOutput.Rows.InsertAt(dt_row, 0);
                            dt_set.Tables.Add(dtOutput);

                            String[] UtilityRights = null;
                            try
                            {
                                // if ((Session["Username"]) != null)
                                // {

                                // if ((Session["Username"]).ToString() != "")
                                //  {
                                DataTable dtuserright = Generix.getRollMaper(Convert.ToInt32(Request.Form["RollId"]));
                                UtilityRights = dtuserright.Rows[0][2].ToString().Split(',');
                                // }
                                // }

                            }
                            catch (Exception Obx) { }
                            if (dtOutput.Rows.Count > 0)
                            {
                                Table tblOutput = Generix.convertDataTable2HTMLTable(dtOutput, true, false);
                                tblOutput.Attributes["class"] = "imagetable";
                                System.Web.UI.HtmlControls.HtmlInputCheckBox chkMenuGroup = null;

                                foreach (TableRow trOutput in tblOutput.Rows)
                                {
                                    if (trOutput.TableSection == TableRowSection.TableBody)
                                    {
                                        chkMenuGroup = new System.Web.UI.HtmlControls.HtmlInputCheckBox();
                                        chkMenuGroup.Attributes["style"] = "margin-left:80px;";
                                        chkMenuGroup.Attributes["type"] = "checkbox";
                                        chkMenuGroup.Attributes["Id"] = "chkBox2_" + trOutput.Cells[0].Text;
                                        //if ((Session["Username"]) != null)
                                        {
                                            if (UtilityRights.Contains(trOutput.Cells[0].Text))
                                            {
                                                chkMenuGroup.Checked = true;
                                            }
                                        }
                                        chkMenuGroup.Attributes["Class"] = "chkBoxUtilities";
                                        chkMenuGroup.Attributes["Group"] = "chkBoxUtilities";
                                        //chkMenuGroup.Attributes["disabled"] = "true";
                                        chkMenuGroup.Attributes["value"] = trOutput.Cells[0].Text;
                                        trOutput.Cells[0].Controls.Add(chkMenuGroup);
                                    }
                                }

                                Page.Controls.Add(tblOutput);
                            }
                            else
                            {
                                Response.Write("ERROR: No Entries Found!");
                            }

                            break;

                        case "GETREPORTGROUPBYROLEMASTERREADONLY":
                            dtOutput = Generix.getReportGroup(Convert.ToInt32(Session["Bank"]));

                            DataSet dtsetronly = new DataSet();
                            DataRow dtrowronly = dtOutput.NewRow();
                            dtrowronly["REPORT TYPE"] = "SELECT / DESELECT ALL";
                            dtOutput.Rows.InsertAt(dtrowronly, 0);
                            dtsetronly.Tables.Add(dtOutput);

                            String[] ReportRightsrdonly = null;
                            try
                            {
                                //if ((Session["Username"]) != null)
                                //{
                                //    if ((Session["Username"]).ToString() != "")
                                //    {
                                //if (Convert.ToString(Request.QueryString["RoleMaster"]) != null)
                                //{

                                //String RoleMaster = Convert.ToString(Request.QueryString["RoleMaster"]);
                                //DataTable dtuserright = Generix.getRollMaper(Convert.ToInt32(Session["RollId"]));

                                DataTable dtuserright = Generix.getRollMaper(Convert.ToInt32(Request.Form["RollId"]));
                                ReportRightsrdonly = dtuserright.Rows[0][1].ToString().Split(',');
                                //}
                                //}
                            }
                            catch (Exception Obx) { }

                            if (dtOutput.Rows.Count > 0)
                            {
                                Table tblOutput = Generix.convertDataTable2HTMLTable(dtOutput, true, false);
                                tblOutput.Attributes["class"] = "imagetable";
                                System.Web.UI.HtmlControls.HtmlInputCheckBox chkMenuGroup = null;

                                foreach (TableRow trOutput in tblOutput.Rows)
                                {
                                    if (trOutput.TableSection == TableRowSection.TableBody)
                                    {
                                        chkMenuGroup = new System.Web.UI.HtmlControls.HtmlInputCheckBox();
                                        chkMenuGroup.Attributes["style"] = "margin-left:80px;";
                                        chkMenuGroup.Attributes["type"] = "checkbox";
                                        chkMenuGroup.Attributes["Id"] = "chkBox1_" + trOutput.Cells[0].Text;
                                        //if ((Session["Username"]) != null)
                                        {
                                            if (ReportRightsrdonly.Contains(trOutput.Cells[0].Text))
                                            {
                                                chkMenuGroup.Checked = true;
                                            }

                                        }

                                        chkMenuGroup.Attributes["Class"] = "chkBoxMenuGroup";
                                        chkMenuGroup.Attributes["Group"] = "chkBoxMenuGroup";
                                        chkMenuGroup.Attributes["Disabled"] = "true";
                                        chkMenuGroup.Attributes["value"] = trOutput.Cells[0].Text;
                                        trOutput.Cells[0].Controls.Add(chkMenuGroup);
                                    }
                                }

                                Page.Controls.Add(tblOutput);
                            }
                            else
                            {
                                Response.Write("ERROR: No Entries Found!");
                            }

                            break;

                        case "GETUTILITIESBYROLEMASTERREADONLY":
                            dtOutput = Generix.getUtilities(Convert.ToInt32(Session["Bank"]));
                            DataSet dt_setro = new DataSet();
                            DataRow dt_rowro = dtOutput.NewRow();
                            dt_rowro["UTILITIES NAME"] = "SELECT / DESELECT ALL";
                            dtOutput.Rows.InsertAt(dt_rowro, 0);
                            dt_setro.Tables.Add(dtOutput);

                            String[] UtilityRightsro = null;
                            try
                            {
                                // if ((Session["Username"]) != null)
                                // {

                                // if ((Session["Username"]).ToString() != "")
                                //  {
                                DataTable dtuserright = Generix.getRollMaper(Convert.ToInt32(Request.Form["RollId"]));
                                UtilityRightsro = dtuserright.Rows[0][2].ToString().Split(',');
                                // }
                                // }

                            }
                            catch (Exception Obx) { }
                            if (dtOutput.Rows.Count > 0)
                            {
                                Table tblOutput = Generix.convertDataTable2HTMLTable(dtOutput, true, false);
                                tblOutput.Attributes["class"] = "imagetable";
                                System.Web.UI.HtmlControls.HtmlInputCheckBox chkMenuGroup = null;

                                foreach (TableRow trOutput in tblOutput.Rows)
                                {
                                    if (trOutput.TableSection == TableRowSection.TableBody)
                                    {
                                        chkMenuGroup = new System.Web.UI.HtmlControls.HtmlInputCheckBox();
                                        chkMenuGroup.Attributes["style"] = "margin-left:80px;";
                                        chkMenuGroup.Attributes["type"] = "checkbox";
                                        chkMenuGroup.Attributes["Id"] = "chkBox2_" + trOutput.Cells[0].Text;
                                        //if ((Session["Username"]) != null)
                                        {
                                            if (UtilityRightsro.Contains(trOutput.Cells[0].Text))
                                            {
                                                chkMenuGroup.Checked = true;
                                            }
                                        }
                                        chkMenuGroup.Attributes["Class"] = "chkBoxUtilities";
                                        chkMenuGroup.Attributes["Group"] = "chkBoxUtilities";
                                        chkMenuGroup.Attributes["disabled"] = "true";
                                        chkMenuGroup.Attributes["value"] = trOutput.Cells[0].Text;
                                        trOutput.Cells[0].Controls.Add(chkMenuGroup);
                                    }
                                }

                                Page.Controls.Add(tblOutput);
                            }
                            else
                            {
                                Response.Write("ERROR: No Entries Found!");
                            }

                            break;

                        case "INSERTROLEMASTER":
                            if (Generix.insertRole(Convert.ToString(Request.Form["Role"])))
                            {

                                Response.Write("1");
                            }
                            else
                            {
                                Response.Write("0");
                            }

                            break;

                        case "SETROLEMASTER":
                            if (Generix.UpdateRoleValues(Convert.ToString(Request.Form["sReportRights"]), Convert.ToString(Request.Form["sUtilitiesRights"]), Convert.ToInt32(Request.Form["RollId"])))
                            {

                                Response.Write("1");
                            }

                            break;


                        //start sheetal to clear checkboxes
                        case "INSERTUSER":
                            if (Generix.insertUser(Convert.ToString(Request.Form["userName"]), Convert.ToInt32(Session["Bank"]), Convert.ToString(Request.Form["userPass"]), Convert.ToString(Request.Form["userEmail"]), Convert.ToString(Request.Form["sMobileNo"]), Convert.ToInt32(Request.Form["sShowSensitiveData"]), Convert.ToInt32(Session["ActiveUserCode"]), Convert.ToInt32(Request.Form["Checker"]), Convert.ToInt32(Request.Form["RoleId"]), Convert.ToString(Request.Form["FirstName"]), (Convert.ToString(Request.Form["LastName"]))))
                            {
                                Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Report Generated", "INSERTUSER", "From:" + Convert.ToString(dFrom) + "|To:" + Convert.ToString(dTo));
                                (Session["Username"]) = "";
                                Response.Write("1");
                            }

                            break;

                        //change by abhishek for ATPCM 1632 end
                        case "GETUSERDETAIL":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getUserDetail(Convert.ToString(Request.Form["userName"]));

                            sOutput = "";
                            if (dtOutput != null)
                            {
                                if (dtOutput.Rows.Count > 0)
                                {
                                    foreach (DataColumn dcOutput in dtOutput.Columns)
                                    {
                                        sOutput += ((sOutput == "") ? "" : "|") + dtOutput.Rows[0][dcOutput].ToString();
                                    }
                                }
                                else
                                {
                                    throw new Exception("No Such User Found!");
                                }
                            }
                            else
                            {
                                throw new Exception("User Retreival Failed!");
                            }
                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Report Generated", "GETUSERDETAIL", "From:" + Convert.ToString(dFrom) + "|To:" + Convert.ToString(dTo));
                            Response.Write(sOutput);

                            break;


                        //case "UPDATEUSER":
                        //    if (Generix.insertUser(Convert.ToString(Request.Form["userCode"]), Convert.ToString(Request.Form["userRole"]), Convert.ToString(Request.Form["bankName"]), Convert.ToString(Request.Form["userPass"]), Convert.ToString(Request.Form["userEmail"]), Convert.ToInt32(Request.Form["sLocked"]), false, Convert.ToString(Request.Form["sUserRights"])))
                        //    {
                        //        Generix.auditLog(Session["IP"].ToString(), Convert.ToInt16(Session["LoginCode"]), "Update User", "");
                        //        Response.Write("1");
                        //    }
                        //    break;

                        case "FBNA":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getFBNAdeatils(dFrom, dTo);
                            if (sSkipDownloadFile != "Y") if (dtOutput.Rows.Count != 0) Response.Write("$" + Generix.CreateDownloadCSVFile(dtOutput, Server.MapPath("tempOutputs"), false, ",", "", "") + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, true));
                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Report Generated", Convert.ToString(aQueryString[0]), "FBNA");
                            break;

                        case "CASHPOS":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getCashPosdeatils(dFrom, dTo);
                            if (sSkipDownloadFile != "Y") if (dtOutput.Rows.Count != 0) Response.Write("$" + Generix.CreateDownloadCSVFile(dtOutput, Server.MapPath("tempOutputs"), false, ",", "", "") + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, true));
                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Report Generated", Convert.ToString(aQueryString[0]), "CASHPOS");

                            break;

                        /**** Terminal Configeration Start Here  ****/

                        case "GCHKDIGIT":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            try
                            {
                                if (String.IsNullOrEmpty(sData)) throw new Exception("");

                                /*ATPCM-842,Old Logic*/
                                // sData = Generix.getCheckDigit(sData);
                                /*END*/
                                sData = Generix.getChecksumbySrNo(sData);


                                if (!String.IsNullOrEmpty(sData))
                                {
                                    Response.Write(sData);
                                }
                                else
                                {
                                    Response.Write("Error");
                                }
                            }
                            catch (Exception xObj)
                            {
                                Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "GCHKDIGIT", "Exception", xObj.ToString());
                                Response.Write("Error");
                            }

                            break;

                        case "GCHKDIGIT2":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            try
                            {
                                if (String.IsNullOrEmpty(sData)) throw new Exception("");

                                sData = Generix.getCheckDigit(sData);
                                //sData = Generix.getChecksumbySrNo(sData);


                                if (!String.IsNullOrEmpty(sData))
                                {
                                    Response.Write(sData);
                                }
                                else
                                {
                                    Response.Write("Error");
                                }
                            }
                            catch (Exception xObj)
                            {
                                Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "GCHKDIGIT2", "Exception", xObj.ToString());
                                Response.Write("Error");
                            }

                            break;

                        case "GPINCODEDTL":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            try
                            {
                                using (dtOutput = Generix.getData("Select stateNo+'|'+stateCode+'|'+Left(Upper(City),13)+Replicate(' ',13-Len(Left(Upper(City),13))) From dbo.pincode Where countryCode='IN' And pincode='" + sData + "'", 3))
                                {
                                    if (dtOutput != null)
                                    {
                                        if (dtOutput.Rows.Count > 0)
                                        {
                                            Response.Write(dtOutput.Rows[0][0].ToString());
                                        }
                                    }
                                }
                            }
                            catch (Exception xObj)
                            {
                                Response.Write("");
                            }

                            break;

                        case "CTRML":
                        case "CCTRML":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            try
                            {
                                Generix.setTerminalConfig(Convert.ToInt32(Session["ParticipantID"]), ASCIIEncoding.ASCII.GetString(Convert.FromBase64String(sData)).Split('?'), sData, Convert.ToInt32(Session["ActiveUserCode"]), Convert.ToString(aQueryString[0]).Equals("CCTRML"));

                                Response.Write("1");
                            }
                            catch (Exception xObj)
                            {
                                Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "CCTRML", "Exception", xObj.ToString());
                                Response.Write(xObj.Message);
                            }

                            break;

                        case "GTRMLL":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            try
                            {
                                String sAdditionalParticipantID = Convert.ToString(Generix.getData("Select IsNull(AdditionalParticipantID,'') From dbo.Banks Where ParticipantID='" + Convert.ToString(Session["ParticipantID"]) + "'", 3).Rows[0][0]);

                                dtOutput = Generix.getTerminalConfig(Convert.ToInt32(Session["ParticipantID"]), "");

                                if (!String.IsNullOrEmpty(sAdditionalParticipantID))
                                {
                                    DataTable dtOutput2;

                                    foreach (String sParticipantID in sAdditionalParticipantID.Split(','))
                                    {
                                        if (!Convert.ToString(Session["ParticipantID"]).Equals(sParticipantID.Replace("'", "")))
                                        {
                                            dtOutput2 = Generix.getTerminalConfig(Convert.ToInt32(sParticipantID.Replace("'", "")), "");

                                            dtOutput.Merge(dtOutput2);
                                        }
                                    }
                                }

                                if (dtOutput != null)
                                {
                                    if (dtOutput.Rows.Count > 0)
                                    {
                                        Table oTable4 = Generix.convertDataTable2HTMLTable(dtOutput, true, false);
                                        System.Web.UI.HtmlControls.HtmlButton oButton;

                                        foreach (TableRow oRow in oTable4.Rows)
                                        {
                                            if (oRow.TableSection == TableRowSection.TableBody)
                                            {
                                                oButton = new System.Web.UI.HtmlControls.HtmlButton();
                                                oButton.Attributes["type"] = "button";
                                                oButton.Attributes["id"] = "btn_" + oRow.Cells[1].Text;
                                                oButton.Attributes["rel"] = oRow.Cells[8].Text;
                                                oButton.InnerText = "Edit";
                                                oButton.Attributes["class"] = "btnTerminalEditAction ui-corner-all";
                                                oButton.Style.Add("color", "#ffffff");
                                                oButton.Style.Add("background-color", (oRow.Cells[0].Text.ToUpper().Equals("PENDING") ? "#c40505" : "#609f19"));

                                                oRow.Cells[0].Controls.Add(oButton);

                                                oRow.Cells[1].Style.Add("color", "#ffffff");
                                                oRow.Cells[1].Style.Add("background-color", (oRow.Cells[5].Text.ToUpper().Equals("IN SERVICE") ? "#609f19" : "#c40505"));
                                            }
                                        }

                                        Page.Controls.Add(oTable4);
                                    }
                                    else
                                    {
                                        Response.Write("");
                                    }
                                }
                                else
                                {
                                    Response.Write("");
                                }
                            }
                            catch (Exception xObj)
                            {
                                Response.Write("Error - " + xObj.Message);
                            }

                            break;

                        case "GTRML":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            try
                            {
                                /*To check which bank is allow to Alpha numeric ATM ID*/
                                if (!Regex.IsMatch(sATM.Replace("'", ""), Convert.ToString(Session["RegularExpression_Terminal"]), RegexOptions.Compiled))
                                {
                                    //Response.Redirect("/Logout");
                                    throw new Exception("Invalid Terminal Id!");
                                }
                                else
                                {
                                    dtOutput = Generix.getTerminalConfig(Convert.ToInt32(sData), sATM.Replace("'", ""));
                                    sOutput = "";
                                    if (dtOutput != null)
                                    {
                                        if (dtOutput.Rows.Count > 0)
                                        {
                                            foreach (DataRow drOutput in dtOutput.Rows)
                                            {
                                                foreach (DataColumn dcOutput in dtOutput.Columns)
                                                {
                                                    if (dcOutput.ColumnName.Equals("created_by"))
                                                    {
                                                        sOutput += (String.IsNullOrEmpty(sOutput) ? "" : "?");
                                                        try
                                                        {
                                                            sOutput += Generix.getData("Select UserName From dbo.LoginUser Where Code=" + Convert.ToString(drOutput[dcOutput]), 3).Rows[0][0].ToString();
                                                        }
                                                        catch (Exception xObj)
                                                        {
                                                            sOutput += "0";
                                                        }
                                                    }
                                                    else
                                                        sOutput += (String.IsNullOrEmpty(sOutput) ? "" : "?") + Convert.ToString(drOutput[dcOutput]);
                                                }
                                            }
                                            string SerialNo = Generix.getSerialNoByCheckSum(Convert.ToString(dtOutput.Rows[0]["check_digits"]), Convert.ToString(dtOutput.Rows[0]["val_under_ksk"]));
    
                                            sOutput += "?" + Convert.ToString(SerialNo);

                                        }
                                    }
                                }

                                sOutput = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(sOutput));

                                Response.Write(sOutput);
                            }
                            catch (Exception xObj)
                            {
                                Response.Write("Error - " + xObj.Message);
                            }

                            break;

                        case "GTRMLEVNTS":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            try
                            {
                                dtOutput = Generix.getTerminalEvents(sATM.Replace("'", ""));

                                if (dtOutput != null)
                                {
                                    Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, false));
                                }
                            }
                            catch (Exception xObj)
                            {
                                Response.Write("Error - " + xObj.Message);
                            }

                            break;

                        case "GTRMLDV":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            try
                            {
                                dtOutput = Generix.getTerminalDeviceStatus(sATM.Replace("'", ""));

                                if (dtOutput != null)
                                {
                                    Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, false));
                                }
                            }
                            catch (Exception xObj)
                            {
                                Response.Write("Error - " + xObj.Message);
                            }

                            break;

                        case "CMDTRML":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }

                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Command Execute", "CMDTRML", "ATMID:" + sATM + "|Command:" + sData);
                            if (Generix.sendTerminalCommand(sATM.Replace("'", ""), sData.Replace("_", "")))
                            {
                                Response.Write("1");
                            }
                            else
                            {
                                Response.Write("0");
                            }

                            break;

                        case "CMDTRMLSTATE":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getTerminalCommandState(sATM.Replace("'", ""));

                            if (dtOutput != null)
                            {
                                Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, false));
                            }
                            else
                            {
                                Response.Write("");
                            }

                            break;

                        case "ATMRESYNC":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            try
                            {
                                if (Generix.sendResyncCommand("AtmApp", Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["atmappCommandPort"])))
                                //if (Generix.ResyncCommandBAT("AtmApp", Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["atmappCommandPort"]), Convert.ToInt32(Session["ActiveUserCode"])))
                                {
                                    Response.Write("1");
                                }
                                else
                                {
                                    Response.Write("0");
                                }
                            }
                            catch (Exception xObj)
                            {
                                Response.Write("-1");
                            }

                            break;

                        /** Terminal Configeration End Here**/

                        case "INSERTFEE":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.insertFee(Convert.ToInt32(Session["Bank"]), Convert.ToString(Request.Form["post_tranType"]), Convert.ToString(Request.Form["fee"]), Convert.ToInt16(Session["ActiveUserCode"]));
                            sOutput = "";

                            if (dtOutput != null)
                            {
                                if (dtOutput.Rows.Count > 0)
                                {
                                    foreach (DataRow drOutput in dtOutput.Rows)
                                    {
                                        sOutput += ((sOutput == "") ? "" : "~");
                                        foreach (DataColumn dcOutput in dtOutput.Columns)
                                        {
                                            sOutput += ((sOutput == "") ? "" : "|") + Convert.ToString(drOutput[dcOutput]);
                                        }
                                    }
                                }
                            }
                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Report Generated", Convert.ToString(aQueryString[0]), "Insert Fee");
                            Response.Write(sOutput.Replace("~", "|"));
                            break;

                        case "FEEDETAIL":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getFeeDetail(Convert.ToInt32(Session["Bank"]), Convert.ToString(Request.Form["tranType"]));
                            sOutput = "";

                            if (dtOutput != null)
                            {
                                if (dtOutput.Rows.Count > 0)
                                {
                                    foreach (DataRow drOutput in dtOutput.Rows)
                                    {
                                        sOutput += ((sOutput == "") ? "" : "~");
                                        foreach (DataColumn dcOutput in dtOutput.Columns)
                                        {
                                            sOutput += ((sOutput == "") ? "" : "|") + Convert.ToString(drOutput[dcOutput]);
                                        }
                                    }
                                }
                            }
                            Response.Write(sOutput.Replace("~", "|"));

                            break;
                        case "DREF":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getExtractFile(dFrom, "Excel");
                            Generix.CreateDownloadCSV_ExcelFile(dtOutput, Server.MapPath("Downloads") + "\\" + Session["ExtractsFolder"].ToString(), false, ",", "", "", "Excel");
                            dtOutput = Generix.getExtractFile(dFrom, "txt");
                            if (sSkipDownloadFile != "Y") if (dtOutput.Rows.Count != 0)
                                    Response.Write("@" + Generix.CreateDownloadCSV_ExcelFile(dtOutput, Server.MapPath("tempOutputs"), false, ",", "", "", "txt") + "|");

                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Report Generated", Convert.ToString(aQueryString[0]), "DREF");

                            break;

                        case "AFD":

                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getAadharFeedDetails(dFrom, dTo, Convert.ToString(Session["IssuerNo"]));
                            if (sSkipDownloadFile != "Y") if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, true));
                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Report Generated", Convert.ToString(aQueryString[0]), "AFD");
                            break;

                        case "DOR":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getDiscountOfferDetails(dFrom, dTo);
                            if (sSkipDownloadFile != "Y") if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, true));
                            //Generix.CreateDownloadCSV_ExcelFile(dtOutput, Server.MapPath("Downloads") + "\\" + Session["ExtractsFolder"].ToString(), false, ",", "", "", "Excel");

                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Report Generated", Convert.ToString(aQueryString[0]), "DOR");

                            break;
                        case "ADMINLOGIN":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            try
                            {


                                dtOutput = Generix.getUserDetails((Session["BANK"]).ToString());

                                if (dtOutput != null)
                                {


                                    if (dtOutput.Rows.Count > 0)
                                    {
                                        Table oTable4 = Generix.convertDataTable2HTMLTable(dtOutput, true, false);
                                        System.Web.UI.HtmlControls.HtmlButton oButton;
                                        System.Web.UI.HtmlControls.HtmlButton oButton1;
                                        System.Web.UI.HtmlControls.HtmlButton oButton2;
                                        foreach (TableRow oRow in oTable4.Rows)
                                        {
                                            if (oRow.TableSection == TableRowSection.TableBody)
                                            {
                                                oButton = new System.Web.UI.HtmlControls.HtmlButton();
                                                oButton1 = new System.Web.UI.HtmlControls.HtmlButton();
                                                oButton2 = new System.Web.UI.HtmlControls.HtmlButton();

                                                oButton.Attributes["type"] = "button";
                                                oButton.Attributes["id"] = "btn_" + oRow.Cells[3].Text;
                                                oButton.Attributes["rel"] = oRow.Cells[3].Text;
                                                oButton.InnerText = "Edit";
                                                oButton.Attributes["class"] = "btnEditAction ui-corner-all";
                                                oButton.Style.Add("color", "#609f19");
                                                oRow.Cells[0].Controls.Add(oButton);

                                                //oRow.Cells[2].Style.Add("color", "#a8d8eb;");
                                                oButton1.Attributes["type"] = "button";
                                                oButton1.Attributes["id"] = "btn1_" + oRow.Cells[3].Text;
                                                oButton.Attributes["rel"] = oRow.Cells[3].Text;
                                                if (oRow.Cells[6].Text == "ACTIVE") { oButton1.InnerText = "DE-ACTIVATE"; }
                                                else if (oRow.Cells[6].Text == "DE-ACTIVE") { oButton1.InnerText = "ACTIVATE"; oButton.Disabled = true; }
                                                else
                                                {
                                                    oButton1.InnerText = "ACTIVATE";
                                                    oButton1.Disabled = true;
                                                }

                                                oButton1.Attributes["class"] = "btnActiveAction ui-corner-all";
                                                oButton1.Style.Add("color", "#609f19");


                                                oRow.Cells[1].Controls.Add(oButton1);

                                                //oRow.Cells[2].Style.Add("color", "#a8d8eb;");
                                                oButton2.Attributes["type"] = "button";
                                                oButton2.Attributes["id"] = "btn_" + oRow.Cells[3].Text;
                                                oButton2.Attributes["rel"] = oRow.Cells[3].Text;
                                                oButton2.InnerText = "DELETE";
                                                if (oRow.Cells[6].Text == "DELETED")
                                                {
                                                    oButton2.Disabled = true;
                                                    oButton1.Disabled = true;
                                                    oButton.Disabled = true;
                                                }
                                                oButton2.Attributes["class"] = "btnDeleteAction ui-corner-all";
                                                oButton2.Style.Add("color", "#609f19");


                                                oRow.Cells[2].Controls.Add(oButton2);


                                            }
                                        }

                                        Page.Controls.Add(oTable4);
                                    }
                                    else
                                    {
                                        Response.Write("");
                                    }
                                }
                                else
                                {
                                    Response.Write("");
                                }
                            }
                            catch (Exception xObj)
                            {
                                Response.Write("Error - " + xObj.Message);
                            }

                            break;
                        case "DELETEUSER":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            if (Generix.DELETEUSER((Convert.ToString(Request.Form["USERNAME"])), (Convert.ToString(Request.Form["Status"])), Convert.ToString(Session["ActiveUser"]))) ;
                            {
                                Response.Write("1");
                            }

                            break;

                        case "DELETEROLE":
                            if (Generix.DELETEROLE((Convert.ToString(Request.Form["RollMaster"])))) ;
                            {
                                Response.Write("1");
                            }

                            break;

                        case "ROLEDISPLAY":
                            try
                            {


                                dtOutput = Generix.FunfillRoleMastergridView();

                                if (dtOutput != null)
                                {


                                    if (dtOutput.Rows.Count > 0)
                                    {
                                        Table oTable4 = Generix.convertDataTable2HTMLTable(dtOutput, true, false);
                                        System.Web.UI.HtmlControls.HtmlButton oButton;
                                        //System.Web.UI.HtmlControls.HtmlButton oButton1;
                                        System.Web.UI.HtmlControls.HtmlButton oButton2;
                                        foreach (TableRow oRow in oTable4.Rows)
                                        {
                                            if (oRow.TableSection == TableRowSection.TableBody)
                                            {
                                                oButton = new System.Web.UI.HtmlControls.HtmlButton();
                                                //oButton1 = new System.Web.UI.HtmlControls.HtmlButton();
                                                oButton2 = new System.Web.UI.HtmlControls.HtmlButton();

                                                oButton.Attributes["type"] = "button";
                                                oButton.Attributes["id"] = "btn_" + oRow.Cells[1].Text;
                                                oButton.Attributes["rel"] = oRow.Cells[1].Text;
                                                oButton.InnerText = "Edit";
                                                oButton.Attributes["class"] = "btnEditAction ui-corner-all";
                                                oButton.Style.Add("color", "#609f19");
                                                oRow.Cells[0].Controls.Add(oButton);

                                                oRow.Cells[1].Style.Add("color", "#a8d8eb;");
                                                //oButton1.Attributes["type"] = "button";
                                                //oButton1.Attributes["id"] = "btn1_" + oRow.Cells[1].Text;
                                                //oButton.Attributes["rel"] = oRow.Cells[1].Text;
                                                //if (oRow.Cells[6].Text == "ACTIVE") { oButton1.InnerText = "DE-ACTIVATE"; }
                                                //else if (oRow.Cells[6].Text == "DE-ACTIVE") { oButton1.InnerText = "ACTIVATE"; oButton.Disabled = true; }
                                                //else
                                                //{
                                                //    oButton1.InnerText = "ACTIVATE";
                                                //    oButton1.Disabled = true;
                                                //}

                                                //oButton1.Attributes["class"] = "btnActiveAction ui-corner-all";
                                                //oButton1.Style.Add("color", "#609f19");


                                                //oRow.Cells[1].Controls.Add(oButton1);

                                                oRow.Cells[1].Style.Add("color", "#a8d8eb;");
                                                oButton2.Attributes["type"] = "button";
                                                oButton2.Attributes["id"] = "btn_" + oRow.Cells[1].Text;
                                                oButton2.Attributes["rel"] = oRow.Cells[1].Text;
                                                oButton2.InnerText = "DELETE";
                                                //if (oRow.Cells[6].Text == "DELETED")
                                                //{
                                                //    oButton2.Disabled = true;
                                                //    oButton1.Disabled = true;
                                                //    oButton.Disabled = true;
                                                //}
                                                oButton2.Attributes["class"] = "btnDeleteAction ui-corner-all";
                                                oButton2.Style.Add("color", "#609f19");


                                                oRow.Cells[1].Controls.Add(oButton2);

                                            }
                                        }

                                        Page.Controls.Add(oTable4);
                                    }
                                    else
                                    {
                                        Response.Write("");
                                    }
                                }
                                else
                                {
                                    Response.Write("");
                                }
                            }
                            catch (Exception xObj)
                            {
                                Response.Write("Error - " + xObj.Message);
                            }

                            break;

                        case "GETUSERRIGHT":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getUserRightsDetail(Convert.ToString(Request.Form["USERNAME"]));
                            if (dtOutput != null)
                            {


                                if (dtOutput.Rows.Count > 0)
                                {
                                    //Response.Redirect("UserManagement.aspx");
                                    Response.Write("1");
                                }
                            }

                            break;

                        case "GETUSEREMAILS":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            try
                            {

                                String cOperation = Request.Form["cOperation"];
                                if (cOperation == "GET_EMAIL")
                                {

                                    Int32 id_mail = Convert.ToInt32(Request.Form["id"]);
                                    //string UserName = Request.Form["User"];
                                    dtOutput = Generix.getEmailDetails(cOperation, id_mail);
                                }
                                else if (cOperation == "UPDATE_EMAIL")
                                {
                                    string cEmail = Convert.ToString(Request.Form["cEmail"]).Replace("\n", "").TrimEnd(',');

                                    if (cEmail != "" && cEmail != null)
                                    {

                                        String[] Mails = cEmail.Split(',');

                                        for (int i = 0; i < Mails.Length; i++)
                                        {
                                            for (int j = 0; j < Mails.Length; j++)
                                            {
                                                if (i != j)
                                                {
                                                    if (Mails[i].ToString().ToUpper() == Mails[j].ToString().ToUpper())
                                                    {
                                                        Response.Write("Error:Duplicate Mail id : " + Mails[i]);
                                                        return;
                                                    }
                                                }
                                            }

                                            if (!Regex.IsMatch(Mails[i], @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
                                            {
                                                Response.Write("Error:Invalid Mail fromat : " + Mails[i]);
                                                return;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Response.Write("Error:Invalid Mail fromat.please enter email.");
                                        return;

                                    }
                                    Int32 id_mail = Convert.ToInt32(Request.Form["id"]);

                                    dtOutput = Generix.getEmailDetails(cOperation, id_mail, cEmail, Convert.ToString(Session["ActiveUser"]));
                                }
                                else
                                {

                                    string cEmail = Convert.ToString(Request.Form["cEmail"]).Replace("\n", "").TrimEnd(',');
                                    if (cEmail != "" && cEmail != null)
                                    {

                                        String[] Mails = cEmail.Split(',');

                                        for (int i = 0; i < Mails.Length; i++)
                                        {
                                            for (int j = 0; j < Mails.Length; j++)
                                            {
                                                if (i != j)
                                                {
                                                    if (Mails[i].ToString().ToUpper() == Mails[j].ToString().ToUpper())
                                                    {
                                                        Response.Write("Error:Duplicate Mail id : " + Mails[i]);
                                                        return;
                                                    }
                                                }
                                            }

                                            if (!Regex.IsMatch(Mails[i], @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
                                            {
                                                Response.Write("Error:Invalid Mail fromat : " + Mails[i]);
                                                return;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Response.Write("Error:Invalid Mail fromat.please enter email.");
                                        return;
                                    }

                                    string cReportName = Request.Form["cReportName"];
                                    //string UserName = Request.Form["User"];
                                    dtOutput = Generix.SetEmailDetails(cOperation, cReportName, cEmail, Convert.ToString(Session["ActiveUser"]));
                                }

                                if (dtOutput != null)
                                {


                                    if (dtOutput.Rows.Count > 0)
                                    {
                                        //Response.Redirect("UserManagement.aspx");
                                        Response.Write(Convert.ToString(dtOutput.Rows[0][0]));
                                    }
                                    else
                                    {
                                        Response.Write("Error while fetching Emails");

                                    }
                                }


                            }
                            catch (Exception xObj)
                            {
                                Response.Write("Error - " + xObj.Message);
                            }
                            break;


                        //add by uddesh for email master  jira iD ATPBF-853 END


                        ///*Added for international usage RBL-ATPCM-862* START/
                        case "EDIU":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            try
                            {
                                String strCardNo, strIntUse;

                                strCardNo = Convert.ToString(Request.Form["Cards"]);
                                strIntUse = Request.Form["IntUse"].ToString();

                                //if (strCardNo != "" && strCardNo != null)
                                //{                             
                                //    if (!Regex.IsMatch(strCardNo, "^[0-9,%]*$", RegexOptions.Compiled))
                                //    {
                                //        Response.Write("0|Error - Invalid Argument!");
                                //    }
                                //}



                                if (strCardNo != "" && strIntUse != "")
                                {

                                    DataTable dtIntNaData = Generix.fnInsternationalUsageData(Convert.ToInt32(Session["Bank"]), strCardNo, strIntUse, Convert.ToString(Session["ActiveUser"]));

                                    if (dtIntNaData != null)
                                    {
                                        if (dtIntNaData.Rows.Count == 0)
                                        {
                                            Response.Write("0|Error while uploading international usage request");

                                        }
                                        if (Convert.ToString(dtIntNaData.Rows[0][0]) == "0")
                                        {
                                            Response.Write(Convert.ToString(dtIntNaData.Rows[0][0] + "|" + dtIntNaData.Rows[0][1]));

                                        }
                                    }
                                    else
                                    {
                                        Response.Write("0|Error while uploading international usage request");

                                    }

                                    string SourceID = string.Empty;
                                    string SwitchRsp = string.Empty;
                                    string requesttype = string.Empty;
                                    ClscardBlockUnblock ObjBlockUnblock = new ClscardBlockUnblock();
                                    APIMessage ObjAPImsg = ObjBlockUnblock.generateBlockUnblockRequets(Convert.ToString(Session["Bank"]), strIntUse == "0" ? "00" : "41", sCards.Replace("'", ""), Convert.ToString(Session["AcquirerId"]), Convert.ToString(Session["SwitchType"]), "1");

                                    SourceID = ObjBlockUnblock.getsourceId(Convert.ToString(Session["Bank"]), Convert.ToString(Session["SwitchType"]));
                                    requesttype = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["API_Request_Type"]);
                                    SwitchRsp = new CallCardAPI().Process(requesttype, ObjAPImsg, SourceID);

                                    DataTable dtUpdate = Generix.fnUpdateInsternationalUsageData(SwitchRsp, Convert.ToInt32(dtIntNaData.Rows[0][1]));
                                    if (dtUpdate != null)
                                    {
                                        if (dtUpdate.Rows.Count == 0)
                                        {
                                            Response.Write("0|Error while updating switch response");

                                        }


                                        if (SwitchRsp.Equals("000"))
                                        {
                                            Response.Write(Convert.ToString(dtUpdate.Rows[0][0]));
                                        }
                                        else
                                        {

                                            Response.Write("0|" + (strIntUse == "1" ? "International usage Block Fail" : "International usage unblock Fail!"));
                                        }


                                    }
                                    else
                                    {
                                        Response.Write("0|Error while updating switch response");

                                    }



                                }
                                else
                                {
                                    Response.Write("0|Error - Parameters Missing!");

                                }
                            }
                            catch (Exception xObj)
                            {
                                Response.Write("0|Error - " + xObj.Message);
                            }
                            break;

                        case "IUED":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            dtOutput = Generix.getIUED(Convert.ToInt32(Session["Bank"]), dFrom, dTo);

                            //if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateDownloadFile(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            if (dtOutput.Rows.Count != 0) Response.Write(Generix.CreateExcel(dtOutput, Server.MapPath("tempOutputs")) + "|");
                            Page.Controls.Add(Generix.convertDataTable2HTMLTable(dtOutput, true, true));
                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Report Generated", Convert.ToString(aQueryString[0]), "From:" + Convert.ToString(dFrom) + "|To:" + Convert.ToString(dTo));

                            break;

                        ///*Added for international usage RBL-ATPCM-862* END/

                        /// ATPCM - 1245

                        case "GRGP":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            try
                            {
                                String From;
                                Int32 Reportid;

                                Reportid = Convert.ToInt32(Request.Form["Reportid"]);
                                From = Convert.ToString(Request.Form["From"]).Substring(0, 10);

                                dtOutput = Generix.getReportPath(Reportid, From);

                                if (dtOutput != null)
                                {
                                    if (dtOutput.Rows.Count > 0)
                                    {
                                        Table oTable4 = Generix.convertDataTable2HTMLTable(dtOutput, true, false);
                                        System.Web.UI.HtmlControls.HtmlButton oButton;
                                        System.Web.UI.HtmlControls.HtmlLink olink;
                                        foreach (TableRow oRow in oTable4.Rows)
                                        {
                                            if (oRow.TableSection == TableRowSection.TableBody)
                                            {
                                                oButton = new System.Web.UI.HtmlControls.HtmlButton();
                                                oButton.Attributes["type"] = "button";
                                                oButton.Attributes["id"] = "btn_" + oRow.Cells[0].Text;
                                                oButton.Attributes["rel"] = oRow.Cells[1].Text;
                                                oButton.InnerText = "Download";
                                                oButton.Attributes["class"] = "btnReptDown ui-corner-all";
                                                oButton.Style.Add("color", "#ffffff");
                                                oButton.Style.Add("background-color", "#609f19");

                                                oRow.Cells[0].Controls.Add(oButton);


                                                //oRow.Cells[1].Style.Add("color", "#ffffff");
                                                //oRow.Cells[1].Style.Add("background-color", "#c40505");
                                            }
                                        }

                                        Page.Controls.Add(oTable4);
                                    }
                                    else
                                    {
                                        Response.Write("");
                                    }
                                }
                                else
                                {
                                    Response.Write("");
                                }
                            }
                            catch (Exception xObj)
                            {
                                Response.Write("Error - " + xObj.Message);
                            }

                            break;


                        case "DRFL":
                            if (Request.HttpMethod.ToUpper() != "POST")
                            {
                                throw new Exception("Invalid Http Method");
                            }
                            String PathFile = Convert.ToString(Request.Form["FilePath"]);


                            string filePath = Server.MapPath(PathFile);
                            HttpResponse res = HttpContext.Current.Response;
                            res.Clear();
                            res.AppendHeader("content-disposition", "attachment; filename=" + filePath);
                            res.ContentType = "application/octet-stream";
                            res.WriteFile(filePath);
                            res.Flush();
                            res.End();


                            break;


                        //ATPCM-1245 end

                        default:
                            Response.Write("0|Invalid Function!");
                            Generix.auditLog(Convert.ToString(Session["ActiveUser"]), "Unauthorized Access To Reports", Convert.ToString(aQueryString[0]), "");

                            break;
                    }
                }
                else
                {
                    throw new Exception("Invalid Argument!");
                }
            }
            catch (Exception xObj)
            {
                Response.Write("ERROR: " + xObj.Message);
            }
        }

    }
}
