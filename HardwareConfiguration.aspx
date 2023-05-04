<%@ Page Title="" Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="HardwareConfiguration.aspx.cs" Inherits="Reports.HardwareConfiguration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('#txtTerminalId').button();

            $(".textAlphaNumeric").keypress(function (e) {
                if (((e.which < 65 || e.which > 122) && (e.which < 48 || e.which > 57)) || e.which == 95 || e.which == 94) {
                    e.preventDefault();
                }
            });
        });
    </script>
    <style type="text/css">
        .ButtonClass {
            height: 25px;
            width: 100px;
            display: inline-block;
            position: relative;
            padding: 0;
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

    <div id="divContentWrapper">
        <form id="HDCID" method="post" runat="server" action="/HDC">
            <div class="ui-widget-header">Configured Receipt Printer</div>
            <div style="padding-left: 42%; padding-top: 10%">


                <table>
                    <tr style="background-color: #75c2bc;">
                        <b>Configured Receipt Printer</b>
                    </tr>
                    <tr>
                        <td>Terminal Id:
                        </td>
                        <td>
                            <asp:TextBox ID="txtTerminalId" runat="server" class="textAlphaNumeric" MaxLength="8"></asp:TextBox>
                            <%--<input class="textAlphaNumeric" type="text" id="txtCardNum" style="width: 150px" maxlength="8" />--%>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:Button ID="btnConfigure" runat="server" OnClick="btnConfigure_Click" class="ButtonClass" type="button" Text="Configure" />
                        </td>
                    </tr>

                </table>

            </div>
        </form>
    </div>
</asp:Content>
