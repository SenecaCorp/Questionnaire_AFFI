using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Questionnaire.Entities
{
    public class Question
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Summary { get; set; }
        public string PostAnnotation { get; set; }
        public int Index { get; set; }
        public int CategoryId { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
    }
}