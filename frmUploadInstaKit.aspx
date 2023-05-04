<%@ Page Title="" Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="frmUploadInstaKit.aspx.cs" Inherits="Reports.frmUploadInstaKit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="JS/jquery.dataTables.min.js"></script>
    <link href="CSS/jquery.dataTables.css" rel="stylesheet" />

    <script type="text/javascript">
        $(document).ready(function () {
            UploadInstaKit();

            $('[id$="DivGridView"]').find('Table').DataTable();
        });
    </script>

    <style type="text/css">
        .Grid {
            background-color: #fff;
            margin: 5px 0 10px 0;
            border: solid 1px #525252;
            border-collapse: collapse;
            font-family: Calibri;
            color: #474747;
        }

            .Grid td {
                padding: 2px;
                border: solid 1px rgba(27, 35, 34, 0.22);
                background: #d5ecea;
                text-align: center;
                font-size: medium;
            }

            .Grid th {
                padding: 4px 2px;
                color: #fff;
                background: #3e7f86 url(Images/grid-header.png) repeat-x top;
                border-left: solid 1px #525252;
                font-size: large;
            }

            .Grid .alt {
                background: #fcfcfc url(Images/grid-alt.png) repeat-x top;
            }

            .Grid .pgr {
                background: #363670 url(Images/grid-pgr.png) repeat-x top;
            }

                .Grid .pgr table {
                    margin: 3px 0;
                }

                .Grid .pgr td {
                    border-width: 0;
                    padding: 0 6px;
                    border-left: solid 1px #666;
                    font-weight: bold;
                    color: #fff;
                    line-height: 12px;
                }

                .Grid .pgr a {
                    color: Gray;
                    text-decoration: none;
                }

                    .Grid .pgr a:hover {
                        color: #000;
                        text-decoration: none;
                    }

        .ButtonClass {
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
        <div class="ui-widget-header">Upload Card Fees Details</div>
        <div id="divUploadFile">
            <form id="frmUploadFile" method="post" runat="server" action="/UPIK">
                <table width="100%">
                    <tr>
                        <td>
                            <asp:FileUpload ID="FileUploaderInstaKit" runat="server"></asp:FileUpload>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <button class="ClUpInstaKit" id="BtnuploadIK" type="button">Upload File</button>
                            &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Label runat="server" ID="lblPath" Style="display: none;"></asp:Label>

                        </td>
                    </tr>
                    <tr class="ui-widget-header">
                        <td>Output Window</td>
                    </tr>
                    <tr>
                        <td>
                            <br />

                            <div id="DivMain" align="center" runat="server">
                                <div id="DivMsg" align="center" runat="server" style="font-size: medium; font-weight: bold; overflow: auto;">
                                    Below Details are Invalid,Please update and re-upload the file.
                                </div>
                                <br />
                                <div id="DivGrid" align="center" runat="server" style=" height: auto; width: 820px;  ">
                                    <%--<asp:GridView ID="GridCardFees" runat="server"></asp:GridView>--%>
                                    
                                    <div id="DivGridView" runat="server" style="width: 800px;"></div>

                                    <asp:GridView ID="GridCardFees" runat="server" AutoGenerateColumns="false" Width="700px"
                                        CssClass="Grid"
                                        AlternatingRowStyle-CssClass="alt"
                                        PagerStyle-CssClass="pgr" Visible="false">
                                        <Columns>
                                            <asp:BoundField DataField="CustId" HeaderText="CustId" />
                                            <asp:BoundField DataField="SchemeCode" HeaderText="SchemeCode" />
                                            <asp:BoundField DataField="AccountNumber" HeaderText="AccountNumber" />
                                            <asp:BoundField DataField="ActivationDate" HeaderText="ActivationDate" />
                                            <asp:BoundField DataField="BIN" HeaderText="BIN" />
                                        </Columns>
                                    </asp:GridView>

                                </div>
                                <div id="DivButton" align="center" runat="server" style="width: 600px; overflow: auto;">
                                    <br />
                                    <asp:Button ID="BtnSave" runat="server" Text="Save" OnClick="BtnSave_Click" CssClass="ButtonClass" Style="height: 25px; width: 50px;" />
                                </div>
                            </div>
                            <div>
                            </div>

                        </td>
                    </tr>
                </table>
            </form>
        </div>

    </div>

</asp:Content>
