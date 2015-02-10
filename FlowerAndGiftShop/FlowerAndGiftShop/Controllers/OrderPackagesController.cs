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

namespace FlowerAndGiftShop.Controllers
{
    public class OrderPackagesController : Controller
    {
        #region CONSTRUCTOR
        private FlowerAndGiftShopContext db = new FlowerAndGiftShopContext();
        #endregion

        #region GET METHODS
        // GET: OrderPackages
        [Authorize]
        public ActionResult Index()
        {
            string userID = User.Identity.GetUserId();
            Customer cust = (Customer)db.Customer.Where(s => s.UserID.Equals(userID)).FirstOrDefault();
            if (cust != null)
            {
                ViewBag.UserType = "customer";
            }
            else
            {
                ViewBag.UserType = "employee";
            }
            IQueryable<OrderPackages> orderPackages = db.OrderPackages.Where(s => s.CustomerID.Contains(userID));
            List<Order> ordersForView = new List<Order>();
            foreach (var order in orderPackages)
            {
                Order newOrder = (Order) db.Order.Where(o => o.ID == order.OrderID && o.OrderStatusID == 6).FirstOrDefault();
                if (newOrder != null)
                {
                    ordersForView.Add(newOrder);
                    ViewBag.customerShopCart = newOrder;
                }
            }

            return View(ordersForView);
            //return View(db.Order.ToList());
        }

        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Order.Find(id);            
            Customer cust = (Customer)db.Customer.Where(s => s.UserID.Equals(order.CustomerID)).FirstOrDefault();
            if (cust != null)
            {
                ViewBag.UserType = "customer";
            }
            else
            {
                ViewBag.UserType = "employee";
            }
            ViewBag.StatusOrderItem = db.OrderStatus.Find(order.OrderStatusID);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        [Authorize]
        public ActionResult Remove(int id)
        {
            Order order = db.Order.Find(id);
            order.OrderStatusID = 4;
            db.Entry(order).State = EntityState.Modified;

            //where condition for orderID
            OrderPackages orderPackage = db.OrderPackages.Where(o => o.OrderID == order.ID).FirstOrDefault();
            db.OrderPackages.Remove(orderPackage);
            db.SaveChanges();

            //TODO: send message for successfully removed item
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult BuyItem(int id)
        {         
            Order order = db.Order.Find(id);
            order.OrderStatusID = 1;
            db.Entry(order).State = EntityState.Modified;

            OrderPackages orderPackage = db.OrderPackages.Where(o => o.OrderID == order.ID).FirstOrDefault();
            db.OrderPackages.Remove(orderPackage);
            db.SaveChanges();

            //TODO: send message for successfully buyed item
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult BuyAllItems(string ids)
        {
            var listIds = ids.Split(',');
            
            foreach (var id in listIds){
                if (id != "0" && id != null)
                {
                    Order order = db.Order.Find(int.Parse(id));
                    order.OrderStatusID = 1;
                    db.Entry(order).State = EntityState.Modified;

                    OrderPackages orderPackage = db.OrderPackages.Where(o => o.OrderID == order.ID).FirstOrDefault();
                    db.OrderPackages.Remove(orderPackage);
                    db.SaveChanges();
                }
            }
            //TODO: send message for successfully buyed items
            return RedirectToAction("Index");
        }
        #endregion

        #region HELPER METHODS
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}
