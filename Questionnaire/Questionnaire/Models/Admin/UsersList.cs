using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Questionnaire.Entities;

namespace QuestionnairePrototype.Models.Admin
{
    public class UsersList
    {
        private QuestionnaireContext _context = new QuestionnaireContext();
        private List<UserEntry> _users = new List<UserEntry>();
        
        public UsersList()
        {
            var list = _context.SubmittedeStatusForUsers.ToList();
            foreach (var user in _context.UserPurchases)
            {
                    SubmittedStatusForUser completeStatus = (from x in list.OfType<SubmittedStatusForUser>() where x.UserId == user.Id select x).FirstOrDefault();
                    var userEntry = new UserEntry
                    {
                        User = user,
                        WasCompleteQuestionnaire = (completeStatus != null)
                    };
                    _users.Add(userEntry);
                    
            }
            _users = _users.OrderBy(x => x.User.Name).ToList();
        }

        public IList<UserEntry> Users
        {
            get
            {
                return _users;
            }
        }
    }

    public class UserEntry
    {
        public UserPurchase User;
        public bool WasCompleteQuestionnaire;
    }
}