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
            return ValidateResponse("Fill", request) as ParkingLot;
        }

        public new List<ParkingLot> GetParkingLotListFromCompanyID(int companyID)
        {
            string url = IoC.API_URL + "parkingLot/getParkingLotsByCompanyId";
            string json = companyID.ToString();

            HttpWebRequest request = Connect_dl.BuildRequest(url, "POST", json);
            return ValidateResponse("GetParkingLotListFromCompanyID", request) as List<ParkingLot>;
        }

        public new ParkingLot UpdateParkingLot()
        {
            string url = IoC.API_URL + "parkingLot/updateParkingLot";
            string json = Connect_dl.BuildJson(this);

            HttpWebRequest request = Connect_dl.BuildRequest(url, "POST", json);
            return ValidateResponse("UpdateParkingLot", request) as ParkingLot;
        }

        public new List<Device> GetCamerasDeployed()
        {
            string url = IoC.API_URL + "parkingLot/getCamerasDeployedAtParkingLot";
            string json = Connect_dl.BuildJson(this);

            HttpWebRequest request = Connect_dl.BuildRequest(url, "POST", json);
            return ValidateResponse("GetCamerasDeployed", request) as List<Device>;
        }

        public new bool? Delete()
        {
            string url = IoC.API_URL + "parkingLot/delete";
            string json = this.LotID.ToString();

            HttpWebRequest request = Connect_dl.BuildRequest(url, "POST", json);
            return ValidateResponse("Delete", request) as bool?;

        }

        public new ParkingLot Create()
        {
            string url = IoC.API_URL + "parkingLot/create";
            string json = Connect_dl.BuildJson(this);

            HttpWebRequest request = Connect_dl.BuildRequest(url, "POST", json);
            return ValidateResponse("Create", request) as ParkingLot;            
        }

        private object ValidateResponse(string method, HttpWebRequest request)
        {
            switch (method)
            {
                case "Fill":
                    ParkingLot parkingLotFill = new ParkingLot();
                    if (request != null)
                    {
                        Dictionary<HttpStatusCode, string> response = Connect_dl.GetResponse(request);
                        HttpStatusCode code = response.FirstOrDefault().Key;
                        string httpResponse = response.FirstOrDefault().Value;

                        if (code == HttpStatusCode.OK)
                        {
                            //return parkingLot
                            parkingLotFill = Newtonsoft.Json.JsonConvert.DeserializeObject<ParkingLot>(httpResponse);
                            return parkingLotFill;
                        }
                        else
                        {
                            //return parkingLots with no info
                            return parkingLotFill;
                        }
                    }
                    else
                    {
                        //return parkingLots with no info
                        return parkingLotFill;
                    }

                case "GetParkingLotListFromCompanyID":
                    List<ParkingLot> parkingLots = new List<ParkingLot>();
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
                case "UpdateParkingLot":
                    ParkingLot parkingLotUpdate = new ParkingLot();
                    if (request != null)
                    {
                        Dictionary<HttpStatusCode, string> response = Connect_dl.GetResponse(request);
                        HttpStatusCode code = response.FirstOrDefault().Key;
                        string httpResponse = response.FirstOrDefault().Value;

                        if (code == HttpStatusCode.OK)
                        {
                            //return parking lot
                            parkingLotUpdate = Newtonsoft.Json.JsonConvert.DeserializeObject<ParkingLot>(httpResponse);
                            return parkingLotUpdate;
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
                case "GetCamerasDeployed":
                    List<Device> devicesToLot = new List<Device>();
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
                case "Delete":
                    bool? deleteResult = false;
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
                case "Create":
                    ParkingLot parkingLotCreate = new ParkingLot();
                    if (request != null)
                    {
                        Dictionary<HttpStatusCode, string> response = Connect_dl.GetResponse(request);
                        HttpStatusCode code = response.FirstOrDefault().Key;
                        string httpResponse = response.FirstOrDefault().Value;

                        if (code == HttpStatusCode.OK)
                        {
                            //return parking lot
                            parkingLotCreate = Newtonsoft.Json.JsonConvert.DeserializeObject<ParkingLot>(httpResponse);
                            return parkingLotCreate;
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
                default:
                    return false;
            }
        }

        #endregion Methods
    }
}
