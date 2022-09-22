using OnlineShoppingStore_2108G1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;    

namespace OnlineShoppingStore_2108G1.Controllers
{
    public class HomeController : Controller
    {
        private ecommerce_AUHEntities db = new ecommerce_AUHEntities();

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Register()
        {
            return View();
        }



        [HttpPost]
        public ActionResult Register(User obj)
        {
            if(ModelState.IsValid)
            {
                //Email Exists Validation
                var isEmailExists = db.Users.Any(x => x.Email == obj.Email);                              
                if(isEmailExists == true)
                {
                    ModelState.AddModelError("EmailExists","This email already exists!");
                    return View(obj);
                }

                //Password, Confirm Password Validation
                if(obj.Password != obj.ConfirmPassword)
                {
                    ModelState.AddModelError("PasswordNotSame", "Both passwords must match!");
                    return View(obj);
                }

                obj.Email_Verified = "N";
                obj.Registration_Date = DateTime.Now;
                obj.Role = "Customer";

                db.Users.Add(obj);
                db.SaveChanges();

                TempData["Success"] = "<script> alert('Your account has been created!'); </script>";
                return RedirectToAction("Index");
            }
            return View();
        }




        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Login(User user)
        {
            if (ModelState.IsValid)
            {
                var obj = db.Users.Where(x=>x.Email.Equals(user.Email)
                            && x.Password.Equals(user.Password)
                            && x.Email_Verified.Equals("Y")).FirstOrDefault();
               if(obj != null) //On success
               {
                    Session["LoggedInUserID"] = obj.User_ID;
                    Session["LoggedInUserEmail"] = obj.Email;
                    Session["LoggedInUserAddress"] = obj.Address;
                    Session["LoggedInUserContact"] = obj.Contact_Number;
                    Session["LoggedInUserName"] = obj.Name;
                    Session["LoggedInUserCity"] = obj.City;


                    TempData["Success"] = "<script> alert('Successfully logged in!'); </script>";
                    return RedirectToAction("Index","Home");
               }                
            }
            ModelState.AddModelError("InvalidLogin", "Email or Password is incorrect!");
            return View();
        }

        public ActionResult Shop(int? page)
        {
            ViewBag.CategoriesList = new SelectList(db.Categories, "Category_ID", "Category_Name");

            Products_WithFilter m = new Products_WithFilter();
            int pagSize = 5;
            int pageNumber = (page ?? 1);
            m.ProductList = db.Products
                           .OrderBy(a => a.Product_id)
                           .ToPagedList(pageNumber, pagSize);

            return View(m);
        }

        [HttpPost]
        public ActionResult Shop(int? page, decimal min_price, decimal max_price, string CategoriesList)
        {
            ViewBag.CategoriesList = new SelectList(db.Categories, "Category_ID", "Category_Name", CategoriesList);

            Products_WithFilter m = new Products_WithFilter();
            int pagSize = 5;
            int pageNumber = (page ?? 1);

            if(CategoriesList == "")
            {
                m.ProductList = db.Products.OrderBy(a => a.Product_id).Where(a => a.Price >= min_price && a.Price <= max_price).ToPagedList(pageNumber, pagSize);
            }
            else
            {
                int ctID = Convert.ToInt32(CategoriesList);
                m.ProductList = db.Products.OrderBy(a => a.Product_id).Where(a => a.Price >= min_price && a.Price <= max_price && a.Category_ID == ctID).ToPagedList(pageNumber, pagSize);
            }
        

            return View(m);
        }


    }
}