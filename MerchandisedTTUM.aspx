<%@ Page Title="" Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="MerchandisedTTUM.aspx.cs" Inherits="Reports.MerchandisedTTUM" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script language="javascript" type = "text/javascript">
         $(document).ready(function () {
             MerchandisedCreditTTUM();
         });
          </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="divMerchant" style="position:relative; height: 30px; float:left; width: 1000px; z-index: 1000"></div>
    <div id="merchantButton" style="position:relative; margin-left:1000px; text-align: center; clear:both; top:100px; z-index: 999;">
        <%--<button id="forcemerchant" type="button">Force Match</button><br /><br />--%>
        <button id="uploadMCT" type="button">Generate TTUM File</button><br /><br />
        
    </div>
</asp:Content>
