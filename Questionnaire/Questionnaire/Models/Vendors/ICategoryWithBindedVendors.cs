using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Questionnaire.Entities;

namespace QuestionnairePrototype.Models.Vendors
{
    public interface ICategoryWithBindedVendors
    {
        int Id { get; }
        string Title { get; }
        IList<Vendor> BindedVendors{ get; }
        IList<Vendor> VendorsCanBeBinded { get; }
    }
}
