using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using SpotCheckAdminPortal.Models;

namespace SpotCheckAdminPortal
{

    public partial class Signup : System.Web.UI.Page
    {
        #region Properties

        readonly Array states = new string[] { "Select State", "Alaska", "Alabama", "Arkansas", "American Samoa", "Arizona", "California", "Colorado", "Connecticut", "District of Columbia", "Delaware", "Florida", "Georgia", "Guam", "Hawaii", "Iowa", "Idaho", "Illinois", "Indiana", "Kansas", "Kentucky", "Louisiana", "Massachusetts", "Maryland", "Maine", "Michigan", "Minnesota", "Missouri", "Mississippi", "Montana", "North Carolina", "North Dakota", "Nebraska", "New Hampshire", "New Jersey", "New Mexico", "Nevada", "New York", "Ohio", "Oklahoma", "Oregon", "Pennsylvania", "Puerto Rico", "Rhode Island", "South Carolina", "South Dakota", "Tennessee", "Texas", "Utah", "Virginia", "Virgin Islands", "Vermont", "Washington", "Wisconsin", "West Virginia", "Wyoming" };
        private bool? _signUpResult;
        public bool? SignUpResult
        {
            get
            {
                return _signUpResult;
            }
            set
            {
                _signUpResult = value;
                switch (_signUpResult)
                {
                    case null:
                        //show error message
                        ErrorLabel.Text = "There was an error signing up. Try again later.";
                        break;
                    case false:
                        //show incorrect password message
                        ErrorLabel.Text = "Company already exists with specified username.";
                        break;
                    case true:
                        //open new page
                        ErrorLabel.Text = "";
                        Response.Redirect("index.aspx");
                        break;
                }
            }
        }

        #endregion Properties

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            DropDownState.DataSource = states;
            DropDownState.DataBind();
        }

        protected void LoginInsteadButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("index.aspx");
        }

        protected void SignUpButton_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                Company company = new Company()
                {
                    CompanyUsername = TextBoxCompanyUsername.Text,
                    CompanyName = TextBoxCompanyName.Text,
                    Address = TextBoxCompanyStreet.Text,
                    City = TextBoxCompanyCity.Text,
                    ZipCode = TextBoxCompanyZipCode.Text,
                    State = DropDownState.SelectedValue.ToString(),
                    CompanyPassword = TextBoxCompanyPassword1.Text
                };

                SignUpResult = company.SignUp();
            }
        }

        #endregion Events

        #region Methods

        private bool ValidateInput()
        {
            //validate zipcode
            string zipCode = TextBoxCompanyZipCode.Text;
            foreach(char c in zipCode.ToCharArray())
            {
                if (!Char.IsDigit(c))
                {
                    return false;
                    //show specific error message for zip code
                }
            }
            if(zipCode.Length != 5)
            {
                return false;
                //show specific error message for zip code
            }
            if(DropDownState.SelectedIndex == 0)
            {
                return false;
                //show specific error message for state
            }
            string password1 = TextBoxCompanyPassword1.Text;
            string password2 = TextBoxCompanyPassword2.Text;

            if(password1 != password2)
            {
                return false;
                //show specific error message for password
            }

            //if all pass then return true
            return true;
        }

        #endregion Methods
    }
}
