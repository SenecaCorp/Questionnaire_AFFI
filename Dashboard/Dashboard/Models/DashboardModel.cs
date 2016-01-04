using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Questionnaire.Entities;

namespace Dashboard.Models
{
    public class DashboardModel
    {
        public IEnumerable<DashboardItem> Items { get; set; }
        public UserPurchase LastPurchase { get; set; }
        public string LastPurchasePassword { get; set; }

        public IEnumerable<DashboardItem> NonActivatedItems
        {
            get
            {
                return Items.Where(i => i.status == Dashboard.Models.PurchaseStatus.ReadyForActivate)
                            .OrderBy(i => i.Facility)
                            .ThenByDescending(i => i.DateOfPurchase);
            }
        }

        public IEnumerable<DashboardItem> ActiveItems
        {
            get
            {
                return Items.Where(i => i.status == Dashboard.Models.PurchaseStatus.Active)
                            .OrderBy(i => i.Facility)
                            .ThenByDescending(i => i.UserExpirationDate);
            }
        }

        public IEnumerable<DashboardItem> ExpiredItems
        {
            get
            {
                return Items.Where(i => i.status == Dashboard.Models.PurchaseStatus.Expired)
                            .OrderBy(i => i.Facility)
                            .ThenByDescending(i => i.UserExpirationDate);
            }
        }
    }
}