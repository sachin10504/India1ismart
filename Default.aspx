<%@ Page Title="" Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Reports.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script language="javascript" type="text/javascript">
        $(document).ready(function() {
            defaultPageLoad();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="divContentWrapper" >
        <div class="ui-widget-header"></div>
        <div>
            <table>
                <tr>
                   <%-- <td>
                        <select id="cmbTranGroup" name="cmbTranGroup" multiple="multiple">
                            <asp:Literal ID="litReportGroup" runat="server"></asp:Literal>
                        </select>
                    </td>--%>
                    <td>
                         <select id="cmbTranGroup" name="cmbTranGroup" multiple="multiple">
                            <asp:Literal ID="litReportGroup" runat="server"></asp:Literal>
                        </select>
                        <select id="cmbTranType_0" name="cmbTranType_0" style="display: none">
                            <asp:Literal ID="litReports" runat="server"></asp:Literal>
                        </select>
                        <select id="cmbTranType" name="cmbTranType" multiple="multiple">
                        </select>
                    </td>
                 <%--   <td id="tdStatus" style="display:none">
                        <input type="checkbox" id="chkStatus_1" name="chkStatus" value="1" />Active&nbsp;
                        <input type="checkbox" id="chkStatus_2" name="chkStatus" value="0" />InActive&nbsp;
                        <input type="checkbox" id="chkStatus_3" name="chkStatus" value="2" />Hotlist&nbsp;
                    </td>--%>
                   <%-- <td id="tdTerminals">
                        <input type="checkbox" id="chkTerminals_1" name="chkTerminals" value="4" />OffUs&nbsp;
                        <input type="checkbox" id="chkTerminals_2" name="chkTerminals" value="2" />Remote OnUs&nbsp;
                        <input type="checkbox" id="chkTerminals_3" name="chkTerminals" value="3" />OnUs&nbsp;
                        <select id="cmbTerminals" name="cmbTerminals" multiple="multiple">
                            <asp:Literal ID="litTerminals" runat="server"></asp:Literal>
                        </select>
                    </td>--%>
                   <%-- <td>
                        <select id="cmbMerchants" name="cmbMerchants" multiple="multiple">
                            <asp:Literal ID="litMerchants" runat="server"></asp:Literal>
                        </select>                    
                    </td> --%>  
                    <td align="left" colspan="1">
                        From&nbsp;<input readonly type="text" id="txtFrom" class="showDate" />&nbsp;To&nbsp;<input readonly type="text" id="txtTo" class="showDate" />
                    </td>
                     <td align="right" colspan="2">
                        <button id="cmdGenerate">Generate</button>&nbsp;
                        <button id="cmdExportToXL">Download</button>&nbsp;
                        <button id="cmdReset">Reset</button>
                    </td>
                   <%-- <td>
                        <button id="cmdCustomCards" title="Custom Cards">Card Patern</button>
                        <div id="divCustomCards">
                            <textarea id="txtCards"></textarea>
                        </div>
                    </td>--%>
                </tr>
                <tr>
                    <%--<td align="left" colspan="1">
                        From&nbsp;<input readonly type="text" id="txtFrom" class="showDate" />&nbsp;To&nbsp;<input readonly type="text" id="txtTo" class="showDate" />
                    </td>--%>
                    <%--<td align="left" colspan="2">
                        <input type="checkbox" id="chkTerminalType_1" name="chkTerminalType" value="1" checked="checked"/>ATM&nbsp;
                        <input type="checkbox" id="chkTerminalType_2" name="chkTerminalType" value="2" />POS&nbsp;|&nbsp;
                        <input type="checkbox" id="chkNode_1" name="chkNode" value="1" />Self&nbsp;
                        <input type="checkbox" id="chkNode_2" name="chkNode" value="2" />VISA&nbsp;
                        <input type="checkbox" id="chkNode_3" name="chkNode" value="3" />NPCI&nbsp;
                        <input type="checkbox" id="chkNode_4" name="chkNode" value="4" />Master&nbsp;&nbsp;|&nbsp;
                        <input type="checkbox" id="chkOrigin_1" name="chkOrigin" value="1" />Domestic&nbsp;
                        <input type="checkbox" id="chkOrigin_2" name="chkOrigin" value="2" />International                        
                    </td>--%>
                   <%-- <td align="right" colspan="2">
                        <button id="cmdGenerate">Generate</button>&nbsp;
                        <button id="cmdExportToXL">Download</button>&nbsp;
                        <button id="cmdReset">Reset</button>
                    </td>--%>
                </tr>
            </table>
        </div>
        <%--<div class="ui-widget-header">Output Window</div>--%>
        <div class="ui-widget-header"></div>
        <div id="divOutputWindow">

        </div>
    </div>
</asp:Content>
