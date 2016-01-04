using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuestionnairePrototype.Models.Questionnaire
{
    public class AFFIHeaderModel
    {
        public string FacilityName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public bool WasEverSubmitted { get; set; }
        public DateTime lastSubmitted { get; set; }

        public AFFIHeaderModel()
        {
        }

        public AFFIHeaderModel(string _facilityName, string _name, string _email, bool _wasEverSubmitted, DateTime _lastSubmitted)
        {
            FacilityName = _facilityName;
            Name = _name;
            Email = _email;
            WasEverSubmitted = _wasEverSubmitted;
            lastSubmitted = _lastSubmitted;
        }
    }
}
