<%@ Page Title="" Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="FrmUploadBTISurprise.aspx.cs" Inherits="Reports.FrmUploadBTISurprise" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="JS/jquery.dataTables.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            fileUploadAtm();
            $('[id$="DivGridView"]').find('Table').DataTable();
            $('[id$="DivGridView"]').find('Table').addClass('CssTable');
        });
    </script>

    <style type="text/css">
        .CssTable table {
            width: 100%;
            border-collapse: collapse;
        }

        .CssTable tr:nth-of-type(odd) {
            background: #eee;
        }

        .CssTable th {
            background: #333;
            color: white;
            font-weight: bold;
            font-size: medium;
            text-align: center;
        }

        .CssTable td, th {
            padding: 6px;
            border: 1px solid #ccc;
            font-weight: bold;
            text-align: center;
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
        <div class="ui-widget-header">BTI Surprise File Upload</div>
        <div id="divUploadFile">
            <form id="frmUploadFile" method="post" runat="server" action="/USG">
                <table width="100%">
                    <tr>
                        <td>
                            <asp:FileUpload ID="BTIfileUpload" runat="server"></asp:FileUpload>
                            &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <button class="UploadBTI" id="btnupload" type="button">Upload File</button>

                        </td>
                    </tr>
                    <tr class="ui-widget-header">
                        <td>Output Window</td>
                    </tr>
                    <tr>
                        <td>
                            <br />

                            <div id="DivMain" align="center" runat="server">
                                <div id="DivMsg" align="center" runat="server" style="font-size: medium; font-weight: bold;">
                                    Below Details are Invalid,Please update and re-upload the file.
                                </div>
                                <br />
                                <div id="DivGrid" align="center" runat="server" style="height: auto; width: 820px;">
                                    <%--<asp:GridView ID="GridCardFees" runat="server"></asp:GridView>--%>
                                    <div id="DivGridView" runat="server" style="width: 800px;"></div>

                                    <asp:GridView ID="GridCardFees" runat="server" AutoGenerateColumns="false" Width="800px"
                                        CssClass="Grid"
                                        AlternatingRowStyle-CssClass="alt"
                                        PagerStyle-CssClass="pgr" Visible="false">
                                        <Columns>
                                            
                                            <asp:BoundField DataField="ATMID" HeaderText="ATMID" />
                                            <asp:BoundField DataField="Avg_Sum" HeaderText="Avg_Sum" />
                                           
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
