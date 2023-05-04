<%@ Page Title="" Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="errorPage.aspx.cs" Inherits="Reports.errorPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="font-size: 28px; font-weight: bold; font-style: italic; color: #FF0000">
        <asp:Literal runat="server" ID="litErrorMessage"></asp:Literal>
    </div>
</asp:Content>
