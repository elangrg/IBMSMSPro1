using IBMSMSPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace IBMSMSPro.Controllers
{
    public class SecurityCrediController : Controller
    {
        // GET: SecurityCredi
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserViewModel user)
        {
            if (ModelState.IsValid)
            {

                Models.SchoolDbEntities _db = new SchoolDbEntities();

                Models.User  usr = _db.Users.SingleOrDefault(dbusr => dbusr.UserName.ToLower() == user
               .UserName.ToLower() && dbusr.Password == user.Password);

                if (usr != null)
                {
                    FormsAuthentication.SetAuthCookie(usr.UserName, false);
                    CurrentUserModel cusr = new CurrentUserModel();
                    cusr.UserName = usr.UserName;
                    cusr.ReferenceToId = usr.ReferenceID;
                    cusr.UserID = usr.UserID;
                    cusr.Role = usr.Role;
                    #region prv code

                    //if (usr.Role == "PHYSICIAN")
                    //{

                    //    cusr.FirstName = _db.Physicians.Find(usr.ReferenceToId).FirstName;
                    //    cusr.LastName = _db.Physicians.Find(usr.ReferenceToId).LastName;
                    //} 
                    #endregion
                    if (usr.Role.ToUpper() == "TRAINER")
                    {
                        var trn = _db.Trainers.Find(usr.ReferenceID);
                        cusr.FirstName = trn.TrainerName;
                    }

                    if (usr.Role.ToUpper() == "STUDENT")
                    {
                        var std = _db.Students.Find(usr.ReferenceID);
                        cusr.FirstName = std.StudentName;
                    }


                    if (usr.Role.ToUpper() == "ADMIN")
                    {
                        cusr.FirstName = "ADMIN";
                    }



                    Session["CurrentUser"] = cusr;
                    return RedirectToAction("Index", usr.Role);
                }
            }
            ModelState.AddModelError("", "invalid Username or Password");
            return View();
        }





        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

    }
}