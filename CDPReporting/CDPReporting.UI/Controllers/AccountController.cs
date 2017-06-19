using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using SMR.KM.Security.Security;
using SMR.KM.Business.Services;
using log4net;
using SMR.KM.Business.Models;
using SMR.KM.Business.Interfaces;
using SMR.KM.Common;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using CDPReporting.UI.Models;
using CDPReporting.Business;

namespace CDPReporting.UI.Controllers
{
    #region DefaultController
    //[Authorize]
    //public class AccountController : Controller
    //{
    //    private ApplicationSignInManager _signInManager;
    //    private ApplicationUserManager _userManager;

    //    public AccountController()
    //    {
    //    }

    //    public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
    //    {
    //        UserManager = userManager;
    //        SignInManager = signInManager;
    //    }

    //    public ApplicationSignInManager SignInManager
    //    {
    //        get
    //        {
    //            return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
    //        }
    //        private set 
    //        { 
    //            _signInManager = value; 
    //        }
    //    }

    //    public ApplicationUserManager UserManager
    //    {
    //        get
    //        {
    //            return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
    //        }
    //        private set
    //        {
    //            _userManager = value;
    //        }
    //    }

    //    //
    //    // GET: /Account/Login
    //    [AllowAnonymous]
    //    public ActionResult Login(string returnUrl)
    //    {
    //        ViewBag.ReturnUrl = returnUrl;
    //        return View();
    //    }

    //    //
    //    // POST: /Account/Login
    //    [HttpPost]
    //    [AllowAnonymous]
    //    [ValidateAntiForgeryToken]
    //    public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
    //    {
    //        if (!ModelState.IsValid)
    //        {
    //            return View(model);
    //        }

    //        // This doesn't count login failures towards account lockout
    //        // To enable password failures to trigger account lockout, change to shouldLockout: true
    //        var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
    //        switch (result)
    //        {
    //            case SignInStatus.Success:
    //                return RedirectToLocal(returnUrl);
    //            case SignInStatus.LockedOut:
    //                return View("Lockout");
    //            case SignInStatus.RequiresVerification:
    //                return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
    //            case SignInStatus.Failure:
    //            default:
    //                ModelState.AddModelError("", "Invalid login attempt.");
    //                return View(model);
    //        }
    //    }

    //    //
    //    // GET: /Account/VerifyCode
    //    [AllowAnonymous]
    //    public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
    //    {
    //        // Require that the user has already logged in via username/password or external login
    //        if (!await SignInManager.HasBeenVerifiedAsync())
    //        {
    //            return View("Error");
    //        }
    //        return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
    //    }

    //    //
    //    // POST: /Account/VerifyCode
    //    [HttpPost]
    //    [AllowAnonymous]
    //    [ValidateAntiForgeryToken]
    //    public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
    //    {
    //        if (!ModelState.IsValid)
    //        {
    //            return View(model);
    //        }

    //        // The following code protects for brute force attacks against the two factor codes. 
    //        // If a user enters incorrect codes for a specified amount of time then the user account 
    //        // will be locked out for a specified amount of time. 
    //        // You can configure the account lockout settings in IdentityConfig
    //        var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
    //        switch (result)
    //        {
    //            case SignInStatus.Success:
    //                return RedirectToLocal(model.ReturnUrl);
    //            case SignInStatus.LockedOut:
    //                return View("Lockout");
    //            case SignInStatus.Failure:
    //            default:
    //                ModelState.AddModelError("", "Invalid code.");
    //                return View(model);
    //        }
    //    }

    //    //
    //    // GET: /Account/Register
    //    [AllowAnonymous]
    //    public ActionResult Register()
    //    {
    //        return View();
    //    }

    //    //
    //    // POST: /Account/Register
    //    [HttpPost]
    //    [AllowAnonymous]
    //    [ValidateAntiForgeryToken]
    //    public async Task<ActionResult> Register(RegisterViewModel model)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
    //            var result = await UserManager.CreateAsync(user, model.Password);
    //            if (result.Succeeded)
    //            {
    //                await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);
                    
    //                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
    //                // Send an email with this link
    //                // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
    //                // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
    //                // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

    //                return RedirectToAction("Index", "Home");
    //            }
    //            AddErrors(result);
    //        }

    //        // If we got this far, something failed, redisplay form
    //        return View(model);
    //    }

    //    //
    //    // GET: /Account/ConfirmEmail
    //    [AllowAnonymous]
    //    public async Task<ActionResult> ConfirmEmail(string userId, string code)
    //    {
    //        if (userId == null || code == null)
    //        {
    //            return View("Error");
    //        }
    //        var result = await UserManager.ConfirmEmailAsync(userId, code);
    //        return View(result.Succeeded ? "ConfirmEmail" : "Error");
    //    }

    //    //
    //    // GET: /Account/ForgotPassword
    //    [AllowAnonymous]
    //    public ActionResult ForgotPassword()
    //    {
    //        return View();
    //    }

    //    //
    //    // POST: /Account/ForgotPassword
    //    [HttpPost]
    //    [AllowAnonymous]
    //    [ValidateAntiForgeryToken]
    //    public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            var user = await UserManager.FindByNameAsync(model.Email);
    //            if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
    //            {
    //                // Don't reveal that the user does not exist or is not confirmed
    //                return View("ForgotPasswordConfirmation");
    //            }

    //            // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
    //            // Send an email with this link
    //            // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
    //            // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
    //            // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
    //            // return RedirectToAction("ForgotPasswordConfirmation", "Account");
    //        }

    //        // If we got this far, something failed, redisplay form
    //        return View(model);
    //    }

    //    //
    //    // GET: /Account/ForgotPasswordConfirmation
    //    [AllowAnonymous]
    //    public ActionResult ForgotPasswordConfirmation()
    //    {
    //        return View();
    //    }

    //    //
    //    // GET: /Account/ResetPassword
    //    [AllowAnonymous]
    //    public ActionResult ResetPassword(string code)
    //    {
    //        return code == null ? View("Error") : View();
    //    }

    //    //
    //    // POST: /Account/ResetPassword
    //    [HttpPost]
    //    [AllowAnonymous]
    //    [ValidateAntiForgeryToken]
    //    public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
    //    {
    //        if (!ModelState.IsValid)
    //        {
    //            return View(model);
    //        }
    //        var user = await UserManager.FindByNameAsync(model.Email);
    //        if (user == null)
    //        {
    //            // Don't reveal that the user does not exist
    //            return RedirectToAction("ResetPasswordConfirmation", "Account");
    //        }
    //        var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
    //        if (result.Succeeded)
    //        {
    //            return RedirectToAction("ResetPasswordConfirmation", "Account");
    //        }
    //        AddErrors(result);
    //        return View();
    //    }

    //    //
    //    // GET: /Account/ResetPasswordConfirmation
    //    [AllowAnonymous]
    //    public ActionResult ResetPasswordConfirmation()
    //    {
    //        return View();
    //    }

    //    //
    //    // POST: /Account/ExternalLogin
    //    [HttpPost]
    //    [AllowAnonymous]
    //    [ValidateAntiForgeryToken]
    //    public ActionResult ExternalLogin(string provider, string returnUrl)
    //    {
    //        // Request a redirect to the external login provider
    //        return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
    //    }

    //    //
    //    // GET: /Account/SendCode
    //    [AllowAnonymous]
    //    public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
    //    {
    //        var userId = await SignInManager.GetVerifiedUserIdAsync();
    //        if (userId == null)
    //        {
    //            return View("Error");
    //        }
    //        var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
    //        var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
    //        return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
    //    }

    //    //
    //    // POST: /Account/SendCode
    //    [HttpPost]
    //    [AllowAnonymous]
    //    [ValidateAntiForgeryToken]
    //    public async Task<ActionResult> SendCode(SendCodeViewModel model)
    //    {
    //        if (!ModelState.IsValid)
    //        {
    //            return View();
    //        }

    //        // Generate the token and send it
    //        if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
    //        {
    //            return View("Error");
    //        }
    //        return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
    //    }

    //    //
    //    // GET: /Account/ExternalLoginCallback
    //    [AllowAnonymous]
    //    public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
    //    {
    //        var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
    //        if (loginInfo == null)
    //        {
    //            return RedirectToAction("Login");
    //        }

    //        // Sign in the user with this external login provider if the user already has a login
    //        var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
    //        switch (result)
    //        {
    //            case SignInStatus.Success:
    //                return RedirectToLocal(returnUrl);
    //            case SignInStatus.LockedOut:
    //                return View("Lockout");
    //            case SignInStatus.RequiresVerification:
    //                return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
    //            case SignInStatus.Failure:
    //            default:
    //                // If the user does not have an account, then prompt the user to create an account
    //                ViewBag.ReturnUrl = returnUrl;
    //                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
    //                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
    //        }
    //    }

    //    //
    //    // POST: /Account/ExternalLoginConfirmation
    //    [HttpPost]
    //    [AllowAnonymous]
    //    [ValidateAntiForgeryToken]
    //    public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
    //    {
    //        if (User.Identity.IsAuthenticated)
    //        {
    //            return RedirectToAction("Index", "Manage");
    //        }

    //        if (ModelState.IsValid)
    //        {
    //            // Get the information about the user from the external login provider
    //            var info = await AuthenticationManager.GetExternalLoginInfoAsync();
    //            if (info == null)
    //            {
    //                return View("ExternalLoginFailure");
    //            }
    //            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
    //            var result = await UserManager.CreateAsync(user);
    //            if (result.Succeeded)
    //            {
    //                result = await UserManager.AddLoginAsync(user.Id, info.Login);
    //                if (result.Succeeded)
    //                {
    //                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
    //                    return RedirectToLocal(returnUrl);
    //                }
    //            }
    //            AddErrors(result);
    //        }

    //        ViewBag.ReturnUrl = returnUrl;
    //        return View(model);
    //    }

    //    //
    //    // POST: /Account/LogOff
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public ActionResult LogOff()
    //    {
    //        AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
    //        return RedirectToAction("Index", "Home");
    //    }

    //    //
    //    // GET: /Account/ExternalLoginFailure
    //    [AllowAnonymous]
    //    public ActionResult ExternalLoginFailure()
    //    {
    //        return View();
    //    }

    //    protected override void Dispose(bool disposing)
    //    {
    //        if (disposing)
    //        {
    //            if (_userManager != null)
    //            {
    //                _userManager.Dispose();
    //                _userManager = null;
    //            }

    //            if (_signInManager != null)
    //            {
    //                _signInManager.Dispose();
    //                _signInManager = null;
    //            }
    //        }

    //        base.Dispose(disposing);
    //    }

    //    #region Helpers
    //    // Used for XSRF protection when adding external logins
    //    private const string XsrfKey = "XsrfId";

    //    private IAuthenticationManager AuthenticationManager
    //    {
    //        get
    //        {
    //            return HttpContext.GetOwinContext().Authentication;
    //        }
    //    }

    //    private void AddErrors(IdentityResult result)
    //    {
    //        foreach (var error in result.Errors)
    //        {
    //            ModelState.AddModelError("", error);
    //        }
    //    }

    //    private ActionResult RedirectToLocal(string returnUrl)
    //    {
    //        if (Url.IsLocalUrl(returnUrl))
    //        {
    //            return Redirect(returnUrl);
    //        }
    //        return RedirectToAction("Index", "Home");
    //    }

    //    internal class ChallengeResult : HttpUnauthorizedResult
    //    {
    //        public ChallengeResult(string provider, string redirectUri)
    //            : this(provider, redirectUri, null)
    //        {
    //        }

    //        public ChallengeResult(string provider, string redirectUri, string userId)
    //        {
    //            LoginProvider = provider;
    //            RedirectUri = redirectUri;
    //            UserId = userId;
    //        }

    //        public string LoginProvider { get; set; }
    //        public string RedirectUri { get; set; }
    //        public string UserId { get; set; }

    //        public override void ExecuteResult(ControllerContext context)
    //        {
    //            var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
    //            if (UserId != null)
    //            {
    //                properties.Dictionary[XsrfKey] = UserId;
    //            }
    //            context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
    //        }
    //    }
    //    #endregion
    //}
    #endregion

    public class AccountController : BaseController
    {
        public IFormsAuthenticationService FormsService { get; set; }
        public IMembershipService MembershipService { get; set; }
        private ILog _log;
        private IUser _userServices;

        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }
            base.Initialize(requestContext);
            this._userServices = new UserServices();
        }
        //
        // GET: /Account/LogOn
        public ActionResult Login()
        {
            try
            {
                _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                if (Session["windowsUserName"] != null)
                {
                    string windowUserName = Session["windowsUserName"].ToString();
                    var windowUser = _userServices.GetUserDetailsFromAD(windowUserName);
                    if (_log.IsInfoEnabled) _log.Info(windowUser.UserName + "is user returned from GetUserDetailsFromAD method ");
                    var logon = new LogOnModel
                    {
                        UserName = windowUser.UserName,
                    };
                    return View(logon);
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                if (_log.IsErrorEnabled) _log.Error(ex.Message, ex);
                throw ex;
            }

        }

        //
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult Login(LogOnModel model, string returnUrl)
        {
            try
            {
                _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                AppLog oAppLog = new AppLog();
                oAppLog.UserName = model.UserName;
                oAppLog.Method = Convert.ToString(System.Reflection.MethodBase.GetCurrentMethod().Name);
                oAppLog.Module = Convert.ToString(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                oAppLog.LogDate = DateTime.UtcNow;
                if (ModelState.IsValid)
                {
                    if (_log.IsInfoEnabled) _log.Info("Logging in..Calling KMSEcurity Login method.");
                    if (KMSecurity.Login(model.UserName, model.Password, model.RememberMe))
                    {
                        if (_log.IsInfoEnabled) _log.Info(model.UserName + " has been logged in....");
                        oAppLog.Message = "LogOn was sucessfull.";
                        this.LogActivity(oAppLog);
                        if (logDB.IsInfoEnabled) logDB.Info("");
                        if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                            && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        if (_log.IsInfoEnabled) _log.Info(model.UserName + " has not been logged in....");
                        oAppLog.Message = "LogOn was unsucessfull.";
                        this.LogActivity(oAppLog);
                        if (logDB.IsInfoEnabled) logDB.Info("");
                        ModelState.AddModelError("", "The user name or password provided is incorrect.");
                    }

                }
            }
            catch (Exception ex)
            {
                if (_log.IsErrorEnabled) _log.Error(ex.Message, ex);
                throw ex;
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }
        public ActionResult LogOnWithWindowsAuthentication()
        {
            try
            {

                string username = string.Empty;
                string returnUrl = string.Empty;
                NameValueCollection n = Request.QueryString;
                if (n.HasKeys())
                {
                    username = n.GetKey(0);
                    if (username.Contains("userName"))
                    {
                        username = n.Get(0);
                    }
                    if (n.Keys.Count > 1)
                    {
                        returnUrl = n.GetKey(1);
                        if (returnUrl.Contains("returnUrl"))
                        {

                            returnUrl = n.Get(1);
                        }
                    }
                }
                Session["windowsUserName"] = username;
                _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                if (_log.IsInfoEnabled) _log.Info(username + " from LDAP");
                AppLog oAppLog = new AppLog();
                oAppLog.UserName = username;
                oAppLog.Method = Convert.ToString(System.Reflection.MethodBase.GetCurrentMethod().Name);
                oAppLog.Module = Convert.ToString(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                oAppLog.LogDate = DateTime.UtcNow;
                if (_log.IsInfoEnabled) _log.Info("Logging in..Calling KMSEcurity LoginWithWindowsAuthentication method.");
                if (KMSecurity.LoginWithWindowsAuthentication(username, false))
                {
                    if (_log.IsInfoEnabled) _log.Info(username + " has been logged in....");
                    oAppLog.Message = "LogOn was sucessfull.";
                    this.LogActivity(oAppLog);
                    if (logDB.IsInfoEnabled) logDB.Info("");
                }
                else
                {
                    if (_log.IsInfoEnabled) _log.Info(username + " has not been logged in....");
                    oAppLog.Message = "LogOn was unSucessfull.";
                }
                if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                          && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                if (_log.IsErrorEnabled) _log.Error(ex.Message, ex);
                throw ex;
            }
            // If we got this far, something failed, redisplay form
        }


        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            AppLog oAppLog = new AppLog();
            oAppLog.UserName = CurrentUser.UserName;
            oAppLog.Method = Convert.ToString(System.Reflection.MethodBase.GetCurrentMethod().Name);
            oAppLog.Module = Convert.ToString(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            oAppLog.LogDate = DateTime.UtcNow;
            try
            {

                if (_log.IsInfoEnabled) _log.Info("Logging off..Calling Logout method of KMSecurity in AccountController.");
                KMSecurity.Logout();
                oAppLog.Message = "LogOff was sucessfull";
                this.LogActivity(oAppLog);
                if (logDB.IsInfoEnabled) logDB.Info("");
                if (_log.IsInfoEnabled) _log.Info("Logged off.");
                if (_log.IsInfoEnabled) _log.Info("Redirection to the Home page.");
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                oAppLog.Message = "LogOff was unsucessfull";
                this.LogActivity(oAppLog);
                if (logDB.IsInfoEnabled) logDB.Info("");
                if (_log.IsErrorEnabled) _log.Error(ex.Message, ex);
                throw ex;
            }
        }

        //
        // GET: /Account/Register

        public ActionResult Register()
        {
            try
            {
                _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                string windowName = HttpContext.User.Identity.Name;

                if (_log.IsInfoEnabled) _log.Info("Window Name" + windowName);

                string windowUserName = string.Empty;
                Regex rx = new Regex(@"[^\\\\]*$");
                windowUserName = Convert.ToString(rx.Match(windowName));

                if (_log.IsInfoEnabled) _log.Info("Register for AD User:-" + windowUserName);

                var windowUser = _userServices.GetUserDetailsFromAD(windowUserName);

                if (_log.IsInfoEnabled) _log.Info(windowUser.UserName + " is user returned from GetUserDetailsFromAD method ");

                ViewBag.PasswordLength = MembershipService.MinPasswordLength;
                ViewBag.PasswordLength = MembershipService.MinPasswordLength;

                var plantlist = _userServices.GetPlant();
                List<Plant> plants = new List<Plant>();
                plants = plantlist.ResultList.ToList();
                var defaultPlant = new Plant
                {
                    PlantId = Guid.NewGuid(),
                    PlantName = "--Please Select Plant--"
                };
                plants.Insert(0, defaultPlant);

                PlantResult oPR = new PlantResult();
                oPR.ResultList = plants;
                string dName = string.Empty;
                string email = string.Empty;
                bool IsAddetail = false;

                if (string.IsNullOrEmpty(Convert.ToString(windowUser.FirstName)))
                {
                    dName = windowUserName;
                }
                else
                {
                    dName = windowUser.FirstName;
                }

                if (!string.IsNullOrEmpty(Convert.ToString(windowUser.Email)))
                {
                    email = Convert.ToString(windowUser.Email).ToLower();
                    IsAddetail = true;
                }


                if (_log.IsInfoEnabled) _log.Info("Data filled in RegisterModel to reder register view");

                var register = new RegisterModel
                {
                    Plants = oPR,
                    UserName = windowUserName,
                    Password = "Smr1234",
                    Email = email,
                    DisplayName = dName,
                    IsADDetailFetched = IsAddetail
                };

                return View(register);
            }
            catch (Exception ex)
            {
                if (_log.IsErrorEnabled) _log.Error(ex.Message, ex);
                throw ex;
            }


        }

        //
        // POST: /Account/Register

        [HttpPost]
        public ActionResult Register(RegisterModel model, string returnUrl)
        {
            try
            {
                _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                if (_log.IsInfoEnabled) _log.Info("calling register post method");
                if (_log.IsInfoEnabled) _log.Info(ModelState.IsValid);
                if (ModelState.IsValid)
                {
                    // Attempt to register the user
                    if (_log.IsInfoEnabled) _log.Info("calling CreateUser method of MembershipService");
                    MembershipCreateStatus createStatus = MembershipService.CreateUser(model.UserName, model.Password, model.Email, model.PlantId);

                    if (createStatus == MembershipCreateStatus.Success)
                    {
                        if (_log.IsInfoEnabled) _log.Info("User Created from CreateUser method of MembershipService.");
                        FormsService.SignIn(model.UserName, false /* createPersistentCookie */);
                        UserServices oUserServices = new UserServices();
                        var item = oUserServices.UserIdByUserName(model.UserName);
                        string message = item.IsAnonymous ? "use the password you registered with" : "use your windows logon password";
                        EmailServices oEmailServices = new EmailServices();
                        oEmailServices.RegisteredUserEmail(model.Email, model.UserName, item.UserId, message);

                        AdminServices oAdminServices = new AdminServices();
                        oAdminServices.InsertDefaultUserNotification(item.UserId);

                        if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                            && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        if (_log.IsInfoEnabled) _log.Info("Failed to register User from CreateUser method of MembershipService.");
                        ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
                    }
                }
            }
            catch (Exception ex)
            {
                if (_log.IsErrorEnabled) _log.Error(ex.Message, ex);
                throw ex;
            }
            // If we got this far, something failed, redisplay form

            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            model.Plants = _userServices.GetPlant();
            return View(model);
        }

        //
        // GET: /Account/ChangePassword

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        public ActionResult ForgotPassword()
        {
            try
            {
                _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                return View();
            }
            catch (Exception ex)
            {
                if (_log.IsErrorEnabled) _log.Error(ex.Message, ex);
                throw ex;
            }

        }
        [HttpPost]
        public ActionResult ForgotPassword(SMR.KM.Business.Models.User userForgot)
        {
            try
            {
                _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                if (!string.IsNullOrEmpty(userForgot.UserName))
                {
                    var user = _userServices.GetUser(userForgot.UserName);
                    if (user != null)
                    {
                        if (user.IsAnonymous == false)
                        {
                            throw new Exception("You are registered as an active directory user. Please use your window credentials to logon.");
                        }
                        else
                        {
                            string decryptedPwd = KMEncryptDecrypt.Decrypt(user.Password, SiteConfig.KMEncryptDecryptKey);
                            EmailServices oEmailServices = new EmailServices();
                            bool isSucess = oEmailServices.ForgotPasswordEmail(user, decryptedPwd);
                            if (isSucess)
                            {
                                throw new Exception("Please check your mailbox for password.");
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Specified user does not exist.");
                    }
                }
                else
                {
                    throw new Exception("Username is required.");
                }
                return View();
            }
            catch (Exception ex)
            {
                if (_log.IsErrorEnabled) _log.Error(ex.Message, ex);
                throw ex;
            }

        }
        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}