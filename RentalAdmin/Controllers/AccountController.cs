using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using RentalAdmin.helper;
using RentalAdmin.Models;

namespace RentalAdmin.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private RentalEntities db = new RentalEntities();

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            if(User.Identity.IsAuthenticated)
            {
                return RedirectToAction("index", "Properties", new { a = "a" });
            }
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        //[helper.GoogleRecaptchaActionFilter]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, true, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    //if(string.IsNullOrEmpty(returnUrl))
                    //{
                    //    return RedirectToAction("index", "Properties",new { a="a"});
                    //}
                    return RedirectToLocalAfterLogin(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "خطار در ورودی");
                    return View(model);
            }
        }

        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        //[helper.GoogleRecaptchaActionFilter]
        public async Task<ActionResult> Register(RegisterViewModel model,string returnurl)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email,PhoneNumber=model.PhoneNumber };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    //db.AspNetUsers.Where(a=>a)
                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToLocalAfterRegister(returnurl); // RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        //[helper.GoogleRecaptchaActionFilter]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Email =helper.ConvertString.MyTrimToLower(model.Email);
                var user = await db.AspNetUsers.Where(a => a.Email == model.Email).FirstOrDefaultAsync();
                //if (user == null)
                //{
                //    return null;
                //}
                //if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                //{
                //    // Don't reveal that the user does not exist or is not confirmed
                //    return View("ForgotPasswordConfirmation");
                //}

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                //  await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                string EmailReason = "ChangePassword";
            
                string reciverEmailName = user.UserFName +" "+ user.UserLName;
                System.Guid directmessageid = System.Guid.NewGuid();
                GeneralEmailModel Emailmodel = new Models.GeneralEmailModel()
                {
                    UserName = reciverEmailName,
                    To = user.Email,
                    Subject = "تغییر رمز عبور _ RentalIr",
                    Body = "",
                    Goodbye = "با تشکر",
                    EmailPreHearder = "تغییر رمز عبور",
                    LinkUrl = callbackUrl,
                    LinkTxt = "تغییر رمز عبور",
                    UnsubscribeUrl = "id=" + directmessageid.ToString()

                };
                try
                {
                    EmailManager em = new EmailManager(db);
                    var ress = await em.sendDirectMessageasync(user.Email, "تغییر رمز عبور _ RentalIr", "تغییر رمز عبور _ RentalIr", "LGeneral",
                         Emailmodel, "تغییر رمز عبور _ RentalIr", EmailReason, 23, directmessageid, true);
                    em = null;
                }
                catch
                {
                    return RedirectToAction("ForgotPasswordConfirmation", "Account");
                }

                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        //[helper.GoogleRecaptchaActionFilter]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                ((List<string>)TempData["message"]).Add(Models.StaticList.ErrorInRegisterByExternalLogin);
                return RedirectToAction("Login");
            }
            var userExist = db.AspNetUsers.Where(a => a.Email == loginInfo.Email).Select(a => new { a.UserName, a.Id, a.EmailConfirmed }).FirstOrDefault();
            ///*************************************************************************************************************************
            //amniate in If paeen Bayad chek shavad *******************************************************************************************
            ///*************************************************************************************************************************
            if (userExist != null && !string.IsNullOrEmpty(userExist.UserName))
            {
                loginInfo.DefaultUserName = userExist.UserName;
                if (!db.AspNetUserLogins.Any(a => a.UserId == userExist.Id))
                {
                    AspNetUserLogin userLogin = new AspNetUserLogin
                    {
                        LoginProvider = loginInfo.Login.LoginProvider
                        ,
                        ProviderKey = loginInfo.Login.ProviderKey,
                        UserId = userExist.Id
                    };
                    db.AspNetUserLogins.Add(userLogin);
                    await db.SaveChangesAsync();
                }
            }
            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: true);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocalAfterLogin(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                //case SignInStatus.RequiresVerification:
                //    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default://yani yek karbar jadid ast.
                    {
                        // Check UserName to be valide
                        ApplicationUser user = null;

                        string checkUserName = ConvertEmailToUserName("ga" + loginInfo.Email);
                        if (checkUserName == null)
                        {
                            checkUserName = "ga" + DateTime.Now.Month.ToString()
                                + DateTime.Now.Day.ToString()
                                + DateTime.Now.Hour.ToString()
                                + DateTime.Now.Second.ToString();
                        }
                        else
                        {
                            user = new ApplicationUser
                            {
                                UserName = checkUserName,
                                Email = loginInfo.Email
                            };
                        }

                        //add extra info from google to profile
                        //در این قسمت ادرس عکس گوگل پلاس طرف دریافت می شود
                        //string imageUrl = loginInfo.ExternalIdentity.FindFirstValue(ClaimTypes.Webpage);
                        //if (imageUrl != "" && imageUrl != null)

                        string Image = "";
                        bool? Gender = null;
                        string PersonalWebPage = "";
                        string FName = "";
                        string LName = "";
                        string googlePlus = "";
                        string GenderdString = "";
                        Nullable<DateTime> birthday = null;
                        try
                        {
                            var urls = loginInfo.ExternalIdentity.FindFirst(ClaimTypes.Webpage);
                            if (urls != null)
                            {
                                Image = urls.Value;
                            }
                        }
                        catch (Exception)
                        {

                        }
                        try
                        {
                            var urls = loginInfo.ExternalIdentity.FindFirst(ClaimTypes.DateOfBirth);
                            if (urls != null)
                            {
                                birthday = DateTime.Parse(urls.Value);
                            }
                        }
                        catch (Exception)
                        {

                        }
                        // int.Parse(loginInfo.ExternalIdentity.FindFirstValue(ClaimTypes.SerialNumber));
                        //switch (urlStatus)
                        //{
                        //    case 0:
                        //        break;
                        //    case 1://PersonalWebPage
                        //        PersonalWebPage = urls.ElementAt(0).Value;
                        //        break;
                        //    case 3://googlePlus
                        //        googlePlus = urls.ElementAt(0).Value;
                        //        break;
                        //    case 7://Image
                        //        Image = urls.ElementAt(0).Value;
                        //        break;
                        //    case 4://googlePlus+PersonalWebPage
                        //        PersonalWebPage = urls.ElementAt(0).Value;
                        //        googlePlus = urls.ElementAt(1).Value;
                        //        break;
                        //    case 8://PersonalWebPage+Image
                        //        PersonalWebPage = urls.ElementAt(0).Value;
                        //        Image = urls.ElementAt(1).Value;
                        //        break;
                        //    case 10://Image+googlePlus
                        //        googlePlus = urls.ElementAt(0).Value;
                        //        Image = urls.ElementAt(1).Value;
                        //        break;
                        //    case 11://googlePlus+PersonalWebPage+Image
                        //        PersonalWebPage = urls.ElementAt(0).Value;
                        //        googlePlus = urls.ElementAt(1).Value;
                        //        Image = urls.ElementAt(2).Value;
                        //        break;
                        //    default:
                        //        break;
                        //}
                        try
                        {
                            FName = loginInfo.ExternalIdentity.FindFirstValue(ClaimTypes.GivenName);
                            LName = loginInfo.ExternalIdentity.FindFirstValue(ClaimTypes.StreetAddress);
                            GenderdString = loginInfo.ExternalIdentity.FindFirstValue(ClaimTypes.Gender);

                        }
                        catch
                        {
                        }

                        switch (GenderdString)
                        {
                            case "male":
                                Gender = false;
                                break;
                            case "female":
                                Gender = true;
                                break;
                            default:
                                Gender = null;
                                break;
                        }
                        //user.UserFName = string.IsNullOrEmpty(FName) ? "بی نام" : FName;
                        //user.UserLName = string.IsNullOrEmpty(LName) ? "بی نام" : LName;
                        //user.UserGender = Gender;
                        user.EmailConfirmed = true;
                        var resultt = await UserManager.CreateAsync(user);
                        if (resultt.Succeeded)
                        {
                            resultt = await UserManager.AddLoginAsync(user.Id, loginInfo.Login);
                            if (resultt.Succeeded)
                            {
                                await SignInManager.SignInAsync(user, isPersistent: true, rememberBrowser: false);
                                try
                                {
                                    var userInserted = db.AspNetUsers.Where(a => a.Id == user.Id).FirstOrDefault();
                                    userInserted.UserImageURL = Image;
                                    userInserted.UserFName = FName;
                                    userInserted.UserLName = LName;
                                    userInserted.UserGender = Gender;
                                    userInserted.UserBirthday = birthday;
                                    db.Entry(userInserted).State = System.Data.Entity.EntityState.Modified;
                                    await db.SaveChangesAsync();
                                   // sendEmailAfterRegister(userInserted, false, true);
                                    // vaghti karbar az external login estefade mikonad yani rahati and sorat baraye vorood ro mikhaste
                                    // pas hata be page favorite ham nabayad beravad.
                                    //TempData["ShouldCompleteProfile"] = true;
                                    //return RedirectToLocal("~/appuser/favorite");//رفتن به صفحه علاقه مندی ها در صورتی که با گوگل یا فیسبوک عضو شوند
                                    // send good and moasser message ta karbar khoshesh biad var karbar ro mojab konim password bezare.
                                    ((List<string>)TempData["SuccessMessage"]).Add(string.Format(StaticList.AfterExternalLoginSuccessRegister1, user.Email));
                                    ((List<string>)TempData["SuccessMessage"]).Add(StaticList.AfterExternalLoginSuccessRegister2);
                                    return RedirectToLocalAfterRegister(returnUrl);
                                }
                                catch (Exception)
                                { }
                                ((List<string>)TempData["SuccessMessage"]).Add(string.Format(StaticList.AfterExternalLoginSuccessRegister1, user.Email));
                                ((List<string>)TempData["SuccessMessage"]).Add(StaticList.AfterExternalLoginSuccessRegister2);
                                return RedirectToLocalAfterRegister(returnUrl);
                                //return RedirectToAction("favorite", "appuser");
                            }
                        }
                        ((List<string>)TempData["message"]).Add(StaticList.ErrorInCreateNewUser);
                        return RedirectToLocalAfterRegister(returnUrl);
                    }
                    // این زیری برای حالتی بوده است که ما می خوایم که اهراز هویت رو از طریف شبکه اجتماعی از کاربر بگریم بعدش اگر کاربر خواست با یک ایمیل دیگه بنویسد
                    //new mine
                    //ViewBag.ReturnUrl = returnUrl;
                    //ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    //return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }
        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        private string ConvertEmailToUserName(string Email)
        {
            Email = Email.ToLower();
            if (db.AspNetUsers.Any(a => a.Email == Email))
                return string.Empty;
            string EditedUserName = Email;
            if (Email.Contains("@"))
            {
                EditedUserName = Email.Substring(0, Email.IndexOf('@'));
            }
            if (string.IsNullOrEmpty(EditedUserName))
            {
                return string.Empty;
            }
            if (EditedUserName.Count() > 28)
            {
                EditedUserName.Substring(0, 28);
            }
            if (EditedUserName.Count() < 5)
            {
                EditedUserName = "accoo".Substring(0, 5 - EditedUserName.Count()) + EditedUserName;
            }
            System.Text.RegularExpressions.Regex rgx = new System.Text.RegularExpressions.Regex("^[a-z0-9.]{5,32}$");
            if (!rgx.IsMatch(EditedUserName))
            {
                string re = string.Empty;
                foreach (var item in EditedUserName)
                {
                    if ((item > 'a' && item < 'z') || (item > '0' && item < '9'))
                        re += item;
                }
                EditedUserName = re;
            }
            string resualt = EditedUserName;
            int i = 1;
            for (; db.AspNetUsers.Any(a => a.UserName == resualt) && i < 20; i++)
            {
                resualt = EditedUserName;
                resualt += "." + i.ToString();
            }
            if (i == 20)
            {
                return null;
            }
            return resualt;
        }

        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
        private ActionResult RedirectToLocalAfterRegister(string returnUrl)
        {
            return RedirectToAction("dashboard", "agent");
        }
        private ActionResult RedirectToLocalAfterLogin(string returnUrl)
        {

            return RedirectToAction("dashboard", "agent");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}