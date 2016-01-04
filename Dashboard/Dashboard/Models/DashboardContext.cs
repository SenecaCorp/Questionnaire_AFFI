using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Questionnaire.Entities;

namespace Dashboard.Models
{
    public class DashboardContext : DbContext
    {
        public DbSet<UserPurchase> UserPurchases { get; set; }
    }
}