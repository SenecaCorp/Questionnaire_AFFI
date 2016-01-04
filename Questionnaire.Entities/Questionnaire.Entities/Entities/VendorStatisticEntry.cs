using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Questionnaire.Entities
{
    public class VendorStatisticsEntry
    {
        public const string CLICK_TYPE = "CLICK";
        public const string CONTACT_ME_TYPE = "CONTACT_ME";

        
        public int Id { get; set; }
        public int UserId { get; set; }
        public int VendorId { get; set; }
        public int CategoryId { get; set; }
        public string Type { get; set; }
        public DateTime OccurredAt { get; set; }
    }
}