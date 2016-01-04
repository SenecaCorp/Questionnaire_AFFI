using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Questionnaire.Entities
{
    public class Vendor
    {
        public Vendor()
        {
            Deleted = false;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string LinkToWebsite { get; set; }
        public string LinkDisplayText { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public bool Deleted { get; set; }
    }
}