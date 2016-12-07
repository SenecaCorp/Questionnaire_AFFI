using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Questionnaire.Webservices;

namespace Questionnaire.Webservices
{
    public partial class Tester : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            UserService x = new UserService();

            //fsma-tool-en

            //fsma-tool-es

            x.createUserAccount("spanish Test", "champsq@gmail.com", "Test Facility", "jchampion@seneca.com", "fsma-tool-en");
        }
    }
}