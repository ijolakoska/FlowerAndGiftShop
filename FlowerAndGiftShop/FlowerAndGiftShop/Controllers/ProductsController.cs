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
    public class ProductsController : Controller
    {
        #region PROPERTIES AND CONSTRUCTORS
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
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [Authorize(Roles = "admin, employee")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        public ActionResult Index(string typeProduct)
        {
            var products = from p in db.Product
                           select p;

            if (!String.IsNullOrEmpty(typeProduct))
            {
                products = db.Product.Where(s => s.Type.Contains(typeProduct));
            }

            var flowers = from f in db.Flower
                           select f;

            if (!String.IsNullOrEmpty(typeProduct))
            {
                flowers = db.Flower.Where(s => s.Type.Contains(typeProduct));
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
            //IEnumerable<Flower> listF = flowers;
            //IEnumerable<Product> listP = products;
            //var list = products.Concat(listF);
            return View(products.ToList());

        }

        public ActionResult Details(int? id, decimal? sale)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerOrder custOrder = new CustomerOrder();
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            custOrder.Product = product;
            if (sale != null)
            {
                custOrder.Order = new Order();
                custOrder.Order.Sale = (decimal)sale;
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
            return View(custOrder);
        }

        [Authorize]
        public ActionResult AddToCart(int productID)
        {
            string userID = User.Identity.GetUserId();
            Customer customer = (Customer)db.Customer.Where(s => s.UserID.Equals(userID)).FirstOrDefault();
            Product product = db.Product.Find(productID);
            if (product == null)
            {
                return HttpNotFound();
            }

            ShopCart shopCart = new ShopCart();
            shopCart.CustomerID = customer.ID;
            shopCart.ItemID = product.ID;
            shopCart.ItemType = "Product";
            shopCart.ItemName = product.Name;
            shopCart.Price = product.Price;

            db.ShopCart.Add(shopCart);
            db.SaveChanges();
            return RedirectToAction("../ShopCart/Index");
        }
        #endregion

        #region POST METHODS
        [Authorize(Roles = "admin, employee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    file.SaveAs(HttpContext.Server.MapPath("Images/Products/") + file.FileName);
                    string fileName = "Images/Products/" + file.FileName;
                    product.Image = fileName;
                }

                db.Product.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        [Authorize(Roles = "admin, employee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    file.SaveAs(HttpContext.Server.MapPath("~/Images/Products/") + file.FileName);
                    string fileName = "~/Images/Products/" + file.FileName;
                    product.Image = fileName;
                }

                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        [Authorize(Roles = "admin, employee")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Product.Find(id);
            db.Product.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        public ActionResult BuyItem(CustomerOrder customerOrder, string command)
        {
            int itemID = customerOrder.Product.ID;
            var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new { x.Key, x.Value.Errors })
                    .ToArray();
            if (ModelState.IsValid)
            {
                Order order = new Order();
                itemID = (int)customerOrder.Product.ID;
                Product product = db.Product.Find(itemID);
                if (product == null)
                {
                    return HttpNotFound();
                }
                customerOrder.Product = product;
                order.CustomerID = User.Identity.GetUserId();
                order.ItemType = "Product";
                order.ItemName = product.Name;
                order.ItemID = itemID;
                order.DateOrdered = DateTime.Now;
                order.DateCompleted = customerOrder.Order.DateCompleted.Date;
                order.Description = customerOrder.Order.Description;
                order.Price = product.Price;
                order.Sale = customerOrder.Order.Sale;
                order.TotalPrice = order.Price - (order.Sale * order.Price / 100); 

                if (customerOrder.Order.Quantity > 0)
                {
                    order.Quantity = customerOrder.Order.Quantity;
                    if (order.Quantity > customerOrder.Product.Quantity)
                    {
                        TempData["notice"] = "You must select smaller number of items!!!";
                        return RedirectToAction("Details/" + itemID);
                    }
                    product.Quantity = product.Quantity - order.Quantity;
                }
                order.Delivery = customerOrder.Order.Delivery;
                if (customerOrder.Order.Delivery)
                    if (customerOrder.Order.PlaceDelivery != null)
                    {
                        order.PlaceDelivery = customerOrder.Order.PlaceDelivery;
                    }

                string controllerName = "Product";
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
                    db.Entry(product).State = EntityState.Modified;
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
