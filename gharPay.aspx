<%@ Page Title="" Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="gharPay.aspx.cs" Inherits="Reports.gharPay" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script language="javascript" type="text/javascript">
        $(document).ready(function() {
            gharPayPageLoad('#<%=divOutputWindow.ClientID%>');
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="divContentWrapper" >
        <div class="ui-widget-header">GharPay</div>
        <div id="divUploadFile">
            <form id="frmGharPay" runat="server" method="post" action="/GPay">
                <table width="100%">
                    <tr>
                        <td colspan=3>
                            <!--
                            <select id="cmbFiles">
                                <option value="0">Daily Cash-Back</option>
                                <option value="1">Daily Cash-Back Credits</option>
                                <option value="2">Weekly Offer</option>
                                <option value="3">Offers for New Customers</option>
                            </select>
                            -->
                            File:&nbsp;<asp:FileUpload ID="gpFileUpload" runat="server" />
                            <button class="gpUploadButton" id="cmdUpload" type="button" runat="server">Upload</button>
                            Date:&nbsp;<input readonly type="text" id="txtFrom" class="showDate" />
                            <button id="cmdShowProcessed" type="button">Processed</button>
                            <button id="cmdShowFailed" type="button">Failed</button>
                            <button id="cmdShowReject" type="button">Show Reject</button>
                            <button id="cmdShow" type="button">Show</button>
                            <input type="checkbox" id="chkSelectAll" />All
                            <button id="cmdProcess" type="button">Process</button>
                            <button id="cmdReject" type="button">Reject</button><br />
                            <button id="cmdExportToXL" type="button">Download</button>
                            <button id="cmdTTUM" type="button">Generate TTUM</button>
                            <%--<button id="cmdReset" type="button">Reset</button>--%>
                        </td>
                    </tr>
                    <tr class="ui-widget-header">
                        <td colspan=3>Output Window</td>
                    </tr>
                    <tr>
                        <td colspan=3>
                            <div class="divOutputWindow" id="divOutputWindow" style="height: 400px; width: 100%; overflow: auto" runat="server">
                                    
                            </div>
                        </td>
                    </tr>
                </table>
            </form>
        </div>
    </div>
</asp:Content>
