using System;
namespace SpotCheckAdminPortal.Models
{
    public class Spot
    {
        #region Properties

        public int spotId { get; set; }
        public int floorNum { get; set; }
        public int lotId { get; set; }
        public int deviceId { get; set; }
        public bool isOpen { get; set; }
        public int topLeftXCoordinate { get; set; }
        public int topLeftYCoordinate { get; set; }
        public int bottomRightXCoordinate { get; set; }
        public int bottomRightYCoordinate { get; set; }

        #endregion

        #region Constructor
        /*public Spot()
        {
            this.SpotID = -1;
            this.FloorNum = -1;
            this.LotID = -1;
            this.DeviceID = -1;
            this.IsOpen = null;
            this.CompanyUsername = null;
            this.CompanyPassword = null;
        }*/

        #endregion


    }
}
