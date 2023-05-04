<%@ Page Title="" Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Reports.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!--Added security point $-->
    <!--start-->
    <script type="text/javascript" src="JS/RSA.js"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function() {
            loginPageLoad(document.getElementById("<%=this.Form.ClientID%>"));
                    });
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('[id$="txtUserPass"]').focus(function () {
                $(this).val('');
            });
        });
</script>
  <!--end-->      
        
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="padding-left: 42%; background-image :url(images/Paysis_background.jpg); height: 475px; width: 920px; padding-top: 10%"> 
        
        <form id="frmLogin" runat="server" method="post" action="Login.aspx">
            <table>
                <tr>
                    <td>User Name</td>
                    <td><input id="txtUserName" name="txtUserName" type="text" /></td>
                </tr>
                <tr>
                    <td>Secret Word</td>
                    <td><input id="txtUserPass" type="password" /></td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <button id="cmdLogin" type="button">Log In</button>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <p style="text-align: center;"><a href="/ForgotPass"><b><u>Forgot Password</u></b></a></p>
                    </td>
                </tr>
                <!--Added security point $-->
                <!--start-->
                <tr>
                    <td></td>
                    <td><input id="txtpass" type="text" name="txtpass" value="" style="display:none;" /></td>
                </tr>
                <!--end-->
            </table>
            <!--Added security point-->
            <!--start-->
            <asp:HiddenField ID="HdnPublic" runat="server" Value="" />
            <!--end-->
        </form>
    </div>  
</asp:Content>
