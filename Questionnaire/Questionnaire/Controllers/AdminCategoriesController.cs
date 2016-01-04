using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuestionnairePrototype.Models.Admin;
using QuestionnairePrototype.Models;
using Questionnaire.Entities;
using System.Web.Security;

namespace QuestionnairePrototype.Controllers
{ 
    public class AdminCategoriesController : Controller
    {
        private QuestionnaireContext db = new QuestionnaireContext();

        public ActionResult Index()
        {
            var hash = (string)HttpContext.Session["facilityDateHash"];

            //if user is not an admin, then logout user
            if (String.IsNullOrEmpty(hash))
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", "Home");
            }
            
            List<AdminCategory> allCategoriesList = new List<AdminCategory>();
           
            db.Categories.OrderBy(c => c.Index).ToList().ForEach(x => allCategoriesList.Add(new AdminCategory(x)));
            
            return View(allCategoriesList);
        }


        public ActionResult Create()
        {
            Category cat = new Category();
            cat.Index = db.Categories.Count() + 1;
            cat.ImageDisplayFlag = true;
            return View(cat);
        }

        [HttpPost]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                category.ImageDisplayFlag = true;

                category.RiskRanges = new List<RiskRange>();
                category.RiskRanges.Add(new RiskRange(1));
                category.RiskRanges.Add(new RiskRange(2));
                category.RiskRanges.Add(new RiskRange(3));
                db.Categories.Add(category);

                Category entry = db.Entry(category).Entity;
                System.Data.Entity.DbSet<Category> dbset = db.Categories;
               
                db.SaveChanges();
                return RedirectToAction("Edit/" + category.Id);
            }

            return View(category);
        }

        //[HttpPost]
        //public ActionResult Create(AdminCategory admincategory)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Categories.Add(admincategory);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(admincategory);
        //}
        
       
 
        public ActionResult Edit(int id)
        {
            Category category = db.Categories.Find(id);
            return View(category);
        }

        //
        // POST: /AdminCategories/Edit/5

        [HttpPost]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                Category entry = db.Entry(category).Entity;
                System.Data.Entity.DbSet<Category> dbset = db.Categories;
                //If we changed index, we need to shift other elements
                int oldIndex = dbset.First(x => x.Id == entry.Id).Index;

                shiftIndexes(entry, oldIndex, dbset);

                //Ugly workaround 
                Category dbQ = dbset.First(x => x.Id == entry.Id);
                dbQ.Index = entry.Index;
                dbQ.Annotation = entry.Annotation;
                dbQ.Weight = entry.Weight;
                dbQ.Title = entry.Title;
                dbQ.ImageDisplayFlag = entry.ImageDisplayFlag;
                db.SaveChanges();
                
                //db.Entry(category).State = EntityState.Modified;
                //db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(category);
        }

        public ActionResult EditRisk(int id)
        {
            RiskRange risk = db.RiskRanges.Find(id);
            return View(risk);
        }

        [HttpPost]
        public ActionResult EditRisk(RiskRange risk)
        {
            if (ModelState.IsValid)
            {
                db.Entry(risk).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", new { id = risk.CategoryId });
            }
            return View(risk);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
                return RedirectToAction("./");

            Category category = db.Categories.Find(id);

            if (category == null)
                return RedirectToAction("./");

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);

            System.Data.Entity.DbSet<Category> dbset = db.Categories;
            int oldIndex = category.Index;
            category.Index = dbset.Count() + 100;

            shiftIndexes(category, oldIndex, dbset);

            db.Categories.Remove(category);
            db.SaveChanges();

            return RedirectToAction("Index");
        }


        private void shiftIndexes(Category entry, int oldIndex, System.Data.Entity.DbSet<Category> dbset)
        {
            if (entry.Index - oldIndex == 1)
            {
                //Move forward by 1 step
                try
                {
                    dbset.First(x => x.Index == entry.Index).Index = oldIndex; //If was last row - it will throw an exception
                }
                catch (Exception e)
                { }
            }
            else if (entry.Index > oldIndex)
            {
                //We moved forward
                foreach (var q in dbset.ToList())
                {
                    if (oldIndex < q.Index && q.Index < entry.Index)
                        q.Index = q.Index - 1;
                }
                entry.Index = entry.Index - 1;
            }
            else if (entry.Index < oldIndex)
            {
                //We moved backwards
                foreach (var q in dbset.ToList())
                {
                    if (entry.Index <= q.Index && q.Index < oldIndex)
                        q.Index = q.Index + 1;
                }
            }

            if (entry.Index > dbset.Count())
                entry.Index = dbset.Count();
            else if (entry.Index < 1)
                entry.Index = 1;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}