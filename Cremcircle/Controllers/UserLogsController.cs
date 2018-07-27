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
    public class UserLogsController : Controller
    {
        private DBAuthContext db = new DBAuthContext();

        // GET: UserLogs/Activity
        [AuthorizeUserRoles]
        public ActionResult AllUserActivities()
        {
            //var userLogs = db.UserLogs.Include(u => u.User);
            //return View(userLogs.ToList());
            return View();
        }

        [HttpPost]
        public ActionResult LoadDataActivity()
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

            var v = (from a in db.UserLogs select new { LoginName = a.User.LoginName, a.AccessType, a.AccessIP, a.AccessDate, DT_RowId = a.Id });

            //SEARCHING...
            if (!string.IsNullOrEmpty(txtSearch))
            {
                v = v.Where(a => a.AccessType.Contains(txtSearch) || a.AccessIP.Contains(txtSearch) || a.LoginName.Contains(txtSearch));
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

        // GET: UserLogs/Index
        public ActionResult Index()
        {
            //var userLogs = db.UserLogs.Include(u => u.User);
            //return View(userLogs.ToList());
            return View();
        }

        [HttpPost]
        public ActionResult LoadData()
        {
            //Get Parameters
            string lgnNm = HttpContext.User.Identity.Name.ToString();

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

            var v = (from a in db.UserLogs select new { LoginName = a.User.LoginName, a.AccessType, a.AccessIP, a.AccessDate, DT_RowId = a.Id });

            //SEARCHING...
            if (!string.IsNullOrEmpty(txtSearch))
            {
                v = v.Where(a => a.LoginName == lgnNm && (a.AccessType.Contains(txtSearch) || a.AccessIP.Contains(txtSearch) || a.LoginName.Contains(txtSearch)));
            }
            else
            {
                v = v.Where(a => a.LoginName == lgnNm);
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
