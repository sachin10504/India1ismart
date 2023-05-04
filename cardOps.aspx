<%@ Page Title="" Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="cardOps.aspx.cs" Inherits="Reports.cardOps" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" language="javascript">
        $(document).ready(function() {
            
            cardOpsPageLoad(document.getElementById("frmCardOps"));
            

            $('#cmdReset,#cmdResetAll').click(function() { cardOpsPageLoad(); });
        });
    </script>
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="divContentWrapper" >
        <%--<asp:HiddenField ID="hdnholdResp" runat="server"  />--%>
        <input type="hidden" id="hdnholdResp"/>
        <div class="ui-widget-header">Card Operations - Search Criterion</div>
        <div>
            <form id="frmCardOps" runat="server"><button id="btnApplyCardOperationsAccessRights" type="button" style="display:none"></button>
            <table width="100%">
                <tr>
                    <td>Card Number</td>
                    <td><input class="textNumeric" type="text" id="txtCardNum" style="width: 150px " maxlength="16"  /><input type="text" id="txtEncCardNum" style="display: none" /><select id="cmbCardNum"></select></td>
                    <td>Customer Id</td>
                    <td><input class="textNumeric" type="text" id="txtCustId" style="width: 150px" /></td>
                    <td>Account Number</td>
                    <td colspan="2"><input class="textNumeric" type="text" id="txtAccNum" style="width: 150px" /></td>
                    <td><button id="cmdSearch" type="button" style="display:none">Search</button><button id="cmdReset" type="button" style="display:none">Reset</button></td>
                </tr>
                <tr class="ui-widget-header">
                    <td colspan="8">Customer Details</td>
                </tr>
                <tr>
                    <td>Customer Id</td>
                    <td class="tdDataHolders" id="lblCustId" style="text-align:right;"></td>
                    <td>Name</td>
                    <!--<td class="tdDataHolders" colspan="2" id="lblCustName"></td>-->
                    <td><input class="modifiable" id="txtCustName" type="text" style="width: 250px" /></td>
                    <td>Date Of Birth</td>
                    <td class="tdDataHolders" id="lblCustDOB" colspan="2"></td>
                </tr>
                <tr>
                    <td>Address</td>
                    <td class="tdDataHolders modifiable" colspan="7" id="lblAddress"></td>
                </tr>
                <tr>
                    <td>Mobile Number</td>
                    <td><input class="textNumeric modifiable" id="txtMobile" type="text" style="width: 150px" /></td>
                    <td>Email Id</td>
                    <td><input class="modifiable" id="txtEmail" type="text" style="width: 250px" /></td>
                    <%--change by uddesh ATPCM-862 Start--%>
                    <td >International Usage Block</td>
                    <td><input id="chkIntUsage" type="checkbox" style="width: 20px" /></td>
                </tr>
                <tr>
                    <td>Accounts Linked</td>
                    <td class="tdDataHolders" colspan="7" id="lblAccountsCustLinked"></td>
                </tr>
                <tr class="ui-widget-header">
                    <td colspan="8">Card Details</td>
                </tr>                
                <tr>
                    <td>Card Number</td>
                    <td class="tdDataHolders" id="lblCardNum" style="text-align:right;"></td>
                    <td>Card Program</td>
                    <td class="tdDataHolders" id="lblCardProgram"></td>
                    <td>Expiry Date</td>
                    <td class="tdDataHolders" id="lblCardExpiryDate" colspan="2"></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Card Status</td>
                    <td class="tdDataHolders" id="lblCardStatus"></td>
                    <td>Hold Response Code</td>
                    <td>
                        <select id="cmbHoldResponseCode" multiple="multiple">
                            <option value="NULL" selected="selected">None</option>
                            <option value="00">00 - Unblock Card</option>
                            <option value="01" disabled="disabled">01 - Refer To Card Issuer</option>
                            <option value="05" >05 - Temporary Block</option>                        
                            <%--<option value="05" disabled="disabled">05 - Do Not Honor</option>--%>
                            <%--<option value="06">06 - Temporary Block</option>--%>
                            <option value="14" disabled="disabled">14 - Invalid Card Number</option>
                            <option value="36" disabled="disabled">36 - Restricted Card:Pick Up</option>
                            <option value="41">41 - Lost Card</option>
                            <option value="43">43 - Stolen Card</option>
                            <option value="45" disabled="disabled">45 - Account Closed</option>
                            <option value="54" disabled="disabled">54 - Expired Card</option>
                            <option value="59" disabled="disabled">59 - Suspected Fraud</option>
                            <option value="62" disabled="disabled">62 - Restricted Card</option>
                        </select>
                        <input type="text" id="txtHoldResponseCode" style="display: none" />                   
                    </td>
                    <td>Card Issued</td>
                    <td class="tdDataHolders" id="lblCardIssuedOn" colspan="2"></td>
                    <td style="display: none" class="tdDataHolders" id="lblCardActivatedOn"></td>
                </tr>
                <tr>
                    <td>Accounts Linked</td>
                    <td class="tdDataHolders" colspan="7" id="lblAccountsLinked"></td>
                </tr>
                <tr class="ui-widget-header">
                    <td colspan="8">Card Limits</td>
                </tr>
                <tr>
                    <td>No of Purchases</td>
                    <td><input class="textNumeric modifiable limits" id="txtNoOfPurchases" type="text" style="width: 150px" /></td>
                    <td>Daily Purchase Amount</td>
                    <td><input class="textDecimal modifiable limits" id="txtDailyPurchaseAmt" type="text" style="width: 150px" /></td>
                    <td>PT Purchase Amount</td>
                    <td colspan="2"><input class="textDecimal modifiable limits" id="txtPTPurchaseAmt" type="text" style="width: 150px" /></td>
                    <td></td>
                </tr>
                <tr>
                    <td>No of Withdrawals</td>
                    <td><input class="textNumeric modifiable limits" id="txtNoOfWithdrawals" type="text" style="width: 150px" /></td>
                    <td>Daily Withdrawal Amount</td>
                    <td><input class="textDecimal modifiable limits" id="txtDailyWithdrawalAmt" type="text" style="width: 150px" /></td>
                    <td>PT Withdrawal Amount</td>
                    <td colspan="2"><input class="textDecimal modifiable limits" id="txtPTWithdrawalAmt" type="text" style="width: 150px" /></td>
                    <td></td>
                </tr>
                <tr>
                    <td>No of Payments</td>
                    <td><input class="textNumeric modifiable limits" id="txtNoOfPayments" type="text" style="width: 150px" /></td>
                    <td>Daily Payment Amount</td>
                    <td><input class="textDecimal modifiable limits" id="txtDailyPaymentAmt" type="text" style="width: 150px" /></td>
                    <td>PT Payment Amount</td>
                    <td colspan="2"><input class="textDecimal modifiable limits" id="txtPTPaymentAmt" type="text" style="width: 150px" /></td>
                    <td></td>
                </tr>
                <tr>
                    <td colspan="2"></td>
                    <td>Daily CNP Amount</td>
                    <td><input class="textDecimal modifiable limits" id="txtDailyCNPAmt" type="text" style="width: 150px" /></td>
                    <td>PT CNP Amount</td>
                    <td colspan="2"><input class="textDecimal modifiable limits" id="txtPTCNPAmt" type="text" style="width: 150px" /></td>
                    <td></td>
                </tr>
                <tr class="ui-widget-header">
                    <td colspan="8">Remarks</td>
                </tr> 
                <tr>
                    <td class="tdDataHolders" colspan="2" id="lblRemarks" style="border-right-style: solid; border-right-color: #000000; border-right-width: thin;"></td>
                    <td class="tdDataHolders" colspan="2" id="lblRemarks2"></td><td class="tdDataHolders" colspan="3" id="lblRemarks3"></td>
                </tr>
                <tr>
                    <td colspan="8" align="center">
                        <button id="cmdSave" type="button">Save</button>
                        <button id="cmdResetAll" type="button">Reset</button>
                        <button id="cmdUploadData" type="button">Upload Customer Data</button>
                        <!--<input type="file" style="display: none" name="fileUploadData" id="fileUploadData" />-->
                        <asp:FileUpload ID="fileUploadData" CssClass="hideMe fileUploadData" runat="server" />
                        <button id="cmdHotlist" type="button">Hotlist</button>
                        <button id="cmdRepin" type="button">Repin</button>
                        <!--<button id="cmdReissue" type="button">Reissue</button>-->
                        <button id="cmdAddonrequest" type="button">Add On Request</button>
                        <button id="cmdUploadReissue_" type="button">Upload Reissue</button>
                        <asp:FileUpload ID="fileUploadReissue" CssClass="hideMe fileUploadReissue" runat="server" />
                        <button id="cmdUploadhotlist" type="button">Upload HotList</button>
                        <asp:FileUpload ID="fileUploadHotlist" CssClass="hideMe fileUploadHotlist" runat="server" />
                        <button id="cmdInstaPIN" type="button">InstaPIN</button>
                           <%--change by uddesh ATPCM-862 Start--%>
                        <button id="btnIntNatUsage" type="button">International Usage</button>
                        <div>
                            <button id="cmdReissue_" type="button">Reissue</button>
                            <button id="cmdSelectReissue_" type="button">Select BIN</button>
                        </div>
                        <ul style="text-align: left; overflow: scroll; height: 150px;">
                            <asp:Literal ID="litBINS" runat="server"></asp:Literal>
                        </ul>
                    </td>
                </tr>
                <tr class="ui-widget-header" style="display: none">
                    <td colspan="8">Last 5 Transactions</td>
                </tr>                  
                <tr style="display: none">
                    <td colspan="8" id="tdTranWindow" style="height: 200px; vertical-align: top"><div class="tdDataHolders" id="divTranWindow"></div></td>
                </tr>
                <tr>
                    <td>                        
                        <div id="divRemarks">
                            <textarea id="txtRemarks" name="txtRemarks"> </textarea>
                        </div>
                    </td>
                    <td>
                        <div id="divAddOnRemark">
                            <table>
                                <tr>
                                    <td>Cust Id:</td>
                                    <td><input class="textNumeric" type="text" id="txtAddonCustId" style="width: 150px" /></td>
                                 </tr>
                                <tr>
                                    <td>Account No:</td>
                                    <td><input class="textNumeric" type="text" id="txtAddonAccno" style="width: 150px" /></td>
                                </tr>
                                <tr>
                                    <td>Name:</td>
                                    <td><input type="text" id="txtAddonName" style="width: 150px" /></td>
                                </tr>
                              </table>
                        </div>
                    </td>
                    
                </tr>
            </table>
            </form>
        </div>
    </div>
</asp:Content>
