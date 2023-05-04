using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Reports
{
    public class ClsincomingMasterCardChargeback
    {
        public string PAN { get; set; }
        public string ProcessingCode { get; set; }
        public string Amount { get; set; }
        public string ConversionRate { get; set; }
        public string TxnDate { get; set; }
        public string MEName { get; set; }
        public string POSDataCode { get; set; }
        public string Functioncode { get; set; }
        public string ReasonCode { get; set; }
        public string MCC { get; set; }
        
        public string ARN { get; set; }
        public string Auth_Code { get; set; }
        public string RRN { get; set; }
        public string MECity { get; set; }
        public string MECountry { get; set; }
        
        public string TID { get; set; }
        public string MID { get; set; }
        public string DocIndicator { get; set; }
        public string CC { get; set; }
        public string MessageTXT { get; set; }
        public string CRID { get; set; }
        public string TxnOriginatorID { get; set; }
        public string ReceiverID { get; set; }
        public string FileID { get; set; }
        public string Bank { get; set; }
        public string CBDate { get; set; }
        
         
        
    }
}