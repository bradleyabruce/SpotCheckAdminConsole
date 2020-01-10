using System;
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

        #region GetParkingLotListFromCompanyID

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

        #endregion
    }
}
