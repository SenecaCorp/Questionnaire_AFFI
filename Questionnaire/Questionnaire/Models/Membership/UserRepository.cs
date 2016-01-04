using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Configuration;
using Questionnaire.Entities;

namespace QuestionnairePrototype.Models.Membership
{
    public class UserRepository : IUserRepository
    {
        public static string INVALID_USERNAME_OR_PASSWORD = "Incorrect user name or password.";
        public static string SUBSCRIPTION_IS_EXPIRED = "Your subscription has expired.";

        private QuestionnaireContext _context = new QuestionnaireContext();

        public string CreateNewUser(SignUpModel model)
        {
            if (IsNewUserValid(model))
            {
                var salt = GenerateSequenceOfChars(20);
                // TODO: check facility name length
                var password = GeneratePassword(model.FacilityName);

                var user = new UserPurchase
                {
                    Email = model.Email,
                    PasswordSalt = salt,
                    Password = HashPassword(password, salt),
                    FacilityName = model.FacilityName,
                    UserRegistrationDate = DateTime.Now,
                    UserExpirationDate = DateTime.Now.AddMonths(6)
                };

                _context.UserPurchases.Add(user);
                _context.SaveChanges();

                return password;
            }

            return null;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        private string GeneratePassword(string facilityName)
        {
            return facilityName.Substring(0, 3) + GenerateSequenceOfChars(5);
        }

        public UserPurchase GetUserByEmail(string email, string hash)
        {
            var users = _context.UserPurchases.Where(x => x.Email == email);
            string calculatedHash;
            foreach (UserPurchase user in users)
            {
                calculatedHash = user.Id + user.DateOfPurchase.ToString();
                if (calculatedHash == hash)
                {
                    return user;
                }
            }

            return null;
        }

        public UserPurchase GetUserById(int userId)
        {
            return _context.UserPurchases.Find(userId);
        }

        public IList<UserPurchase> getUsers()
        {
            return _context.UserPurchases.ToList<UserPurchase>();
        }

        public bool ValidateUser(string login, string password, ref string error, ref string hash)
        {
            var users = _context.UserPurchases.Where(x => x.Email == login);
            if (users == null)
            {
                error = INVALID_USERNAME_OR_PASSWORD;
                return false;
            }

            foreach (UserPurchase user in users)
            {
                var salt = user.PasswordSalt;
                var pw1 = user.Password;
                var pw2 = HashPassword(password, salt);

                int result = DateTime.Compare(user.UserExpirationDate, DateTime.Now);

                if (pw1 == pw2)
                {
                    if (result < 0)
                    {
                        error = SUBSCRIPTION_IS_EXPIRED;
                        return false;
                    }
                    else
                    {
                        //hash = user.FacilityName + user.DateOfPurchase.ToString();
                        hash = user.Id + user.DateOfPurchase.ToString();
                        return true;
                    }
                }
            }

            error = INVALID_USERNAME_OR_PASSWORD;
            return false;
        }

        private Boolean IsNewUserValid(SignUpModel model)
        {
            // TODO: implement
            return true;
        }

        private static string GenerateSequenceOfChars(int size)
        {
            var rng = new Random();
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

            var buffer = new char[size];

            for (var i = 0; i < size; i++)
            {
                buffer[i] = chars[rng.Next(chars.Length)];
            }

            return new string(buffer);
        }

        public static string HashPassword(string password, string salt)
        {
            var pw = salt + password;
            return FormsAuthentication.HashPasswordForStoringInConfigFile(pw, FormsAuthPasswordFormat.SHA1.ToString());
        }
    }

    public interface IUserRepository
    {
        string CreateNewUser(SignUpModel model);
        bool ValidateUser(string login, string password, ref string error, ref string hash);
    }
}