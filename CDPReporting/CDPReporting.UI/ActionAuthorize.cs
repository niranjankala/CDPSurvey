using System.Web;
using System.Web.Mvc;
using SMR.KM.Business.Services;
using CDPReporting.UI.Controllers;

namespace CDPReporting.UI
{
    public class ActionAuthorizeAttribute : AuthorizeAttribute
    {
        PermissionServices _permissionServices = new PermissionServices();
        bool _isAuthorized=false;

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return _isAuthorized;
            
        }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {            
            string contro = (string)filterContext.RequestContext.RouteData.Values["controller"];
            string action = (string)filterContext.RequestContext.RouteData.Values["action"];

            BaseController controller = filterContext.Controller as BaseController;
            if (controller == null || controller.CurrentUser.UserName == null)
            {
                _isAuthorized = false;
                base.OnAuthorization(filterContext);
                return;
            }
            _isAuthorized = true;
            //Commented as user is now auto login
            //if (_permissionServices.IsUserAuthorized(controller.CurrentUser.UserId, contro, action) == false)
            //{
            //    _isAuthorized = false;
            //    base.OnAuthorization(filterContext);
            //    return;
            //}

            base.OnAuthorization(filterContext); 
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {       
            base.HandleUnauthorizedRequest(filterContext);
            RedirectToRegister(filterContext);
            //RedirectToLogin(filterContext);
        }


        public void RedirectToLogin(AuthorizationContext filterContext)
        {
            HttpResponseBase response = filterContext.HttpContext.Response;           
            UrlHelper ur = new UrlHelper(filterContext.RequestContext);
            response.Redirect(ur.RouteUrl(new { controller = "Account", action = "LogOn", returnUrl = filterContext.HttpContext.Request.RawUrl }));

            //if (filterContext.HttpContext.Request.IsAjaxRequest())
            //{
            //    // It was an AJAX request => no need to redirect
            //    // to the login url, just return a JSON object
            //    // pointing to this url so that the redirect is done 
            //    // on the client

            //    var referrer = filterContext.HttpContext.Request.UrlReferrer;

            //    filterContext.Result = new JsonResult
            //    {
            //        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
            //        Data = new
            //        {
            //            redirectTo = FormsAuthentication.LoginUrl +
            //                "?ReturnUrl=" +
            //            referrer.LocalPath.Replace("/", "%2f")
            //        }
            //    };
            //}
            //else
            //{
            //    response.Redirect(ur.RouteUrl(new { controller = "Account", action = "LogOn", returnUrl = filterContext.HttpContext.Request.RawUrl }));
            //}
        }

        public void RedirectToRegister(AuthorizationContext filterContext)
        {
            HttpResponseBase response = filterContext.HttpContext.Response;
            UrlHelper ur = new UrlHelper(filterContext.RequestContext);
            response.Redirect(ur.RouteUrl(new { controller = "Account", action = "Register", returnUrl = filterContext.HttpContext.Request.RawUrl }));
           
        }
    }
}