using System;
namespace SpotCheckAdminPortal.Models
{
    public static class IoC
    {
        static Company _currentCompany;
        public static Company CurrentCompany
        {
            get
            {
                return _currentCompany;
            }
            set
            {
                _currentCompany = value;
            }
        }
    }
}
