using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FlowerAndGiftShop.DataAccessLayer;
using FlowerAndGiftShop.Models;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using FlowerAndGiftShop.ViewModels;

namespace FlowerAndGiftShop.Controllers
{
    public class ServicesController : Controller
    {
        #region PROPERTIES AND CONSTRUCTORS
        private FlowerAndGiftShopContext db = new FlowerAndGiftShopContext();
        #endregion

        #region GET METHODS
        // GET: Services/Create
        [Authorize(Roles = "admin, employee")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "admin, employee")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = db.Service.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }
        
        // GET: Services/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = db.Service.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }
        
        // GET: Services
        public ActionResult Index(String name)
        {
            string userID = User.Identity.GetUserId();
            if (userID != null)
            {
                Customer cust = (Customer)db.Customer.Where(s => s.UserID.Equals(userID)).FirstOrDefault();
                ViewBag.UserType = (cust != null) ? "customer" : "employee";
            }
            else
            {
                ViewBag.UserType = "unauthorized";
            }    
 
            return View();
        }

        public ActionResult Promotion(string name)
        {
            var products = from p in db.Product
                           select p;

            if (!String.IsNullOrEmpty(name))
            {
                products = db.Product.Where(s => s.Type.Contains(name));
            }
            string userID = User.Identity.GetUserId();
            if (userID != null)
            {
                Customer cust = (Customer)db.Customer.Where(s => s.UserID.Equals(userID)).FirstOrDefault();
                ViewBag.UserType = (cust != null) ? "customer" : "employee";
            }
            else
            {
                ViewBag.UserType = "unauthorized";
            }
            return View(products.ToList());

        }

        public ActionResult CreatePromotion(string name, int saleValue)
        {
            //TODO: to be implemented
            return RedirectToAction("Index");
        }

        // GET: Services/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = db.Service.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            string userID = User.Identity.GetUserId();
            if (userID != null)
            {
                Customer cust = (Customer)db.Customer.Where(s => s.UserID.Equals(userID)).FirstOrDefault();
                ViewBag.UserType = (cust != null) ? "customer" : "employee";
            }
            else
            {
                ViewBag.UserType = "unauthorized";
            }     
            return View(service);
        }

        [Authorize]
        public ActionResult AddToCart(int serviceID)
        {
            string userID = User.Identity.GetUserId();
            Customer customer = (Customer)db.Customer.Where(s => s.UserID.Equals(userID)).FirstOrDefault();
            Service service = db.Service.Find(serviceID);
            if (service == null)
            {
                return HttpNotFound();
            }

            ShopCart shopCart = new ShopCart();
            shopCart.CustomerID = customer.ID;
            shopCart.ItemID = service.ID;
            shopCart.ItemType = "Service";
            shopCart.ItemName = service.Name;
            shopCart.Price = service.Price;

            db.ShopCart.Add(shopCart);
            db.SaveChanges();
            return RedirectToAction("../ShopCart/Index");
        }
        #endregion

        #region POST METHODS
        [Authorize(Roles = "admin, employee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Service service, HttpPostedFileBase file)
        {
            
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    file.SaveAs(HttpContext.Server.MapPath("~/Images/Services/") + file.FileName);
                    string fileName = "Images/Services/" + file.FileName;
                    service.Image = fileName;
                }

                db.Service.Add(service);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(service);
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Description,Type,Price,Image")] Service service)
        {
            if (ModelState.IsValid)
            {
                db.Entry(service).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(service);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Service service = db.Service.Find(id);
            db.Service.Remove(service);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        public ActionResult BuyItem(CustomerOrder customerOrder)
        {
            int itemID = customerOrder.Product.ID;

            if (ModelState.IsValid)
            {
                Order order = new Order();
                itemID = (int)customerOrder.Product.ID;
                Service service = db.Service.Find(itemID);
                if (service == null)
                {
                    return HttpNotFound();
                }
                customerOrder.Service = service;
                order.CustomerID = User.Identity.GetUserId(); // Membership.GetUser().ProviderUserKey; 
                order.ItemType = "Service";
                order.ItemName = service.Name;
                order.ItemID = itemID;
                order.DateOrdered = DateTime.Now;
                order.DateCompleted = customerOrder.Order.DateCompleted.Date;
                order.Description = customerOrder.Order.Description;
                order.Price = service.Price;
                order.Sale = 0;
                order.TotalPrice = order.Price - (order.Sale / 100 * order.Price);
                order.OrderStatusID = 1;

                if (customerOrder.Order.Quantity > 0)
                {
                    order.Quantity = customerOrder.Order.Quantity;
                    if (order.Quantity > customerOrder.Product.Quantity)
                    {
                        TempData["notice"] = "You must select smaller number of items!!!";
                        return RedirectToAction("Details/" + itemID);
                    }
                    service.Quantity = service.Quantity - order.Quantity;
                }
                order.Delivery = customerOrder.Order.Delivery;
                if (customerOrder.Order.Delivery)
                    if (customerOrder.Order.PlaceDelivery != null)
                    {
                        order.PlaceDelivery = customerOrder.Order.PlaceDelivery;
                    }
                try
                {
                    db.Order.Add(order);
                    db.Entry(service).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
            }

            return RedirectToAction("Index", "Orders", new { area = "" });

        }
        #endregion
        

        
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
