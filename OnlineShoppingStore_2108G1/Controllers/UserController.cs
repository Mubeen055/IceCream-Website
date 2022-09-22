using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using OnlineShoppingStore_2108G1.Models;
using System.Data.Entity;

namespace OnlineShoppingStore_2108G1.Controllers
{
    public class UserController : Controller
    {
        private ecommerce_AUHEntities db = new ecommerce_AUHEntities();
        public ActionResult Logout()
        {
            Session["LoggedInUserName"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }


      public ActionResult EditProfile()
        {
            if(Session["LoggedInUserName"] == null)
            {
                return RedirectToAction("Login", "Home");

            }
            int LoggedInUserID = Convert.ToInt32(Session["LoggedInUserID"]);
            User obj = db.Users.Find(LoggedInUserID);
            return View(obj);
                 
        }

        [HttpPost]
         
        public ActionResult EditProfile(User obj)
        {
            if(ModelState.IsValid)
            {
                obj.Email_Verified = "Y";
                obj.Role = "Customer";
                db.Entry(obj).State = EntityState.Modified;
                db.SaveChanges();
                TempData["ProfileUpdateSuccess"] = "<Script>alert('Profile Update Successfully!')</Script>";
                return RedirectToAction("Index", "Home");

            }
            return View(obj);
        }

    }
}