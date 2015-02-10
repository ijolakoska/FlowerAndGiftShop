using FlowerAndGiftShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FlowerAndGiftShop.ViewModels
{
    public class CustomerOrder
    {
        public Customer Customer { get; set; }
        public Order Order { get; set; }
        public Flower Flower { get; set; }
        public Product Product { get; set; }
        public Service Service { get; set; }
    }
}