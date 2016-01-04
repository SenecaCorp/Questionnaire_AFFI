using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Dashboard.Models
{
    public class PurchasedItemModel
    {
        [Required]
        public string Name { set; get; }

        [Required]
        public string Facility { set; get; }

        [Required]
        public string Email { set; get; }

        [Required]
        public string DateOfPurchase { set; get; }

        public PurchasedItemModel(string facility, string name, string email, string date)
        {
            Name = name;
            Facility = facility;
            Email = email;
            DateOfPurchase = date;
        }

        public PurchasedItemModel()
        {

        }
    }
}
