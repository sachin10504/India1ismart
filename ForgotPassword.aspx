<%@ Page Title="" Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="Reports.ForgotPassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" language="javascript">
        $(document).ready(function() {
            forgotPasswordPageLoad(document.<%=Form.ClientID%>);
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="padding-left: 42%; padding-top: 10%">
        <form id="forgotPass" runat="server" method="post" action="ForgotPassword.aspx">
            <table>
                <tr>
                    <td>User Name</td>
                    <td><input id="txtUserName" name="txtUserName" type="text" /></td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <button id="cmdReset" type="button">Reset</button>
                    </td>
                </tr>
            </table>
        </form>
    </div>


</asp:Content>
