using System.Linq;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using ReflectionIT.Common.Data.Configuration;
using ReflectionIT.Common.Data.SqlClient;
using OfficeOpenXml;
using System.Data.OleDb;

using IPM;
using System.Globalization;
using System;
using System.Collections.Generic;
//using Convertor;

namespace Reports
{
    public partial class MasterCardReadTT112 : System.Web.UI.Page
    {
        string uploadFileName = " ", data = "";
        int orgCount = 0;
        int dupCount = 0;
        private string FileID;
        private string CRID;
        private string ApprovalCode;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack && MCChargeBackFileUpload.HasFile)
            {
                uploadFileName = MCChargeBackFileUpload.FileName;

                MCChargeBackFileUpload.SaveAs(Server.MapPath("tempOutputs") + "\\" + uploadFileName);

                // String[] streamdata = File.ReadAllLines(Server.MapPath("tempOutputs") + "\\" + uploadFileName);
                //StreamReader sr = new StreamReader(Server.MapPath("tempOutputs") + "\\" + uploadFileName);
                //data = sr.ReadToEnd();

                //sr.Close();
               // ClsConvertor conv = new ClsConvertor();

                Byte[] AscipmData = System.IO.File.ReadAllBytes(Server.MapPath("tempOutputs") + "\\" + uploadFileName);


                ClsISOReader iso8583 = new ClsISOReader();


                List<Record> lstRecords = new List<Record>();
                lstRecords = iso8583.FunParseData(AscipmData);

                List<ClsincomingMasterCardChargeback> CBList = new List<ClsincomingMasterCardChargeback>();

                foreach (Record obj in lstRecords)
                {
                    if (obj.messageHeader == "1442" && (obj.FunctionCode == "450" || obj.FunctionCode == "451" || obj.FunctionCode == "453" || obj.FunctionCode == "454"))
                    {//chargeback generation
                        ClsincomingMasterCardChargeback CB = new ClsincomingMasterCardChargeback();
                        CB.PAN = Convert.ToString(obj.data[2]);
                        CB.ProcessingCode = Convert.ToString(obj.data[3]);
                        CB.Amount = Convert.ToString(obj.data[4]);
                        //CB.Billing_Amt = Convert.ToString(obj.data[]);
                        CB.Auth_Code = Convert.ToString(obj.data[38]);
                        CB.ARN = Convert.ToString(obj.data[31]);
                        //    CB.DocIndicator = Convert.ToString(obj.data[262]);
                        CB.MEName = Convert.ToString(obj.data[43]).Split('\\')[0];
                        CB.MECity = Convert.ToString(obj.data[43]).Split('\\')[2];
                        CB.MECountry = Convert.ToString(obj.data[43]).Split('\\')[3].Substring(13, 2);
                        CB.CC = Convert.ToString(obj.data[49]);
                        CB.ConversionRate = Convert.ToString(obj.data[9]);
                        CB.CRID = Convert.ToString(obj.data[95]);
                        CB.FileID = Convert.ToString(obj.data[48]);
                        CB.Functioncode = Convert.ToString(obj.data[24]);
                        CB.MCC = Convert.ToString(obj.data[26]);
                        CB.MessageTXT = Convert.ToString(obj.data[72]);
                        CB.POSDataCode = Convert.ToString(obj.data[22]);
                        CB.ReasonCode = Convert.ToString(obj.data[25]);
                        CB.ReceiverID = Convert.ToString(obj.data[100]);
                        CB.RRN = Convert.ToString(obj.data[37]);
                        CB.TID = Convert.ToString(obj.data[41]);
                        CB.MID = Convert.ToString(obj.data[42]);
                        DateTime a = DateTime.ParseExact(obj.data[12].ToString().Substring(0, 6).Trim(), "yyMMdd", CultureInfo.InvariantCulture);
                        CB.TxnDate = string.Format("{0:yyyyMMdd}", a);
                        CB.TxnOriginatorID = Convert.ToString(obj.data[94]);
                        CB.Bank = Session["BankName"].ToString();
                        CB.CBDate = uploadFileName.Substring(8, 10);

                        if (Generix.isAvailable("dbo.Chargeback", "FileID='" + CB.FileID + "'and MID='" + CB.MID + "'and AuthCode='" + CB.Auth_Code + "'", 3))
                        {
                            dupCount++;
                        }
                        else
                        {
                            Generix.FunStoreMCChargeback(CB.PAN, CB.ProcessingCode, CB.Amount, CB.Auth_Code, CB.ARN, CB.MEName, CB.MECity, CB.MECountry, CB.CC, CB.ConversionRate, CB.CRID, CB.FileID, CB.Functioncode, CB.MCC, CB.MessageTXT, CB.POSDataCode, CB.ReasonCode, CB.ReceiverID, CB.RRN, CB.TID, CB.MID, CB.TxnDate, CB.TxnOriginatorID, CB.DocIndicator, CB.Bank, CB.CBDate);
                            CBList.Add(CB);
                            orgCount++;
                        }



                    }

                }

                GridView1.DataSource = CBList;
                GridView1.DataBind();
                ClientScript.RegisterClientScriptBlock(this.GetType(), "TotalInsert", "<script>$(document).ready(function(){$('#divDialog').html('Toal " + orgCount + " Rows inserted successfully! " + dupCount + " Rows Duplicate!').dialog({title: 'Error...',show: 'slide',hide: 'blind',modal: true,buttons: {'Ok': function () {$(this).dialog('close');$(this).dialog('destroy');}}});});</script>");
            }
        }
    }
}