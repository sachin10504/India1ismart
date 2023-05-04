<%@ Page Title="" Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="AdminLogin.aspx.cs" Inherits="Reports.AdminLogin" ValidateRequest="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="JS/AdminLogin.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            AdminLogin();
           
        });
    </script>
    
    <style type="text/css">
        .divTabs {
            overflow: auto;
            height: 300px;
        }

        #spanLog {
            color: #ffffff;
        }
         .ButtonClass {
            display: inline-block;
            position: relative;
            padding: 0;
            width:50px;
            height:25px;
            margin-right: .1em;
            text-decoration: none !important;
            cursor: pointer;
            text-align: center;
            zoom: 1;
            overflow: hidden;
            border-top-left-radius: 4px;
            border-top-right-radius: 4px;
            border-bottom-left-radius: 4px;
            border-bottom-right-radius: 4px;
            border: 1px solid #d3d3d3;           
            background: #75c2bc url(../images/ui-bg_glass_75_75c2bc_1x400.png) 50% 50% repeat-x;
            font-weight: normal;
            color: #555555;
            font-family: Courier New;
            font-size: 8pt;
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="divContentWrapper"></div>
    <div class="ui-widget-header">USER MANAGEMENT <span id="spanLog"></span></div>
    
    <div id="divUserListA">
        <form runat="server" id="frm">
           <asp:button id="btnAdd" CssClass="ButtonClass"  text="ADD" runat="server" onclick="btnConfirm_Click"/>
           <%--<asp:button id="btnRefresh" CssClass="ButtonClass"  text="Reload" runat="server" ClientIDMode="Static"/>--%>
           
            <button id="btnRefresh" CssClass="ButtonClass" type="button">Reload</button>

        </form>
            
            </div><div id="divUserListB"></div>
</asp:Content>
