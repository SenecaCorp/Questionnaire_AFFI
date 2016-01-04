using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Questionnaire.Entities;

namespace QuestionnairePrototype.Models.Report
{
    public class QuestionnaireReport
    {
        //TODO[REFACTOR]: replace list to hashmap
        private List<CategoryRecomendation> categories = new List<CategoryRecomendation>();
        private OverallRecomendation overall = new OverallRecomendation();

        public IList<Vendor> OverallVendors { internal set; get; }

        public OverallRecomendation Overall
        {
            get
            {
                calculateScoreIfNeeded();
                return overall;
            }
        }

        public List<CategoryRecomendation> Categories
        {
            get
            {
                calculateScoreIfNeeded();
                return categories;
            }
        }

        private bool scoreCalculated = false;

        private void calculateScoreIfNeeded()
        {
            if (!scoreCalculated)
                calculateScore();
        }

        //TODO[CATCH EXCEPTION]
        public void calculateScore()
        {
            foreach (CategoryRecomendation category in categories)
            {
                category.calculateScore();
                overall.AddCategoryScore(category.QuestionsValueSum,
                                         category.QuestionsMaxValueSum,
                                         category.Weight);
            }
            overall.CalculateScore();
            scoreCalculated = true;
        }

        public QuestionnaireReport(Dictionary<int, int> questionAnsverMapping)
        {
            QuestionRecomendation newQuestion;
            foreach (int questionId in questionAnsverMapping.Keys)
            {
                newQuestion = new QuestionRecomendation(questionId, questionAnsverMapping[questionId]);
                AddQuestion(newQuestion);
            }
        }

        private QuestionnaireContext context = new QuestionnaireContext();

        public void AddQuestion(QuestionRecomendation questionRecomendations)
        {
            scoreCalculated = false;
            
            try
            {
                int categoryId = context.Questions.Find(questionRecomendations.questionId).CategoryId;
                CategoryRecomendation parentCategory = null;

                foreach (CategoryRecomendation category in categories)
                {
                    if (category.CategoryId == categoryId)
                    {
                        parentCategory = category;
                        break;
                    }
                }

                if (parentCategory == null)
                {
                    parentCategory = new CategoryRecomendation(categoryId);
                    categories.Add(parentCategory);
                }

                parentCategory.QuestionRecomendations.Add(questionRecomendations);

                OverallVendors = context.Vendors.Join(context.OverallCategoryVendorEntries,
                                    v => v.Id,
                                    ocve => ocve.VendorId,
                                    (v, ocve) => v
                                 ).ToList();
            }
            catch
            {
                throw new ArgumentException("Unknown question category");
            }
        }
    }
}