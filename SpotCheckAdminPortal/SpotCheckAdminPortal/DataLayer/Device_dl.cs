using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
         string url = IoC.API_URL + "device/getDevicesByCompanyID";
         string json = companyID.ToString();

         HttpWebRequest request = Connect_dl.BuildRequest(url, "POST", json);
         return ValidateResponse("GetDeviceListFromCompanyID", request) as List<Device>;
      }

      public new Device Create()
      {
         string url = IoC.API_URL + "device/adminPortalAssignDevice";
         string json = Connect_dl.BuildJson(this, true);    //remove date properties

         HttpWebRequest request = Connect_dl.BuildRequest(url, "POST", json);
         return ValidateResponse("Create", request) as Device;
      }

      public new Device Update()
      {
         string url = IoC.API_URL + "device/updateAndReturn";
         string json = Connect_dl.BuildJson(this, true);

         HttpWebRequest request = Connect_dl.BuildRequest(url, "POST", json);
         return ValidateResponse("Update", request) as Device;
      }

      public new Device Fill()
      {
         string url = IoC.API_URL + "device/fill";
         string json = this.DeviceID.ToString();

         HttpWebRequest request = Connect_dl.BuildRequest(url, "POST", json);
         return ValidateResponse("Fill", request) as Device;
      }

      public new bool? Delete()
        {
            string url = IoC.API_URL + "device/removeFromCompany";
            string json = this.DeviceID.ToString();  

            HttpWebRequest request = Connect_dl.BuildRequest(url, "POST", json);
            return ValidateResponse("Delete", request) as bool?;
        }

      #endregion End Methods

      #region ValidateMethod

      private object ValidateResponse(string method, HttpWebRequest request)
      {
         switch (method)
         {
            case "GetDeviceListFromCompanyID":
               List<Device> devices = new List<Device>();
               if (request != null)
               {
                  Dictionary<HttpStatusCode, string> response = Connect_dl.GetResponse(request);
                  HttpStatusCode code = response.FirstOrDefault().Key;
                  string httpResponse = response.FirstOrDefault().Value;

                  if (code == HttpStatusCode.OK)
                  {
                     //return devices
                     devices = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Device>>(httpResponse);
                     return devices;
                  }
                  else
                  {
                     if (httpResponse == "No devices are linked to this company.")
                     {
                        //Return empty list
                        return devices;
                     }
                     else
                     {
                        //return null
                        return null;
                     }
                  }
               }
               else
               {
                  //return null
                  return null;
               }

            case "Create":
               Device deviceCreate = new Device();
               if (request != null)
               {
                  Dictionary<HttpStatusCode, string> response = Connect_dl.GetResponse(request);
                  HttpStatusCode code = response.FirstOrDefault().Key;
                  string httpResponse = response.FirstOrDefault().Value;

                  if (code == HttpStatusCode.OK)
                  {
                     //return parking lot
                     deviceCreate = Newtonsoft.Json.JsonConvert.DeserializeObject<Device>(httpResponse);
                     return deviceCreate;
                  }
                  else
                  {
                     return null;
                  }
               }
               else
               {
                  return null;
               }

            case "Update":
               Device deviceUpdate = new Device();
               if (request != null)
               {
                  Dictionary<HttpStatusCode, string> response = Connect_dl.GetResponse(request);
                  HttpStatusCode code = response.FirstOrDefault().Key;
                  string httpResponse = response.FirstOrDefault().Value;

                  if (code == HttpStatusCode.OK)
                  {
                     //return parking lot
                     deviceUpdate = Newtonsoft.Json.JsonConvert.DeserializeObject<Device>(httpResponse);
                     return deviceUpdate;
                  }
                  else
                  {
                     return null;
                  }
               }
               else
               {
                  return null;
               }
            case "Fill":
               Device deviceFill = new Device();
               if (request != null)
               {
                  Dictionary<HttpStatusCode, string> response = Connect_dl.GetResponse(request);
                  HttpStatusCode code = response.FirstOrDefault().Key;
                  string httpResponse = response.FirstOrDefault().Value;

                  if (code == HttpStatusCode.OK)
                  {
                     //return parking lot
                     deviceFill = Newtonsoft.Json.JsonConvert.DeserializeObject<Device>(httpResponse);
                     return deviceFill;
                  }
                  else
                  {
                     return null;
                  }
               }
               else
               {
                  return null;
               }
                case "Delete":
                    if (request != null)
                    {
                        Dictionary<HttpStatusCode, string> response = Connect_dl.GetResponse(request);
                        HttpStatusCode code = response.FirstOrDefault().Key;
                        string httpResponse = response.FirstOrDefault().Value;

                        if (code == HttpStatusCode.OK)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                default:
               return false;
         }
      }

      #endregion End Validate Method
   }
}

