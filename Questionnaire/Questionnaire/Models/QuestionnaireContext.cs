using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Questionnaire.Entities;

namespace QuestionnairePrototype.Models
{
    public class QuestionnaireContext : DbContext
    {
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<RiskType> RiskTypes { get; set; }
        public DbSet<RiskRange> RiskRanges { get; set; }
        public DbSet<OverallRange> OverallRanges { get; set; }
        public DbSet<ApplicationSettingsEntry> ApplicationSettingsEntries { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }

        public DbSet<SubmittedStatusForUser> SubmittedeStatusForUsers { get; set; }

        public DbSet<UserPurchase> UserPurchases { get; set; }

        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<CategoryToVendorMap> CategoryToVendorMaps { get; set; }
        public DbSet<VendorStatisticsEntry> VendorStatisticsEntries { get; set; }
        public DbSet<OverallCategoryVendorEntry> OverallCategoryVendorEntries { get; set; }
    }
}