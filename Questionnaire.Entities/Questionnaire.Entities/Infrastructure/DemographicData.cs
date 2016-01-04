using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Questionnaire.Entities.Infrastructure
{
    public interface DemographicData
    {
        FacilitySize? SizeOfFacility { get; set; }
        IndustrialClassification? IndustrialClassification { get; set; }
        AnotherProductClassification? AnotherProductClassification { get; set; }
        AdditionalProductClassification? AdditionalProductClassification { get; set; }
    }
}
