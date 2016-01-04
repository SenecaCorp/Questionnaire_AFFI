using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Questionnaire.Entities
{
    public class RiskRange
    {
        public int Id { get; set; }
        
        public int RiskTypeId { get; set; }
       
        public string Comment { get; set; }

        [RegularExpression("\\d+([,.]\\d+)?", ErrorMessage = "Weight must be between 0 and 1, e.g. 0.42.")]
        [Range(0.0, 1.0, ErrorMessage = "Weight must be between 0 and 1, e.g. 0.42.")]
        public double MaxValue { get; set; }

        public int CategoryId { get; set; }

        public RiskRange(int riskTypeId)
        {
            this.RiskTypeId = riskTypeId;
        }

        public RiskRange()
        {
        }

    }
}