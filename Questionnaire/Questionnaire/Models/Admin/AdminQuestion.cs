using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Questionnaire.Entities;

namespace QuestionnairePrototype.Models.Admin
{
    public class AdminQuestion
    {
        private QuestionnaireContext _context = new QuestionnaireContext();
        public String shortText { get; set; }
        public int id { get; set; }
        public int Index { get; set; }
        public int QuestionnaireIndex { get; set; }
        public String CategoryName { get; set; }

        public AdminQuestion(Question q)
        {
            this.id = q.Id;
            this.Index = q.Index;

            this.shortText =_context.Questions.Where(x => x.Id == this.id).First().Title
                .Replace("<b>", String.Empty)
                .Replace("</b>", String.Empty)
                .Replace("<ul>", String.Empty)
                .Replace("<li>", String.Empty);
            if (this.shortText.Length > 100)
            {
                this.shortText = this.shortText.Substring(0, 100) + "...";
            }

            Category thisCategory = null;
            if (q.CategoryId == -1)
                this.CategoryName = "INVALID";
            else
            {
                thisCategory = _context.Categories.First(x => x.Id == q.CategoryId);
                this.CategoryName = thisCategory.Title;
            }

            //Questionnaire index is calculated in awful way
            QuestionnaireIndex = 0;
            _context.Categories.Where(x => x.Index < thisCategory.Index).ToList().ForEach(x => QuestionnaireIndex += x.Questions.Count());
            QuestionnaireIndex += thisCategory.Questions.Where(x => x.Index <= Index).Count();

         }
    }
}
