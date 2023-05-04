<%@ Page Title="" Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="masterForcedMatch.aspx.cs" Inherits="Reports.masterForcedMatch" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            getMasterForcedMatch();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="divMaster" style="position:relative;height: 32px; float:left; width: 600px; z-index: 1000">
    </div>
        
    <div id="divSwitchMaster" style="position:relative;height: 32px; float:right; width: 600px; z-index: 1000">
    </div>
        
    <div id="divMatch" style="position:relative;text-align: center; clear:both; top:100px; z-index: 999;">
        <button id="Back" type="button">Back</button><br /><br />
        <button id="Search" type="button">SearchRecord</button><br /><br />
        <button id="forcematch" type="button">Force Match Only</button><br /><br />
        <button id="masforceMatch" type="button">Master Force Match</button><br /><br />
        <button id="swtforceMatch" type="button">Switch Force Match</button><br /><br />
        <button id="masterUpload" type="button">Master Upload File </button><br /><br />
        <button id="SMUpload" type="button">Switch Upload File</button>
    </div>


</asp:Content>
