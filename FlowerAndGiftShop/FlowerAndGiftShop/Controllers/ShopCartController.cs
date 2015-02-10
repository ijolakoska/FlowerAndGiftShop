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
    public class ShopCartController : Controller
    {
        private FlowerAndGiftShopContext db = new FlowerAndGiftShopContext();

        #region GET METHODS
        public ActionResult Index()
        {
            string userID = User.Identity.GetUserId();
            Customer customer = (Customer)db.Customer.Where(s => s.UserID.Equals(userID)).FirstOrDefault();
            if (customer != null)
            {
                ViewBag.UserType = "customer";
            }
            else
            {
                ViewBag.UserType = "employee";
            }

            IQueryable<ShopCart> shopCart = db.ShopCart.Where(s => s.CustomerID == customer.ID);
            CustomerShopCart customerShopCart = new CustomerShopCart();
            customerShopCart.Flowers = new List<Flower>();
            customerShopCart.Products = new List<Product>();
            customerShopCart.Services = new List<Service>();
            customerShopCart.ShopCart = new List<ShopCart>();
            customerShopCart.Customer = customer;

            if (shopCart != null)
            {
                foreach (var item in shopCart)
                {
                    if (item.ItemType.ToLower() == "flower")
                    {
                        Flower flower = (Flower)db.Flower.Where(f => f.ID.Equals(item.ItemID)).FirstOrDefault();
                        customerShopCart.Flowers.Add(flower);
                    }
                    if (item.ItemType.ToLower() == "produt")
                    {
                        Product product = (Product)db.Product.Where(p => p.ID.Equals(item.ItemID)).FirstOrDefault();
                        customerShopCart.Products.Add(product);
                    }
                    if (item.ItemType.ToLower() == "service")
                    {
                        customerShopCart.Services.Add((Service)db.Service.Where(s => s.ID.Equals(item.ItemID)).FirstOrDefault());
                    }
                    customerShopCart.ShopCart.Add(item);
                }
                ViewBag.customerShopCart = customerShopCart.ShopCart;
            }
            else
            {
                ///ShopCart newShopCart = createNewShopCart(cust);
                //customerShopCart.ShopCart = newShopCart;
                customerShopCart = null;
                ViewBag.customerShopCart = null;
            }
            //customerShopCart.Customer = customer;

            return View(customerShopCart.ShopCart);
        }

        // GET: ShopCart/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShopCart shopCart = db.ShopCart.Find(id);
            if (shopCart == null)
            {
                return HttpNotFound();
            }
            return View(shopCart);
        }

        // GET: ShopCart/Create
        public ActionResult Create()
        {
            return View();
        }

        // GET: ShopCart/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShopCart shopCart = db.ShopCart.Find(id);
            if (shopCart == null)
            {
                return HttpNotFound();
            }
            return View(shopCart);
        }

        // GET: ShopCart/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShopCart shopCart = db.ShopCart.Find(id);
            if (shopCart == null)
            {
                return HttpNotFound();
            }
            return View(shopCart);
        }

        public ActionResult Remove(int id)
        {
            ShopCart shopCart = db.ShopCart.Find(id);
            db.ShopCart.Remove(shopCart);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ItemDetails(int id, string type)
        {
            string controllerName = "";
            if (type.ToLower() == "flower")
            {
                //Flower flower = (Flower)db.Flower.Where(f => f.ID.Equals(id)).FirstOrDefault();
                controllerName = "Flowers";
            }
            if (type.ToLower() == "product")
            {
                //Product product = (Product)db.Product.Where(p => p.ID.Equals(id)).FirstOrDefault();
                controllerName = "Products";
            }
            if (type.ToLower() == "service")
            {
                //Service service = (Service)db.Service.Where(s => s.ID.Equals(id)).FirstOrDefault();
                controllerName = "Services";
            }

            //string userID = User.Identity.GetUserId();
            //Customer customer = (Customer)db.Customer.Where(s => s.UserID.Equals(userID)).FirstOrDefault();
            //ShopCart shopCart = db.ShopCart.Where(p => p.ItemID.Equals(id) && p.CustomerID == customer.ID).FirstOrDefault();
            //db.ShopCart.Remove(shopCart);
            //db.SaveChanges();

            return RedirectToAction("../" + controllerName + "/Details/" + id);
        }
        #endregion

        #region POST METHODS
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CustomerID,ItemID,ItemType,ItemName,Price,Sale,TotalPrice")] ShopCart shopCart)
        {
            if (ModelState.IsValid)
            {
                db.ShopCart.Add(shopCart);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(shopCart);
        }
                
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CustomerID,ItemID,ItemType,ItemName,Price,Sale,TotalPrice")] ShopCart shopCart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shopCart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(shopCart);
        }
                
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ShopCart shopCart = db.ShopCart.Find(id);
            db.ShopCart.Remove(shopCart);
            db.SaveChanges();
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
