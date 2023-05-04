<%@ Page Title="" Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="ChangePass.aspx.cs" Inherits="Reports.ChangePass" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" language="javascript">
        $(document).ready(function() {
            changePassPageLoad(document.<%=Form.ClientID%>);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="padding-left: 42%; padding-top: 10%">
        <form id="frmChangePass" runat="server" method="post" action="ChangePass.aspx">
            <table>
                <tr>
                    <td>Old Secret Word</td>
                    <td><input id="txtOldUserPass" name="txtOldUserName" type="password" /></td>
                </tr>
                <tr title="Enhance complexity by including Upper, Lower, Numeric & Special characters.">
                    <td>New Secret Word</td>
                    <td><input id="txtUserPass" name="txtUserPass" type="password" />&nbsp;<img title="Enhance complexity by including Upper, Lower, Numeric & Special characters." src="images/help_big.gif" alt="Help" height="10%" width="10%" style="cursor: pointer" />&nbsp;<span id="spanComplexity"></span></td>
                </tr>
                <tr>
                    <td>Confirm Secret Word</td>
                    <td><input id="txtUserPassC" name="txtUserPassC" type="password" /></td>
                </tr>                
                <tr>
                    <td colspan="2" align="center">
                        <button id="cmdChange" type="button">Change</button>&nbsp;
                        <button id="cmdCancel" type="button">Cancel</button>
                    </td>
                </tr>
            </table>
        </form>
    </div> 
</asp:Content>
