using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Questionnaire.Entities;
using QuestionnairePrototype.Models.Vendors;
using QuestionnairePrototype.Models;
using System.Web.UI;
using QuestionnairePrototype.Infrastructure;

namespace QuestionnairePrototype.Controllers
{
    [Authorize]
    public class AdminVendorsController : Controller
    {
        private QuestionnaireContext _context = new QuestionnaireContext();

        //
        // GET: /Default1/

        public ViewResult Index()
        {
            List<VendorWithStatistic> statisticItems = new List<VendorWithStatistic>();
            _context.Vendors.ToList().ForEach(x => statisticItems.Add(new VendorWithStatistic(x)));
            return View(statisticItems);
        }

        //
        // GET: /Default1/Details/5

        public ViewResult Details(int id)
        {
            Vendor vendor = _context.Vendors.Find(id);
            return View(vendor);
        }

        //
        // GET: /Default1/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Default1/Create

        [HttpPost]
        public ActionResult Create(Vendor vendor)
        {
            if (ModelState.IsValid)
            {
                _context.Vendors.Add(vendor);
                _context.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(vendor);
        }
        
        //
        // GET: /Default1/Edit/5
 
        public ActionResult Edit(int id)
        {
            Vendor vendor = _context.Vendors.Find(id);
            return View(vendor);
        }

        //
        // POST: /Default1/Edit/5

        [HttpPost]
        public ActionResult Edit(Vendor vendor)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(vendor).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vendor);
        }

        //
        // GET: /Default1/Delete/5
 
        public ActionResult Delete(int id)
        {
            Vendor vendor = _context.Vendors.Find(id);
            return View(vendor);
        }

        //
        // POST: /Default1/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Vendor vendor = _context.Vendors.Find(id);
            vendor.Deleted = true;

            var mappingsForDelete = _context.CategoryToVendorMaps.Where(x => x.VendorId == id).ToList();

            foreach(var mapping in mappingsForDelete)
            {
                _context.CategoryToVendorMaps.Remove(mapping);
            }

            _context.SaveChanges();
            
            return RedirectToAction("Index");
        }

        //
        // GET: /Default1/Delete/5

        public ActionResult Restore(int id)
        {
            Vendor vendor = _context.Vendors.Find(id);
            vendor.Deleted = false;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }

        public ViewResult Statistics(int id)
        {
            List<VendorEmailStatisticEntry> statisticItems = new List<VendorEmailStatisticEntry>();
            _context.VendorStatisticsEntries.Where(x => x.VendorId == id && x.Type == VendorStatisticsEntry.CONTACT_ME_TYPE).ToList()
                .ForEach(x => statisticItems.Add(new VendorEmailStatisticEntry(x)));
            return View(statisticItems);
        }

        public HttpResponseBase ExportStatistics(int id)
        {
            List<VendorEmailStatisticEntry> statisticItems = new List<VendorEmailStatisticEntry>();
            _context.VendorStatisticsEntries.Where(x => x.VendorId == id && x.Type == VendorStatisticsEntry.CONTACT_ME_TYPE).ToList()
                .ForEach(x => statisticItems.Add(new VendorEmailStatisticEntry(x)));

            ExcelWriter writer = new ExcelWriter();
            writer.startWorksheet("Statistics");
            writer.startRow();

            writer.addCell("Person name");
            writer.addCell("Person email");
            writer.addCell("Facility name");
            writer.addCell("Category name");
            writer.addCell("Date/time");
            writer.endRow();

            foreach (VendorEmailStatisticEntry entry in statisticItems)
            {
                writer.startRow();
                writer.addCell(entry.User.Name);
                writer.addCell(entry.User.Email);
                writer.addCell(entry.User.FacilityName);
                writer.addCell(entry.CategoryName);
                writer.addCell(entry.OccurredAt);
                writer.endRow();
            }

            writer.endWorksheet();
            writer.endWorkbook();

            string vendorName = _context.Vendors.Where(x => x.Id == id).First().Name;
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=Statistics for " + vendorName + ".xls");
            Response.Charset = "";
            Response.ContentType = "application/ms-excel";
            Response.Write(writer.getString());
            Response.End();

            return Response;
        }
    }
}