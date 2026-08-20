using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Models/Product.cs

namespace SweetBakery.Models
{
    public class Product
    {
        public string Name { get; set; }
        public int Stock { get; set; }
        public int Price { get; set; }
        public int ExpDay { get; set; }
        public int ExpMonth { get; set; }
        public int ExpYear { get; set; }

        public bool IsLowStock => Stock < 5;

        public bool ExpiresOn(DateTime date) =>
            ExpDay == date.Day && ExpMonth == date.Month && ExpYear == date.Year;
    }
}