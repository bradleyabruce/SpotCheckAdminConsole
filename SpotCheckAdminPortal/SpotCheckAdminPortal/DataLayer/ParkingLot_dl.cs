using System.Collections.Generic;
using System.Linq;
using System.Net;
using SpotCheckAdminPortal.Models;

namespace SpotCheckAdminPortal.DataLayer
{
    public class ParkingLot_dl : ParkingLot
    {
        public ParkingLot_dl(ParkingLot parkingLot)
        {
            this.LotID = parkingLot.LotID;
            this.LotName = parkingLot.LotName;
            this.Address = parkingLot.Address;
            this.City = parkingLot.City;
            this.ZipCode = parkingLot.ZipCode;
            this.State = parkingLot.State;
            this.CompanyID = parkingLot.CompanyID;
            this.OpenSpots = parkingLot.OpenSpots;
            this.TotalSpots = parkingLot.TotalSpots;
            this.Lat = parkingLot.Lat;
            this.Lon = parkingLot.Lon;
        }

        #region Methods

        public new ParkingLot Fill()
        {
            ParkingLot parkingLot = new ParkingLot();
            parkingLot.LotID = this.LotID;

            string url = IoC.API_URL + "parkingLot/fill";
            string json = this.LotID.ToString();

            HttpWebRequest request = Connect_dl.BuildRequest(url, "POST", json);

            if(request != null)
            {
                Dictionary<HttpStatusCode, string> response = Connect_dl.GetResponse(request);
                HttpStatusCode code = response.FirstOrDefault().Key;
                string httpResponse = response.FirstOrDefault().Value;

                if (code == HttpStatusCode.OK)
                {
                    //return parkingLot
                    parkingLot = Newtonsoft.Json.JsonConvert.DeserializeObject<ParkingLot>(httpResponse);
                    return parkingLot;
                }
                else
                {
                    //return parkingLots with no info
                    return parkingLot;
                }
            }
            else
            {
                //return parkingLots with no info
                return parkingLot;
            }
        }

        public new List<ParkingLot> GetParkingLotListFromCompanyID(int companyID)
        {
            List<ParkingLot> parkingLots = new List<ParkingLot>();

            string url = IoC.API_URL + "parkingLot/getParkingLotsByCompanyId";
            string json = companyID.ToString();

            HttpWebRequest request = Connect_dl.BuildRequest(url, "POST", json);

            if (request != null)
            {
                Dictionary<HttpStatusCode, string> response = Connect_dl.GetResponse(request);
                HttpStatusCode code = response.FirstOrDefault().Key;
                string httpResponse = response.FirstOrDefault().Value;

                if (code == HttpStatusCode.OK)
                {
                    //return devices
                    parkingLots = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ParkingLot>>(httpResponse);
                    return parkingLots;
                }
                else
                {
                    if (httpResponse == "No devices are linked to this company.")
                    {
                        //Return empty list
                        return parkingLots;
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
        }

        public new ParkingLot UpdateParkingLot()
        {
            ParkingLot parkingLot = new ParkingLot();

            string url = IoC.API_URL + "parkingLot/updateParkingLot";
            string json = Connect_dl.BuildJson(this);

            HttpWebRequest request = Connect_dl.BuildRequest(url, "POST", json);

            if (request != null)
            {
                Dictionary<HttpStatusCode, string> response = Connect_dl.GetResponse(request);
                HttpStatusCode code = response.FirstOrDefault().Key;
                string httpResponse = response.FirstOrDefault().Value;

                if (code == HttpStatusCode.OK)
                {
                    //return parking lot
                    parkingLot = Newtonsoft.Json.JsonConvert.DeserializeObject<ParkingLot>(httpResponse);
                    return parkingLot;
                }
                else
                {
                    //return null
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public new List<Device> GetCamerasDeployed()
        {
            List<Device> devicesToLot = new List<Device>();

            string url = IoC.API_URL + "parkingLot/getCamerasDeployedAtParkingLot";
            string json = Connect_dl.BuildJson(this);

            HttpWebRequest request = Connect_dl.BuildRequest(url, "POST", json);

            if (request != null)
            {
                Dictionary<HttpStatusCode, string> response = Connect_dl.GetResponse(request);
                HttpStatusCode code = response.FirstOrDefault().Key;
                string httpResponse = response.FirstOrDefault().Value;

                if (code == HttpStatusCode.OK)
                {
                    //return parking lot
                    devicesToLot = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Device>>(httpResponse);
                    return devicesToLot;
                }
                else
                {
                    if (httpResponse == "No deployed devices found")
                    {
                        //Return empty list
                        return devicesToLot;
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
                return null;
            }
        }

        public new bool? Delete()
        {
            bool? deleteResult = false;

            string url = IoC.API_URL + "parkingLot/delete";
            string json = Connect_dl.BuildJson(this);

            HttpWebRequest request = Connect_dl.BuildRequest(url, "POST", json);

            if (request != null)
            {
                Dictionary<HttpStatusCode, string> response = Connect_dl.GetResponse(request);
                HttpStatusCode code = response.FirstOrDefault().Key;
                string httpResponse = response.FirstOrDefault().Value;

                if (code == HttpStatusCode.OK)
                {
                    //return parking lot
                    deleteResult = Newtonsoft.Json.JsonConvert.DeserializeObject<bool?>(httpResponse);
                    return deleteResult;
                }
                else
                {
                    if (httpResponse == "Devices still deployed to parking lot.")
                    {
                        //Return empty list
                        return false;
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
                return null;
            }
        }

        #endregion Methods
    }
}
