using System;
using System.Collections.Generic;

namespace SpotCheckAdminPortal.Models
{
    public static class IoC
    {
        #region Properties

        public static string API_URL = "http://173.91.255.135:8080/SpotCheckServer-2.1.8.RELEASE/";

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

        static List<ParkingLot> _parkingLotList;
        public static List<ParkingLot> ParkingLotList
        {
            get
            {
                return _parkingLotList;
            }
            set
            {
                _parkingLotList = value;
            }
        }

        #endregion

        #region Methods

        public static void ClearSessionIoC()
        {
            CurrentCompany = null;
            DeviceList = null;
            ParkingLotList = null;
        }

        #endregion
    }
}
