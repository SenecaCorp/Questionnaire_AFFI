using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Web.Services.Protocols;
using Dashboard.Models;

namespace serviceutils
{
    public class AvectraDataProvider : IPurchasedItemsProvider
    {
        private string authName;
        private string authPassword;
        private Dashboard.avectra1.netForumXMLOnDemand adapter;

        public AvectraDataProvider(string _authName, string _authPassword)
        {
            authName = _authName;
            authPassword = _authPassword;
            try
            {
                authenticate();
            }
            catch (SoapException e)
            {
                throw new AvectraFetchingDataException("Authentication error." + e.StackTrace);
            }
        }

        private void authenticate()
        {
            Console.WriteLine("Authorizating... Username: " + authName + " Password: " + authPassword);
            adapter = new Dashboard.avectra1.netForumXMLOnDemand();
            adapter.Authenticate(authName, authPassword);
            
            Console.WriteLine("Authorization complete. Token value: " + adapter.AuthorizationTokenValue.Token);
        }

        public IList<PurchasedItem> getPurchasedItems(string userName, string password)
        {
            //Obtain information about user
            XmlNode response = null;
            try
            {
                response = adapter.CheckEWebUser(userName, password);
            }
            catch
            {
                throw new AvectraFetchingDataException("Timed out. Check your Internet connection.");
            }

            Console.WriteLine("Checking web user. Username: " + userName + " Password: " + password);
            Console.WriteLine("Check web user response: " + response.OuterXml);
            //If there is no information about user, throw exception
            if (response == null || response.InnerXml == "")
                throw new AvectraFetchingDataException("Invalid userid or password.");

            //If user doesn't have any purhases - return empty list of facilities
            if (response.FirstChild == null)
                return null;

            //If user has purchases - get user's first purchase's cst_key
            string userKey = String.Empty;

            //Also we remember user's email and full name to put them inside each entry. 
            string userEmail = String.Empty;
            string userFullName = String.Empty;

            XmlNodeList FirstResultNodes = response.FirstChild.ChildNodes;
            foreach (XmlNode node in FirstResultNodes)
            {
                switch (node.Name)
                {
                    case "cst_key":
                        userKey = node.InnerText;
                        break;

                    case "eml_address":
                        userEmail = node.InnerText;
                        break;

                    case "cst_name_cp":
                        userFullName = node.InnerText;
                        break;
                }
            }

            //Check whether userKey string is empty
            if (String.IsNullOrEmpty(userKey))
                throw new AvectraFetchingDataException("Error getting user key for user.");

            //Get customer event for user for all time
            XmlNode purchasesRawData = adapter.GetActiveProductListByIndividual(userKey, "1/1/1900");

            //Loop through result and fill list with data about facilities. 
            List<PurchasedItem> records = new List<PurchasedItem>();
            PurchasedItem userRecord;
            foreach (XmlNode node in purchasesRawData)
            {
                userRecord = new PurchasedItem();
                userRecord.Email = String.Empty;//userEmail;
                userRecord.Name = String.Empty;//userFullName;

                foreach (XmlNode child in node.ChildNodes)
                {
                    switch (child.Name)
                    {
                        case "prd_name":
                            userRecord.Facility = String.Empty;//child.InnerText;
                            break;

                        case "add_date":
                            userRecord.DateOfPurchase = DateTime.Parse(child.InnerText);
                            break;
                    }
                }
                records.Add(userRecord);
            }

            return records;
        }
    }
}