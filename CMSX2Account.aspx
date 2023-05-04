<%@ Page Language="C#" MasterPageFile="~/Reports.Master"  AutoEventWireup="true" CodeBehind="CMSX2Account.aspx.cs" Inherits="Reports.CMSX2account" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            CMSX2AccountPageLoad('#<%=divOutputWindow.ClientID%>');
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="divContentWrapper" >
        <div class="ui-widget-header">CMSX2_account</div>
        <div id="divUploadFile">
            <form id="frmCMSX2account" runat="server" method="post" action="/CMSX2acc">
                <table width="100%">
                    <tr>
                        <td colspan="3">
                            CMSX2Account File:&nbsp;<asp:FileUpload ID="CMSX2AccountFileUpload" runat="server" />
                            <button class="CMSX2AccountUploadButton" id="cmdUpload" type="button" runat="server">Upload</button>
                            
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            Output Window
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <div class="divOutputWindow" id="divOutputWindow" style="height: 400px; width: 100%; overflow: auto" runat="server">
                                    
                            </div>
                        </td>
                    </tr>
                </table>
            </form>
        </div>
    </div>
</asp:Content>