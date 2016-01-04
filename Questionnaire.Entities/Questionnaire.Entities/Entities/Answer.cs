using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Questionnaire.Entities
{
    public class Answer
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }
        public int Value { get; set; }
        public string Comment { get; set; }
        public int Index { get; set; }
        public int QuestionId { get; set; }
        
        [NotMapped]
        public bool IsSelected { get; set; }

        public Answer()
        {
            IsSelected = false;
        }
    }
}