using System;
using System.Collections.Generic;
using SpotCheckAdminPortal.Models;

namespace SpotCheckAdminPortal
{

    public partial class Dashboard : System.Web.UI.Page
    {
        #region Properties

        private Company company = new Company();
        private List<Device> devices = new List<Device>();
        private List<ParkingLot> parkingLots = new List<ParkingLot>();

        #endregion End Properties

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            //get information
            company = IoC.CurrentCompany;

            Device d = new Device();
            devices = d.GetDeviceListFromCompanyID((int)company.CompanyID);

            ParkingLot pl = new ParkingLot();
            parkingLots = pl.GetParkingLotListFromCompanyID((int)company.CompanyID);


            bool validate = IoC.ValidateInfo();
            if (validate)
            {
                Response.Redirect("index.aspx");
            }
            SetLabels();
            
        }

        #endregion End Events


        #region Methods

        private void SetLabels()
        {
            int totalSpots = 0;
            int openSpots = 0;
            double spotAvailability = 0;
            foreach(ParkingLot lot in parkingLots)
            {
                if(lot.TotalSpots != null && lot.OpenSpots != null)
                {
                    totalSpots += (int)lot.TotalSpots;
                    openSpots += (int)lot.OpenSpots;
                }
            }

            if(totalSpots != 0)
            {
                spotAvailability = Math.Round((double)openSpots / (double)totalSpots * 100, 1);
            }
            else
            {
                spotAvailability = 0;
            }

            //set page info per user
            CompanyNameLiteral.Text = company.CompanyName;
            CameraDeploymentCountLiteral.Text = devices.Count.ToString();
            ParkingLotsCountLiteral.Text = parkingLots.Count.ToString();
            ParkingSpotAvailabilityLiteral.Text = spotAvailability.ToString() + "%";
            ParkingSpotAvailabilityBar.Style.Add(System.Web.UI.HtmlTextWriterStyle.Width, spotAvailability.ToString() + "%");
        }

        #endregion End Methods
    }
}
