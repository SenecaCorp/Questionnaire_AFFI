using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Questionnaire.Entities;

namespace QuestionnairePrototype.Models.Admin
{
    public class AdminCategory
    {
        private QuestionnaireContext _context = new QuestionnaireContext();
        public String Text { get; set; }
        public int Id { get; set; }
        public int Weight { get; set; }

        public AdminCategory(Category c)
        {
            this.Id = c.Id;
            this.Weight = c.Weight;
            this.Text =_context.Categories.Where(x => x.Id == this.Id).First().Title;
        }
    }
}
