using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace QuestionnairePrototype.Models.Membership
{
    public class SignUpModel
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "Email")]
        [RegularExpression(@"^[a-zA-Z0-9_.@]*$", ErrorMessage = "Invalid username.")]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Facility name")]
        public string FacilityName { get; set; }//TODO: check that FacilityName is longer that 3 character
    }
}