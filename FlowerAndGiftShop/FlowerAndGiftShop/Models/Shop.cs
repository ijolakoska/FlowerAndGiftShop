using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace FlowerAndGiftShop.Models
{
    public class Shop
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Logo { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Flower> Flowers { get; set; }
        public virtual ICollection<Service> Services { get; set; }
    }

    public class Order
    {
        public int ID { get; set; }
        public string CustomerID { get; set; }
        public string ItemType { get; set; }
        public string ItemName { get; set; }
        public int ItemID { get; set; }
        public DateTime DateOrdered { get; set; }
        public bool Delivery { get; set; }
        public string PlaceDelivery { get; set; }
        public bool HasCompleted { get; set; }
        public DateTime DateCompleted { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal Sale { get; set; }
        public decimal TotalPrice { get; set; }
        public int OrderStatusID { get; set; }

        public virtual Customer Customer { get; set; }
    }
    public class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public string Type { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
    }
    public class Flower
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public string Type { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }

    }
    public class Service
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public string Type { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
    }

    public class OrderStatus
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class ShopCart
    {
        public int ID { get; set; }
        public int CustomerID { get; set; }
        public int ItemID { get; set; }
        public string ItemType { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Sale { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class OrderPackages
    {
        public int ID { get; set; }
        public int OrderID { set; get; }
        public string CustomerID { get; set; }
        public DateTime DateOrdered { get; set; }
    }
}