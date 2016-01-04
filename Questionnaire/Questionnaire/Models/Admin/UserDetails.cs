using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Questionnaire.Entities;
using QuestionnairePrototype.Models.Report;

namespace QuestionnairePrototype.Models.Admin
{
    public class UserDetails
    {
        public UserPurchase UserPurchase { internal set; get; }
        public QuestionnaireReport QuestionnaireReport { internal set; get; }

        public UserDetails(UserPurchase user, QuestionnaireReport questionnaireReport)
        {
            this.UserPurchase = user;
            this.QuestionnaireReport = questionnaireReport;
        }
    }
}