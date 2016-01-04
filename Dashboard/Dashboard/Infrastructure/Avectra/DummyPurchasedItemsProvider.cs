using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dashboard.Models;

namespace Dashboard.Infrastructure.Avectra
{
    public class DummyPurchasedItemsProvider : IPurchasedItemsProvider
    {
        public IList<PurchasedItem> getPurchasedItems(string userKey)
        {
            IList<PurchasedItem> items = new List<PurchasedItem>();

            items.Add(new PurchasedItem
            {
                Name = "TestName",
                Facility = "Test Faciltity",
                Email = "pshenichniy.eugene@gmail.com",
                DateOfPurchase = DateTime.Now.AddDays(-4)
            });
            items.Add(new PurchasedItem
            {
                Name = "TestName",
                Facility = "Test Faciltity 1",
                Email = "pshenichniy.eugene@gmail.com",
                DateOfPurchase = DateTime.Now.AddDays(-8)
            });

            return items;
        }
    }
}