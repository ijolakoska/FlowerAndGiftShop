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
using System.IO;
using FlowerAndGiftShop.ViewModels;
using System.Web.Security;
using Microsoft.AspNet.Identity;

namespace FlowerAndGiftShop.Controllers
{
    public class FlowersController : Controller
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
            Flower flower = db.Flower.Find(id);
            if (flower == null)
            {
                return HttpNotFound();
            }
            return View(flower);
        }

        [Authorize(Roles = "admin, employee")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Flower flower = db.Flower.Find(id);
            if (flower == null)
            {
                return HttpNotFound();
            }
            return View(flower);
        }

        public ActionResult Index(string typeFlower, string searchedText)
        {
           

            var flowers = from f in db.Flower
                          select f;

            if (!String.IsNullOrEmpty(typeFlower))
            {
                flowers = db.Flower.Where(s => s.Type.Contains(typeFlower));
            }

            if (!String.IsNullOrEmpty(searchedText))
            {
                searchedText = searchedText.ToLower();
                flowers = db.Flower.Where(s => s.Type.ToLower().Contains(searchedText) || s.Name.ToLower().Contains(searchedText));
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
            return View(flowers.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FlowerOrder flowerOrder = new FlowerOrder();
            Flower flower = db.Flower.Find(id);
            if (flower == null)
            {
                return HttpNotFound();
            }
            flowerOrder.Flower = flower;

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

            return View(flowerOrder);

        }

        [Authorize]
        public ActionResult AddToCart(int flowerID)
        {
            string userID = User.Identity.GetUserId();
            Customer customer = (Customer)db.Customer.Where(s => s.UserID.Equals(userID)).FirstOrDefault();
            Flower flower = db.Flower.Find(flowerID);
            if (flower == null)
            {
                return HttpNotFound();
            }

            ShopCart shopCart = new ShopCart();
            shopCart.CustomerID = customer.ID;
            shopCart.ItemID = flower.ID;
            shopCart.ItemType = "Flower";
            shopCart.ItemName = flower.Name;
            shopCart.Price = flower.Price;

            db.ShopCart.Add(shopCart);
            db.SaveChanges();
            return RedirectToAction("../ShopCart/Index");
        }
        #endregion

        #region POST METHODS
        [Authorize(Roles = "admin, employee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Flower flower, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    file.SaveAs(HttpContext.Server.MapPath("~/Images/Flowers/") + file.FileName);
                    string fileName = "Images/Flowers/" + file.FileName;
                    flower.Image = fileName;
                }

                db.Flower.Add(flower);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(flower);
        }

        [Authorize(Roles = "admin, employee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Flower flower, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    file.SaveAs(HttpContext.Server.MapPath("~/Images/Flowers/") + file.FileName);
                    string fileName = "Images/Flowers/" + file.FileName;
                    flower.Image = fileName;
                }

                db.Entry(flower).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(flower);
        }

        [Authorize(Roles = "admin, employee")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Flower flower = db.Flower.Find(id);
            db.Flower.Remove(flower);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        public ActionResult BuyItem(FlowerOrder flowerOrder, string command)
        {            
                int itemID = flowerOrder.Flower.ID;
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new { x.Key, x.Value.Errors })
                    .ToArray();
                if (ModelState.IsValid)
                {
                    Order order = new Order();
                    itemID = (int)flowerOrder.Flower.ID;
                    Flower flower = db.Flower.Find(itemID);
                    if (flower == null)
                    {
                        return HttpNotFound();
                    }
                    flowerOrder.Flower = flower;
                    order.CustomerID = User.Identity.GetUserId();
                    order.ItemType = "Flower";
                    order.ItemName = flower.Name;
                    order.ItemID = itemID;
                    order.DateOrdered = DateTime.Now;
                    //DateTime date;
                    //date.Date = flowerOrder.Order.DateCompleted.Date;
                    //date.Add = int.Parse(timeOrder.ToString().Split(':')[0]);
                    order.DateCompleted = flowerOrder.Order.DateCompleted.Date;
                    order.Description = flowerOrder.Order.Description;
                    order.Price = flower.Price;
                    order.Sale = flowerOrder.Order.Sale * order.Price / 100;
                    order.TotalPrice = order.Price - (flowerOrder.Order.Sale * order.Price / 100);

                    if (flowerOrder.Order.Quantity > 0)
                    {
                        order.Quantity = flowerOrder.Order.Quantity;
                        if (order.Quantity > flowerOrder.Flower.Quantity)
                        {
                            TempData["notice"] = "You must select smaller number of items!!!";
                            return RedirectToAction("Details/" + itemID);
                        }
                        flower.Quantity = flower.Quantity - order.Quantity;
                        order.TotalPrice = order.TotalPrice * order.Quantity;
                    }
                    order.Delivery = flowerOrder.Order.Delivery;
                    if (flowerOrder.Order.Delivery)
                        if (flowerOrder.Order.PlaceDelivery != null)
                        {
                            order.PlaceDelivery = flowerOrder.Order.PlaceDelivery;
                        }

                    string controllerName = "Flower";
                    if (command == "Buy Item")
                    {
                        order.OrderStatusID = 1;
                        controllerName = "Orders";
                    }
                    else if (command == "Add To Cart")  //into order packages
                    {
                        order.OrderStatusID = 6;
                        controllerName = "OrderPackages";
                    }

                    try
                    {
                        db.Order.Add(order);
                        db.Entry(flower).State = EntityState.Modified;
                        db.SaveChanges();
                        if (command == "Add To Cart")  //into order packages
                        {
                            OrderPackages orderPackage = new OrderPackages();
                            orderPackage.OrderID = order.ID;
                            orderPackage.CustomerID = User.Identity.GetUserId();
                            orderPackage.DateOrdered = DateTime.Now;
                            db.OrderPackages.Add(orderPackage);
                        }

                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                    }
                    return RedirectToAction("Index", controllerName, new { area = "" });
                }
                else
                    return RedirectToAction("Details/" + itemID);

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
