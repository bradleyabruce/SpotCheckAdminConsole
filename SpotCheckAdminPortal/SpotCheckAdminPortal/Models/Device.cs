using System;
using System.Collections.Generic;
using SpotCheckAdminPortal.DataLayer;
using SpotCheckAdminPortal.Enums;

namespace SpotCheckAdminPortal.Models
{
    public class Device
    {
        #region Properties

        public int DeviceID { get; set; }
        public string DeviceName { get; set; }
        public string LocalIpAddress { get; set; }
        public string ExternalIpAddress { get; set; }
        public string MacAddress { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public int? CompanyID { get; set; }
        public bool? TakeNewImage { get; set; }
        public int DeviceStatusID { get; set; }
        public int? ParkingLotID { get; set; }

        #endregion

        #region Methods

        public List<Device> GetDeviceListFromCompanyID(int companyID)
        {
            Device_dl device_dl = new Device_dl(this);
            return device_dl.GetDeviceListFromCompanyID(companyID);
        }

        public Device Create()
        {
            Device_dl device_dl = new Device_dl(this);
            return device_dl.Create();
        }

        public Device Update()
        {
            Device_dl device_dl = new Device_dl(this);
            return device_dl.Update();
        }

        public Device Fill()
        {
            Device_dl device_dl = new Device_dl(this);
            return device_dl.Fill();
        }

        public bool Delete()
        {
            Device_dl device_dl = new Device_dl(this);
            return device_dl.Delete();
        }

        public bool Undeploy()
        {
            Device_dl device_dl = new Device_dl(this);
            return device_dl.Undeploy();
        }

        public bool IsDeployed()
        {
            if (this.DeviceStatusID == (int)eDeviceStatus.DeviceStatus.Deployed || this.DeviceStatusID == (int)eDeviceStatus.DeviceStatus.ReadyForSpots)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

    }
}
