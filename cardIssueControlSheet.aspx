<%@ Page Title="" Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="cardIssueControlSheet.aspx.cs" Inherits="Reports.cardIssueControlSheet" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            cardIssueControlSheet();
        });

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="Div">
            <table align="center">
            <tr>
                <td>
                    <label>BC Code:</label>
                </td>
                <td>
                    <select id="textBcCode" name="textBcCode">
                        <asp:Literal ID="bcCodeLit" runat="server"></asp:Literal>
                    </select>
                </td>
                <td>
                    <label>CardType:</label>
                </td>
                <td>
                    <select id="txtCardType" name="txtCardType">
                        <asp:Literal ID="cardsubTypeLit" runat="server"></asp:Literal>
                    </select>
                </td>


                <td>
                        Date&nbsp;<input  type="text" id="txtFrom" class="showDate"/> <button type="button" id="cmdGenerate" >Show</button>&nbsp;<button type="button" id="cmdExportToXL" >Download</button>
                </td>
            </tr>
          </table>

    </div>
    <div class="divOutputWindow" id="divOutputWindow" style="height: 400px; width: 100%; overflow: auto"></div>

</asp:Content>
