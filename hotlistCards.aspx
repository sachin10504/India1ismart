<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="hotlistCards.aspx.cs" Inherits="Reports.hotlistCards" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    
    <link rel="Stylesheet" type="text/css" href="CSS/System.css" />
    <link rel="Stylesheet" type="text/css" href="CSS/jquery-ui.css" />
    <link rel="Stylesheet" type="text/css" href="CSS/jquery.multiselect.css" />
    <link rel="Stylesheet" type="text/css" href="CSS/jquery.multiselect.filter.css" />
    <link rel="Stylesheet" type="text/css" href="CSS/flexigrid.css" />
    <link rel="Stylesheet" type="text/css" href="CSS/jquery.pgrid.default.css" />
    <link rel="Stylesheet" type="text/css" href="JS/fancybox/jquery.fancybox.css" />
    <link rel="Stylesheet" type="text/css" href="http://fonts.googleapis.com/css?family=Emblema+One" />
    
    <link rel="icon" href="http://agsttl.com/images/site/ags-ico.jpg" type="image/x-icon" />
    <link rel="shortcut icon" href="http://agsttl.com/images/site/ags-ico.jpg" type="image/x-icon" />
    
    <script language="javascript" type="text/javascript" src="JS/jquery.js"></script>
    <script language="javascript" type="text/javascript" src="JS/jquery-ui.js"></script>
    <script language="javascript" type="text/javascript" src="JS/jquery.multiselect.js"></script>
    <script language="javascript" type="text/javascript" src="JS/jquery.multiselect.filter.js"></script>
    <script language="javascript" type="text/javascript" src="JS/jquery-ui-timepicker-addon.js"></script>
    <script language="javascript" type="text/javascript" src="JS/flexigrid.js"></script>
    <script language="javascript" type="text/javascript" src="JS/jquery.pgrid.min.js"></script>
    <script language="javascript" type="text/javascript" src="JS/jquery.mousewheel.js"></script>
    <script language="javascript" type="text/javascript" src="JS/fancybox/jquery.fancybox.js"></script>
    <script language="javascript" type="text/javascript" src="JS/fancybox/jquery.mousewheel.js"></script>
    <script language="javascript" type="text/javascript" src="JS/fancybox/jquery.easing.js"></script>
            
    <script language="javascript" type="text/javascript" src="JS/othUtils.js"></script>
    <script language="javascript" type="text/javascript" src="JS/pageActions.js"></script>
    
    <script language="javascript" type="text/javascript">
        $(document).ready(function() {
            hostlistCardPageLoad();
        });
    </script>
</head>
<body>
    <form id="frmHostListCards" runat="server">
        <table align="center">
            <tr>
                <td>Card No</td>
                <td>Customer ID</td>
            </tr>
            <tr>
                <td><input type="text" id="txtCardNo" style="width: 150px;" /><select id="cmbCardNo"></select><input type="text" id="txtEncCardNo" style="width: 335px; display: none;" /></td>
                <td><input type="text" id="txtCustId" style="width: 150px;" /></td>
            </tr>
            <tr>
                <td>Cardholder Name</td>
                <td><input type="text" id="txtCardholderName" readonly="readonly" style="width: 335px;" /></td>
            </tr>
            <tr>
                <td>Current Status</td>
                <td><input type="text" id="txtCurrentStatus" readonly="readonly" style="width: 335px;" /></td>
            </tr>
            <tr>
                <td>Hold Response Code</td>
                <td>
                    <select id="cmbHoldResponseCode" name="cmbHoldResponseCode" multiple="multiple">
                        <option value="01">01 - Refer To Card Issuer</option>
                        <option value="05">05 - Do Not Honor</option>
                        <option value="06">06 - Temporary Block</option>
                        <option value="14">14 - Invalid Card Number</option>
                        <option value="36">36 - Restricted Card:Pick Up</option>
                        <option value="41">41 - Lost Card</option>
                        <option value="43">43 - Stolen Card</option>
                        <option value="45">45 - Account Closed</option>
                        <option value="54">54 - Expired Card</option>
                        <option value="59">59 - Suspected Fraud</option>
                        <option value="62">62 - Restricted Card</option>
                    </select>
                    <input type="text" id="txtHoldResponseCode" style="display: none" />
                </td>
            </tr>
            <tr>
                <td>Remarks (50char max)</td>
                <td><textarea id="txtRemarks" style="width: 335px; height: 145px;" rows="0" cols="0"></textarea></td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <button type="button" id="cmdOk">Hotlist</button>&nbsp;
                    <button type="button" id="cmdCancel">Cancel</button>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
