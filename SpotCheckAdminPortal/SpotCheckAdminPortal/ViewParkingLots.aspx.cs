using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SpotCheckAdminPortal.Models;

namespace SpotCheckAdminPortal
{

    public partial class ViewParkingLots : System.Web.UI.Page
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

            //Validate info
            bool validate = IoC.ValidateInfo();
            if (validate)
            {
                Response.Redirect("index.aspx");
            }

            //set label
            CompanyNameLiteral.Text = company.CompanyName;


            CreateParkingLotDropDowns();
            //create list of parking lots
            //System.Web.UI.HtmlControls.HtmlGenericControl div = new System.Web.UI.HtmlControls.HtmlGenericControl();


        }

        #endregion

        #region Methods

        private void CreateParkingLotDropDowns()
        {
            foreach (ParkingLot parkingLot in parkingLots)
            {
                //outer div
                HtmlGenericControl outerDiv = new HtmlGenericControl("div");
                outerDiv.Attributes.Add("class", "card shadow mb");
                outerDiv.Attributes.Add("style", "width: 30%");

                HyperLink link = new HyperLink();
                link.NavigateUrl = "#collapseParkingLot" + parkingLot.LotID;
                link.Attributes.Add("class", "d-block card-header py-3");
                link.Attributes.Add("data-toggle", "collapse");
                link.Attributes.Add("role", "button");

                HtmlGenericControl h6 = new HtmlGenericControl("h6");
                h6.InnerHtml = parkingLot.LotName;
                h6.Attributes.Add("class", "m-0 font-weight-bold text-primary");
                link.Controls.Add(h6);

                HtmlGenericControl middleDiv = new HtmlGenericControl("div");
                middleDiv.Attributes.Add("class", "collapse hide");
                middleDiv.ID = "collapseParkingLot" + parkingLot.LotID;

                HtmlGenericControl innerDiv = new HtmlGenericControl("div");
                innerDiv.Attributes.Add("class", "card-body");
                string innerHtml = "<p><strong>" + parkingLot.Address + ", " + parkingLot.City + ", " + parkingLot.State + " " + parkingLot.ZipCode + "</strong></p> ";
                innerHtml += "<p>Total Spots: " + parkingLot.TotalSpots + "</p>";
                innerHtml += "<p>Open Spots: " + parkingLot.OpenSpots + "</p>";
                innerDiv.InnerHtml = innerHtml;
                middleDiv.Controls.Add(innerDiv);

                outerDiv.Controls.Add(link);
                outerDiv.Controls.Add(middleDiv);

                divContainer.Controls.Add(outerDiv);
            }

        }



        #endregion


    }
}
