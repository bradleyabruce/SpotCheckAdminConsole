using System;
using System.Collections.Generic;

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

        static List<Device> _deviceList;
        public static List<Device> DeviceList
        {
            get
            {
                return _deviceList;
            }
            set
            {
                _deviceList = value;
            }
        }
    }
}
