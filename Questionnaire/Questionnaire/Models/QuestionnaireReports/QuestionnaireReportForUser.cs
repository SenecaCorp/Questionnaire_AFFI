using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuestionnairePrototype.Models.Report;
using Questionnaire.Entities;

namespace QuestionnairePrototype.Models.Report
{

    public class QuestionnaireReportForUser
    {
        public QuestionnaireReport Report { get; set; }
        public UserPurchase UserPurchase { get; set; }

        public QuestionnaireReportForUser(QuestionnaireReport report, UserPurchase user)
        {
            this.Report = report;
            this.UserPurchase = user;
        }
    }
}