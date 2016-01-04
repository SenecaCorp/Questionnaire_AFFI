using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Questionnaire.Entities;

namespace QuestionnairePrototype.Models.Vendors
{
    public class CategoryWithBindedVendors : ICategoryWithBindedVendors
    {
        private QuestionnaireContext _context = new QuestionnaireContext();

        private Category category;
        public IList<Vendor> BindedVendors { internal set; get; }
        public IList<Vendor> VendorsCanBeBinded { internal set; get; }

        public CategoryWithBindedVendors(Category category)
        {
            this.category = category;
            var bindedVendors = _context.Vendors.Where(x => x.Deleted != true).Join(_context.CategoryToVendorMaps, v => v.Id, cvm => cvm.VendorId,
                (v, cvm) => new { v, cvm.CategoryId, cvm.Index }).Where(c => c.CategoryId == this.category.Id).OrderBy(x => x.Index);
            
            this.BindedVendors = new List<Vendor>();
            bindedVendors.ToList().ForEach(x => this.BindedVendors.Add(x.v));

            this.VendorsCanBeBinded = new List<Vendor>();
            var vendorsAllowToBeBinded = _context.Vendors.Where(x => x.Deleted != true).ToList<Vendor>().Except(BindedVendors);
            vendorsAllowToBeBinded.ToList().ForEach(x => this.VendorsCanBeBinded.Add(x));
        }

        public int Id
        {
            get { return category.Id; }
        }

        public string Title
        {
            get { return category.Title; }
        }
    }
}