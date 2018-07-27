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

namespace Cremcircle.Controllers
{
    [Authorize]
    public class RolesController : Controller
    {
        private DBAuthContext db = new DBAuthContext();

        // GET: Roles
        [AuthorizeUserRoles]
        public ActionResult Index()
        {
            //return View(db.Roles.ToList());
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

            var v = (from a in db.Roles select new { a.ID, a.RoleName, a.IsActive, DT_RowId = a.ID });

            //SEARCHING...
            if (!string.IsNullOrEmpty(txtSearch))
            {
                v = v.Where(a => a.RoleName.Contains(txtSearch));
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

        // GET: Roles/ChangeStatus/5
        [AuthorizeUserRoles]
        public ActionResult ChangeStatus(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role role = db.Roles.Where(r => r.ID == id && r.ID > 12).SingleOrDefault();
            if (role == null)
            {
                return HttpNotFound();
            }

            bool tempVar = true;
            if (role.IsActive)
            {
                tempVar = false;
            }

            role.IsActive = tempVar;

            db.Entry(role).State = EntityState.Modified;
            db.SaveChanges();

            Session["siteMsgTyp"] = "success";
            Session["siteMsg"] = "The selected option status changed successfully.";

            return RedirectToAction("Index", "Roles");
        }

        // GET: Roles/Create
        [AuthorizeUserRoles]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Roles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,RoleName,IsActive")] Role role)
        {
            if (ModelState.IsValid)
            {
                db.Roles.Add(role);
                db.SaveChanges();

                Session["siteMsgTyp"] = "success";
                Session["siteMsg"] = "The Role was added successfully.";

                return RedirectToAction("Index");
            }

            return View(role);
        }

        // GET: Roles/Edit/5
        [AuthorizeUserRoles]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role role = db.Roles.Where(r => r.ID == id && r.ID > 12).SingleOrDefault();
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        // POST: Roles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,RoleName,IsActive")] Role role)
        {
            if (ModelState.IsValid)
            {
                db.Entry(role).State = EntityState.Modified;
                db.SaveChanges();

                Session["siteMsgTyp"] = "success";
                Session["siteMsg"] = "The Role was modified successfully.";

                return RedirectToAction("Index");
            }
            return View(role);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public JsonResult IsTitleUnique(string RoleName, long ID = 0)
        {
            //check if any of the Title matches the Title specified in the Parameter using the ANY extension method.   
            return Json(!db.Roles.Any(x => x.RoleName == RoleName && x.ID != ID), JsonRequestBehavior.AllowGet);
        }
    }
}
