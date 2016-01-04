using System;
using System.Linq;
using Questionnaire.Entities;

namespace QuestionnairePrototype.Models.Report
{
    public class OverallRecomendation
    {
        private double categoriesValueSum = 0;
        private double categoriesMaxValueSum = 0;
        private double overallScore = 0;

        private string riskTypeName;
        private string recomendationText;

        public double OverallScore
        {
            get
            {
                return overallScore;
            }
        }

        public string RiskTypeName
        {
            get
            {
                return riskTypeName;
            }
        }

        public string RecomendationText
        {
            get
            {
                return recomendationText;
            }
        }

        internal void AddCategoryScore(double categoryValueSum, double categoryMaxValueSum, double categoryWeight)
        {
            categoriesValueSum += (categoryValueSum / categoryMaxValueSum) * categoryWeight;
            categoriesMaxValueSum +=  categoryWeight;
        }

        private QuestionnaireContext context = new QuestionnaireContext();

        internal void CalculateScore()
        {
            if(categoriesMaxValueSum == 0)
                throw new Exception("Empty category");

            overallScore = categoriesValueSum / categoriesMaxValueSum;

            OverallRange overallRange = getOverallRange();

            if(overallRange == null)
                throw new Exception("Out of Range");

            riskTypeName = context.RiskTypes.Find(overallRange.RiskTypeId).RiskName;
            recomendationText = overallRange.Comment;
        }

        private OverallRange getOverallRange()
        {
            //TODO: use method OrderBy
            foreach (OverallRange item in context.OverallRanges.ToList())
            {
                if (overallScore <= item.MaxValue)
                    return item;
            }
            return null;
        }

    }
}