using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Questionnaire.Entities
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Annotation { get; set; }
        public int Index { get; set; }

        [Range(1, 100)]
        public int Weight { get; set; }
        
        public bool ImageDisplayFlag { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<RiskRange> RiskRanges { get; set; }
    }
}