<%@ Page Title="" Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="GpayAgn.aspx.cs" Inherits="Reports.GpayAgn" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script language="javascript" type = "text/javascript">
        $(document).ready(function () {
            getGharPayAgeing();
        });
          </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="Div1" style="position:relative; height: 32px; float:left; width: 900px; z-index: 1000"></div>
        
        
    <div id="div6" style="position:relative; margin-left:900px; text-align: center; clear:both; top:100px; z-index: 999;">
        
        <button id="AGNUpload" type="button">GPay Ageing Upload File</button><br /><br />
        <%--<button id="cmdExportToXL" type="button">DownLoad</button><br /><br />--%>

    </div>

</asp:Content>
