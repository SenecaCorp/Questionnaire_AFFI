using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Dashboard.Models
{
    public class DashboardItem
    {
        public string Name { set; get; }
        public string Facility { set; get; }
        public string Email { set; get; }
        public DateTime DateOfPurchase { get; set; }
        public DateTime UserRegistrationDate { get; set; }
        public DateTime UserExpirationDate { get; set; }
        public PurchaseStatus status { set; get; }      
    }

    public enum PurchaseStatus
    {
        [Display(Name = "Active")]
        Active,
        [Display(Name = "Expired")]
        Expired,
        [Display(Name = "Ready for activate")]
        ReadyForActivate
    }
}