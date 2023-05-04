using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Reports.Utilities.Loggers;

namespace Reports
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_PreSendRequestHeaders()
        {
            Response.Headers.Remove("Server");
            Response.Headers.Remove("X-AspNet-Version");
            Response.Headers.Remove("X-Powered-By");
        }

       

        // Added By Expect-CT Appsec point
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //added appsec point6  by 29-01-2021
            //Response.Headers.Set("Server", "");
            //Response.Headers.Set("X-AspNet-Version", "");
            //Response.Headers.Set("X-AspNetMvc-Version", "");
            //point 05-03-2021
           
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            Response.Cache.SetExpires(DateTime.Now);

            Response.Cache.SetNoStore();

            Response.Cache.SetMaxAge(new TimeSpan(0, 0, 30));

            //commented for PCI_DSS  by $@hin 04-04-2023
            //if (HttpContext.Current.Request.IsSecureConnection)
            //{
            //    string baseUrl = HttpContext.Current.Request.Url.ToString();
            //    string headerValue = "$\"max-age=0, report-uri=\\" + baseUrl;
            //    HttpContext.Current.Response.AddHeader("Expect-CT", headerValue);
            //}


            //added for PCI_DSS  by $@hin 04-04-2023
            //var app = sender as HttpApplication;
            //if (app != null && app.Context != null)
            //{
            //    app.Context.Response.Headers.Remove("Server");
            //    //app.Context.Response.Headers.Remove("X-AspNet-Version");
            //    //app.Context.Response.Headers.Remove("X-Powered-By");                
            //}
        }

        protected void Application_Error(object sender, EventArgs e)
        {            
            try
            {
                Exception objErr = Server.GetLastError().GetBaseException();
                var serverError = Server.GetLastError() as HttpException;
                if (null != serverError)
                {
                    int errorCode = serverError.GetHttpCode();
                    
                    if (404 == errorCode)
                    {
                        Server.ClearError();
                        Response.Redirect("errorPage.aspx?Err=Page Not Found", false);
                    }
                    else if (500 == errorCode)
                    {
                        Server.ClearError();
                        Response.Redirect("errorPage.aspx?Err=Unexcepted Error Occured Kindly Contact Administrator", false);
                    }
                    else if (403 == errorCode)
                    {
                        Server.ClearError();
                        Response.Redirect("errorPage.aspx?Err=No Access", false);
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error in Global.asax,Application_Error :" + Ex.ToString());
            }
        }
        //added by on 20-05-19 PCI issues start
        protected void Session_Start(object sender, EventArgs e)
        {
            try
            {

                //HttpCookie appCookie = new HttpCookie("AppCookie");
                //appCookie.Value = "written " + DateTime.Now.ToString();
                //appCookie.Expires = DateTime.Now.AddDays(1);
                //appCookie.Path = "/Admin";
                //appCookie.Secure = true;
                //appCookie.HttpOnly = true;
                //Response.Cookies.Add(appCookie);

                //Response.Cookies[FormsAuthentication.FormsCookieName].Secure = true;
                //Response.Cookies["ASP.NET_SessionId"].Secure = true;

                //if (Response.Cookies.Count > 0)
                //{
                //    foreach (string s in Response.Cookies.AllKeys)
                //    {
                //        Response.Cookies[s].Secure = true;
                //    }
                //}
                //Added by Appsec point session cookies path
                Response.Cookies["ASP.NET_SessionId"].Path = "/Admin/";
            }
            catch (Exception Ex)
            {
                throw new Exception("Error in Global.asax ,Session_Start:" + Ex.ToString());
            }
        }
        
        //Added by Appsec point 28-02-2022
        protected void Application_EndRequest(object sender, EventArgs e)
        {
            //Added for Cookie Does Not Contain The "secure" Attribute
            try
            {
                if (Request.IsSecureConnection == true && HttpContext.Current.Request.Url.Scheme == "https")
                {
                    Request.Cookies["ASP.NET_SessionID"].Secure = true;
                    if (Request.Cookies.Count > 0)
                    {
                        foreach (string s in Request.Cookies.AllKeys)
                        {
                            Request.Cookies[s].Secure = true;
                        }
                    }                 
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        //added by on 20-05-19 PCI issues end
    }
}