using System;
using SpotCheckAdminPortal.Models;

namespace SpotCheckAdminPortal.DataLayer
{
    public class Device_dl : Device
    {
        string API_URL = "http://173.91.255.135:8080/SpotCheckServer-2.1.8.RELEASE/";

        public Device_dl(Device device)
        {
            this.DeviceID = device.DeviceID;
            this.DeviceName = device.DeviceName;
            this.LocalIpAddress = device.LocalIpAddress;
            this.ExternalIpAddress = device.ExternalIpAddress;
            this.MacAddress = device.MacAddress;
            this.LotID = device.LotID;
            this.FloorNumber = device.FloorNumber;
            this.LastUpdateDate = device.LastUpdateDate;
            this.CompanyID = device.CompanyID;
            this.TakeNewImage = device.TakeNewImage;
        }
    }
}
