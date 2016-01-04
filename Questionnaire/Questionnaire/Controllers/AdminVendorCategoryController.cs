using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Questionnaire.Entities;
using QuestionnairePrototype.Models;
using QuestionnairePrototype.Models.Vendors;

namespace QuestionnairePrototype.Controllers
{
    [Authorize]
    public class AdminVendorCategoryController : Controller
    {
        //
        // GET: /AdminVendorCategory/

        private QuestionnaireContext _context = new QuestionnaireContext();
        
        public ActionResult Index()
        {
            List<ICategoryWithBindedVendors> categoriesWithVendors = new List<ICategoryWithBindedVendors>();
            _context.Categories.ToList().ForEach(x => categoriesWithVendors.Add(new CategoryWithBindedVendors(x)));

            OverallCategoryWithBindedVendors overallCategory = new OverallCategoryWithBindedVendors();
            categoriesWithVendors.Insert(0, overallCategory);

            return View(categoriesWithVendors);
        }

        public ActionResult BindVendorToCategory(int vendorId, int categoryId)
        {
            if (categoryId == OverallCategoryWithBindedVendors.OVERALL_CATEGORY_ID)
            {
                _context.OverallCategoryVendorEntries.Add(new OverallCategoryVendorEntry { VendorId = vendorId });
            }
            else
            {
                CategoryToVendorMap newMapping = new CategoryToVendorMap
                {
                    VendorId = vendorId,
                    CategoryId = categoryId,
                    Index = 101
                };

                _context.CategoryToVendorMaps.Add(newMapping);
            }

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult DropVendorByCategory(int vendorId, int categoryId)
        {
            if (categoryId == OverallCategoryWithBindedVendors.OVERALL_CATEGORY_ID)
            {
                var overallCategoryMappingForDelete = _context.OverallCategoryVendorEntries.Where(x => x.VendorId == vendorId).First();
                _context.OverallCategoryVendorEntries.Remove(overallCategoryMappingForDelete);
            }
            else
            {
                var mappingForDelete = _context.CategoryToVendorMaps.Where(x => x.VendorId == vendorId && x.CategoryId == categoryId).First();
                _context.CategoryToVendorMaps.Remove(mappingForDelete);
            }

            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
