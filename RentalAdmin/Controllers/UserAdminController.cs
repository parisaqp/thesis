
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using RentalAdmin.Models;
using PagedList;

namespace RentalAdmin.Controllers
{
    [Authorize(Roles = "admin")]
    public class UserAdminController : Controller
    {
        private static int maxUserToC = 15;
        private RentalEntities db = new RentalEntities();
        public UserAdminController()
        {
        }


        public UserAdminController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        private ApplicationUserManager _userManager;
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

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }


        public ActionResult IIntroduced(string userID)
        {
           var theUser=  db.AspNetUsers.Where(a => a.Id == userID).FirstOrDefault();
            if(theUser==null)
            {
                return new HttpStatusCodeResult( HttpStatusCode.BadRequest);
            }
            return View( theUser);
        }
        [ActionName("IIntroduced")]
        [HttpPost]
        public ActionResult IIntroducedPost(string userID)
        {
            var theUser = db.AspNetUsers.Where(a => a.Id == userID).FirstOrDefault();
            if (theUser == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var me = db.AspNetUsers.Where(a => a.UserName == User.Identity.Name).FirstOrDefault();
            if(theUser.Id==null)
            {
                theUser.UserIDIntroduced = me.Id;
                db.Entry(theUser).State = EntityState.Modified;
                db.SaveChanges();
            }
            
            return RedirectToAction("FindUser",new { username= theUser.UserName });
        }
        public ActionResult todeyregister()//
        {
            DateTime emrooz = DateTime.UtcNow.AddDays(-1);//MyDateTimeExtensions.ConvertTimeZoneDateToUtcDate(MyDateTimeExtensions.FirstOfToday());
            var res = db.AspNetUsers.Where(a => a.RegisterDate > emrooz).ToList();
            return View(res);
        }
        [HttpGet]
        public ActionResult index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult FindUser(string phonenumber, string username, string email, string lname, string fname)//
        {
            Models.admin.FindUserViewModel model = new Models.admin.FindUserViewModel();
            // List<AspNetUser> found = new List<AspNetUser>();
            var res = db.AspNetUsers.Where(a => 1 == 1);
            if (!string.IsNullOrEmpty(email))
            {
                email = email.ToLower();
                res = res.Where(a => a.Email == email);
            }
            if (!string.IsNullOrEmpty(username))
            {
                username = username.ToLower();
                res = res.Where(a => a.UserName == username);
            }
            if (!string.IsNullOrEmpty(phonenumber))
            {
                phonenumber = phonenumber.ToLower();
                res = res.Where(a => a.PhoneNumber == phonenumber);
            }

            if (!string.IsNullOrEmpty(fname))
            {
                res = res.Where(a => a.UserFName == fname);
            }
            if (!string.IsNullOrEmpty(lname))
            {
                res = res.Where(a => a.UserLName == lname);
            }
            model.fname = fname;
            model.lname = lname;
            model.username = username;
            model.email = email;
            model.phonenumber = phonenumber;
            model.users = new List<AspNetUser>();
            model.users = res.OrderByDescending(a => a.RegisterDate).Take(maxUserToC).ToList();


            return View(model);

        }



        [HttpGet]
        public ActionResult UserDetails(string username)//
        {
            if (!string.IsNullOrEmpty(username))
            {
                username = username.Trim();
                var user = db.AspNetUsers.Where(a => a.UserName == username).FirstOrDefault();

            }
            return View();
        }



        public ActionResult resetPasswordAdmin(string userID)
        {
            var theUser = db.AspNetUsers.Where(a => a.Id == userID).FirstOrDefault();
            if (theUser.UserName == User.Identity.Name)
            {
                ViewBag.MessageToView = "نمی توانید ایمیل خود را تغییر دهید.";
                return View(theUser);
            }
            return View(theUser);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult resetPasswordAdmin(string userID, string newPass)
        {
            var theUser = db.AspNetUsers.Where(a => a.Id == userID).FirstOrDefault();
            if (string.IsNullOrEmpty(newPass))
            {
                ViewBag.MessageToView = "ایمیل جدید را وارد کنید.";
                return View(theUser);
            }
            newPass = newPass.Trim().ToLower();
            if (theUser.UserName == User.Identity.Name)
            {
                ViewBag.MessageToView = "نمی توانید پسورد خود را تغییر دهید.";
                return View(theUser);
            }
            if (db.AspNetUserRoles.Any(a => a.UserId == theUser.Id))
            {
                ViewBag.MessageToView = "نمی توان اطلاعات مدیر را تغییر داد";
                return View(theUser);
            }
            string token = UserManager.GeneratePasswordResetToken(theUser.Id);
            var result = UserManager.ResetPassword(theUser.Id, token, newPass);

            if (result.Succeeded)
            {
                ViewBag.MessageToView = "پسورد به درستی تغییر کرد";
                return RedirectToAction("index", "admin");
            }

            return View(theUser);
        }




        ////
        //// GET: /Users/
        //public async Task<ActionResult> UsersAdminIndex(string sortOrder, string searchString, int? Page, int? PageSize, bool? JustTeacher)//
        //{
        //    ViewBag.JustTeacher = JustTeacher;
        //    //if (PageSize == null)
        //    //    PageSize = 15;
        //    //int pageNumber = (Page ?? 1);
        //    //var users = ;
        //    //var AA= UserManager.Users;
        //    //return View(await UserManager.Users.ToListAsync());
        //    //var users;
        //    //if (!String.IsNullOrEmpty(searchString))
        //    //{
        //    //    users = db.Messages.Where(a => a.MessageTitle.Contains(searchString) || a.MessageBody.Contains(searchString)).ToList();
        //    //}
        //    //else
        //    //{
        //    //    users = db.Messages.ToList();
        //    //}
        //    //List<Message> message2 = new List<Message>();// = db.Messages.OrderByDescending(a=>a.MessageDate);
        //    //switch (sortOrder)
        //    //{
        //    //    case "Date":
        //    //        message = message.OrderByDescending(s => s.MessageDate).ToList();
        //    //        break;
        //    //    case "IsGenral":
        //    //        message = message.OrderByDescending(s => s.MessageIsGeneral).ToList();
        //    //        break;
        //    //    case "Title":
        //    //        message2 = message.OrderBy(s => s.MessageTitle).ToList();
        //    //        break;
        //    //    case "SendFrom":
        //    //        message2 = message.OrderBy(s => s.MessageSendFrom).ToList();
        //    //        break;
        //    //    default:
        //    //        message2 = message.OrderByDescending(s => s.MessageDate).ToList();
        //    //        break;
        //    //}

        //    //return View(message.ToPagedList(pageNumber, pageSize));
        //    return View(await UserManager.Users.ToListAsync());
        //}

        ////
        //// GET: /Users/Details/5
        //public async Task<ActionResult> UsersAdminDetails(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    var user = await UserManager.FindByIdAsync(id);

        //    ViewBag.RoleNames = await UserManager.GetRolesAsync(user.Id);

        //    return View(user);
        //}

        ////
        //// GET: /Users/Create
        //public async Task<ActionResult> UsersAdminCreate()
        //{
        //    //Get the list of Roles
        //    ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
        //    return View();
        //}

        ////
        //// POST: /Users/Create
        //[HttpPost]
        //public async Task<ActionResult> UsersAdminCreate(RegisterViewModel userViewModel, params string[] selectedRoles)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = new ApplicationUser { UserName = userViewModel.Email, Email = userViewModel.Email };
        //        var adminresult = await UserManager.CreateAsync(user, userViewModel.Password);

        //        //Add User to the selected Roles 
        //        if (adminresult.Succeeded)
        //        {
        //            if (selectedRoles != null)
        //            {
        //                var result = await UserManager.AddToRolesAsync(user.Id, selectedRoles);
        //                if (!result.Succeeded)
        //                {
        //                    ModelState.AddModelError("", result.Errors.First());
        //                    ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
        //                    return View();
        //                }
        //            }
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", adminresult.Errors.First());
        //            ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
        //            return View();

        //        }
        //        return RedirectToAction("UsersAdminIndex");
        //    }
        //    ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
        //    return View();
        //}

        ////
        //// GET: /Users/Edit/1
        //public async Task<ActionResult> UsersAdminEdit(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    var user = await UserManager.FindByIdAsync(id);
        //    if (user == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    var userRoles = await UserManager.GetRolesAsync(user.Id);

        //    return View(new EditUserViewModel()
        //    {
        //        Id = user.Id,
        //        UserName = user.UserName,
        //        RolesList = RoleManager.Roles.ToList().Select(x => new SelectListItem()
        //        {
        //            Selected = userRoles.Contains(x.Name),
        //            Text = x.Name,
        //            Value = x.Name
        //        })
        //    });
        //}

        ////
        //// POST: /Users/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> UsersAdminEdit([Bind(Include = "UserName,Id")] EditUserViewModel editUser, params string[] selectedRole)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await UserManager.FindByIdAsync(editUser.Id);
        //        if (user == null)
        //        {
        //            return HttpNotFound();
        //        }

        //        //user.UserName = editUser.UserName;

        //        var userRoles = await UserManager.GetRolesAsync(user.Id);

        //        selectedRole = selectedRole ?? new string[] { };

        //        var result = await UserManager.AddToRolesAsync(user.Id, selectedRole.Except(userRoles).ToArray<string>());

        //        if (!result.Succeeded)
        //        {
        //            ModelState.AddModelError("", result.Errors.First());
        //            return View();
        //        }
        //        result = await UserManager.RemoveFromRolesAsync(user.Id, userRoles.Except(selectedRole).ToArray<string>());

        //        if (!result.Succeeded)
        //        {
        //            ModelState.AddModelError("", result.Errors.First());
        //            return View();
        //        }
        //        return RedirectToAction("UsersAdminIndex");
        //    }
        //    ModelState.AddModelError("", "Something failed.");
        //    return View();
        //}

        ////
        //// GET: /Users/Delete/5
        //public async Task<ActionResult> UsersAdminDelete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    var user = await UserManager.FindByIdAsync(id);
        //    if (user == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(user);
        //}

        ////
        //// POST: /Users/Delete/5
        //[HttpPost, ActionName("UsersAdminDelete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> UsersAdminDeleteConfirmed(string id)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (id == null)
        //        {
        //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //        }

        //        var user = await UserManager.FindByIdAsync(id);
        //        if (user == null)
        //        {
        //            return HttpNotFound();
        //        }
        //        var result = await UserManager.DeleteAsync(user);
        //        if (!result.Succeeded)
        //        {
        //            ModelState.AddModelError("", result.Errors.First());
        //            return View();
        //        }
        //        return RedirectToAction("UsersAdminIndex");
        //    }
        //    return View();
        //}
        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        if (_userManager != null)
        //        {
        //            _userManager.Dispose();
        //            _userManager = null;
        //        }

        //        if (_roleManager != null)
        //        {
        //            _roleManager.Dispose();
        //            _roleManager = null;
        //        }
        //    }

        //    base.Dispose(disposing);
        //}
    }
}
