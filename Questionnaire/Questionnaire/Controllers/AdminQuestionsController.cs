using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Questionnaire.Entities;
using QuestionnairePrototype.Models;
using QuestionnairePrototype.Models.Admin;
using System.Data;
using QuestionnairePrototype.Models.Membership;
using System.Web.Security;

namespace QuestionnairePrototype.Controllers
{
    [ValidateInput(false)]
    public class AdminQuestionsController : Controller
    {
        
        private QuestionnaireContext _context = new QuestionnaireContext();
        private UserRepository _userRepository = new UserRepository();

        public ActionResult Index(int? sortType)
        {
            var hash = (string)HttpContext.Session["facilityDateHash"];

            //if user is not an admin, then logout user
            if (String.IsNullOrEmpty(hash))
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", "Home");
            }
            
            List<AdminQuestion> allQuestionsList = new List<AdminQuestion>();
            _context.Questions.ToList().ForEach(x => allQuestionsList.Add(new AdminQuestion(x)));

            //Sort Questions
            if(sortType == null ||sortType == 2)
                allQuestionsList = allQuestionsList.OrderBy(r => r.CategoryName).ThenBy(n => n.QuestionnaireIndex).ToList();
            else if(sortType == 0)
                allQuestionsList = allQuestionsList.OrderBy(r => r.QuestionnaireIndex).ToList();
            else if (sortType == 1)
                allQuestionsList = allQuestionsList.OrderBy(r => r.Index).ToList();
            else
                allQuestionsList = allQuestionsList.OrderBy(r => r.CategoryName).ToList();
            
            
            return View(allQuestionsList);
        }

        private string getUsersEmail()
        {
            return HttpContext.Session["email"].ToString();
        }

        /////EDIT////////////////////////////////////
        public ActionResult Edit(int id)
        {
            Question question = _context.Questions.Find(id);
            return View(question);
        }

        public ActionResult EditAnswer(int id)
        {
            Answer answer = _context.Answers.Find(id);
            return View(answer);
        }

        [HttpPost]
        public ActionResult Edit(Question question)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(question).State = EntityState.Modified;
                _context.SaveChanges();
                //return RedirectToAction("Index");

               // Question entry = _context.Entry(question).Entity;
               // System.Data.Entity.DbSet<Question> dbset = _context.Questions;
               
                //If we changed index, we need to shift other elements
               // int oldIndex = dbset.First(x => x.Id == entry.Id).Index;

               // shiftIndexes(entry, oldIndex, dbset);

                //Ugly workaround 
                //Question dbQ = dbset.First(x => x.Id == entry.Id);
                //dbQ.Index = entry.Index;
                //dbQ.CategoryId = question.CategoryId;
                //dbQ.PostAnnotation = entry.PostAnnotation;
                //dbQ.Summary = entry.Summary;
                //dbQ.Title = entry.Title;
                //_context.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(question);
        }

        [HttpPost]
        public ActionResult EditAnswer(Answer answer)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(answer).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Edit", new { id = answer.QuestionId });
            }
            return View(answer);
        }


        /////CREATE QUESTION////////////////////
        public ActionResult Create()
        {
            Question q = new Question();
            q.Index = _context.Questions.Count() + 1;
            return View(q);
        }

        [HttpPost]
        public ActionResult Create(Question question)
        {
            if (ModelState.IsValid)
            {
                //Always set a new question as the first question in its category. The user can change the order of the question after it is saved
                question.Index = 1;
                
                _context.Questions.Add(question);

                Question entry = _context.Entry(question).Entity;
                
                _context.SaveChanges();
                return RedirectToAction("Edit/" + question.Id);
            }

            return View(question);
        }

        ///DELETE QUESTION//////////
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return RedirectToAction("./");

            Question q = _context.Questions.Find(id);

            if (q == null)
                return RedirectToAction("./");

            return View(q);
        }
                
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Question q = _context.Questions.Find(id);

            System.Data.Entity.DbSet<Question> dbset = _context.Questions;
            int oldIndex = q.Index;
            q.Index = dbset.Count() + 100;

            shiftIndexes(q, oldIndex, dbset);

            _context.Questions.Remove(q);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }



        //ADD QUESTION ANSWER/////
        public ActionResult AddAnswer(int qId)
        {
            Answer q = new Answer();
            q.Index = _context.Questions.ToList().Find(x => x.Id == qId).Answers.Count() + 1;
            q.QuestionId = qId;
            return View(q);
        }

        [HttpPost]
        public ActionResult AddAnswer(Answer answer)
        {
            if (ModelState.IsValid)
            {
                _context.Questions.ToList().Find(x => x.Id == answer.QuestionId).Answers.Add(answer);

                Answer entry = _context.Entry(answer).Entity;
                _context.SaveChanges();
                return RedirectToAction("Edit/" + answer.QuestionId);
            }

            return View(answer);
        }


        public ActionResult DeleteAnswer(int? id)
        {
            if (id == null)
                return Redirect("../");
            Answer q = _context.Answers.Find(id);
            if (q == null)
                return Redirect("../");
            return View(q);
        }

       
        [HttpPost, ActionName("DeleteAnswer")]
        public ActionResult DeleteAnswerConfirmed(int id)
        {
            Answer q = _context.Answers.Find(id);

            IEnumerable<Answer> dbset = _context.Answers.Where(x => x.QuestionId == q.QuestionId).ToList();
            int oldIndex = q.Index;
            q.Index = dbset.Count() + 1;

            shiftIndexes(q, oldIndex, dbset);
            _context.Answers.Remove(q);
            _context.SaveChanges();

            return RedirectToAction("./Edit/" + q.QuestionId);
        }


        private void shiftIndexes(Answer entry, int oldIndex, IEnumerable<Answer> dbset)
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

        private void shiftIndexes(Question entry, int oldIndex, System.Data.Entity.DbSet<Question> dbset)
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


    }
}
