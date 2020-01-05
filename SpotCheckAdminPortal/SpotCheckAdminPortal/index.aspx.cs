using System;
using System.Web;
using System.Web.UI;
using SpotCheckAdminPortal.Models;

namespace SpotCheckAdminPortal
{
    public partial class index : System.Web.UI.Page
    {
        private bool? _loginResult;
        public bool? LoginResult
        {
            get
            {
                return _loginResult;
            }
            set
            {
                _loginResult = value;
                switch (_loginResult)
                {
                    case null:
                        //show error message
                        ErrorLabel.Text = "There was an error logging in. Try again later.";
                        break;
                    case false:
                        //show incorrect password message
                        ErrorLabel.Text = "Incorrect Username or Password.";
                        break;
                    case true:
                        //open new page
                        ErrorLabel.Text = "";
                        Response.Redirect("Dashboard.aspx");
                        break;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //on page load, assume we have logged out and clear the global variables
            Company company = new Company();
            IoC.CurrentCompany = company;
        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            Company company = new Company
            {
                CompanyUsername = TextBoxCompanyUsername.Text,
                CompanyPassword = TextBoxCompanyPassword.Text
            };

            LoginResult = company.Login();
        }

        protected void SignUpInsteadButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("Signup.aspx");
        }
    }
}
