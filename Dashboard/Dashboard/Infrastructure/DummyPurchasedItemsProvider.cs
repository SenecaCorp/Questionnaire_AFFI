using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dashboard.Models;
using serviceutils;

namespace Dashboard.Infrastructure.Avectra
{
    public class DummyPurchasedItemsProvider : IPurchasedItemsProvider
    {
        private string email;

        public DummyPurchasedItemsProvider(string email)
        {
            this.email = email;
        }

        public IList<PurchasedItem> getPurchasedItems(string userName, string password)
        {
            IList<PurchasedItem> items = new List<PurchasedItem>();

            for (int i = 0; i < 5; ++i)
            {
                items.Add(new PurchasedItem
                {
                    Name = "TestName",
                    Facility = "Test Faciltity" + i.ToString(),
                    Email = email,
                    DateOfPurchase = DateTime.Now.AddMonths(-2 * i - 1)
                });
            }

            return items;
        }
    }
}