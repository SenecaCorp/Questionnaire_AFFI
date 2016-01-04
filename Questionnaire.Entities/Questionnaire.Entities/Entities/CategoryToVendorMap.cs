using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Questionnaire.Entities
{
    public class CategoryToVendorMap
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int VendorId { get; set; }
        public int Index { get; set; }
    }
}