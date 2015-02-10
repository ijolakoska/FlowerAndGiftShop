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
using FlowerAndGiftShop.ViewModels;
using System.Web.Security;
using Microsoft.AspNet.Identity;

namespace FlowerAndGiftShop.Controllers
{
    public class OrdersController : Controller
    {
        #region PROPERTIES
        #endregion

        #region CONSTRUCTORS
        private FlowerAndGiftShopContext db = new FlowerAndGiftShopContext();
        #endregion

        #region GET METHODS

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
            Order order = db.Order.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        [Authorize(Roles = "admin, employee")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Order.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        [Authorize(Roles = "employee")]
        public ActionResult Complete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Order.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            order.OrderStatusID = 2;
            order.HasCompleted = true;
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Index()
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
            var orders = db.Order.Where(s => s.CustomerID.Contains(userID) 
                                                && s.OrderStatusID != 4
                                                && s.OrderStatusID != 5
                                                && s.OrderStatusID != 6);
            return View(orders);
        }

        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerOrder customerOrder = new CustomerOrder();
            Order order = db.Order.Find(id);
            customerOrder.Order = order;
            Customer cust = (Customer)db.Customer.Where(s => s.UserID.Equals(order.CustomerID)).FirstOrDefault();
            if (cust != null)
            {
                customerOrder.Customer = cust;
                ViewBag.UserType = "customer";
            }
            else
            {
                ViewBag.UserType = "employee";
            }

            if (order.ItemType.Equals("Flower"))
            {
                customerOrder.Flower = db.Flower.Find(order.ItemID);
            }
            else if (order.ItemType.Equals("Product"))
            {
                customerOrder.Product = db.Product.Find(order.ItemID);
            }
            else if (order.ItemType.Equals("Service"))
            {
                customerOrder.Service = db.Service.Find(order.ItemID);
            }
            else
            {
                return HttpNotFound();
            }
            ViewBag.StatusOrderItem = db.OrderStatus.Find(order.OrderStatusID);
            //TempData["ItemType"] = order.ItemType;

            if (order == null)
            {
                return HttpNotFound();
            }
            return View(customerOrder);
        }

        [Authorize(Roles = "customer")]
        public ActionResult Cancel(int id)
        {
            Order order = db.Order.Find(id);
            order.OrderStatusID = 3;
            order.HasCompleted = true;
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
                
        public ActionResult Remove(int id)
        {
            Order order = db.Order.Find(id);
            order.OrderStatusID = 4;  //Remove from list - set as deleted
            order.HasCompleted = true;
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion

        #region POST METHODS
        [Authorize(Roles = "admin, employee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Order order)
        {
            if (ModelState.IsValid)
            {
                order.OrderStatusID = 1;
                db.Order.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(order);
        }

        [Authorize(Roles = "admin, employee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        [Authorize(Roles = "admin, employee")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Order.Find(id);
            //db.Order.Remove(order);
            order.OrderStatusID = 4;  //set status "deleted"
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        #endregion

        #region OTHER METHODS
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
