using IBMSMSPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IBMSMSPro.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated == true) {  IBMSMSPro.Models.CurrentUserModel usr = Session["CurrentUser"] as IBMSMSPro.Models.CurrentUserModel;



            if (usr == null)
            {
                IBMSMSPro.Models.SchoolDbEntities _db = new IBMSMSPro.Models.SchoolDbEntities();

                var usrdb = _db.Users.SingleOrDefault(dbusr => dbusr.UserName.ToUpper() == User.Identity.Name);



                if (usrdb != null)
                {

                    usr = new IBMSMSPro.Models.CurrentUserModel();
                    usr.UserName = usrdb.UserName;
                        
                    usr.ReferenceToId = usrdb.ReferenceID;
                    usr.UserID = usrdb.UserID;
                    usr.Role = usrdb.Role;


                        if (usr.Role.ToUpper() == "TRAINER")
                        {
                            var trn = _db.Trainers.Find(usr.ReferenceToId);
                            usr.FirstName = trn.TrainerName;
                        }

                        if (usr.Role.ToUpper() == "STUDENT")
                        {
                            var std = _db.Students.Find(usr.ReferenceToId);
                            usr.FirstName = std.StudentName;
                        }


                        if (usr.Role.ToUpper() == "ADMIN")
                        {
                            usr.FirstName = "ADMIN";
                        }


                        Session["CurrentUser"] = usr;

                    return RedirectToAction("Index", usr.Role);



                }



            } 
            }
              

            return View();
        }
        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

    }
}