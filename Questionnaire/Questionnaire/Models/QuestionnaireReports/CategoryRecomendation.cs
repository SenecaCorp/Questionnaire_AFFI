using System;
using System.Collections.Generic;
using System.Linq;
using Questionnaire.Entities;

namespace QuestionnairePrototype.Models.Report
{
    public class CategoryRecomendation
    {
        public int categoryId;

        private double questionsValueSum = 0;
        private double questionsMaxValueSum = 0;
        private double weight;
        private int index;

        private double categoryScore;
        private string categoryTitle;

        private string categoryComment;
        private string riskTypeName;

        private bool dislayImage;

        private List<QuestionRecomendation> questionRecomendations;
        public List<Vendor> vendorList;

        internal int CategoryId
        {
            get
            {
                return categoryId;
            }
        }

        public int Index
        {
            get
            {
                return index;
            }
        }

        public double Weight
        {
            get
            {
                return weight;
            }
        }

        public string CategoryComment
        {
            get
            {
                return categoryComment;
            }
        }

        public string CategoryTitle
        {
            get
            {
                return categoryTitle;
            }
        }

        public string RiskTypeName
        {
            get
            {
                return riskTypeName;
            }
        }

        public double CategoryScore
        {
            get
            {
                return categoryScore;
            }
        }

        public double QuestionsValueSum
        {
            get
            {
                return questionsValueSum;
            }
        }

        public double QuestionsMaxValueSum
        {
            get
            {
                return questionsMaxValueSum;
            }
        }

        public bool DisplayImage
        {
            get
            {
                return dislayImage;
            }
        }

        public List<QuestionRecomendation> QuestionRecomendations
        {
            get
            {
                return questionRecomendations;
            }
        }

        private QuestionnaireContext context = new QuestionnaireContext();

        public CategoryRecomendation(int categoryId)
        {
            this.categoryId = categoryId;
            
            var vendors = context.Vendors.Join(context.CategoryToVendorMaps, v=>v.Id, cvm=>cvm.VendorId,
                (v,cvm) => new {v,cvm.CategoryId, cvm.Index}).Where(c => c.CategoryId == categoryId).OrderBy(x => x.Index);

            vendorList = new List<Vendor>();
            foreach (var vendor in vendors)
            {
                vendorList.Add(vendor.v);
            }

            questionRecomendations = new List<QuestionRecomendation>();

            var category = context.Categories.Find(categoryId);

            weight = category.Weight;
            categoryTitle = category.Title;
            dislayImage = category.ImageDisplayFlag;
            index = category.Index;
        }

        public void calculateScore()
        {
            foreach (QuestionRecomendation question in questionRecomendations)
            {
                questionsValueSum += question.checkedValue;
                questionsMaxValueSum += question.maxValue;
            }

            if (questionsMaxValueSum == 0.0)
                throw new Exception("category is emty");

            categoryScore = questionsValueSum / questionsMaxValueSum;

            RiskRange riskRange = getRiskRange();

            if (riskRange == null)
                throw new Exception("out of risk range");

            riskTypeName = context.RiskTypes.Find(riskRange.RiskTypeId).RiskName;
            categoryComment = riskRange.Comment;
        }

        private RiskRange getRiskRange()
        {
            //TODO: use method OrderBy
            foreach (RiskRange item in context.Categories.Find(CategoryId).RiskRanges.ToList())
            {
                if (categoryScore <= item.MaxValue)
                    return item;
            }
            return null;
        }
    }
}