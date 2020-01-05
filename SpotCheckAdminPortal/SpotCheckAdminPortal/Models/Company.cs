using System;
using SpotCheckAdminPortal.DataLayer;

namespace SpotCheckAdminPortal.Models
{
    public class Company
    {
        #region Properties

        public int? CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string CompanyUsername { get; set; }
        public string CompanyPassword { get; set; }

        #endregion

        #region Constructors

        public Company()
        {
            this.CompanyID = null;
            this.CompanyName = null;
            this.Address = null;
            this.ZipCode = null;
            this.City = null;
            this.State = null;
            this.CompanyUsername = null;
            this.CompanyPassword = null;
        }

        #endregion

        #region Methods

        public bool? Login()
        {
            Company_dl company_dl = new Company_dl(this);
            return company_dl.Login();
        }

        public bool? SignUp()
        {
            Company_dl company_dl = new Company_dl(this);
            return company_dl.SignUp();
        }

        #endregion
    }
}
