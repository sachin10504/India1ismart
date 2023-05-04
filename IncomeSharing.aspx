<%@ Page Title="" Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="IncomeSharing.aspx.cs" Inherits="Reports.IncomeSharing" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" >
        $(document).ready(function () {
            IncomeSharing();
        });
    </script>
    
    <style type="text/css">
        #cmdADD {
            width: 119px;
        }
    </style>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="divContentWrapper">
        <div class="ui-widget-header">Income Sharing <span id="spanLog"></span></div>
        <div id="Ïncomeshare">
          <asp:HyperLink ID="ADD" runat="server">ADD</asp:HyperLink>
            <asp:HyperLink ID="Edit" runat="server">Edit</asp:HyperLink>
            <table width="100%" style="height: 133px">

                 <tr><td>BC NAME:</td><td><input id="BCNAME"  placeholder="BC NAME" /></td>
                     <td>BC Account:</td><td><input id="BCAcc"  class="textNumeric" placeholder="BC Account" /></td></tr>

                <tr><td>RBL Percentage %:</td><td id="RBL"><input id="RBLpercentage"  class="textNumeric" placeholder="RBL Percentage" /></td>
                    <td>BC Percentage %:</td><td id="BC"><input id="BCpercentage"  class="textNumeric"  placeholder="BC Percentage" /></td>
                </tr>
                <tr><td>Max amount:</td><td><input id="RblMaxAmount"  class="textNumeric" placeholder="Max Amount" /></td>
                    <td>Min amount:</td><td><input id="RblMinAmount"  class="textNumeric" placeholder="Min Amount" /></td>
                </tr>
                <tr><td>Balance Inquary:</td><td><input id="BI"  class="textNumeric" placeholder="Balance Inquary" /></td>
                    <td>Cash Deposite:</td><td><input id="CD"  class="textNumeric" placeholder="Cash Deposite" /></td>
                    <td>Cash withdrawal:</td><td><input id="CW"  class="textNumeric" placeholder="Cash withdrawal" /></td>
                </tr>
                <tr><td>Max amount:</td><td><input id="NPCIMaxAmount"  class="textNumeric" placeholder="Max Amount" /></td>
                    <td>Min amount:</td><td><input id="NPCIMinAmount"  class="textNumeric" placeholder="Min Amount" /></td>
                </tr>
                             <tr>
                                 <td id="Submit" colspan="8" align="center"><button id="cmdADD" type="submit" >ADD</button></td>
                             </tr>
                </table>

        </div>
        <div id="div5"></div>
    </div>
</asp:Content>
