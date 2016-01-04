using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Questionnaire.Entities;

namespace QuestionnairePrototype.Models.Vendors
{
    public class VendorEmailStatisticEntry
    {
        private QuestionnaireContext _context = new QuestionnaireContext();
        public Vendor Vendor { internal set; get; }
        public String CategoryName { internal set; get; }
        public DateTime OccurredAt { internal set; get; }
        public UserPurchase User { internal set; get; }

        public VendorEmailStatisticEntry(VendorStatisticsEntry entry)
        {
            this.Vendor = _context.Vendors.Where(x => x.Id == entry.VendorId).First();
            
            if (entry.CategoryId == 0)
                this.CategoryName = "General List of ‘Suggested Service Providers‘";
            else
                this.CategoryName = _context.Categories.Where(x => x.Id == entry.CategoryId).First().Title;

            this.OccurredAt = entry.OccurredAt;
            this.User = _context.UserPurchases.Where(x => x.Id == entry.UserId).First();
        }

    }
}
