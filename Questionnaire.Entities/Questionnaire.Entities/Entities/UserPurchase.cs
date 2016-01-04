using Questionnaire.Entities.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Questionnaire.Entities
{
    public class UserPurchase : DemographicData
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string ActivatorsEmail { get; set; }

        [Required]
        [StringLength(40)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [StringLength(100)]
        public string FacilityName { get; set; }

        [StringLength(20)]
        public string PasswordSalt { get; set; }

        public string Name { get; set; }

        public DateTime DateOfPurchase { get; set; }
        public DateTime UserRegistrationDate { get; set; }
        public DateTime UserExpirationDate { get; set; }

        public FacilitySize? SizeOfFacility {
            get { return EnumHelper.ParseEnum<FacilitySize?>(SizeOfFacilityValue); }
            set { SizeOfFacilityValue = EnumHelper.EnumName(value); }
        }
        public string SizeOfFacilityValue { get; private set; }

        public IndustrialClassification? IndustrialClassification {
            get { return EnumHelper.ParseEnum<IndustrialClassification?>(IndustrialClassificationValue); }
            set { IndustrialClassificationValue = EnumHelper.EnumName(value); }
        }
        public string IndustrialClassificationValue { get; private set; }

        public AnotherProductClassification? AnotherProductClassification {
            get { return EnumHelper.ParseEnum<AnotherProductClassification?>(AnotherProductClassificationValue); }
            set { AnotherProductClassificationValue = EnumHelper.EnumName(value); }
        }
        public string AnotherProductClassificationValue { get; private set; }

        public AdditionalProductClassification? AdditionalProductClassification {
            get { return EnumHelper.ParseEnum<AdditionalProductClassification?>(AdditionalProductClassificationValue); }
            set { AdditionalProductClassificationValue = EnumHelper.EnumName(value); }
        }
        public string AdditionalProductClassificationValue { get; private set; }

        public Boolean UserIsDeleted { get; set; }

        public Boolean IsAdmin { get; set; }

        public Boolean OverallAdmin { get; set; }
    }
}