using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Questionnaire.Entities;
using Questionnaire.Entities.Infrastructure;

namespace QuestionnairePrototype.Models.Questionnaire
{
    public class QuestionnaireWithSelectedAnswers
    {
        public QuestionnaireWithSelectedAnswers(IList<Category> categories)
        {
            _categories = categories;
        }

        public QuestionnaireWithSelectedAnswers(IList<Category> categories, IDictionary<int, int> selectedAnswers)
        {
            _categories = categories;
            foreach (var category in _categories)
            {
                foreach (var question in category.Questions)
                {
                    try
                    {
                        int selectedAnswer = selectedAnswers[question.Id];
                        Answer answer = (from x in question.Answers.OfType<Answer>() where x.Id == selectedAnswer select x).FirstOrDefault();
                        answer.IsSelected = true;
                    }
                    catch
                    {
                        continue;
                    }
                }
            }          
        }

        private IList<Category> _categories;

        public IList<Category> Categories {
            get {
                return this._categories;
            }
        }
    }
}