using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace Reports
{
    public partial class downloadFile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string sFileName = Server.MapPath("tempOutputs");
            string sOutputFormat = "";
            System.IO.FileStream fsObj = null;

            if (Convert.ToString(Session["Active"]) != "1")
            {
                Response.Redirect("/ErrorPage");
                Response.End();
            }

            sFileName += "/" + Convert.ToString(Request.QueryString["Fn"]);
            sOutputFormat = Convert.ToString(Request.QueryString["Fm"]);

            fsObj = System.IO.File.Open(sFileName, FileMode.Open);
            byte[] btFile = new byte[fsObj.Length];
            fsObj.Read(btFile, 0, Convert.ToInt32(fsObj.Length));
            fsObj.Close();

            if (sOutputFormat == null || sOutputFormat == "" || sOutputFormat == "xls")
            {
                Response.AddHeader("Content-disposition", "attachment; filename=downloadFile.xls");
            }
            else if (sOutputFormat == "csv")
            {
                Response.AddHeader("Content-disposition", "attachment; filename=downloadFile.csv");
            }
            else if (sOutputFormat == "rar")
            {
                Response.AddHeader("Content-disposition", "attachment; filename=downloadFile.rar");
                File.Delete(Server.MapPath("tempOutputs") + "\\" + Convert.ToString(Request.QueryString["Fn"]));
            }
            else if (sOutputFormat == "xlsx")
            {
                Response.AddHeader("Content-disposition", "attachment; filename=" + Convert.ToString(Request.QueryString["Fn"]) + ".xlsx");
            }
            else if (sOutputFormat == "Pre")
            {
                Response.AddHeader("Content-disposition", "attachment; filename=" + Convert.ToString(Request.QueryString["Fn"]) + ".Pre");
                File.Delete(Server.MapPath("tempOutputs") + "\\" + Convert.ToString(Request.QueryString["Fn"]));
                Session["FileName"] = "";
            }
            else if (sOutputFormat == "txt")
            {
                Response.AddHeader("Content-disposition", "attachment; filename=downloadFile.txt");
            }
            else if (sOutputFormat=="txtwithname")
            {
                Response.AddHeader("Content-disposition", "attachment; filename=" + Convert.ToString(Request.QueryString["Fn"]) + ".txt");
            }
            Response.ContentType = "application/octet-stream";
            Response.BinaryWrite(btFile);
            Response.End();
        }
    }
}
