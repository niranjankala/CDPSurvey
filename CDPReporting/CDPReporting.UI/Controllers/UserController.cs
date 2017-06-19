using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CDPReporting.UI.Models;
using SMR.KM.Data.Entities;
using SMR.KM.Business.Interfaces;
using SMR.KM.Business.Models;
using SMR.KM.Business.Services;
using SMR.KM.Security.Security;
using SMR.KM.Common;
using System.IO;
using System.Web.Hosting;
using log4net;
using System.Web.Security;
using System.Web.Routing;
using System.Web.Script.Services;
using System.Runtime.Remoting.Contexts;
using System.Web.Services;
using CDPReporting.UI;
using CDPReporting.UI.Controllers;

namespace CDPReporting.Controllers
{
    [ActionAuthorize]
    public class UserController : BaseController
    {
        private KMEntities db = new KMEntities();
        private IUser _userServices;
        private IGroup _groupServices;
        private ILog _log;
        private FileServices fileServices;
        public IMembershipService MembershipService { get; set; }
        private EmailServices _emailServices;
        //private PermissionServices _permissionServices;
        protected override void Initialize(RequestContext requestContext)
        {
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }
            base.Initialize(requestContext);
        }

        public UserController()
        {
            this._userServices = new UserServices();
            this.fileServices = new FileServices();
            this._emailServices = new EmailServices();
            //this._permissionServices = new PermissionServices();
            _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            _groupServices = new GroupServices();
        }

        public UserController(IUser userServices)
        {
            this._userServices = userServices;
            this.fileServices = new FileServices();
        }

        //
        // GET: /User/
        // [OutputCache(CacheProfile = "CacheProfile1", VaryByParam="userName")]
        public ViewResult Index(string userName = null )
        {
            try
            {
                if (_log.IsInfoEnabled) _log.Info("Calling GetUserByInitial method of UserService.");
                if (userName != null)
                {
                    if (_log.IsDebugEnabled) _log.Debug("Calling GetUserByInitial method of userService");
                    var users = _userServices.GetUserByInitial(userName, CurrentUser);
                    ViewBag.InitialChar = userName;
                    return View(users);
                }
                else
                {
                    var users = _userServices.GetUserByInitial("A", CurrentUser);
                    ViewBag.InitialChar = "A";
                    return View(users);
                }

            }
            catch (Exception ex)
            {
                if (_log.IsErrorEnabled) _log.Error(ex.Message, ex);
                throw ex;
            }

        }

        public ViewResult Details(Guid id, string userName)
        {
            try
            {
                if (_log.IsInfoEnabled) _log.Info("Calling GetUser by userId method of UserService.");
                var users = _userServices.GetUser(id, CurrentUser);
                if (string.IsNullOrEmpty(userName))
                {
                    users.isFromProfileIndex = false;
                }
                else
                {
                    users.isFromProfileIndex = true;
                }
                if (users.IsAnonymous == false)
                {
                    users.IsAnonymous = true;
                }
                else
                {
                    users.IsAnonymous = false;
                }
                ViewBag.InitialChar = userName;
                return View(users);
            }
            catch (Exception ex)
            {
                if (_log.IsErrorEnabled) _log.Error(ex.Message, ex);
                throw ex;
            }

        }

        //
        // GET: /User/Create
       
        public ActionResult Create()
        {
            try
            {
                if (_log.IsInfoEnabled) _log.Info("Calling GetPlant method of UserService.");
                if (Request.IsAuthenticated)
                {
                    var user = new SMR.KM.Business.Models.User()
                    {
                        Plants = _userServices.GetPlant(),
                        UserGroups = _userServices.GetUserGroups()
                    };
                    return View(user);
                }
                else
                {
                    return RedirectToAction("Logon", "Account");
                }
            }
            catch (Exception ex)
            {
                if (_log.IsErrorEnabled) _log.Error(ex.Message, ex);
                throw ex;
            }
        }

        //
        // POST: /User/Create

        [HttpPost]
      
        public ActionResult Create(SMR.KM.Business.Models.User user)
        {
            try
            {
                if (_log.IsInfoEnabled) _log.Info("Calling AddUser method of UserService.");
                if (string.IsNullOrEmpty(user.UserAvatar))
                {
                    ModelState.Remove("UserAvatar");
                    user.UserAvatar = Url.Content("~/Content/Images/Anonymsuser.jpg");
                }
               // bool canCreateProfile = _permissionServices.CanCreateProfile(CurrentUser, "Create", "User");
                //if (!canCreateProfile)
                //{
                //    throw new Exception("You are not authorized to create user.");
                //}
                if (ModelState.IsValid)
                {
                    MembershipCreateStatus createStatus = MembershipService.CreateUser(user.UserName, user.Password, user.Email, user.PlantId);
                    if (createStatus == MembershipCreateStatus.Success)
                    {
                        var item = _userServices.UserIdByUserName(user.UserName);
                        _userServices.AddUser(user, CurrentUser);
                        
                        string message = item.IsAnonymous ? "use the password you registered with" : "use your windows logon password";
                        _emailServices.RegisteredUserEmail(user.Email, string.IsNullOrEmpty(user.FirstName) && string.IsNullOrEmpty(user.LastName) ? user.UserName : string.IsNullOrEmpty(user.FirstName) ? user.LastName : string.IsNullOrEmpty(user.LastName) ? user.FirstName : user.FirstName.ToUpper() == user.LastName.ToUpper() ? user.FirstName.ToUpper() :user.LastName.ToUpper() + " " + user.FirstName.ToUpper(), user.UserId, message);
                        return RedirectToAction("Index", "User");
                    }
                    else
                    {
                        ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
                    }

                }
            }
            catch (DataException ex)
            {
                if (_log.IsErrorEnabled) _log.Error(ex.Message, ex);
                throw ex;
            }
            user.Plants = _userServices.GetPlant();
            user.UserGroups = _userServices.GetUserGroups();
            return View(user);
        }

        //
        // GET: /User/Edit/5

        public ActionResult Edit(Guid id, string initialChar)
        {
            try
            {
                if (_log.IsInfoEnabled) _log.Info("Calling GetUser by userId method of UserService.");
                var users = _userServices.GetUser(id, CurrentUser);
                if (users.IsAnonymous == false)
                {
                    users.IsAnonymous = true;
                }
                else
                {
                    users.IsAnonymous = false;
                }
                users.Plants = _userServices.GetPlant();
                users.UserGroups = _userServices.GetUserGroups();
                ViewBag.InitialChar = initialChar;
                users.PasswordCheck = users.Password;
                return View(users);
            }
            catch (Exception ex)
            {
                if (_log.IsErrorEnabled) _log.Error(ex.Message, ex);
                throw ex;
            }
        }

        //
        // POST: /User/Edit/5

        [HttpPost]
        public ActionResult Edit(SMR.KM.Business.Models.User user, string initialChar)
        {
            try
            {
                if (_log.IsInfoEnabled) _log.Info("Calling EditUser method of UserService.");
                if (ModelState.IsValid)
                {
                    string hashedPassword = user.PasswordCheck == user.Password ? user.Password : KMEncryptDecrypt.Encrypt(user.Password, SiteConfig.KMEncryptDecryptKey);
                    if (user.IsAnonymous == false)
                    {
                        user.IsAnonymous = true;
                    }
                    else
                    {
                        user.IsAnonymous = false;
                    }
                    user.Password = hashedPassword;
                    _userServices.EditUser(user, CurrentUser);
                    user.Plants = _userServices.GetPlant();
                    user.UserGroups = _userServices.GetUserGroups();
                    ViewBag.InitialChar = initialChar;
                    return RedirectToAction("Details","User", new { id = user.UserId, userName = initialChar });
                }
            }
            catch (Exception ex)
            {
                if (_log.IsErrorEnabled) _log.Error(ex.Message, ex);
                throw ex;
            }
            user.Plants = _userServices.GetPlant();
            user.UserGroups = _userServices.GetUserGroups();
            return View(user);
        }

        //
        // GET: /User/Delete/5

        public ActionResult Delete(Guid id)
        {
            try
            {
                if (_log.IsInfoEnabled) _log.Info("Calling GetUser by userId method of UserService.");
                var users = _userServices.GetUser(id, CurrentUser);
                return View(users);
            }
            catch (Exception ex)
            {
                if (_log.IsErrorEnabled) _log.Error(ex.Message, ex);
                throw ex;
            }
        }

        //
        // POST: /User/Delete/5

        [HttpPost]
        public ActionResult DeleteConfirmed(Guid id)
        {
            try
            {
                if (_log.IsInfoEnabled) _log.Info("Calling DeleteUser method of UserService.");
                _userServices.DeleteUser(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                if (_log.IsErrorEnabled) _log.Error(ex.Message, ex);
                throw ex;
            }
        }
        public string Upload(HttpPostedFileBase fileData)
        {
            try
            {
                var fileName = string.Empty;
                if (_log.IsInfoEnabled) _log.Info("Calling method to Upload User Avatar");
                HttpFileCollectionBase hfc = Request.Files;
                for (int i = 0; i < hfc.Count; i++)
                {
                    HttpPostedFileBase hpf = hfc[i];
                    if (!Directory.Exists(Server.MapPath(DirectoryPaths.UserAvatars)))
                    {
                        Directory.CreateDirectory(Server.MapPath(DirectoryPaths.UserAvatars));
                    }
                    fileName = this.fileServices.UploadAvatar(hpf, UploadAvatarType.UserAvatar);
                }
                return Url.Content(DirectoryPaths.UserAvatars + "\\" + fileName);
            }
            catch (Exception ex)
            {
                if (_log.IsErrorEnabled) _log.Error(ex.Message, ex);
                throw ex;
            }

        }
       
        public string UploadChangedImage(HttpPostedFileBase fileData, Guid userId)
        {
            try
            {
                var fileName = string.Empty;
                if (_log.IsInfoEnabled) _log.Info("Calling UploadChangedImage method in UserController.");

                HttpFileCollectionBase hfc =Request.Files;
                for (int i = 0; i < hfc.Count; i++)
                {
                    HttpPostedFileBase hpf = hfc[i];
                    if (_log.IsInfoEnabled) _log.Info("Uploading user avatar" + hpf.FileName + " UserId " + userId);
                    if (!Directory.Exists(Server.MapPath(DirectoryPaths.UserAvatars)))
                    {
                        Directory.CreateDirectory(Server.MapPath(DirectoryPaths.UserAvatars));
                    }
                    fileName = this.fileServices.UploadAvatar(hpf, UploadAvatarType.UserAvatar);
                    this._userServices.ChangeGroupImage(userId, Url.Content(DirectoryPaths.UserAvatars + "\\" + fileName));
                }
                return Url.Content(DirectoryPaths.UserAvatars + "\\" + fileName);
            }
            catch (Exception ex)
            {
                if (_log.IsErrorEnabled) _log.Error(ex.Message, ex);
                throw ex;
            }

        }

        
        /// <summary>
        /// Method to follow user.
        /// </summary>
        /// <param name="followUserId"></param>
        public void FollowUser(Guid userId)
        {
            try
            {
                if (_log.IsInfoEnabled) _log.Info("Calling FollowUser method of userService to follow a user");
                int returnValue = this._userServices.FollowUser(userId, CurrentUser.UserId);
            }
            catch (Exception ex)
            {
                if (_log.IsErrorEnabled) _log.Error(ex.Message, ex);
                throw ex;
            }
        }
        /// <summary>
        /// Method to Unfollow user
        /// </summary>
        /// <param name="followUserId"></param>
        public void UnFollowUser(Guid userId)
        {
            try
            {
                if (_log.IsInfoEnabled) _log.Info("Calling UnFollowUser method of userService to unfollow a user");
                int returnValue = this._userServices.UnFollowUser(userId, CurrentUser.UserId);
            }
            catch (Exception ex)
            {
                if (_log.IsErrorEnabled) _log.Error(ex.Message, ex);
                throw ex;
            }
        }

        /// <summary>
        /// Method to view user wall
        /// </summary>
        /// <param name="followUserId"></param>
        public ViewResult UserWallView()
        {
            try
            {
                if (_log.IsInfoEnabled) _log.Info("Calling ViewUserWall method to view the user wall screen.");
                return View();
            }
            catch (Exception ex)
            {
                if (_log.IsErrorEnabled) _log.Error(ex.Message, ex);
                throw ex;
            }
        }
        /// <summary>
        /// Method to view user emailNotifications
        /// </summary>
        public ViewResult EmailsNotification(Guid userId, string initialChar)
        {
            try
            {
                if (_log.IsInfoEnabled) _log.Info("Calling EmailsNotification method to view the user emails notification.");
                EmailsNotificationColl oUserEmailNotifications = _userServices.GetNotificationAttributes(userId, CurrentUser);
                ViewBag.InitialChar = initialChar;
                return View(oUserEmailNotifications);
            }
            catch (Exception ex)
            {
                if (_log.IsErrorEnabled) _log.Error(ex.Message, ex);
                throw ex;
            }
        }


        public bool AllowEmailsNotification(Guid userId, Guid emailNotificationId)
        {
            try
            {
                if (_log.IsInfoEnabled) _log.Info("Called AllowEmailsNotification method.");
                _userServices.AllowNotifications(userId, emailNotificationId, CurrentUser);
            }
            catch (DataException ex)
            {
                if (_log.IsErrorEnabled) _log.Error(ex.Message, ex);
                throw ex;
            }
            return true;
        }

        public bool DisAllowEmailsNotification(Guid userId, Guid emailNotificationId)
        {
            try
            {
                if (_log.IsInfoEnabled) _log.Info("Called DisAllowEmailsNotification method.");
                _userServices.DisAllowNotifications(userId, emailNotificationId, CurrentUser);
            }
            catch (DataException ex)
            {
                if (_log.IsErrorEnabled) _log.Error(ex.Message, ex);
                throw ex;
            }
            return true;
        }

        //
        // GET:/User/
        // [OutputCache(CacheProfile = "CacheProfile1", VaryByParam="userName")]
        public ViewResult ManageGroups(Guid userId)
        {
            try
            {
                if (_log.IsInfoEnabled) _log.Info("Getting groups of a particular user.");
                if (_log.IsDebugEnabled) _log.Debug("Getting user specific groups.");
                var groups =_userServices.GetAllGroups(userId, CurrentUser);
                return View();
            }
            catch (Exception ex)
            {
                if (_log.IsErrorEnabled) _log.Error(ex.Message, ex);
                throw ex;
            }

        }
        public ActionResult ManagePublicGroups(Guid userId)
        {
            try
            {
                if (_log.IsInfoEnabled) _log.Info("Getting groups of a particular user.");
                if (_log.IsDebugEnabled) _log.Debug("Getting user specific groups.");
                var groups = _userServices.GetAllGroups(userId, CurrentUser);
                if (Request.IsAjaxRequest())
                {
                    return PartialView("ManageGroups", groups);
                }
                return View();
              
            }
            catch (Exception ex)
            {
                if (_log.IsErrorEnabled) _log.Error(ex.Message, ex);
                throw ex;
            }

        }

        public ActionResult ManagePrivateGroups(Guid userId)
        {
            try
            {
                if (_log.IsInfoEnabled) _log.Info("Getting groups of a particular user.");
                if (_log.IsDebugEnabled) _log.Debug("Getting user specific groups.");
                var groups = _userServices.GetAllPrivateGroups(userId, CurrentUser);
                if (Request.IsAjaxRequest())
                {
                    return PartialView("ManageGroups", groups);
                }
                return View(groups);
            }
            catch (Exception ex)
            {
                if (_log.IsErrorEnabled) _log.Error(ex.Message, ex);
                throw ex;
            }

        }
        /// <summary>
        /// Method to get All groups
        /// </summary>
        public ActionResult GetAllGroups(Guid userId)
        {
            try
            {
                if (_log.IsInfoEnabled) _log.Info("Calling GetGroupsByUserId method of GroupController");
                var groups = _userServices.GetAllGroups(userId, CurrentUser);
                if (Request.IsAjaxRequest())
                {
                    return PartialView("ManageGroups", groups);
                }
                return View();
            }
            catch (Exception ex)
            {
                if (_log.IsErrorEnabled) _log.Error(ex.Message, ex);
                throw ex;
            }
        }

        /// <summary>
        /// Method to get groups based on userID
        /// </summary>
        public ActionResult MyGroups()
        {
            try
            {
                if (_log.IsInfoEnabled) _log.Info("Calling GetGroupsByUserId method of GroupController");
                var groups = _userServices.GetUserSpecificGroups(CurrentUser.UserId, CurrentUser);
                if (Request.IsAjaxRequest())
                {
                    return PartialView("ManageGroups", groups);
                }
                return View();
            }
            catch (Exception ex)
            {
                if (_log.IsErrorEnabled) _log.Error(ex.Message, ex);
                throw ex;
            }
        }

        public bool JoinGroup(Guid groupId, Guid userId)
        {
            try
            {
                if (CurrentUser.UserName == null)
                {
                    return false;
                }

                if (_log.IsInfoEnabled) _log.Info("Calling JoinGroup method of  groupService to assign user to group");
                int returnValue = _groupServices.JoinGroup(groupId, userId);
                return true;
            }
            catch (Exception ex)
            {
                if (_log.IsErrorEnabled) _log.Error(ex.Message, ex);
                throw ex;
            }
        }


        public void UnJoinGroup(Guid groupId, Guid userId)
        {
            try
            {
                if (_log.IsInfoEnabled) _log.Info("Calling UnJoinGroup method of  groupService to Unassign user to group");
                int returnValue = _groupServices.UnJoinGroup(groupId, userId);
            }
            catch (Exception ex)
            {
                if (_log.IsErrorEnabled) _log.Error(ex.Message, ex);
                throw ex;
            }
        }

        public ActionResult UserNav(Guid userId)
        {
            var user = new SMR.KM.Business.Models.User();
            user = _userServices.GetUser(userId, CurrentUser);
            return PartialView("UserNav", user);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public void UploadImage(SMR.KM.Business.Models.User user)
        {


        }
    }
}