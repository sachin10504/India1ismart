<%@ Page Title="" Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="EmailMaster.aspx.cs" Inherits="Reports.EmailMaster" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            emailop();

        });
    </script>

    <style type="text/css">
        #report_type {
            width: 278px;
        }
        #emailSave {
            width: 76px;
        }
        .auto-style2 {
            width: 975px;
        }
        .auto-style3 {
            margin-left: 0px;
            width: 363px;
            height: 76px;
        }
        .auto-style4 {
            height: 39px;
            width: 527px;
        }
        .auto-style5 {
            width: 256px;
        }
        .auto-style9 {
            height: 32px;
            width: 131px;
        }
        .auto-style10 {
            height: 39px;
            width: 131px;
        }
        .auto-style11 {
            width: 131px;
        }
        .auto-style12 {
            height: 37px;
            width: 131px;
        }
        .auto-style14 {
            height: 37px;
            width: 527px;
        }
        .auto-style15 {
            height: 59px;
            width: 131px;
        }
        .auto-style16 {
            margin-left: 0px;
            width: 270px;
        }
        .auto-style17 {
            height: 59px;
            width: 527px;
        }
        .auto-style19 {
            height: 59px;
            width: 591px;
        }
        .auto-style21 {
            height: 59px;
            width: 120px;
        }
        .auto-style22 {
            height: 32px;
            width: 113px;
        }
        .auto-style24 {
            height: 32px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="divContentWrapper">
        <div class="ui-widget-header" >
            <h1 style="font-size:medium;color:white"> EMAIL MASTER</h1>

             </div>
        <div>
       <%--     <form action="/" method="post">

                <div>
                    <input type="radio" id="radioEdit" name="test"/>Male<br />
                    <input type="radio" id="radioFemale" name="test"/>Female
                    <br />
                    <input type="button" value="Test!" id="btnTest" onclick="GetGender();"/>
            </div>--%>

            <%--</form>--%>
              <form id="formEmailMaster" method="post" runat="server">

                <table id="EmailTable" class="auto-style2">
                    <tr>
                        <td class="auto-style9" align="left" style="vertical-align: top"><label style="font-family: Verdana, Geneva, Tahoma, sans-serif; font-size: small;">Operation Type:</label></td>
                        <td class="auto-style24" style="vertical-align: top">
                        <%--    <asp:RadioButton ID="rdb_Edit" runat="server" GroupName="operation"  Text="EDIT" Checked="True"/>
                            <asp:RadioButton ID="rdb_add" runat="server" GroupName="operation"  Text="ADD NEW"/>--%>
                            <input type="radio" id="rdb_Edit" name="radioAuthStatus" />
                            EDIT 
                                                    
                        </td>
                   
                        <td class="auto-style22" style="vertical-align: top" id="id_add_new">
                            <input type="radio" id="rdb_add" name="radioAuthStatus" />
                            ADD NEW 
                        
                        </td>
                   
                        <td class="auto-style24" style="vertical-align: top" colspan="2">
                            &nbsp;</td>
                   
                    </tr>
                    <tr id="id_tr_add">
                        <td class="auto-style12" align="left" style="vertical-align: top"><label style="font-family: Verdana, Geneva, Tahoma, sans-serif; font-size: small;">Report Type:</label></td>
                        <td class="auto-style14" style="vertical-align: top" colspan="4"><input type="text" id="txtReport" align="middle" class="auto-style16"/>
                   
                        </tr>
                    <tr id="id_tr_edit">
                        <td class="auto-style10" align="left" style="vertical-align: top"><label style="font-family: Verdana, Geneva, Tahoma, sans-serif; font-size: small;">Report Type:</label></td>
                        <td class="auto-style4" style="vertical-align: top" colspan="4"><select id="report_type" class="auto-style5" name="D1">
                                <asp:Literal ID="litReportType" runat="server"></asp:Literal>
                            </select> </td>
                   
                    </tr>
                    <tr id="id_tr_email">
                        <td class="auto-style15" align="left" style="vertical-align: top">
                            <label style="font-family: Verdana, Geneva, Tahoma, sans-serif; font-size: small; text-align:justify; vertical-align: top;">Email ID:</label></td>
                        <td class="auto-style21" colspan="3">
                            <textarea id="txtEmail" style="resize:none" class="auto-style3" name="S1"></textarea>
                            
                        

                        </td>
                        <td class="auto-style17" align="left" style="vertical-align: top">
                             <label style="font-family: Verdana, Geneva, Tahoma, sans-serif; font-size: small; font-style: italic; color: #003366;">(Please separates mail id's by 
                                 <span style="color: #003366; font-family: Georgia, &quot;Times New Roman&quot;, Times, serif; font-size: 15px; font-style: italic; font-variant-ligatures: normal; font-variant-caps: normal; font-weight: 400; letter-spacing: normal; orphans: 2; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px; -webkit-text-stroke-width: 0px; text-decoration-style: initial; text-decoration-color: initial; display: inline !important; float: none;">,</span>
                                 )

                             </label>
                        </td>
                                </td>
                    </tr>
                    <tr>
                        <td class="auto-style11">&nbsp;</td>
                          <td class="auto-style21" colspan="3">

                            

                              <input type="button" value="Save" id="emailSave" /></td>
                          <td class="auto-style19">

                             
                                <label id="lbl_msg" style="font-family: Verdana, Geneva, Tahoma, sans-serif; font-size: medium; color: #008000"></label>
                          </td>
                    </tr>

                </table>


            </form>





            
        </div>
    </div>
    <%--<script language="javascript" type="text/javascript">
        $(document).ready(
            function () {
                //added by uddesh
                function enableDisable() {
                    var edit = $('#rdb_Edit')// document.getElementById("yes");
                    var add = $('#rdb_add') //document.getElementById("no");
                    if (edit.checked == true) {
                        $('#emailSave').hide();
                    }
                    if (add.checked == true) {
                        $('#emailSave').hide();
                    }
                }
                //function GetGender() {
                //    if ($('#rdb_Edit').is(':checked')) {
                //        alert('Gender is Male');
                //    }
                //    if ($('#rdb_add').is(':checked')) {
                //        alert('Gender is Female');
                //    }
                //}
            });
    </script>--%>
</asp:Content>
