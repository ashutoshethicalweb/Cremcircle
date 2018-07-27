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
using Cremcircle;

namespace Cremcircle.Controllers
{
    [Authorize]
    public class SecurityTemplatesController : Controller
    {
        private DBAuthContext db = new DBAuthContext();

        // GET: SecurityTemplates
        [AuthorizeUserRoles]
        public ActionResult Index()
        {
            //return View(db.SecurityTemplates.ToList());
            //Update all the new Permissions (if any)
            GenFx.PopulatePermission();

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

            var v = (from a in db.SecurityTemplates select new { a.ID, a.SecurityTemplateName, a.IsActive, DT_RowId = a.ID });

            //SEARCHING...
            if (!string.IsNullOrEmpty(txtSearch))
            {
                v = v.Where(a => a.SecurityTemplateName.Contains(txtSearch));
            }

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
            SecurityTemplate secTemp = db.SecurityTemplates.Where(st => st.ID == id && st.ID > 12).SingleOrDefault();
            if (secTemp == null)
            {
                return HttpNotFound();
            }

            bool tempVar = true;
            if (secTemp.IsActive)
            {
                tempVar = false;
            }

            secTemp.IsActive = tempVar;

            db.Entry(secTemp).State = EntityState.Modified;
            db.SaveChanges();

            Session["siteMsgTyp"] = "success";
            Session["siteMsg"] = "The selected option status changed successfully.";

            return RedirectToAction("Index", "SecurityTemplates");
        }

        // GET: SecurityTemplates/Create
        [AuthorizeUserRoles]
        public ActionResult Create()
        {
            //Get Permissions
            var v = from b in db.Permissions
                           select new
                           {
                               b.ID,
                               b.GroupName,
                               Name = b.ControllerName + " - " + b.ActionName,
                               Checked = false,
                               b.OnlyAdminHidden
                           };
            v = v.Where(b => b.OnlyAdminHidden == false);
            v = v.OrderBy("Name asc");
            var Permissions = v.ToList();
            var PermissionCheckBoxList = new List<CheckBoxViewModel>();
            foreach (var item in Permissions)
            {
                PermissionCheckBoxList.Add(new CheckBoxViewModel { ID = item.ID, GroupName = item.GroupName, Name = item.Name, Checked = item.Checked });
            }

            //Add data to the SecurityTemplateViewModel
            SecurityTemplateViewModel stvm = new SecurityTemplateViewModel
            {
                Permissions = PermissionCheckBoxList
            };

            return View(stvm);
        }

        // POST: SecurityTemplates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,SecurityTemplateName,IsActive,Permissions")] SecurityTemplateViewModel securityTemplateVM)
        {
            if (ModelState.IsValid)
            {
                //Create the SecurityTemplate
                SecurityTemplate securityTemplate = new SecurityTemplate
                {
                    SecurityTemplateName = securityTemplateVM.SecurityTemplateName,
                    IsActive = securityTemplateVM.IsActive
                };

                db.SecurityTemplates.Add(securityTemplate);
                db.SaveChanges();

                if (db.Permissions.Count() > 0)
                {
                    //Permissions Add
                    foreach (var item in securityTemplateVM.Permissions)
                    {
                        if (item.Checked)
                        {
                            db.SecurityTemplatePermissions.Add(new SecurityTemplatePermission { SecurityTemplateID = securityTemplate.ID, PermissionID = item.ID, CreatedDate = DateTime.Now });
                        }
                    }
                }

                //db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();

                Session["siteMsgTyp"] = "success";
                Session["siteMsg"] = "The Security Template was added successfully.";
                return RedirectToAction("Index");
            }

            //Get Permissions
            var v = from b in db.Permissions
                    select new
                    {
                        b.ID,
                        b.GroupName,
                        Name = b.ControllerName + " - " + b.ActionName,
                        Checked = false,
                        b.OnlyAdminHidden
                    };
            v = v.Where(b => b.OnlyAdminHidden == false);
            v = v.OrderBy("Name asc");
            var Permissions = v.ToList();
            var PermissionCheckBoxList = new List<CheckBoxViewModel>();
            foreach (var item in Permissions)
            {
                PermissionCheckBoxList.Add(new CheckBoxViewModel { ID = item.ID, GroupName = item.GroupName, Name = item.Name, Checked = item.Checked });
            }

            //Add data to the SecurityTemplateViewModel
            SecurityTemplateViewModel stvm = new SecurityTemplateViewModel
            {
                Permissions = PermissionCheckBoxList
            };

            return View(stvm);
        }

        // GET: SecurityTemplates/Edit/5
        [AuthorizeUserRoles]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SecurityTemplate securityTemplate = db.SecurityTemplates.Where(st => st.ID == id && st.ID > 1).SingleOrDefault();
            if (securityTemplate == null)
            {
                return HttpNotFound();
            }

            //Get Permissions
            var v = from b in db.Permissions
                              select new
                              {
                                  b.ID,
                                  b.GroupName,
                                  Name = b.ControllerName + " - " + b.ActionName,
                                  b.OnlyAdminHidden,
                                  Checked = ((from bou in db.SecurityTemplatePermissions
                                              where (bou.SecurityTemplateID == id) & (bou.PermissionID == b.ID)
                                              select bou).Count() > 0)
                              };
            v = v.Where(b => b.OnlyAdminHidden == false);
            v = v.OrderBy("Name asc");
            var Permissions = v.ToList();
            var PermissionCheckBoxList = new List<CheckBoxViewModel>();
            foreach (var item in Permissions)
            {
                PermissionCheckBoxList.Add(new CheckBoxViewModel { ID = item.ID, GroupName = item.GroupName, Name = item.Name, Checked = item.Checked });
            }

            //Add data to the SecurityTemplateViewModel
            SecurityTemplateViewModel stvm = new SecurityTemplateViewModel
            {
                SecurityTemplateName = securityTemplate.SecurityTemplateName,
                IsActive = securityTemplate.IsActive,
                Permissions = PermissionCheckBoxList
            };

            return View(stvm);
        }

        // POST: SecurityTemplates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,SecurityTemplateName,IsActive,Permissions")] SecurityTemplateViewModel securityTemplateVM)
        {
            //Get the SecurityTemplate we were editting
            SecurityTemplate securityTemplate = db.SecurityTemplates.Find(securityTemplateVM.ID);

            if (ModelState.IsValid)
            {
                //Update the actual SecurityTemplate
                securityTemplate.SecurityTemplateName = securityTemplateVM.SecurityTemplateName;
                securityTemplate.IsActive = securityTemplateVM.IsActive;

                if (db.Permissions.Count() > 0)
                {
                    //delete existing record relations
                    foreach (var item in db.SecurityTemplatePermissions.Where(bu => bu.SecurityTemplateID == securityTemplateVM.ID))
                    {
                        db.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                    }

                    //Permissions Add
                    foreach (var item in securityTemplateVM.Permissions)
                    {
                        if (item.Checked)
                        {
                            db.SecurityTemplatePermissions.Add(new SecurityTemplatePermission { SecurityTemplateID = securityTemplate.ID, PermissionID = item.ID, CreatedDate = DateTime.Now });
                        }
                    }
                }

                //db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();

                Session["siteMsgTyp"] = "success";
                Session["siteMsg"] = "The Security Template was modified successfully.";
                return RedirectToAction("Index");
            }

            //Get Permissions
            var v = from b in db.Permissions
                              select new
                              {
                                  b.ID,
                                  b.GroupName,
                                  Name = b.ControllerName + " - " + b.ActionName,
                                  b.OnlyAdminHidden,
                                  Checked = ((from bou in db.SecurityTemplatePermissions
                                              where (bou.SecurityTemplateID == securityTemplateVM.ID) & (bou.PermissionID == b.ID)
                                              select bou).Count() > 0)
                              };
            v = v.Where(b => b.OnlyAdminHidden == false);
            v = v.OrderBy("Name asc");
            var Permissions = v.ToList();
            var PermissionCheckBoxList = new List<CheckBoxViewModel>();
            foreach (var item in Permissions)
            {
                PermissionCheckBoxList.Add(new CheckBoxViewModel { ID = item.ID, GroupName = item.GroupName, Name = item.Name, Checked = item.Checked });
            }

            //Add data to the SecurityTemplateViewModel
            SecurityTemplateViewModel stvm = new SecurityTemplateViewModel
            {
                SecurityTemplateName = securityTemplateVM.SecurityTemplateName,
                IsActive = securityTemplateVM.IsActive,
                Permissions = PermissionCheckBoxList
            };

            return View(stvm);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public JsonResult IsSecurityTemplateNameUnique(string SecurityTemplateName, long ID = 0)
        {
            //check if any of the Title matches the Title specified in the Parameter using the ANY extension method.   
            return Json(!db.SecurityTemplates.Any(x => x.SecurityTemplateName == SecurityTemplateName && x.ID != ID), JsonRequestBehavior.AllowGet);
        }
    }
}
