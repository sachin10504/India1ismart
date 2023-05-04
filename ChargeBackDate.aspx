<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChargeBackDate.aspx.cs" Inherits="Reports.ChargeBackDate"  %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
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
            defaultPageLoad();
            NpciChargeBack();
            $('.showDate').removeAttr('disabled').val('');
        });
    </script>
</head>
<body>
    <form id="NpciChgDate" runat="server">
        <table align="center">
            <tr>
                <td align="left" colspan="2">
                        From&nbsp;<input  type="text" id="txtFrom" class="showDate" />&nbsp;To&nbsp;<input  type="text" id="txtTo" class="showDate" />
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <button type="button" id="cmdproceed" >Proceed</button>&nbsp;
                 </td>
            </tr>
      </table>
    </form>
</body>
</html>
