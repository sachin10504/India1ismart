<%@ Page Title="" Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="MasterCardReadTT112.aspx.cs" Inherits="Reports.MasterCardReadTT112" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
       <script type="text/javascript" lang="jv">
           $(document).ready(function () {
               MCChargeBack();
           });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div id="divContentWrapper">
        <div class="ui-widget-header">Master Card ChargeBack Upload</div>
        <div>
          <form id="MCChargeBackUpload" runat="server" method="post" action="UploadMCChargeBack.aspx">
            <table class="pretty" style="width:100%" >
                <tr>
                    <td style="margin-left:-10px;">
                        Master Card ChargeBack File:&nbsp;<asp:FileUpload ID="MCChargeBackFileUpload" runat="server" />
                        <input type="button" id="cmdUpload" value="Upload"/>
                       
                    </td>
                </tr>
                <asp:GridView ID="GridView1" runat="server"></asp:GridView>
            </table>
          </form>
        </div>
    
        </div>
</asp:Content>

