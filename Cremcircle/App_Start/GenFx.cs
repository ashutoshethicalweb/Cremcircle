using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;
using Cremcircle.Models;

namespace Cremcircle.App_Start
{
    public class GenFx
    {
        public static string GetCurrentUserDetails(string columnName)
        {
            String retVal = "";

            string username = HttpContext.Current.User.Identity.Name.ToString();

            using (DBAuthContext db = new DBAuthContext())
            {
                //GetType().GetProperty(columnName).GetValue(x)
                if (columnName == "ID")
                {
                    retVal = db.Users.Where(user => user.LoginName == username).Select(x => x.ID.ToString()).SingleOrDefault();
                }
                else if (columnName == "FullName")
                {
                    retVal = db.Users.Where(user => user.LoginName == username).Select(x => x.FirstName.ToString() + " " + x.LastName.ToString()).SingleOrDefault();
                }
                else if (columnName == "EmailAddress")
                {
                    retVal = db.Users.Where(user => user.LoginName == username).Select(x => x.EmailAddress.ToString()).SingleOrDefault();
                }
                else if (columnName == "UserImage")
                {
                    retVal = db.Users.Where(user => user.LoginName == username).Select(x => x.UserImage.ToString()).SingleOrDefault();
                }
                else if (columnName == "Role")
                {
                    retVal = db.Users.Where(user => user.LoginName == username).Select(x => x.Role.RoleName.ToString()).SingleOrDefault();
                }
                else if (columnName == "RoleID")
                {
                    retVal = db.Users.Where(user => user.LoginName == username).Select(x => x.Role.ID.ToString()).SingleOrDefault();
                }
                //else if (columnName == "AttachedCustomerID")
                //{
                //    retVal = db.Users.Where(user => user.LoginName == username).Select(x => x.CustomerID.ToString()).SingleOrDefault();
                //}
                else
                {
                    retVal = columnName;
                }

            }
            return retVal;
        }

        public static Boolean IsUserAuthorized(string actionName, string controllerName, string loginName = "")
        {
            string username = "";
            DBAuthContext db = new DBAuthContext();
            if (loginName == "")
            {
                username = HttpContext.Current.User.Identity.Name.ToString();

               

            }
            //split controller names
            string[] cntrlers = controllerName.Split(',');


            //check if the logged in user has access to this page
          
            var SecurityTemplate_ID = (from u in db.Users
                                       where u.LoginName == username
                                       select u.SecurityTemplateID).FirstOrDefault();

            //If Administrator == ID(1), then allow
            if (SecurityTemplate_ID == 1)
            {
                //Authorized => let him in
                return true;
            }

            //

        
            var v = (from a in db.SecurityTemplatePermissions
                     select new { a.SecurityTemplateID, a.Permission.ControllerName, a.Permission.ActionName });
            v = v.Where(a => cntrlers.Contains(a.ControllerName.Replace("Controller", "")));
            v = v.Where(a => a.SecurityTemplateID == SecurityTemplate_ID);


            //new code
            string FindAtionNamebaseonControllerName = "";

            foreach (var p in v)
            {
                FindAtionNamebaseonControllerName += p.ActionName + ",";
            }
            if (!string.IsNullOrEmpty(FindAtionNamebaseonControllerName))
            {
                FindAtionNamebaseonControllerName = FindAtionNamebaseonControllerName.Remove(FindAtionNamebaseonControllerName.Length - 1, 1);

            }

            string[] actionlers = FindAtionNamebaseonControllerName.Split(',');
            //SEARCHING...
            if (!string.IsNullOrEmpty(actionName))
            {
                v = v.Where(a => a.ActionName == actionName);
            }
            else
            {
               
                v = v.Where(a => actionlers.Contains(a.ActionName));
            }

            //end new code

            //SEARCHING...

            //old code
            //if (!string.IsNullOrEmpty(actionName))
            //{
            //    v = v.Where(a => a.ActionName == actionName);
            //}
            //else
            //{
            //    v = v.Where(a => a.ActionName == "Index");
                
            //}
            //end old code
            int count = v.Count();

            if (count > 0)
            {
                //Authorized => let him in
                return true;
            }

            return false;
        }
        public static string MakeActive(string a, string c, string typ = "")
        {
            string retVal = "";
            HttpContextBase currentContext = new HttpContextWrapper(HttpContext.Current);
            RouteData routeData = RouteTable.Routes.GetRouteData(currentContext);

            var _c = routeData.Values["controller"].ToString();
            var _a = routeData.Values["action"].ToString();
            var _typ = "";
            if (GenFx.isVarAvailableNotEmpty(HttpContext.Current.Request.QueryString["typ"]))
            {
                _typ = HttpContext.Current.Request.QueryString["typ"].ToString();
            }

            if ((c.Contains(_c)) && ((a == "") || (a.Contains(_a))) && ((typ == "") || (typ.Contains(_typ))))
            {
                retVal = "active";
            }
            //if ((c == _c) && ((a == "") || (a == _a)) && ((typ == "") || (typ.Contains(_typ))))
            //{
            //    retVal = "active";
            //}

            return retVal;
        }




        public static string yesNo(Boolean v)
        {
            return ((v) ? "<font color='green'>Yes</font>" : "<font color='red'>No</font>");
        }

        public static string getConfigValue(string appKey)
        {
            string retVal = "";
            DBAuthContext dba = new DBAuthContext();
            var v = (from s in dba.AppConfigurations
                     where s.AppKey == appKey
                     select new { s.AppConfigValue }).SingleOrDefault();
            if (v != null)
            {
                retVal = v.AppConfigValue;
            }

            return retVal;
        }

        public static int UserRequestCount()
        {
            int reccnt = 0;

            string userID = GetCurrentUserDetails("ID");
            string roleID = GetCurrentUserDetails("RoleID");

            //using (DBAuthContext db = new DBAuthContext())
            //{
            //    reccnt = db.UserRequests.Where(ur => (ur.ToUsers.Contains(userID) || roleID == "1") && ur.RequestStatus == 0).Count();
            //}

            return reccnt;
        }
        public static void AddToUserLog(string str, long usrID = 0)
        {
            long u = (long)val(GetCurrentUserDetails("ID"));
            if (u > 0)
            {
                usrID = u;
            }

            if (usrID > 0)
            {
                UserLog usrlg = new UserLog
                {
                    AccessDate = DateTime.Now,
                    AccessIP = HttpContext.Current.Request.UserHostAddress,
                    AccessType = str,
                    UserID = usrID
                };
                using (DBAuthContext db = new DBAuthContext())
                {
                    db.UserLogs.Add(usrlg);
                    db.SaveChanges();
                }
            }
        }
        /* Common Functions */
        public static double val(object str)
        {
            double number;
            try
            {
                bool result = Double.TryParse(str.ToString(), out number);
                if (result)
                {
                    return number;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static Guid GetGuid(string str)
        {
            Guid retStr;
            try
            {
                retStr = new Guid(str);
            }
            catch
            {
                retStr = Guid.Empty;
            }
            return retStr;
        }
        public static string GetAttributeDisplayName(PropertyInfo property)
        {
            string retVal = property.Name;
            var atts = property.GetCustomAttributes(
                typeof(DisplayAttribute), true);
            if (atts.Length == 0)
                return null;
            if ((atts[0] as DisplayAttribute).Name != "")
            {
                retVal = (atts[0] as DisplayAttribute).Name;
            }
            return retVal;
        }

        public static bool isVarAvailableNotEmpty(object o)
        {
            if ((o == null) || (o.ToString().Trim().Length == 0))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static string fileUpload(string FolderToUpload, HttpPostedFileBase file, Int32 maxSizeAllowedInMB, String typeAllowed)
        {
            String retVal = "";
            if (file != null && file.ContentLength > 0)
            {
                //1 MB = 1050000
                Int32 maxSizeAllowed = maxSizeAllowedInMB * 1050000;

                //check for file size
                if (file.ContentLength > maxSizeAllowed)
                {
                    retVal = "ERROR! The file size greater than " + maxSizeAllowedInMB + " MB. Please upload a smaller file.";
                }
                else
                {
                    //check for file extension
                    string extension = Path.GetExtension(file.FileName);
                    if (!typeAllowed.Contains(extension.ToLower().Replace(".", "")))
                    {
                        retVal = "ERROR! Invalid Extension '" + extension + "'. Allowed extensions are " + typeAllowed;
                    }
                    else
                    {
                        //All good, check if diectory exists, if not create it
                        var originalDirectory = new DirectoryInfo(string.Format("{0}ContentServer", HttpContext.Current.Server.MapPath(@"~/")));

                        var uploadDir = Path.Combine(originalDirectory.ToString(), FolderToUpload);
                        // Create a new target folder, if necessary.
                        if (!Directory.Exists(uploadDir))
                        {
                            Directory.CreateDirectory(uploadDir);
                        }
                        //upload file
                        Guid gid = Guid.NewGuid();
                        string NewFileName = gid.ToString() + extension;
                        var filePath = Path.Combine(uploadDir, NewFileName);
                        var fileUrl = NewFileName;
                        try
                        {
                            file.SaveAs(filePath);
                            retVal = fileUrl;
                        }
                        catch (Exception ex)
                        {
                            retVal = "ERROR! The file could not be uploaded. The following error occured: " + ex.Message;
                        }
                    }
                }
            }

            return retVal;
        }

        public static string ConvertToDateLogic(string val, string dateformat)
        {
            string retVal = "";
            if (val == null)
            {
                return null;
            }

            if (val == "")
            {
                return null;
            }

            if (dateformat.IndexOf("/") > -1)
            {
                string m, d, y = "";
                string[] f = dateformat.Split('/');
                string[] v = val.Split('/');
                if ((f.Length == 3) && (v.Length == 3))
                {
                    y = ((f[0] == "y") ? v[0] : ((f[1] == "y") ? v[1] : v[2]));
                    m = ((f[0] == "m") ? v[0] : ((f[1] == "m") ? v[1] : v[2]));
                    d = ((f[0] == "d") ? v[0] : ((f[1] == "d") ? v[1] : v[2]));
                    //Response.Write(val + " = ");
                    //Response.Write(dateformat + " = ");
                    //Response.Write(y + "-" + m + "-" + d + "<hr />");
                    DateTime dt = new DateTime();
                    try
                    {
                        //dt = Convert.ToDateTime(y + "-" + m + "-" + d);
                        string strY = "";
                        string strM = "";
                        string strD = "";
                        for (int i = 0; i < y.Length; i++) { strY += "y"; }
                        for (int i = 0; i < m.Length; i++) { strM += "M"; }
                        for (int i = 0; i < d.Length; i++) { strD += "d"; }
                        dt = DateTime.ParseExact(y + "-" + m + "-" + d, strY + "-" + strM + "-" + strD, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
                        retVal = dt.ToString("yyyy-MM-dd");
                    }
                    catch (Exception e)
                    {
                        //throw exception
                        retVal = null;
                    }
                }
                else
                {
                    //throw exception
                    retVal = null;
                }
            }
            else if (dateformat.IndexOf("-") > -1)
            {
                string m, d, y = "";
                string[] f = dateformat.Split('-');
                string[] v = val.Split('-');
                if ((f.Length == 3) && (v.Length == 3))
                {
                    y = ((f[0] == "y") ? v[0] : ((f[1] == "y") ? v[1] : v[2]));
                    m = ((f[0] == "m") ? v[0] : ((f[1] == "m") ? v[1] : v[2]));
                    d = ((f[0] == "d") ? v[0] : ((f[1] == "d") ? v[1] : v[2]));
                    try
                    {
                        DateTime dt = Convert.ToDateTime(y + "-" + m + "-" + d);
                        retVal = dt.ToString("yyyy-MM-dd");
                    }
                    catch (Exception e)
                    {
                        //throw exception
                        retVal = null;
                    }
                }
                else
                {
                    //throw exception
                    retVal = null;
                }
            }
            else if (dateformat.IndexOf(".") > -1)
            {
                string m, d, y = "";
                string[] f = dateformat.Split('.');
                string[] v = val.Split('.');
                if ((f.Length == 3) && (v.Length == 3))
                {
                    y = ((f[0] == "y") ? v[0] : ((f[1] == "y") ? v[1] : v[2]));
                    m = ((f[0] == "m") ? v[0] : ((f[1] == "m") ? v[1] : v[2]));
                    d = ((f[0] == "d") ? v[0] : ((f[1] == "d") ? v[1] : v[2]));
                    try
                    {
                        DateTime dt = Convert.ToDateTime(y + "-" + m + "-" + d);
                        retVal = dt.ToString("yyyy-MM-dd");
                    }
                    catch (Exception e)
                    {
                        //throw exception
                        retVal = null;
                    }
                }
                else
                {
                    //throw exception
                    retVal = null;
                }
            }
            else
            {
                //throw exception
                retVal = null;
            }
            return retVal;
        }

        public static void DeleteFile(string oldFile, string folderInsideContentServer)
        {
            if (oldFile != null && oldFile != string.Empty)
            {
                var deleteDirectory = new DirectoryInfo(string.Format("{0}ContentServer", HttpContext.Current.Server.MapPath(@"~/")));
                string deleteFilePath = Path.Combine(deleteDirectory.ToString() + "/" + folderInsideContentServer, Path.GetFileName(oldFile));

                if (deleteFilePath != null && deleteFilePath != string.Empty)
                {
                    if ((System.IO.File.Exists(deleteFilePath)))
                    {
                        System.IO.File.Delete(deleteFilePath);
                    }
                }
            }
        }
        public static void PopulatePermission()
        {
            Assembly asm = Assembly.GetAssembly(typeof(Cremcircle.MvcApplication));

            var controlleractionlist = asm.GetTypes()
                    .Where(type => typeof(System.Web.Mvc.Controller).IsAssignableFrom(type))
                    .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                    .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
                    .Select(x => new { Controller = x.DeclaringType.Name, Action = x.Name, ReturnType = x.ReturnType.Name, Attributes = String.Join(",", x.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", ""))) })
                    .OrderBy(x => x.Controller).ThenBy(x => x.Action).ToList();

            //HttpContext.Current.Response.Write("<table width='100%' cellspacing='5' cellpadding='5' border='1'><tr><td>Ctr</td><td>Controller</td><td>Action</td><td>Attributes</td><td>ReturnType</td></tr>");
            //int ctr = 1;
            foreach (var item in controlleractionlist)
            {
                if (item.Attributes.Contains("AuthorizeUserRoles"))
                {
                    //HttpContext.Current.Response.Write("<tr><td>" + ctr + "</td><td>" + item.Controller + "</td><td>" + item.Action + "</td><td>" + item.Attributes + "</td><td>" + item.ReturnType + "</td></tr>");
                    //ctr++;
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
                                ActionName = item.Action,
                                OnlyAdminHidden = false
                            };

                            db.Permissions.Add(pe);
                            db.SaveChanges();
                        }
                    }
                }
            }
            //HttpContext.Current.Response.Write("</table>");
            //HttpContext.Current.Response.End();
        }
        public static bool ByteArrayToFile(string _FileName, byte[] _ByteArray)
        {
            try
            {
                // Open file for reading
                FileStream _FileStream = new FileStream(_FileName, FileMode.Create, FileAccess.Write);
                // Writes a block of bytes to this stream using data from  a byte array.
                _FileStream.Write(_ByteArray, 0, _ByteArray.Length);

                // Close file stream
                _FileStream.Close();

                return true;
            }
            catch (Exception _Exception)
            {
                Console.WriteLine("Exception caught in process while trying to save : {0}", _Exception.ToString());
            }

            return false;
        }


        public static string EmailToUserifComplaintAdded(string UserEmailAddress,string Password,string Link)
        {
            EmailViewModel emailViewModel = new EmailViewModel();
            // UserComplaint userComplaint = dba.UserComplaints.Find(userComplaintId);
            // UserComplaintDetailsAndRemark userComplaintDetailsAndRemark = dba.UserComplaintDetailsAndRemarks.Find(userComplaintDetailsAndRemarkId);

            emailViewModel.ToEmail = UserEmailAddress;
            emailViewModel.EmailSubject = "Login Activation Link";
            emailViewModel.EMailBody = "<p>Your Login Name:&nbsp;<strong>" + UserEmailAddress + "</strong>&nbsp;and Password:&nbsp;<strong>" + Password + "</strong></p><br/>";
                
            if(!string.IsNullOrEmpty(Link))
            {
                emailViewModel.EMailBody +="<p><strong>Click Below Link For Activate Your Account</strong><p> ";
                emailViewModel.EMailBody += Link;
            }
               

          


            try
            {
                //Configuring webMail class to send eMails
                //gmail smtp server
                WebMail.SmtpServer = "smtp.gmail.com";

                //gmail port to send emails
                WebMail.SmtpPort = 587;
                WebMail.SmtpUseDefaultCredentials = true;
                //sending emails with secure protocol
                WebMail.EnableSsl = true;
                //EmailId used to send emails from application
                WebMail.UserName = "mediaethical@gmail.com";//gmail,yahoo etc id
                WebMail.Password = "ews*ews123";//password
                
                //Sender email address
                WebMail.From = "mediaethical@gmail.com";//SendergmailId@gmail.com

                //Send email

                WebMail.Send(to: emailViewModel.ToEmail, subject: emailViewModel.EmailSubject, body: emailViewModel.EMailBody, cc: emailViewModel.EmailCC
                    , isBodyHtml: true);

                // ViewBag.Status = "Email Send Successfully.";

                return "1";
            }
            catch (Exception)
            {
                //ViewBag.Status = "Problem while sending email.Please check Details.";
                return "0";
            }


        }


        public static string EmailToUserifComplaintAddedForexternallogin(string UserEmailAddress, string Password)
        {
            EmailViewModel emailViewModel = new EmailViewModel();
            // UserComplaint userComplaint = dba.UserComplaints.Find(userComplaintId);
            // UserComplaintDetailsAndRemark userComplaintDetailsAndRemark = dba.UserComplaintDetailsAndRemarks.Find(userComplaintDetailsAndRemarkId);

            emailViewModel.ToEmail = UserEmailAddress;
            emailViewModel.EmailSubject = "Login Activation Link";
            emailViewModel.EMailBody = "<p>Your Login Name:&nbsp;<strong>" + UserEmailAddress + "</strong>&nbsp;and Password:&nbsp;<strong>" + Password + "</strong></p><br/>";

         


            try
            {
                //Configuring webMail class to send eMails
                //gmail smtp server
                WebMail.SmtpServer = "smtp.gmail.com";

                //gmail port to send emails
                WebMail.SmtpPort = 587;
                WebMail.SmtpUseDefaultCredentials = true;
                //sending emails with secure protocol
                WebMail.EnableSsl = true;
                //EmailId used to send emails from application
                WebMail.UserName = "mediaethical@gmail.com";//gmail,yahoo etc id
                WebMail.Password = "ews*ews123";//password

                //Sender email address
                WebMail.From = "mediaethical@gmail.com";//SendergmailId@gmail.com

                //Send email

                WebMail.Send(to: emailViewModel.ToEmail, subject: emailViewModel.EmailSubject, body: emailViewModel.EMailBody, cc: emailViewModel.EmailCC
                    , isBodyHtml: true);

                // ViewBag.Status = "Email Send Successfully.";

                return "1";
            }
            catch (Exception)
            {
                //ViewBag.Status = "Problem while sending email.Please check Details.";
                return "0";
            }


        }

        public static List<SelectListItem> YearList()
        {
            DateTimeFormatInfo info = DateTimeFormatInfo.GetInstance(null);
            List<SelectListItem> yearitems = new List<SelectListItem>();
            int year = DateTime.Now.Year - 118;
            
            for (int Y = year; Y <= DateTime.Now.Year; Y++)

            {
                SelectListItem yearitem = new SelectListItem()
                {
                    Value = Y.ToString(),
                    Text = Y.ToString()
                };

                yearitems.Add(yearitem);

            }

            

            return yearitems;
        }
    }
}