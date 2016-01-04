using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Questionnaire.Entities
{
    public class OverallRange
    {
        public int Id { get; set; }
        public int RiskTypeId { get; set; }
        public string Comment { get; set; }
        public double MaxValue { get; set; }
    }
}