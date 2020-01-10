using System;
using System.Collections.Generic;
using SpotCheckAdminPortal.DataLayer;

namespace SpotCheckAdminPortal.Models
{
    public class Device
    {
        #region Properties

        public int? DeviceID { get; set; }
        public string DeviceName { get; set; }
        public string LocalIpAddress { get; set; }
        public string ExternalIpAddress { get; set; }
        public string MacAddress { get; set; }
        public int? LotID { get; set; }
        public int? FloorNumber { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public int? CompanyID { get; set; }
        public bool? TakeNewImage { get; set; }

        #endregion

        #region Methods

        public List<Device> GetDeviceListFromCompanyID(int companyID)
        {
            Device_dl device_dl = new Device_dl(this);
            return device_dl.GetDeviceListFromCompanyID(companyID);
        }

        #endregion

    }
}
