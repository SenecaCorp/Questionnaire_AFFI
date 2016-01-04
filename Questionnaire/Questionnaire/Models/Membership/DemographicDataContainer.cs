using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Questionnaire.Entities.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace QuestionnairePrototype.Models.Membership
{
    public class DemographicDataContainer : DemographicData
    {
        [Required]
        [Display(Name = "Size of Facility")]
        public FacilitySize? SizeOfFacility { get; set; }
        
        [Required]
        [Display(Name = "Industrial classification")]
        public IndustrialClassification? IndustrialClassification { get; set; }

        [Display(Name = "Another Product classification")]
        public AnotherProductClassification? AnotherProductClassification { get; set; }

        [Display(Name = "Additional Product classification")]
        public AdditionalProductClassification? AdditionalProductClassification { get; set; }
    }
}