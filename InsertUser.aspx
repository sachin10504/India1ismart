<%@ Page Title="" Language="C#" MasterPageFile="~/Reports.Master" AutoEventWireup="true" CodeBehind="InsertUser.aspx.cs" Inherits="Reports.InsertUser" %>
<%--page added for atpcm 1632 by abhishek--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" language="javascript" src="JS/admin.js"></script>
    <script type="text/javascript" language="javascript" src="JS/AdminLogin.js"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            
            $('#tabs').tabs();
            $('#tabs div *').css('font-family', $('body').css('font-family'));
            $('#tabs div *').css('font-size', $('body').css('font-size'));
            usermangement();

            $('.imagetable input[type="checkbox"]').attr('disabled', 'true');
        });



      

    </script>

    <%--<script type="text/javascript">  
        function HandleIT() {
            var name = document.getElementById('<%=dtReturn.ClientID %>').value;  
           var address = document.getElementById('<%=dtReturn.ClientID %>').value;

            PageMethods.ProcessIT(name, address, onSucess, onError);
            function onSucess(result) {
                alert(result);
            }

            function onError(result) {
                alert('Something wrong.');
            }
        }
    </script>  --%>
    <style type="text/css">
        #tabs div {
            overflow: auto;
            height: 200px;
        }

        .ButtonClass {
            display: inline-block;
            position: relative;
            padding: 0;
            width: 50px;
            height: 25px;
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

        #result {
            margin-left: 5px;
        }

        #register .short {
            color: #FF0000;
        }

        #register .weak {
            color: #E66C2C;
        }

        #register .good {
            color: #2D98F3;
        }

        #register .strong {
            color: #006400;
        }

        #pswd_info {
            position: absolute;
            bottom: -25px;
            bottom: -115px\9; /* IE Specific */
            right: 55px;
            width: 250px;
            padding: 15px;
            background: #fefefe;
            font-size: small;
            border-radius: 5px;
            box-shadow: 0 1px 3px #ccc;
            border: 1px solid #ddd;
        }

            #pswd_info h4 {
                margin: 0 0 10px 0;
                padding: 0;
                font-weight: normal;
                font-size: small;
            }

            #pswd_info::before {
                content: "\25B2";
                position: absolute;
                top: -1px;
                left: 25%;
                font-size: 14px;
                line-height: 14px;
                color: #ddd;
                text-shadow: none;
                display: block;
            }

        .invalid {
            background: url(../images/invalid.png) no-repeat 0 50%;
            padding-left: 22px;
            line-height: 24px;
            color: #ec3f41;
        }

        .valid {
            background: url(../images/valid.png) no-repeat 0 50%;
            padding-left: 22px;
            line-height: 24px;
            color: #3a7d34;
        }

        #pswd_info {
            display: none;
        }
        .auto-style1 {
            height: 7px;
        }
    </style>


</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" EnableViewState="true">
    <div id="divContentWrapper">
        <div class="ui-widget-header">User Management</div>
        <div>
            <form id="usrmngForm" method="post" runat="server">
                <div id="tabs">
                    <ul>
                        <li>
                            <a href="#divUser" id="CreateUser" runat="server">Create User</a>
                            <a href="#divUser" id="EditUser" runat="server" visible="false">Edit User</a>

                        </li>
                        <%--<li><a href="#divAccess">Provide Access</a></li>--%>
                    </ul>

                    <div id="divUser" style="width: 150%; height: 90%;">
                        <table>
                            <tr>
                                <td>
                                    <label>User ID:</label></td>
                                <td>
                                    <%--<input type="text" id="txtUserName" maxlength="20" />--%>

                                    <asp:TextBox ID="txtUserName" ClientID="txtUserName" runat="server" CssClass="content" MaxLength="100" ClientIDMode="Static" AutoComplete="false"></asp:TextBox>
                                    <input id="sessionInput" type="hidden" value='<%= Session["username"] %>' />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>First Name:</label></td>
                                <td>
                                    <asp:TextBox ID="txtFirstName" ClientID="txtFirstName" runat="server" CssClass="content" MaxLength="100" ClientIDMode="Static" AutoComplete="false"></asp:TextBox>

                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>Last Name:</label></td>
                                <td>
                                    <asp:TextBox ID="txtLastName" ClientID="txtLastName" runat="server" CssClass="content" MaxLength="100" ClientIDMode="Static" AutoComplete="false"></asp:TextBox>

                                </td>
                            </tr>

                            <tr>
                                <td>
                                    <label>Email ID:</label></td>
                                <td>
                                    <%--<input type="text" id="emailId" style="width:300px;" />--%>
                                    <asp:TextBox ID="emailId" runat="server" CssClass="content" MaxLength="100"></asp:TextBox>
                                </td>
                            </tr>

                            <tr>
                                <td>
                                    <label>Mobile:</label></td>
                                <td>
                                    <asp:TextBox ID="mobile" runat="server" class="textNumeric" CssClass="content" MaxLength="10"></asp:TextBox>
                                    <%--<input type="text" id="mobile" />--%>

                                </td>
                            </tr>
                            <!--start sheetal to hide showsensitive data -->
                            <%--<tr>
                            <td><label>ShowSensetiveData :</label></td>
                            <td>
                            --%>        <%--<input type="checkbox" id="showSensitive"/>--%>
                            <%--<asp:CheckBox ID="showSensitive" runat="server" />
                            </td>
                        </tr>--%>

                            <tr title="Enhance complexity by including Upper, Lower, Numeric & Special characters." id="trPass">
                                <td id="tdpass">Secret Word</td>
                                <td>
                                    <asp:TextBox ID="txtUserPass" runat="server" CssClass="content" MaxLength="100" name="txtUserPass" type="password" AutoComplete="false" TextMode="Password"></asp:TextBox>
                                    <%--<input id="txtUserPass" name="txtUserPass" type="password" />--%>
                                &nbsp;<img title="Enhance complexity by including Upper, Lower, Numeric & Special characters." src="images/help_big.gif" alt="Help" height="10%" style="cursor: pointer; width: 10%;" />&nbsp;
                                <span id="spanComplexity"></span></td>
                                <td><span id="result"></span></td>

                            </tr>
                            <tr id="trconfirPass">
                                <td id="tdconfigpass">Confirm Secret Word</td>
                                <td>
                                    <asp:TextBox ID="txtUserConfirmPass" runat="server" CssClass="content" MaxLength="100" name="txtUserConfirmPass" type="password" TextMode="Password"></asp:TextBox>
                                    <%--<input id="txtUserConfirmPass" name="txtUserConfirmPass" type="password" />--%>

                                </td>
                            </tr>

                            <tr>
                                <td>
                                    <label>Selct Role to Specify User</label></td>
                                <td>
                                    <asp:DropDownList ID="ddlRoleMaperReadOnly"  runat="server" AutoPostBack="false"  ></asp:DropDownList>
                                </td>
                            </tr>
                            <%--<%--<tr>
                            <td><label>Checker:</label></td>
                            <td>
                                 <asp:CheckBox ID="checkerChk" runat="server" />
                                <%--<input type="checkbox" id="checkerChk" />

                            </td>
                        </tr>--%>

                            <%--<tr>
                            <th colspan="4" class="ui-widget-header"><label id="Label1">User Reports Rights</label></th>
                        </tr>

                        <tr>
                            <td colspan="3"><div id="accessReports"></div> </td>
                            <td><div id="accessUtilitiesReports"></div> </td>
                        </tr>--%>
                            <tr>
                                <td ">
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <input type="button" id="cmdInsertUser" value="Save" /></td>
                                <td>
                                    <asp:Button ID="btnBack" CssClass="ButtonClass" Text="BACK" runat="server" OnClick="btnback_Click" /></td>
                                <td>
                                  <asp:HyperLink ID="LinktoRoleMapper" Font-Italic="true" Font-Size="Medium" ForeColor="Red" runat="server" Text="  * This is view only window. To Change The Reports & Utilities Rights For Any Role Please Go To Role Mapper Page." NavigateUrl="~/rolemap" ></asp:HyperLink>
                                </td>
                                <%--<td><input type="button"  id="cmdCancel" value="Cancel"/></td>--%>
                            </tr>
                        </table>
                        <div id="divRoleMapReadOnly" runat="server" visible="true">
                    <table>
                        <tr>
                            <th colspan="4" class="ui-widget-header">
                                <label id="Label1" >User Reports Rights</label></th>
                        </tr>

                        <tr>
                            <td colspan="3">
                                <div id="accessReportsReadOnly"></div>
                            </td>
                            <td>
                                <div id="accessUtilitiesReportsReadOnly"></div>
                            </td>
                        </tr>
                    </table>
                </div>
                    </div>


                    <div id="divAccess" style="display: none">
                        <table>
                            <tr>
                                <td>
                                    <label>UserName:</label></td>
                                <td>
                                    <input type="text" id="Text1" maxlength="20" /></td>
                                <td>
                                    <input type="button" id="cmdGrantAccess" value="Save" /></td>
                            </tr>
                            <tr>
                                <th colspan="8" class="ui-widget-header">
                                    <label id="Label4">User Reports Rights</label></th>
                            </tr>
                            <%--<tr>
                            <td></td>
                            <td><div id="accessReports"></div> </td>
                        </tr>--%>
                        </table>
                    </div>

                </div>
                <div id="pswd_info" style="top: 300px; left: 100px">
                    <h4>Password must meet the following requirements:</h4>
                    <ul>
                        <li id="letter" class="invalid">At least <strong>one letter</strong></li>
                        <li id="capital" class="invalid">At least <strong>one capital letter</strong></li>
                        <li id="number" class="invalid">At least <strong>one number</strong></li>
                        <li id="length" class="invalid">Be at least <strong>8 characters</strong></li>
                    </ul>
                </div>
                
            </form>
        </div>
    </div>



</asp:Content>
