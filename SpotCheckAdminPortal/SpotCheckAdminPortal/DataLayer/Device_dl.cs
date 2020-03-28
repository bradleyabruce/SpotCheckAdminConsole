using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using SpotCheckAdminPortal.Models;

namespace SpotCheckAdminPortal.DataLayer
{
    public class Device_dl : Device
    {
        public Device_dl(Device device)
        {
            this.DeviceID = device.DeviceID;
            this.DeviceName = device.DeviceName;
            this.LocalIpAddress = device.LocalIpAddress;
            this.ExternalIpAddress = device.ExternalIpAddress;
            this.MacAddress = device.MacAddress;
            this.LastUpdateDate = device.LastUpdateDate;
            this.CompanyID = device.CompanyID;
            this.TakeNewImage = device.TakeNewImage;
            this.DeviceStatusID = device.DeviceStatusID;
            this.ParkingLotID = device.ParkingLotID;
        }

        #region Methods

        public new List<Device> GetDeviceListFromCompanyID(int companyID)
        {
            try
            {
                string url = IoC.API_URL + "device/getDevicesByCompanyID";
                string json = companyID.ToString();

                HttpWebRequest request = Connect_dl.BuildRequest(url, "POST", json);
                Tuple<bool, string> result = ValidateResponse(request);

                if (result.Item1)
                {
                    List<Device> devices = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Device>>(result.Item2);
                    return devices;
                }
                else
                {
                    //return empty list
                    List<Device> devices = new List<Device>();
                    return devices;
                }
            }
            catch
            {
                return null;
            }
        }

        public new Device Create()
        {
            try
            {
                string url = IoC.API_URL + "device/adminPortalAssignDevice";
                string json = Connect_dl.BuildJson(this, true);    //remove date properties

                HttpWebRequest request = Connect_dl.BuildRequest(url, "POST", json);
                Tuple<bool, string> result = ValidateResponse(request);

                if (result.Item1)
                {
                    Device deviceCreate = Newtonsoft.Json.JsonConvert.DeserializeObject<Device>(result.Item2);
                    return deviceCreate;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public new Device Update()
        {
            try
            {
                string url = IoC.API_URL + "device/updateAndReturn";
                string json = Connect_dl.BuildJson(this, true);

                HttpWebRequest request = Connect_dl.BuildRequest(url, "POST", json);
                Tuple<bool, string> result = ValidateResponse(request);

                if (result.Item1)
                {
                    Device deviceUpdate = Newtonsoft.Json.JsonConvert.DeserializeObject<Device>(result.Item2);
                    return deviceUpdate;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public new Device Fill()
        {
            try
            {
                string url = IoC.API_URL + "device/fill";
                string json = this.DeviceID.ToString();

                HttpWebRequest request = Connect_dl.BuildRequest(url, "POST", json);
                Tuple<bool, string> result = ValidateResponse(request);

                if (result.Item1)
                {
                    Device deviceFill = Newtonsoft.Json.JsonConvert.DeserializeObject<Device>(result.Item2);
                    return deviceFill;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public new bool Delete()
        {
            try
            {
                string url = IoC.API_URL + "device/removeFromCompany";
                string json = this.DeviceID.ToString();

                HttpWebRequest request = Connect_dl.BuildRequest(url, "POST", json);
                Tuple<bool, string> result = ValidateResponse(request);

                if (result.Item1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public new bool Undeploy()
        {
            try
            {
                string url = IoC.API_URL + "device/undeploy";
                string json = this.DeviceID.ToString();

                HttpWebRequest request = Connect_dl.BuildRequest(url, "POST", json);
                Tuple<bool,string> result = ValidateResponse(request);

                if (result.Item1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public new string GetEncodedImageString()
        {
            try
            {
                string url = IoC.API_URL + "device/retrieveImageString";
                string json = this.DeviceID.ToString();

                HttpWebRequest request = Connect_dl.BuildRequest(url, "POST", json);
                Tuple<bool, string> result = ValidateResponse(request);

                if (result.Item1)
                {
                    return result.Item2;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public new bool ClearImage()
        {
            try
            {
                string url = IoC.API_URL + "device/clearImageFromDatabase";
                string json = this.DeviceID.ToString();

                HttpWebRequest request = Connect_dl.BuildRequest(url, "POST", json);
                Tuple<bool, string> result = ValidateResponse(request);

                if (result.Item1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public new bool SaveSpots(List<Spot> spotList)
        {
            try
            {
                string url = IoC.API_URL + "device/saveSpots";
                string json = JsonConvert.SerializeObject(spotList);

                HttpWebRequest request = Connect_dl.BuildRequest(url, "POST", json);
                Tuple<bool, string> result = ValidateResponse(request);

                if (result.Item1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }


        private Tuple<bool, string> ValidateResponse(HttpWebRequest request)
        {
            if (request != null)
            {
                Dictionary<HttpStatusCode, string> response = Connect_dl.GetResponse(request);
                HttpStatusCode code = response.FirstOrDefault().Key;
                string httpResponse = response.FirstOrDefault().Value;

                if (code == HttpStatusCode.OK)
                {
                    return new Tuple<bool, string>(true, httpResponse);
                }
                else
                {
                    if (code == HttpStatusCode.Conflict)
                    {
                        return new Tuple<bool, string>(false, "");
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
            }
            else
            {
                throw new Exception();
            }
        }

        #endregion End Methods
    }
}

