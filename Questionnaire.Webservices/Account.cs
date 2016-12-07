using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using QuestionnairePrototype.Models.Membership;

namespace Questionnaire.Webservices
{
    public class Account
    {

        // datasource Connection
        private static string DBConnAFFIEnglish = System.Configuration.ConfigurationManager.ConnectionStrings["QuestionnaireEnglish"].ToString();
        private static string DBConnAFFISpanish = System.Configuration.ConfigurationManager.ConnectionStrings["QuestionnaireSpanish"].ToString();

        internal static string Name { get; set; }
        internal static string EmailForUserId { get; set; }
        internal static string Facility { get; set; }
        internal static string Password { get; set; }
        internal static string Salt { get; set; }
        


        //Create a random Password
        internal static string createPassword(string facility)
        {
            Salt = UserRepository.GenerateSequenceOfChars(20);

            Password = UserRepository.GeneratePassword(facility);

            return UserRepository.HashPassword(Password, Salt);
        }


        internal static bool createAccount(string name, string email, string facility, string emailSecondary, string encryptedPassword,  Product.ProudctType productType)
        {
            bool successful = true;

            Name = name;

            //Set primary email address (which is used for the login)
            EmailForUserId = email;

            //If secondaryemail is blank, set to same as primary email
            if (String.IsNullOrEmpty(emailSecondary))
            {
                emailSecondary = email;
            }

            Facility = facility;
            
            string sql = @"Insert Into UserPurchases (Email, ActivatorsEmail, Password, FacilityName, PasswordSalt, Name, DateOfPurchase, UserRegistrationDate, UserExpirationDate, IsAdmin )

                        Values ('" + email + @"', '" + emailSecondary + @"', '" + encryptedPassword + "', '" + facility.Replace("'", "''") + @"',
                                '" + Salt + "', '" + name.Replace("'", "''") + @"', '" + DateTime.Now + @"',   '" + DateTime.Now + @"',   '" + DateTime.Now.AddMonths(6) + @"', 0 )";

            try
            {
                //AFFI ENGLISH SITE
                string DBConnection = DBConnAFFIEnglish;

                //AFFI SPANISH SITE
                if (productType == Product.ProudctType.AffiSpanish)
                {
                    DBConnection = DBConnAFFISpanish;
                }

                using (var sqlConnLocal = new SqlConnection(DBConnection))
                {
                    sqlConnLocal.Open();
                    using (var myCommand = new SqlCommand(sql, sqlConnLocal))
                    {
                        myCommand.CommandTimeout = 200;
                        var result = myCommand.ExecuteScalar();
                    }
                }
                

                
            }
            catch (Exception ex)
            {
                successful = false;

                Mail.sendErrorEmail(ex.Message);
            }
            return successful;
        }



    }
}