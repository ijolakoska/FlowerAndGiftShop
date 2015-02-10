using FlowerAndGiftShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FlowerAndGiftShop.ViewModels
{
    public class FlowerOrder
    {
        public Flower Flower { get; set; }
        public Order Order { get; set; }
    }
}