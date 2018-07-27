using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using System.Linq.Dynamic;
using Cremcircle.Models;
using Cremcircle.App_Start;

namespace Cremcircle.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private DBAuthContext db = new DBAuthContext();

        // GET: Users
        [AuthorizeUserRoles]
        public ActionResult Index()
        {
            //var users = db.Users.Include(u => u.Role);
            //return View(users.ToList());
            return View();
        }

        [HttpPost]
        public ActionResult LoadData()
        {
            //Get Parameters

            //Get Start (paging start index) and length (page size for paging)
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();

            //find search columns info
            var txtSearch = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();

            //Get Sort Column Values
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totaRecords = 0;

            var v = (from a in db.Users select new { a.LoginName, a.FirstName, a.LastName, a.EmailAddress, a.CreatedDate, a.ModifiedDate, RoleName = a.Role.RoleName, SecurityTemplateName = a.SecurityTemplate.SecurityTemplateName, a.IsActive, RoleID = a.RoleID, DT_RowId = a.ID });

            //SEARCHING...
            if (!string.IsNullOrEmpty(txtSearch))
            {
                v = v.Where(a => a.LoginName.Contains(txtSearch) || a.FirstName.Contains(txtSearch) || a.LastName.Contains(txtSearch) || a.EmailAddress.Contains(txtSearch) || a.RoleName.Contains(txtSearch) || a.SecurityTemplateName.Contains(txtSearch));
            }
            //v = v.Where(r => r.RoleID != 11 && r.RoleID != 12);

            //Sorting
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                v = v.OrderBy(sortColumn + " " + sortColumnDir);
            }
            totaRecords = v.Count();
            var data = v.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = totaRecords, recordsTotal = totaRecords, data = data }, JsonRequestBehavior.AllowGet);

        }

        // GET: Users/ChangeStatus/5
        [AuthorizeUserRoles]
        public ActionResult ChangeStatus(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            bool tempVar = true;
            if (user.IsActive)
            {
                tempVar = false;
            }

            user.IsActive = tempVar;

            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();

            Session["siteMsgTyp"] = "success";
            Session["siteMsg"] = "The selected option status changed successfully.";

            return RedirectToAction("Index", "Users");
        }

        // GET: Users/Create
        [AuthorizeUserRoles]
        public ActionResult Create()
        {
            ViewBag.CountryID = new SelectList(db.Countries, "ID", "Name");
            ViewBag.RoleID = new SelectList(db.Roles, "ID", "RoleName");
            ViewBag.SecurityTemplateID = new SelectList(db.SecurityTemplates, "ID", "SecurityTemplateName");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,LoginName,Password,FirstName,LastName,EmailAddress,UserImage,UserSignature,IsActive,RoleID,SecurityTemplateID,Branches,Clusters,Telephone,Fax,CountryID")] UserViewModel userVM)
        {
            int noOfUsers = db.Users.Count();
          

            userVM.CreatedDate = DateTime.Now;
            userVM.ModifiedDate = DateTime.Now;

            //upload the image and return the imageURL
            //var oldFile = userVM.UserImage;
            //string fileName = GenFx.fileUpload("UserImages", Request.Files["ImageFile"], 2, "png,jpg,jpeg,gif");
            //bool imageError = false;
            //if (fileName != "")
            //{
            //    if (fileName.Contains("ERROR!"))
            //    {
            //        imageError = true;
            //        Session["siteMsgTyp"] = "error";
            //        Session["siteMsg"] = fileName;
            //    }
            //    else
            //    {
            //        imageError = false;
            //        userVM.UserImage = fileName;
            //    }
            //}

            ////upload the signature and return the signatureURL
            //var oldSignature = userVM.UserSignature;
            //string fileSignature = GenFx.fileUpload("UserImages", Request.Files["ImageFileSignature"], 2, "png");
            //bool imageErrorSignature = false;
            //if (fileSignature != "")
            //{
            //    if (fileSignature.Contains("ERROR!"))
            //    {
            //        imageErrorSignature = true;
            //        Session["siteMsgTyp"] = "error";
            //        Session["siteMsg"] = ((Session["siteMsg"].ToString() != "") ? Session["siteMsg"] + "; " + fileName : fileName);
            //    }
            //    else
            //    {
            //        imageErrorSignature = false;
            //        userVM.UserSignature = fileSignature;
            //    }
            //}

            if (ModelState.IsValid)
            {
                //Create the User
                User user = new User
                {
                    LoginName = userVM.LoginName,
                    Password = userVM.Password,
                    FirstName = userVM.FirstName,
                    LastName = userVM.LastName,
                    EmailAddress = userVM.EmailAddress,
                    UserImage = userVM.UserImage,
                    //UserSignature = userVM.UserSignature,
                    IsActive = userVM.IsActive,
                    Phonenumber = userVM.Telephone,
                   // Fax = userVM.Fax,
                    CreatedDate = userVM.CreatedDate,
                    ModifiedDate = userVM.ModifiedDate,
                    RoleID = userVM.RoleID,
                    SecurityTemplateID = userVM.SecurityTemplateID,
                    CountryID = userVM.CountryID
            };

                db.Users.Add(user);
                db.SaveChanges();


                Session["siteMsgTyp"] = "success";
                Session["siteMsg"] = "The User was added successfully.";
                return RedirectToAction("Index");
                //if ((!imageError) && (!imageErrorSignature))
                //{
                //    Session["siteMsgTyp"] = "success";
                //    Session["siteMsg"] = "The User was added successfully.";
                //    return RedirectToAction("Index");
                //}
            }
            ViewBag.CountryID = new SelectList(db.Countries, "ID", "Name", userVM.CountryID);
            ViewBag.RoleID = new SelectList(db.Roles, "ID", "RoleName", userVM.RoleID);
            ViewBag.SecurityTemplateID = new SelectList(db.SecurityTemplates, "ID", "SecurityTemplateName", userVM.SecurityTemplateID);

            //Add data to the UserViewModel
            UserViewModel uvm = new UserViewModel
            {
                ID = userVM.ID,
                LoginName = userVM.LoginName,
                Password = userVM.Password,
                FirstName = userVM.FirstName,
                LastName = userVM.LastName,
                EmailAddress = userVM.EmailAddress,
                UserImage = userVM.UserImage,
                //UserSignature = userVM.UserSignature,
                IsActive = userVM.IsActive,
                Telephone = userVM.Telephone,
               // Fax = userVM.Fax,
                CreatedDate = userVM.CreatedDate,
                ModifiedDate = userVM.ModifiedDate,
                RoleID = userVM.RoleID,
                SecurityTemplateID = userVM.SecurityTemplateID,
              
            };

            return View(uvm);
        }

        // GET: Users/Edit/5
        [AuthorizeUserRoles]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.CountryID = new SelectList(db.Countries, "ID", "Name", user.CountryID);
            ViewBag.RoleID = new SelectList(db.Roles, "ID", "RoleName", user.RoleID);
            ViewBag.SecurityTemplateID = new SelectList(db.SecurityTemplates, "ID", "SecurityTemplateName", user.SecurityTemplateID);

            ////Get Branches
            //var Branches = from b in db.BranchOffices
            //               where b.BranchType == "Branch"
            //               select new
            //                 {
            //                     b.ID,
            //                     b.BranchName,
            //                     Checked = ((from bou in db.BranchUsers
            //                                 where (bou.UserID == id) & (bou.BranchOfficeID == b.ID)
            //                                 select bou).Count() > 0)
            //                 };
            //var BranchCheckBoxList = new List<CheckBoxViewModel>();
            //foreach (var item in Branches)
            //{
            //    BranchCheckBoxList.Add(new CheckBoxViewModel { ID = item.ID, Name = item.BranchName, Checked = item.Checked });
            //}

            //Get Clusters
            //var Clusters = from b in db.BranchOffices
            //               where b.BranchType == "Cluster"
            //               select new
            //               {
            //                   b.ID,
            //                   b.BranchName,
            //                   Checked = ((from bou in db.BranchUsers
            //                               where (bou.UserID == id) & (bou.BranchOfficeID == b.ID)
            //                               select bou).Count() > 0)
            //               };
            //var ClusterCheckBoxList = new List<CheckBoxViewModel>();
            //foreach (var item in Clusters)
            //{
            //    ClusterCheckBoxList.Add(new CheckBoxViewModel { ID = item.ID, Name = item.BranchName, Checked = item.Checked });
            //}

            //Add data to the UserViewModel
            UserViewModel uvm = new UserViewModel
            {
                ID = user.ID,
                LoginName = user.LoginName,
                Password = user.Password,
                FirstName = user.FirstName,
                LastName = user.LastName,
                EmailAddress = user.EmailAddress,
                UserImage = user.UserImage,
                //UserSignature = user.UserSignature,
                IsActive = user.IsActive,
                Telephone = user.Phonenumber,
               // Fax = user.Fax,
                CreatedDate = user.CreatedDate,
                ModifiedDate = user.ModifiedDate,
                RoleID = user.RoleID,
                SecurityTemplateID = user.SecurityTemplateID,
               // Branches = BranchCheckBoxList,
                //Clusters = ClusterCheckBoxList
            };

            return View(uvm);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,LoginName,Password,FirstName,LastName,EmailAddress,UserImage,UserSignature,IsActive,CreatedDate,RoleID,SecurityTemplateID,Branches,Clusters,Telephone,Fax,CountryID")] UserViewModel userVM)
        {
            userVM.ModifiedDate = DateTime.Now;

            //upload the image and return the imageURL
            //var oldFile = userVM.UserImage;
            //string fileName = GenFx.fileUpload("UserImages", Request.Files["ImageFile"], 2, "png,jpg,jpeg,gif");
            //bool imageError = false;
            //if (fileName != "")
            //{
            //    if (fileName.Contains("ERROR!"))
            //    {
            //        imageError = true;
            //        Session["siteMsgTyp"] = "error";
            //        Session["siteMsg"] = fileName;
            //    }
            //    else
            //    {
            //        imageError = false;
            //        GenFx.DeleteFile(oldFile, "UserImages");
            //        userVM.UserImage = fileName;
            //    }
            //}

            //upload the signature and return the signatureURL
            //var oldSignature = userVM.UserSignature;
            //string fileSignature = GenFx.fileUpload("UserImages", Request.Files["ImageFileSignature"], 2, "png");
            //bool imageErrorSignature = false;
            //if (fileSignature != "")
            //{
            //    if (fileSignature.Contains("ERROR!"))
            //    {
            //        imageErrorSignature = true;
            //        Session["siteMsgTyp"] = "error";
            //        Session["siteMsg"] = ((Session["siteMsg"].ToString() != "") ? Session["siteMsg"] + "; " + fileName : fileName);
            //    }
            //    else
            //    {
            //        imageErrorSignature = false;
            //        GenFx.DeleteFile(oldSignature, "UserImages");
            //        userVM.UserSignature = fileSignature;
            //    }
            //}

            //Get the user that we are editing
            User user = db.Users.Find(userVM.ID);

            if (ModelState.IsValid)
            {
                //Update the actual User
                user.LoginName = userVM.LoginName;
                user.Password = userVM.Password;
                user.FirstName = userVM.FirstName;
                user.LastName = userVM.LastName;
                user.EmailAddress = userVM.EmailAddress;
                user.UserImage = userVM.UserImage;
               // user.UserSignature = userVM.UserSignature;
                user.IsActive = userVM.IsActive;
                user.Phonenumber = userVM.Telephone;
               // user.Fax = userVM.Fax;
                user.CreatedDate = userVM.CreatedDate;
                user.ModifiedDate = userVM.ModifiedDate;
                user.RoleID = userVM.RoleID;
                user.SecurityTemplateID = userVM.SecurityTemplateID;
                user.CountryID = userVM.CountryID;
                //if (db.BranchOffices.Count() > 0)
                //{
                //    //delete existing record relations
                //    foreach (var item in db.BranchUsers.Where(bu => bu.UserID == userVM.ID))
                //    {
                //        db.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                //    }

                //    //Branches Add
                //    foreach (var item in userVM.Branches)
                //    {
                //        if (item.Checked)
                //        {
                //            db.BranchUsers.Add(new BranchUser { UserID = userVM.ID, BranchOfficeID = item.ID, CreatedDate = DateTime.Now });
                //        }
                //    }
                //    //Clusters Add
                //    foreach (var item in userVM.Clusters)
                //    {
                //        if (item.Checked)
                //        {
                //            db.BranchUsers.Add(new BranchUser { UserID = userVM.ID, BranchOfficeID = item.ID, CreatedDate = DateTime.Now });
                //        }
                //    }
                //}

                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                Session["siteMsgTyp"] = "success";
                Session["siteMsg"] = "The User was modified successfully.";
                return RedirectToAction("Index");
                //if ((!imageError) && (!imageErrorSignature))
                //{
                //    Session["siteMsgTyp"] = "success";
                //    Session["siteMsg"] = "The User was modified successfully.";
                //    return RedirectToAction("Index");
                //}
            }
            ViewBag.CountryID = new SelectList(db.Countries, "ID", "Name", userVM.CountryID);
            ViewBag.RoleID = new SelectList(db.Roles, "ID", "RoleName",userVM.RoleID);
            ViewBag.SecurityTemplateID = new SelectList(db.SecurityTemplates, "ID", "SecurityTemplateName", userVM.SecurityTemplateID);

            ////Get Branches
            //var Branches = from b in db.BranchOffices
            //               where b.BranchType == "Branch"
            //               select new
            //               {
            //                   b.ID,
            //                   b.BranchName,
            //                   Checked = ((from bou in db.BranchUsers
            //                               where (bou.UserID == userVM.ID) & (bou.BranchOfficeID == b.ID)
            //                               select bou).Count() > 0)
            //               };
            //var BranchCheckBoxList = new List<CheckBoxViewModel>();
            //foreach (var item in Branches)
            //{
            //    BranchCheckBoxList.Add(new CheckBoxViewModel { ID = item.ID, Name = item.BranchName, Checked = item.Checked });
            //}

            ////Get Clusters
            //var Clusters = from b in db.BranchOffices
            //               where b.BranchType == "Cluster"
            //               select new
            //               {
            //                   b.ID,
            //                   b.BranchName,
            //                   Checked = ((from bou in db.BranchUsers
            //                               where (bou.UserID == userVM.ID) & (bou.BranchOfficeID == b.ID)
            //                               select bou).Count() > 0)
            //               };
            //var ClusterCheckBoxList = new List<CheckBoxViewModel>();
            //foreach (var item in Clusters)
            //{
            //    ClusterCheckBoxList.Add(new CheckBoxViewModel { ID = item.ID, Name = item.BranchName, Checked = item.Checked });
            //}
            //Add data to the UserViewModel
            UserViewModel uvm = new UserViewModel
            {
                LoginName = userVM.LoginName,
                Password = userVM.Password,
                FirstName = userVM.FirstName,
                LastName = userVM.LastName,
                EmailAddress = userVM.EmailAddress,
               UserImage = userVM.UserImage,
                //UserSignature = userVM.UserSignature,
                IsActive = userVM.IsActive,
                Telephone = userVM.Telephone,
                //Fax = userVM.Fax,
                CreatedDate = userVM.CreatedDate,
                ModifiedDate = userVM.ModifiedDate,
                RoleID = userVM.RoleID,
                SecurityTemplateID = userVM.SecurityTemplateID,
               // Branches = BranchCheckBoxList,
                //Clusters = ClusterCheckBoxList
            };

            return View(uvm);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public JsonResult IsLoginNameUnique(string LoginName, long ID = 0)
        {
            //check if any of the Title matches the Title specified in the Parameter using the ANY extension method.   
            return Json(!db.Users.Any(x => x.LoginName == LoginName && x.ID != ID), JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsOldPasswordMatching(string OldPassword)
        {
            //check if Old Password Matches the Users existing password 
            int id = (int)GenFx.val(GenFx.GetCurrentUserDetails("ID"));
            bool match = db.Users.Any(x => x.Password == OldPassword && x.ID == id);
            return Json(match, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsPasswordSame(string NewPassword, string RepeatPassword)
        {
            //check if both New and repeat Password match
            return Json((NewPassword == RepeatPassword), JsonRequestBehavior.AllowGet);
        }
    }
}
