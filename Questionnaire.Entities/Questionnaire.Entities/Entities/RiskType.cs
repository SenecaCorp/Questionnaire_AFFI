using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Questionnaire.Entities
{
    public class RiskType
    {
        public int Id { get; set; }
        public string RiskName { get; set; }
        public string Color { get; set; }
    }
}