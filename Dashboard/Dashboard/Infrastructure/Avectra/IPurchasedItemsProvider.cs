using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dashboard.Models;

namespace serviceutils
{
    public interface IPurchasedItemsProvider
    {
        IList<PurchasedItem> getPurchasedItems(string userName, string password);
    }
}