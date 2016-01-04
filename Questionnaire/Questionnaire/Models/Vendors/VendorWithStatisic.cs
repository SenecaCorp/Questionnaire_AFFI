using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Questionnaire.Entities;

namespace QuestionnairePrototype.Models.Vendors
{
    public class VendorWithStatistic
    {
        private QuestionnaireContext _context = new QuestionnaireContext();
        public Vendor Vendor { internal set; get; }
        public int ClicksCount { internal set; get; }
        public int ContactMeCount { internal set; get; }

        public VendorWithStatistic(Vendor vendor)
        {
            this.Vendor = vendor;
            ClicksCount = _context.VendorStatisticsEntries.
                                   Where(x => x.VendorId == vendor.Id && x.Type == VendorStatisticsEntry.CLICK_TYPE).
                                   Count();
            ContactMeCount = _context.VendorStatisticsEntries.
                                      Where(x => x.VendorId == vendor.Id && x.Type == VendorStatisticsEntry.CONTACT_ME_TYPE).
                                      Count();
        }
    }
}