using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data.SqlClient;
using System.Web.Security;
using System.Web.Configuration;
using QuestionnairePrototype.Models.Membership;

using System.Net.Mail;
using Questionnaire.Mailing.Infrastructure;


namespace Questionnaire.Webservices
{
    /// <summary>
    /// Summary description for UserService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class UserService : System.Web.Services.WebService
    {

        /// <summary>
        /// Create New User Account. Will return true if successful, false if unsuccessful
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="facility"></param>
        /// <param name="emailSecondary"></param>
        /// <returns></returns>
        [WebMethod]
        public bool createUserAccount(string name, string email, string facility, string emailSecondary, string product)
        {
            bool success;
            
            try
            {
                //DEFAULT TO AFFI ENGLISH SITE
                Product.ProudctType productType = Product.ProudctType.AffiEnglish;

                //AFFI SPANISH SITE
                if (product.ToLower() == "fsma-tool-es")
                {
                    productType = Product.ProudctType.AffiSpanish;
                }
                                
                var encryptedPassword = Account.createPassword(facility);

                success = Account.createAccount(name, email, facility, emailSecondary, encryptedPassword, productType);

                if (!success)
                    throw new Exception("Error Creating User Account for Email: " + email);

                //Send email to User
                Mail.sendEmailToUser(email, productType);

                //If second email is different from primary, send email to second email
                if (!String.IsNullOrEmpty(emailSecondary) && email != emailSecondary)
                {
                    Mail.sendEmailToUser(emailSecondary, productType);
                }

                //Send email to AffI
                Mail.sendEmailToAffi(productType);

            }
            catch (Exception ex)
            {
                Mail.sendErrorEmail(ex.InnerException.ToString());
                
                success = false;
            }            

            return success;
           
        }




    }
}
