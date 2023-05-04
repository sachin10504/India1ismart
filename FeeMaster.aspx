<%@ Page Title="" Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="FeeMaster.aspx.cs" Inherits="Reports.FeeMaster" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            FeeManagement();
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="divContentWrapper">
        <div class="ui-widget-header">Fee Management</div>
        <div>
            <form id="formFeemanagement" method="post" runat="server">

                <table id="feeTable">
                    <tr>
                        <td><label>Transaction Type:</label></td>
                        <td><select id="tran_type" >
                                <asp:Literal ID="litTransactionType" runat="server"></asp:Literal>
                            </select> </td>
                        <td><label>Value:</label> </td>
                        <td><input type="text" id="fees"/></td>
                    </tr>
                    <tr>
                        <td><input type="button" value="Save" id="cmdSave" /></td>
                    </tr>

                </table>


            </form>
        </div>
    </div>



</asp:Content>
