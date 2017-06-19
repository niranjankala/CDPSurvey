using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SMR.KM.Business.Services;
using System.Web.Security;
using SMR.KM.Business.Base;
using SMR.KM.Common;
using System.Text;
using System.Web.Helpers;
using log4net;
using System.Web.Routing;
using System.Text.RegularExpressions;
using SMR.KM.Business.Models;

namespace CDPReporting.UI.Controllers
{
    public class BaseController : Controller
    {
        public User CurrentUser { get; private set; }
        private UserServices _userServices = new UserServices();
        protected static readonly ILog logDB = LogManager.GetLogger("Audit");

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (CurrentUser != null)
            {
                try
                {
                    AppLog oAppLog = new AppLog();
                    oAppLog.UserName = this.CurrentUser.UserName;
                    oAppLog.Method = (string)this.ControllerContext.RouteData.Values["action"];
                    oAppLog.Module = (string)this.ControllerContext.RouteData.Values["controller"];
                    oAppLog.Message = filterContext.HttpContext.Request.Url.ToString();
                    oAppLog.LogDate = DateTime.UtcNow;
                    // oAppLog.MethodId = new Guid();
                    // oAppLog.MethodType = (int)LogType.General;
                    this.LogActivity(oAppLog);
                    if (logDB.IsInfoEnabled) logDB.Info("");
                }
                catch { }
            }


            base.OnActionExecuted(filterContext);

            var result = filterContext.Result as ViewResultBase;
            if (result != null)
            {
                var model = filterContext.Controller.ViewData.Model as DomainBase;

                if (model != null)
                {
                    model.CurrentUser = CurrentUser;
                    model.CurrentUser.IsAuthenticated = filterContext.HttpContext.User.Identity.IsAuthenticated;
                    model.IsAuthenticated = filterContext.HttpContext.User.Identity.IsAuthenticated;
                }
            }

        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            bool isAuthenticated = requestContext.HttpContext.User.Identity.IsAuthenticated;
            if (_log.IsInfoEnabled) _log.Info("Is Authenticated" + isAuthenticated);

            if (isAuthenticated)
            {
                string windowName = requestContext.HttpContext.User.Identity.Name;
                if (_log.IsInfoEnabled) _log.Info("Window Name" + windowName);

                if (!string.IsNullOrEmpty(windowName))
                {
                    string windowUserName = HtmlHelpers.GetWindowsUserName(windowName);
                    if (_log.IsInfoEnabled) _log.Info("Login Name:-" + windowUserName);
                    if (_log.IsInfoEnabled) _log.Info("Calling GetUser method of user services to get user by username");
                    this.CurrentUser = _userServices.GetUser(windowUserName);

                    if (CurrentUser == null)
                    {
                        if (_log.IsInfoEnabled) _log.Info("Calling GetUserDetailsFromAD method of user services to get user ad detail");

                        var windowUser = _userServices.GetUserDetailsFromAD(windowUserName);

                        if (_log.IsInfoEnabled) _log.Info("Calling GetUserByEmailId method of user services to get user by email");

                        if (!string.IsNullOrEmpty(Convert.ToString(windowUser.Email)))
                        {
                            if (_log.IsInfoEnabled) _log.Info("AD User returned " + Convert.ToString(windowUser.Email));

                            this.CurrentUser = _userServices.GetUserByEmailId(Convert.ToString(windowUser.Email));
                        }

                        if (CurrentUser == null)
                        {
                            Session["WindowsUserName"] = windowUserName;

                            this.CurrentUser = new User { UserId = Guid.Empty };
                        }
                    }
                    else
                    {
                        ViewData[ViewDataKeys.CurrentUser] = CurrentUser;
                    }
                }
            }
            else
                this.CurrentUser = new User { UserId = Guid.Empty };

            SetLogInfo(CurrentUser);
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            string controller = (string)this.ControllerContext.RouteData.Values["controller"];
            string action = (string)this.ControllerContext.RouteData.Values["action"];
        }

        protected override void OnException(ExceptionContext filterContext)
        {

            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            AppLog oAppLog = new AppLog();
            oAppLog.UserName = this.CurrentUser.UserName;
            oAppLog.Method = Convert.ToString(System.Reflection.MethodBase.GetCurrentMethod().Name);
            oAppLog.Module = Convert.ToString(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            oAppLog.Message = filterContext.Exception.Message;
            oAppLog.LogDate = DateTime.UtcNow;
            this.LogActivity(oAppLog);
            if (logDB.IsInfoEnabled) logDB.Info("");
            log.Error(filterContext.Exception.Message, filterContext.Exception);
            filterContext.ExceptionHandled = true;
            ViewData["ErrorMessage"] = filterContext.Exception.Message;

            if (!Request.IsAjaxRequest())
            {
                this.View("Error").ExecuteResult(this.ControllerContext);
            }
            else
            {
                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.Clear();
                filterContext.HttpContext.Response.ContentEncoding = Encoding.UTF8;
                filterContext.HttpContext.Response.HeaderEncoding = Encoding.UTF8;
                filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
                filterContext.HttpContext.Response.StatusCode = 418;
                filterContext.Result = new HttpStatusCodeResult(418, filterContext.Exception.Message.ToString());
            }
        }

        protected void SetLogInfo(User currentUser)
        {
            if (currentUser == null) return;
            log4net.ThreadContext.Properties["UserName"] = currentUser.UserName;
            log4net.ThreadContext.Properties["CurrentUserEmail"] = currentUser.Email;
        }

        public void RedirectToLogin()
        {
            HttpResponseBase response = this.ControllerContext.HttpContext.Response;
            UrlHelper ur = new UrlHelper(this.ControllerContext.RequestContext);
            response.Redirect(ur.RouteUrl(new { controller = "Account", action = "LogOn", returnUrl = this.ControllerContext.HttpContext.Request.RawUrl }));

        }



        public void RedirectToRegistor()
        {
            RouteCollection routes = new RouteCollection();

            routes.MapRoute(
           "", // Route name
           "Admin/{controller}/{action}/{id}", // URL with parameters
           new { area = "Admin", controller = "Account", action = "Register", id = UrlParameter.Optional } // Parameter defaults
           , new[] { "WebUI.Areas.Admin.Controllers" } //prioritize admin
       );

        }

        protected void LogActivity(AppLog oAppLog)
        {
            if (oAppLog == null) return;

            log4net.ThreadContext.Properties["UserName"] = oAppLog.UserName;
            log4net.ThreadContext.Properties["Method"] = oAppLog.Method;
            log4net.ThreadContext.Properties["Module"] = oAppLog.Module;
            log4net.ThreadContext.Properties["Message"] = oAppLog.Message;
            log4net.ThreadContext.Properties["LogDate"] = oAppLog.LogDate;
        }



    }
}