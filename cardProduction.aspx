<%@ Page Title="" Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="cardProduction.aspx.cs" Inherits="Reports.cardProduction" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script language="javascript" type="text/javascript">
        $(document).ready(function() {
            cardProductionPageLoad(document.<%=frmCardProduction.ClientID%>,'#<%=cmdUpload.ClientID%>', '#<%=divOutputWindow.ClientID%>');
           // cardProductionPageLoad(document.getElementById("frmCardProduction"), '#<%=cmdUpload.ClientID%>', '#<%=divOutputWindow.ClientID%>');
        });
    </script>
</asp:Content>
<asp:Content ID="Cosntent2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="divContentWrapper" >
        <div class="ui-widget-header">Card Production</div>
            <div id="divUploadFile">
                <form id="frmCardProduction" runat="server" method="post" action="/CProd">
                    <table width="100%">
                        <tr>
                            <td>
                                Card File:&nbsp;<asp:FileUpload ID="cardFileUpload" runat="server" />
                                <button class="cardProdUploadButton" id="cmdUpload" type="button" runat="server">Upload</button>
                                Date:&nbsp;<input readonly type="text" id="txtFrom" class="showDate" />
                                <select id="cmbFiles"></select>
                                <button id="cmdShowProcessed" type="button">Processed</button>
                                <button id="cmdShowRejected" type="button">Rejected</button>
                                <button id="cmdShowReissue" type="button">Reissue</button>
                                <input type="checkbox" id="chkSelectAll" />All
                                <button id="cmdProcess" type="button">Process</button>
                                <button id="cmdReject" type="button">Reject</button><br />
                                <input type="checkbox" id="chkRedownload" />
                                <button id="cmdDownload" type="button">Download</button>
                                <button id="cmdReset" type="button">Reset</button>
                                <button id="cmdSendAccountLinkageSMS" type="button">Send Linkage SMS</button>
                            </td>
                            <td colspan=2></td>
                        </tr>
                        <tr class="ui-widget-header">
                            <td colspan=3>Output Window</td>
                        </tr>
                        <tr>
                            <td colspan=3>
                                <div id="divOutputWindow" style="height: 400px; width: 100%; overflow: auto" runat="server">
                                    
                                </div>
                            </td>
                        </tr>
                    </table>
                </form>
            </div>
        </div>
</asp:Content>
