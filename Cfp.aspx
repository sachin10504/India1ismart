<%@ Page Title="" Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="Cfp.aspx.cs" Inherits="Reports.Cfp" %>

<asp:Content ID="Content1" runat="server" contentplaceholderid="head">
        <script language="javascript" type = "text/javascript">
            $(document).ready(function() {
                getAgeingSwitchMatch();
            });
          </script>
</asp:Content>
<asp:Content ID="Content2" runat="server" contentplaceholderid="ContentPlaceHolder1">
   <div id="Div1" style="position:relative; height: 32px; float:left; width: 900px; z-index: 1000">
    </div>
        
        
    <div id="div6" style="position:relative; margin-left:900px; text-align: center; clear:both; top:100px; z-index: 999;">
        <button id="vmatch" type="button">Force Match</button><br /><br />
        <button id="AGNUpload" type="button">Ageing Upload File</button><br /><br />
        
    </div>
   
</asp:Content>

