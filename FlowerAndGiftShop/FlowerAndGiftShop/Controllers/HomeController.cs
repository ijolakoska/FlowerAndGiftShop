using FlowerAndGiftShop.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using FlowerAndGiftShop.Models;

namespace FlowerAndGiftShop.Controllers
{
    public class HomeController : Controller
    {
        private FlowerAndGiftShopContext db = new FlowerAndGiftShopContext();

        public ActionResult Index()
        {
            string userID = User.Identity.GetUserId();
            Customer customer = (Customer)db.Customer.Where(s => s.UserID.Equals(userID)).FirstOrDefault();
            Employee employee = (Employee)db.Employee.Where(s => s.UserID.Equals(userID)).FirstOrDefault();
            if (customer != null)
            {
                ViewBag.UserType = "customer";
            }
            else if (employee != null)
            {
                ViewBag.UserType = "employee";
            }
            else
            {
                ViewBag.UserType = "unauthorized";
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}