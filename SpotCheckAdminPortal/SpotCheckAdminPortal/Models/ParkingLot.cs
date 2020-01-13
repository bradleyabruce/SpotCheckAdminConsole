using System;
using System.Collections.Generic;
using SpotCheckAdminPortal.DataLayer;

namespace SpotCheckAdminPortal.Models
{
    public class ParkingLot
    {
        #region Properties

        public int? LotID { get; set; }
        public string LotName { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int? CompanyID { get; set; }
        public int? OpenSpots { get; set; }
        public int? TotalSpots { get; set; }
        public double? Lat { get; set; }
        public double? Lon { get; set; }

        #endregion

        #region Constructors

        public ParkingLot()
        {
            this.LotID = null;
            this.LotName = null;
            this.Address = null;
            this.ZipCode = null;
            this.City = null;
            this.State = null;
            this.CompanyID = null;
            this.OpenSpots = null;
            this.TotalSpots = null;
            this.Lat = null;
            this.Lon = null;
        }

        #endregion

        #region Methods

        public ParkingLot Fill()
        {
            ParkingLot_dl parkingLot_dl = new ParkingLot_dl(this);
            return parkingLot_dl.Fill();
        }

        public List<ParkingLot> GetParkingLotListFromCompanyID(int companyID)
        {
            ParkingLot_dl parkingLot_dl = new ParkingLot_dl(this);
            parkingLot_dl.LotID = companyID;
            return parkingLot_dl.GetParkingLotListFromCompanyID(companyID);
        }

        public ParkingLot UpdateParkingLot()
        {
            ParkingLot_dl parkingLot_dl = new ParkingLot_dl(this);
            return parkingLot_dl.UpdateParkingLot();
        }


        #endregion
    }
}
