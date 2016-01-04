using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dashboard.Models
{
    public class PurchasedItem
    {
        public string Name { set; get; }
        public string Facility { set; get; }
        public string Email { set; get; }
        public DateTime DateOfPurchase { set; get; }
    }
}