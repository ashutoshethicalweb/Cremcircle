using Cremcircle.App_Start;
using Cremcircle.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Cremcircle.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account

        private DBAuthContext db = new DBAuthContext();

        public JsonResult IsLoginNameUnique(string EmailAddress, long ID = 0)
        {
            //check if any of the Title matches the Title specified in the Parameter using the ANY extension method.   
            return Json(!db.Users.Any(x => x.LoginName == EmailAddress && x.ID != ID), JsonRequestBehavior.AllowGet);
        }



        [AllowAnonymous]
        public ActionResult ActivateLine(string EmailId)
        {
            if (db.Users.Where(a => a.LoginName == EmailId).Count() > 0)
            {


                User user = db.Users.Where(a => a.LoginName == EmailId).Single();
                if (user.IsActive == true)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    user.IsActive = true;
                    user.ModifiedDate = DateTime.Now;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    Session["siteMsgTyp"] = "success";
                    Session["siteMsg"] = "Your Login Activate successfully. Please Login in.";
                    return RedirectToAction("Login", "Account");
                }

            }

            else
            {
                Session["siteMsgTyp"] = "error";
                Session["siteMsg"] = "Your Login not  Activate successfully.Invalid Infromation.";
                return RedirectToAction("Login", "Account");
            }


        }


        //[AllowAnonymous]
        public ActionResult Register()
        {

            ViewBag.UserAgeDescriptionID = new SelectList(db.UserAgeDescriptions, "ID", "AgeDescription");
            ViewBag.Captcha = "true";
            ViewBag.RoleID = new SelectList(db.Roles.Where(a => a.ID > 2), "ID", "RoleName");
            ViewBag.CountryID = new SelectList(db.Countries, "ID", "Name");
            return View();
        }

        //[AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "ID,FirstName,LastName,Password,ConfirmPassword,Captcha,Telephone,EmailAddress,RoleID,CountryID,UserAgeDescriptionID,capchadata")]RegisterViewModel model)
        {
            if (model.capchadata != model.Captcha)
            {
                ViewBag.Captcha = "false";
            }
            else
            {
                ViewBag.Captcha = "true";

                // Lets first check if the Model is valid or not
                if (ModelState.IsValid)
                {
                    User user = new User
                    {
                        LoginName = model.EmailAddress,
                        Password = model.Password,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        EmailAddress = model.EmailAddress,
                        UserImage = "",
                        //UserSignature = userVM.UserSignature,
                        IsActive = false,
                        Phonenumber = model.Telephone,
                        // Fax = userVM.Fax,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        UserAgeDescriptionID = model.UserAgeDescriptionID,
                        RoleID = model.RoleID,
                        SecurityTemplateID = model.RoleID,
                        CountryID = model.CountryID
                    };

                    db.Users.Add(user);
                    db.SaveChanges();

                    string Link = System.Web.HttpContext.Current.Request.Url.Scheme + "://" + System.Web.HttpContext.Current.Request.Url.Host + (System.Web.HttpContext.Current.Request.Url.Port == 80 ? string.Empty : ":" + System.Web.HttpContext.Current.Request.Url.Port) + Url.Action("ActivateLine", "Account", new { EmailId = user.LoginName });
                    string Check = GenFx.EmailToUserifComplaintAdded(user.LoginName, user.Password, Link);
                    if (Check == "1")
                    {
                        Session["siteMsgTyp"] = "success";
                        Session["siteMsg"] = "You are register  successfully.Login confirmation link send your Email";
                        return RedirectToAction("Login", "Account");
                    }

                    else
                    {
                        Session["siteMsgTyp"] = "error";
                        Session["siteMsg"] = "You are register  successfully.but Login confirmation link can not  send your Email.Please Contact";
                    }


                }
            }
            ViewBag.UserAgeDescriptionID = new SelectList(db.UserAgeDescriptions, "ID", "AgeDescription", model.UserAgeDescriptionID);
            ViewBag.RoleID = new SelectList(db.Roles.Where(a => a.ID > 2), "ID", "RoleName", model.RoleID);
            ViewBag.CountryID = new SelectList(db.Countries, "ID", "Name", model.CountryID);
            //// If we got this far, something failed, redisplay form
            return View(model);
        }



        public ActionResult Login()
        {

            LoginViewModel model = new LoginViewModel();
            model.ChkRememberMe = true;

                if (Request.Cookies["UserName"] != null && Request.Cookies["Password"] != null)
                {
                model.LoginName = Request.Cookies["UserName"].Value;
                model.Password= Request.Cookies["Password"].Value;
                }
            
            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {


            // Lets first check if the Model is valid or not
            if (ModelState.IsValid)
            {
                using (DBAuthContext entities = new DBAuthContext())
                {
                    string username = model.LoginName;
                    string password = model.Password;



                    bool CheckUserActiveornot = entities.Users.Any(user => user.LoginName == username && user.IsActive==false);
                    if(CheckUserActiveornot)
                    {
                        Session["siteMsgTyp"] = "error";
                        //Congrats on signing up for Zoom!In order to activate your account please click the button below to verify your email address:
                        Session["siteMsg"] = "Please Activate Your Account using link send your Email Address";
                        return RedirectToAction("Login", "Account");
                    }


                    // Now if our password was enctypted or hashed we would have done the
                    // same operation on the user entered password here, But for now
                    // since the password is in plain text lets just authenticate directly

                    bool userValid = entities.Users.Any(user => user.LoginName == username && user.Password == password);

                    // User found in the database
                    if (userValid)
                    {
                        //var userData = "";

                        //var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username), }, DefaultAuthenticationTypes.ApplicationCookie);
                        //var ticket = new FormsAuthenticationTicket(1, username, DateTime.UtcNow, DateTime.UtcNow.AddMinutes(30), false, userData, FormsAuthentication.FormsCookiePath);
                        //var encryptedTicket = FormsAuthentication.Encrypt(ticket);
                        //var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket) { HttpOnly = true };
                        //Response.Cookies.Add(authCookie);
                        //AuthenticationManager.SignIn(identity);

                        FormsAuthentication.SetAuthCookie(username, false);

                        //Update User Log
                        long userid = entities.Users.Where(user => user.LoginName == username && user.Password == password).Select(user => user.ID).Single();
                        GenFx.AddToUserLog("Login", userid);


                        if (model.ChkRememberMe.Equals(true))
                        {
                            Response.Cookies["UserName"].Expires = DateTime.Now.AddDays(30);
                            Response.Cookies["Password"].Expires = DateTime.Now.AddDays(30);
                        }
                        else
                        {
                            Response.Cookies["UserName"].Expires = DateTime.Now.AddDays(-1);
                            Response.Cookies["Password"].Expires = DateTime.Now.AddDays(-1);

                        }
                        Response.Cookies["UserName"].Value = model.LoginName;
                        Response.Cookies["Password"].Value = model.Password;

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
                        ModelState.AddModelError("", "The user name or password provided is incorrect.");
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // GET: Account/ForgotPassword
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordViewModel model, string returnUrl)
        {
            // Lets first check if the Model is valid or not
            if (ModelState.IsValid)
            {
                using (DBAuthContext entities = new DBAuthContext())
                {
                    string username = model.EmailAddress;
                    string email = model.EmailAddress;

                    // Now if our password was enctypted or hashed we would have done the
                    // same operation on the user entered password here, But for now
                    // since the password is in plain text lets just authenticate directly

                    var userPassword = entities.Users.Where(user => user.LoginName == username && user.EmailAddress == email).Select(user => user.Password).SingleOrDefault();

                    // User found in the database
                    if (userPassword != null)
                    {
                        string Link = System.Web.HttpContext.Current.Request.Url.Scheme + "://" + System.Web.HttpContext.Current.Request.Url.Host + (System.Web.HttpContext.Current.Request.Url.Port == 80 ? string.Empty : ":" + System.Web.HttpContext.Current.Request.Url.Port) + Url.Action("ActivateLine", "Account", new { EmailId = username });
                        string Check = GenFx.EmailToUserifComplaintAdded(username, userPassword, "");
                        if (Check == "1")
                        {
                            Session["siteMsgTyp"] = "success";
                            Session["siteMsg"] = "You are  Password send your Email Address.Check it";
                            return RedirectToAction("Login", "Account");
                        }

                        else
                        {
                            Session["siteMsgTyp"] = "error";
                            Session["siteMsg"] = "You are  Password  cant not send your Email Address.Please try Again...";
                        }

                        //ModelState.AddModelError("", "Your Password is " + userPassword + ".");
                    }
                    else
                    {
                        ModelState.AddModelError("", "The user name or email address provided is incorrect.");
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        // GET: Account/ChangePassword
        [Authorize]
        public ActionResult ChangePassword()
        {
            int id = (int)GenFx.val(GenFx.GetCurrentUserDetails("ID"));
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View();
        }

        // POST: Account/ChangePassword
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword([Bind(Include = "OldPassword,NewPassword,RepeatPassword")] ChangePasswordViewModel cpModel)
        {
            int id = (int)GenFx.val(GenFx.GetCurrentUserDetails("ID"));
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                user.Password = cpModel.NewPassword;
                user.ModifiedDate = DateTime.Now;

                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();

                Session["siteMsgTyp"] = "success";
                Session["siteMsg"] = "Your password was changed successfully.";

                return RedirectToAction("Index", "Home");
            }
            return View(cpModel);
        }

        // GET: Account/ActivityLog
        [Authorize]
        public ActionResult ActivityLog()
        {
            return View("../Error/PageNotFound");
        }

        // GET: Account/UserSettings
        [Authorize]
        public ActionResult UserSettings()
        {
            return View("../Error/PageNotFound");
        }

        // GET: Account/Logout
        public ActionResult Logout()
        {
            GenFx.AddToUserLog("Logout");
            //HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            //HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //HttpContext.Response.Cache.SetNoStore();
            // Response.Buffer = true;
            //Response.ExpiresAbsolute = DateTime.Now.AddDays(-1d);
            // Response.Expires = -1000;
            // Response.CacheControl = "no-cache";
            // Response.Cache.SetNoStore();
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            FormsAuthentication.SignOut();//you write this when you use FormsAuthentication

            return RedirectToAction("Login", "Account");

        }

        [Authorize]
        public void PopulatePermission()
        {
            Assembly asm = Assembly.GetAssembly(typeof(Cremcircle.MvcApplication));

            var controlleractionlist = asm.GetTypes()
                    .Where(type => typeof(System.Web.Mvc.Controller).IsAssignableFrom(type))
                    .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                    .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
                    .Select(x => new { Controller = x.DeclaringType.Name, Action = x.Name, ReturnType = x.ReturnType.Name, Attributes = String.Join(",", x.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", ""))) })
                    .OrderBy(x => x.Controller).ThenBy(x => x.Action).ToList();


            foreach (var item in controlleractionlist)
            {
                if (item.Attributes.Contains("AuthorizeUserRoles"))
                {
                    //Response.Write(item.Controller + " = " + item.Action + " = " + item.Attributes + " = " + item.ReturnType + "<br />");
                    // Add to the permissions table
                    using (DBAuthContext db = new DBAuthContext())
                    {
                        //check if exixting
                        var existingPermissionCount = db.Permissions.Count(pe => pe.GroupName == "Object Level" && pe.ControllerName == item.Controller && pe.ActionName == item.Action);
                        if (existingPermissionCount == 0)
                        {
                            Permission pe = new Permission
                            {
                                GroupName = "Object Level",
                                ControllerName = item.Controller,
                                ActionName = item.Action
                            };

                            db.Permissions.Add(pe);
                            db.SaveChanges();
                        }
                    }
                }
            }
        }


    }
}