using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Questionnaire.Entities;
using QuestionnairePrototype.Models.Membership;

namespace QuestionnairePrototype.Models.Questionnaire
{
    public class QuestionnaireWithSelectedAnswersForUser
    {
        private QuestionnaireContext _context = new QuestionnaireContext();

        public QuestionnaireWithSelectedAnswers Questionnaire { get; private set; }
        public UserData UserData { get; private set; }
        public DemographicDataContainer DemographicData { get; private set; }
        
        public bool WasEverSubmitted { get; private set; }
        public DateTime lastSubmitted { get; private set; }

        public QuestionnaireWithSelectedAnswersForUser(QuestionnaireWithSelectedAnswers questionniare, UserPurchase user)
        {
            this.Questionnaire = questionniare;

            UserData = new UserData();
            UserData.Email = user.Email;
            UserData.Name = user.Name; 
            UserData.FacilityName = user.FacilityName;

            DemographicData = new DemographicDataContainer();
            DemographicData.SizeOfFacility = user.SizeOfFacility;
            DemographicData.IndustrialClassification = user.IndustrialClassification;
            DemographicData.AdditionalProductClassification = user.AdditionalProductClassification;
            DemographicData.AnotherProductClassification = user.AnotherProductClassification;

            SubmittedStatusForUser status = _context.SubmittedeStatusForUsers.SingleOrDefault(x => x.UserId == user.Id);
            WasEverSubmitted = (status != null);
            if (status != null)
            {
                lastSubmitted = status.SubmitedDate;
            }
        }
    }
}