using OnlineShoppingStore_2108G1.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShoppingStore_2108G1.Controllers
{
    public class AdminController : Controller
    {
        private ecommerce_AUHEntities db = new ecommerce_AUHEntities();
        // GET: Admin
        public ActionResult Index()
        {
            if (Session["LoggedAdminName"] == null)
            {
                RedirectToAction("Index", "Home");
            }
            ViewBag.TotalUsers = db.Users.Count();
            ViewBag.TotalOrders = db.Orders.Count();
            ViewBag.TotalEarnnings = db.Orders.Where(a => a.Order_Status.Equals("Completed"))
                                               .Select(a => a.Total_Amount).Sum();
            return View();
        }

        public ActionResult AddProduct()
        {
            ViewBag.CategoriesList = new SelectList(db.Categories, "Category_ID", "Category_Name");
            if (Session["LoggedAdminName"] == null)
            {
                return RedirectToAction("Admin", "Home");
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
                var obj = db.Users.Where(x => x.Email.Equals(user.Email)
                            && x.Password.Equals(user.Password)
                            && x.Email_Verified.Equals("Y")
                            && x.Role.Equals("Admin")).FirstOrDefault();
                if (obj != null) //On success
                {

                    Session["LoggedAdminName"] = obj.Name;
                    return RedirectToAction("Index", "Admin");
                }
            }
            ModelState.AddModelError("InvalidLogin", "Email or Password is incorrect!");
            return View();
        }

            [HttpPost]
            public ActionResult AddProduct(Product product,HttpPostedFileBase file)
            {
                ViewBag.CategoriesList = new SelectList(db.Categories, "Category_ID", "Category_Name", product.Category_ID);
                if(ModelState.IsValid)
                {
                    string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    string extension = Path.GetExtension(file.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmss") + extension;
                    product.Picture = "/External/Main/Product/" + fileName;
                    fileName = Path.Combine(Server.MapPath("/External/Main/Product/"), fileName);

                    db.Products.Add(product);
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
                return View();
            }



        }
    }
