using System;
namespace SpotCheckAdminPortal.Models
{
    public class TempSpot
    {
        #region Properties

        public int TopLeftXCoordinate { get; set; }
        public int TopLeftYCoordinate { get; set; }
        public int BottomRightXCoordinate { get; set; }
        public int BottomRightYCoordinate { get; set; }

        #endregion

        TempSpot()
        {
            this.TopLeftXCoordinate = -1;
            this.TopLeftYCoordinate = -1;
            this.BottomRightXCoordinate = -1;
            this.BottomRightYCoordinate = -1;
        }
    }
}
