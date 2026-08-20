using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweetBakery.Models
{
    public class Customer
    {
        public string Id { get; set; } = "";
        public string Password { get; set; } = "";
        public bool Active { get; set; }
    }
}
