using FlowerAndGiftShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FlowerAndGiftShop.ViewModels
{
    public class CustomerShopCart
    {
        public Customer Customer { get; set; }
        public List<ShopCart> ShopCart { get; set; }
        public List<Flower> Flowers { get; set; }
        public List<Product> Products { get; set; }
        public List<Service> Services { get; set; }
    }
}