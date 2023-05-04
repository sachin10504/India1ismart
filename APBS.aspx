<%@ Page Title="" Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="APBS.aspx.cs" Inherits="Reports.APBS" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            apbsPageLoad();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="divContentWrapper" >
        <div class="ui-widget-header">APBS</div>
        <div id="divUploadFile">
            <form id="frmAPBS" runat="server" method="post" action="/APBS"><button id="btnApplyAPBSAccessRights" type="button" style="display:none"></button>
                <table width="100%">
                    <tr>
                        <td colspan="3">
                            
                           <asp:FileUpload ID="apbsFileUpload" runat="server" CssClass="hideMe apbsFileUpload"/>
                            <button class="apbsUploadButton" id="cmdUpload" type="button">Upload Transactions File</button>
                            Date:&nbsp;<input readonly type="text" id="txtFrom" class="showDate" />
                            <select id="cmbFiles"></select>
                            <button id="cmdShowProcessed" type="button">Processed</button>
                            <%--<button id="cmdShowFailed" type="button">Failed</button>--%>
                            <button id="cmdShowReject" type="button">Show Reject</button>
                            <button id="cmdShow" type="button">Show</button>
                            <input type="checkbox" id="chkSelectAll" />All
                            <button id="cmdProcess" type="button">Process</button>
                            <button id="cmdReject" type="button">Reject</button><br />
                            <%--<input type="checkbox" id="chkRedownload" />--%>
                            <button id="cmdRespFile" type="button">Generate Response File</button>
                            <button id="cmdExportToXL" type="button">Download</button>
                            <asp:FileUpload ID="fileUploadAadhar" CssClass="hideMe fileUploadAadhar" runat="server" />
                            <button id="cmdDatabaseUpload" type="button" >Database Update</button>
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
