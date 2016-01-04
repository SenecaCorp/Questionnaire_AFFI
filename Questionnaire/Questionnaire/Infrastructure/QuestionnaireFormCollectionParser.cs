using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using QuestionnairePrototype.Models.Questionnaire;
using QuestionnairePrototype.Models.Membership;
using Questionnaire.Entities.Infrastructure;

namespace QuestionnairePrototype.Controllers
{
    class QuestionnaireFormCollectionParser
    {
        public Dictionary<int, int> QuestionAnswerMapping { internal set; get; }
        public DemographicData DemographicData { internal set; get; }

        public QuestionnaireFormCollectionParser(FormCollection formCollection)
        {
            QuestionAnswerMapping = new Dictionary<int, int>();
            DemographicData = new DemographicDataContainer();
            foreach (string _formData in formCollection.AllKeys)
            {
                try
                {
                    int questionId = int.Parse(_formData);
                    int answerId = int.Parse(formCollection[_formData]);
                    QuestionAnswerMapping.Add(questionId, answerId);
                }
                catch
                {
                    string value = formCollection[_formData];
                    switch (_formData)
                    {
                        case "DemographicData.SizeOfFacility":
                            DemographicData.SizeOfFacility = 
                                EnumHelper.ParseEnum<FacilitySize?>(value);
                            break;
                        case "DemographicData.IndustrialClassification":
                            DemographicData.IndustrialClassification = 
                                EnumHelper.ParseEnum<IndustrialClassification?>(value);
                            break;
                        case "DemographicData.AnotherProductClassification":
                            DemographicData.AnotherProductClassification = 
                                EnumHelper.ParseEnum<AnotherProductClassification?>(value);
                            break;
                        case "DemographicData.AdditionalProductClassification":
                            DemographicData.AdditionalProductClassification =
                                EnumHelper.ParseEnum<AdditionalProductClassification?>(value);
                            break;
                    }
                }
            }
        }
    }
}
