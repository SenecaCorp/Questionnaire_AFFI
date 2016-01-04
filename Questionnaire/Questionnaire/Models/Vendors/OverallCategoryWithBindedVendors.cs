using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Questionnaire.Entities;

namespace QuestionnairePrototype.Models.Vendors
{
    public class OverallCategoryWithBindedVendors : ICategoryWithBindedVendors
    {
        public const string OVERALL_CATEGORY_TITLE = "Overall Category";
        public const int OVERALL_CATEGORY_ID = -1;
        
        private QuestionnaireContext _context = new QuestionnaireContext();

        public int Id
        {
            get { return OVERALL_CATEGORY_ID; }
        }

        public string Title
        {
            get { return OVERALL_CATEGORY_TITLE; }
        }

        public IList<Vendor> BindedVendors { internal set; get; }
        public IList<Vendor> VendorsCanBeBinded { internal set; get; }

        public OverallCategoryWithBindedVendors()
        {
            var bindedVendors = _context.Vendors.Where(x => x.Deleted != true).Join(_context.OverallCategoryVendorEntries, v => v.Id, ocve => ocve.VendorId,
                (v, ocve) => new { v });
            
            this.BindedVendors = new List<Vendor>();
            //bindedVendors.ToList().ForEach(x => this.BindedVendors.Add(x.v));
           
            this.VendorsCanBeBinded = new List<Vendor>();
            var vendorsAllowToBeBinded = _context.Vendors.Where(x => x.Deleted != true).ToList<Vendor>().Except(BindedVendors);
            vendorsAllowToBeBinded.ToList().ForEach(x => this.VendorsCanBeBinded.Add(x));
        }
    }
}
